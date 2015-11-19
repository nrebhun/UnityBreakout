/// <summary>
/// Game Manager script.
/// --------------------
///     The Game Manager is an invisible gameObject in the primary scene which is responsible for managing key values
/// of the game, including: score, current level, remaining number of lives, and then number of bricks and rows cleared.
/// The GM is provided the maximum number of lives in the editor, as well as references to the ball and paddle, for
/// easier access to these objects in relevant methods.
/// Additionally, this object keeps track of whether the game is paused or not, 
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {
	// Public variables
	public Text scoreGUIText, livesGUIText, levelGUIText;
	public Text titleGUIText, gameOverGUIText, endGameStatsGUIText, completedLevelGUIText;
	public Button playButton, tryAgainButton, nextLevelButton;
	public GameObject ball, paddle;
	public int maxLives;
	// Private variables
	private int livesRemaining = 0, score = 0, currentLevel = 0, bricksBroken = 0, rowsCleared = 0;
	private bool gamePaused = true, paddleMinimized = false;
	private GameObject topRow;
	private GameObject[] brickSet, rowSet;
	// END GLOBAL VARIABLES ---------------------------------------

	void Start () {
		// Set currently-unnecessary UI elements to be invisible (shrunk to size 0);
		gameOverGUIText.transform.localScale = Vector3.zero;
		tryAgainButton.transform.localScale = Vector3.zero;
		endGameStatsGUIText.transform.localScale = Vector3.zero;

		completedLevelGUIText.transform.localScale = Vector3.zero;
		nextLevelButton.transform.localScale = Vector3.zero;

		Screen.SetResolution (1280, 720, false);
	}
	// END UNITY METHODS ------------------------------------------

	/* ----- Game State methods, including r ----- */
	// True initialization of gameplay
	void beginGame() {
		brickSet = GameObject.FindGameObjectsWithTag ("Brick");
		rowSet = GameObject.FindGameObjectsWithTag ("Row");
		topRow = GameObject.FindGameObjectWithTag ("Top Row");

		titleGUIText.transform.localScale = Vector3.zero;
		playButton.transform.localScale = Vector3.zero;

		resetAllKeyValues ();
		toggleGamePaused ();
	}

	// Board Cleared! Pause game and display proper UI elements
	void levelCleared() {
		toggleGamePaused ();

		completedLevelGUIText.transform.localScale = Vector3.one;
		nextLevelButton.transform.localScale = Vector3.one;
	}

	// Cleanup of board for next level
	void progressToNextLevel() {
		increaseLevelCount ();

		// Reset all bricks
		resetBricks ();
		resetRows ();

		// Reset ball and paddle
		ball.SendMessage ("resetBallPositionAndVelocity");
		paddle.SendMessage ("resetPaddle");
		paddleMinimized = false;

		// Hide Level completion UI
		completedLevelGUIText.transform.localScale = Vector3.zero;
		nextLevelButton.transform.localScale = Vector3.zero;

		// Un-pause game
		toggleGamePaused ();
	}

	// Game over, man! GAME OVER!
	// Pause the game, and make appropriate UI visible
	void gameOver() {
		toggleGamePaused ();

		gameOverGUIText.transform.localScale = Vector3.one;
		tryAgainButton.transform.localScale = Vector3.one;

		updateEndGameStatsText ();
		endGameStatsGUIText.transform.localScale = Vector3.one;
	}

	// Reset game, called after Game Over occurs and player presses "Try Again" button
	void resetGame() {
		gameOverGUIText.transform.localScale = Vector3.zero;
		tryAgainButton.transform.localScale = Vector3.zero;
		endGameStatsGUIText.transform.localScale = Vector3.zero;

		resetAllKeyValues ();
		toggleGamePaused ();
	}

	// Reset key gameplay values
	void resetAllKeyValues() {
		// Reset Level, lives, and score
		resetLevelCount ();
		resetLives ();
		resetScore ();
		// Reactivate/reset Bricks and Rows
		resetBricks ();
		resetRows ();
		// Clear stored values
		rowsCleared = 0;
		bricksBroken = 0;
		paddleMinimized = false;

		// Reset ball and paddle to initial positions
		ball.SendMessage ("Start");
		paddle.SendMessage ("Start");
	}
	
	// Toggle game paused status, and pause moving items
	void toggleGamePaused() {
		gamePaused = !gamePaused;
		ball.SendMessage("togglePaused");
		paddle.SendMessage("togglePaused");
	}

	/* ----- Methods handling Remaining Lives ----- */
	// Update current number of lives
	void lifeLost() {
		// If player has lives remaining, decrement and update appropriate UI element...
		if (livesRemaining > 0) {
			livesRemaining--;
			updateLivesGUIText();
		} else {		// ... otherwise, end the game
			gameOver();
		}
	}

	// Set remaining lives to maximumum, and update appropriate UI element
	void resetLives() {
		livesRemaining = maxLives;
		updateLivesGUIText();
	}

	// Update UI for number of lives remaining
	void updateLivesGUIText() {
		livesGUIText.text = "Lives: " + livesRemaining;
	}
	
	/* ----- Methods handling Score and Score UI ----- */
	// Add points to score, update appropriate UI element, and increase ball speed modifier
	void updateScore(int pointsToAdd) {
		bricksBroken++;
		score += pointsToAdd;
		updateScoreGUIText ();
	}

	// Set player's score to zero, update appropriate UI element, and reset ball speed modifier
	void resetScore() {
		score = 0;
		updateScoreGUIText ();
		ball.SendMessage("resetSpeedModifier");
	}

	// Update UI for player's score
	void updateScoreGUIText() {
		scoreGUIText.text = "Score: " + score;
	}

	/* ----- Methods handling the game board ----- */
	// Re-activate and reset all bricks in the scene
	void resetBricks (){
		foreach (GameObject brick in brickSet) {
			brick.SetActive (true);
			brick.SendMessage ("resetBrick");
		}
	}

	// Reset the value tracked by each row, with a special case for the top row
	void resetRows() {
		foreach (GameObject row in rowSet) {
			row.SendMessage ("resetBricksRemaining");
		}
		topRow.SendMessage ("resetBricksRemaining");
	}

	// When a row clears, increase ball speed
	void rowCleared() {
		ball.SendMessage ("increaseSpeedModifier");
		if ((++rowsCleared % 8) == 0) {
			levelCleared();
		}
	}
	
	// When top row is breached, if paddle hasn't been minimized, make it so
	void topRowBreached() {
		if (!paddleMinimized) {
			paddle.SendMessage("updatePaddleSize");
			paddleMinimized = true;
		}
	}

	/* ----- Methods handling Current Level ----- */
	// Increment the current level count, and update appropriate UI element
	void increaseLevelCount() {
		currentLevel++;
		updateLevelGUIText ();
	}

	// Reset the current level count, and update the appropriate UI element
	void resetLevelCount() {
		currentLevel = 1;
		updateLevelGUIText ();
	}

	// Update UI for current level count
	void updateLevelGUIText() {
		levelGUIText.text = "Level: " + currentLevel;
	}

	// Update UI for nextLevelButton
	void upddateNextLevelButtonGUIText() {
		nextLevelButton.GetComponentInChildren<Text> ().text = "Begin Level" + (currentLevel + 1);
	}

	// Update UI for End-game stats text
	void updateEndGameStatsText() {
		endGameStatsGUIText.text = 	"Bricks Broken: " + bricksBroken + "\n" +
									"Rows Eliminated: " + rowsCleared + "\n" + 
									"Level Achieved: " + currentLevel + "\n" +
									"Final Score: " + score + " points\n\n" +
									"Thanks for playing!";
	}
}
