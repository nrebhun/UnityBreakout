/// <summary>
/// Brick Controller
/// --------------------
///     The brick controller controls each brick individually, setting it to the appropriate color/shade, and
/// calculating the appropriate number of points earned once it is cleared.
/// </summary>

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

	// point/color Values[0-4] : 0 == yellow, 1 == green, 2 == orange, 3 == red
	private int[] pointValues = {0, 0, 0, 0};
	private Color[] colorValues = {Color.black, Color.black, Color.black, Color.black};
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
		if (weakened) {
			if (brickLevel == level.Yellow) {
				brickRenderer.material.color = colorValues[0];
			} else if (brickLevel == level.Green) {
				brickRenderer.material.color = colorValues[1];
			} else if (brickLevel == level.Orange) {
				brickRenderer.material.color = colorValues[2];
			} else {
				brickRenderer.material.color = colorValues[3];
			}
		} else {
			if (brickLevel == level.Yellow) {
				brickRenderer.material.color = colorValues[0] * 2;
			} else if (brickLevel == level.Green) {
				brickRenderer.material.color = colorValues[1] * 2;
			} else if (brickLevel == level.Orange) {
				brickRenderer.material.color = colorValues[2] * 2;
			} else {
				brickRenderer.material.color = colorValues[3] * 2;
			}
		}
	}

	void hitBrick() {
		if (weakened) {
			gameManager.SendMessage("updateScore", getPointsEarned());
			parentRow.SendMessage("reduceBricksRemaining");
			gameManager.SendMessage("playPop");
			gameObject.SetActive(false);
		} else {
			weakened = true;
			gameManager.SendMessage("playBoop");
			setBrickColor();
		}
	}

	// Set the values for earned points
	void setPointValues(int[] newPointValues) {
		pointValues [0] = newPointValues[0];
		pointValues [1] = newPointValues[1];
		pointValues [2] = newPointValues[2];
		pointValues [3] = newPointValues[3];
	}

	// Set the color values
	void setColorValues(Color[] newColorValues) {
		colorValues [0] = newColorValues[0];
		colorValues [1] = newColorValues[1];
		colorValues [2] = newColorValues[2];
		colorValues [3] = newColorValues[3];
	}

	int getPointsEarned() {
		if (brickLevel == level.Yellow) {
			return pointValues[0];
		} else if (brickLevel == level.Green) {
			return pointValues[1];
		} else if (brickLevel == level.Orange) {
			return pointValues[2];
		} else {
			return pointValues[3];
		}
	}

	void resetBrick () {
		weakened = false;
		setBrickColor ();
	}
}