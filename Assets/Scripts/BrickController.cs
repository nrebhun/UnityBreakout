using UnityEngine;
using System.Collections;

public class BrickController : MonoBehaviour {
	// Public Variables
	public enum level {Yellow, Green, Orange, Red}; 
	public level brickLevel;
	public GameObject gameManager;

	// Private Variables
	private bool weakened = false;
	private Renderer brickRenderer;
	private GameObject parentRow;
	// END GLOBAL VARIABLES ---------------------------------------

	void Start () {
		brickRenderer = GetComponent<Renderer> ();
		parentRow = gameObject.transform.root.gameObject;
		// Initialization of brick color
		setBrickColor ();
	}

	/*
	// For Debug Only	
	void OnMouseDown() {
		hitBrick ();
		hitBrick ();
	}
	*/
	// END UNITY METHODS ------------------------------------------

	void setBrickColor() {
		if (this.weakened) {
			if (brickLevel == level.Yellow) {
				brickRenderer.material.color = Color.yellow;
			} else if (brickLevel == level.Green) {
				brickRenderer.material.color = Color.green;
			} else if (brickLevel == level.Orange) {
				brickRenderer.material.color = (Color.red + Color.yellow) / 2;
			} else {
				brickRenderer.material.color = Color.red;
			}
		} else {
			if (brickLevel == level.Yellow) {
				brickRenderer.material.color = Color.yellow * 2;
			} else if (brickLevel == level.Green) {
				brickRenderer.material.color = Color.green * 2;
			} else if (brickLevel == level.Orange) {
				brickRenderer.material.color = Color.red + Color.yellow;
			} else {
				brickRenderer.material.color = Color.red * 2;
			}
		}
	}

	void hitBrick() {
		if (weakened) {
			gameManager.SendMessage("updateScore", getPointsEarned());
			parentRow.SendMessage("reduceBricksRemaining");

			gameObject.SetActive(false);
		} else {
			weakened = true;
			setBrickColor();
		}
	}

	int getPointsEarned() {
		if (brickLevel == level.Yellow) {
			return 1;
		} else if (brickLevel == level.Green) {
			return 3;
		} else if (brickLevel == level.Orange) {
			return 5;
		} else {
			return 7;
		}
	}

	void resetBrick () {
		weakened = false;
		setBrickColor ();
	}
}