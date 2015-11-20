/// <summary>
/// Row controller
/// --------------------
///     The only responsibility of the Row controller is to track the remaining associated bricks, and to
/// communicate to the Game Manager when a row is cleared or when a brick from the top row is cleared.
/// </summary>
using UnityEngine;
using System.Collections;

public class RowController : MonoBehaviour {
	// Public variables
	public GameObject gameManager;
	
	// Private variables
	private int bricksRemainingInRow = 11;
	// END GLOBAL VARIABLES ---------------------------------------

	void resetBricksRemaining() {
		bricksRemainingInRow = 11;
	}
	// END UNITY METHODS ------------------------------------------

	void reduceBricksRemaining() {

		if (--bricksRemainingInRow == 0) {
			gameManager.SendMessage ("rowCleared");
		}

		if (gameObject.tag == "Top Row") {
			gameManager.SendMessage("topRowBreached");
		}
	}
}