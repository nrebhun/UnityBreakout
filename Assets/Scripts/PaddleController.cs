/// <summary>
/// Paddle Controller
/// --------------------
///     The paddle controller polls the keyboard for user input, and moves the paddle accordingly. It also tracks 
/// whether or not it has been minimized, in conjunction with the Game Manager. This redundancy is mostly to prevent the
/// paddle from changing size when it isn't supposed to.
/// </summary>
using UnityEngine;
using System.Collections;

public class PaddleController : MonoBehaviour {
	// Public Variables
	public float speed;				// Decently playable around 200
	public Vector3 initialPosition;

	// Private Variables
	private int moveDirection;		// positive = right, negative = left, 0 = no motion
	private float horizontalAxisInput;
	private Renderer paddleRenderer;
	private bool paused = true, minimized, hitLeftWall = false, hitRightWall = false;
	// END GLOBAL VARIABLES ---------------------------------------
	
	void Start () {
		// Initialize the position, dimensions, and color of the paddle
		transform.position = initialPosition;
		paddleRenderer = GetComponentInChildren<Renderer> ();
		paddleRenderer.material.color = Color.white * 2;
		transform.localScale = new Vector3(3.0f, 0.6f, 1.0f);
		minimized = false;
	}

	void FixedUpdate () {
		// Nullify transform at start of each frame to prevent sliding/Unity weirdness
		transform.Translate (0, 0, 0);

		// If not paused...
		if (!paused) {
			// Poll keyboard for user input
			pollKeyboard ();
			// Add force to paddle in the direction of user input * desired speed
			transform.Translate (moveDirection * (speed / 1000), 0, 0);
		}
	}

	void OnCollisionEnter(Collision col) {
		// Nullify Paddle motion...
		this.transform.Translate (0, 0, 0);

		if (col.gameObject.name == "Left Wall") {
			hitLeftWall = true;
		} else if (col.gameObject.name == "Right Wall") {
			hitRightWall = true;
		}
	}

	void OnCollisionExit(Collision col) {
		// Nullify Paddle motion...
		this.transform.Translate (0, 0, 0);
		
		if (col.gameObject.name == "Left Wall") {
			hitLeftWall = false;
		} else if (col.gameObject.name == "Right Wall") {
			hitRightWall = false;
		}
	}

	// END UNITY METHODS ------------------------------------------

	void pollKeyboard() {
		// Get horizontal directional input from user 
		horizontalAxisInput = Input.GetAxis ("Horizontal");

		// Alter paddle direction accordingly
		if (horizontalAxisInput < 0 && !hitLeftWall) {
			moveDirection = -1;
		} else if (horizontalAxisInput > 0 && !hitRightWall) {
			moveDirection = 1;
		} else {
			moveDirection = 0;
		}
	}

	// Set paddle size to be less-wide if it hasn't already been changed
	void updatePaddleSize() {
		if (!minimized) {
			transform.localScale = new Vector3(2.0f, 0.6f, 1.0f);
			minimized = true;
		}
	}

	// Reset paddle position and size
	void resetPaddle() {
		transform.position = initialPosition;
		transform.localScale = new Vector3(3.0f, 0.6f, 1.0f);
		minimized = false;
	}

	// Toggle Paused status
	void togglePaused() {
		paused = !paused;
	}
}