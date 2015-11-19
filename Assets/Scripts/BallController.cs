using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {
	// Public Variables
	public Vector3 initialPosition, initialVelocity, resetPosition, resetVelocity;
	public GameObject gameManager;
	public float speed;		// Speed of the ball. Good range seems to be between 150 (easy) and 250 (hard) with Paddle's speed set to 20.
	public float resetHeight;
	// Private Variables
	private Vector3 currentVelocity;
	private float speedModifier = 0;
	private bool paused = true;
	private Renderer ballRenderer;
	// END GLOBAL VARIABLES ---------------------------------------

	void Start () {
		ballRenderer = gameObject.GetComponent<Renderer> ();
		// Initialization of position, vector and color
		transform.position =  initialPosition;
		currentVelocity = initialVelocity.normalized * ((speed + speedModifier)  / 1000);
		ballRenderer.material.color = Color.white * 2;
	}

	void FixedUpdate() {
		// Nullify transform at start of each frame to prevent sliding/Unity weirdness
		transform.Translate (0, 0, 0);

		// If not paused...
		if (!paused) {
			// Move the ball!
			transform.Translate (currentVelocity.normalized * ((speed + speedModifier) / 1000));
			if (transform.position.y < resetHeight) {
				ballWasLost ();
			}
		}
	}
	 
	void OnCollisionEnter(Collision col) {
		// On collision, get the other object's name for later repeat-use
		string otherObjectName = col.gameObject.name;

		if (otherObjectName == "Ceiling") {
			// If the ball collides with the Ceiling, invert y-velocity
			currentVelocity.y = -currentVelocity.y;

		} else if ((otherObjectName == "Left Wall") || (otherObjectName == "Right Wall")) {
			// Otherwise, if the ball collides with a wall, invert x-velocity
			currentVelocity.x = -currentVelocity.x;

		} else if (otherObjectName == "Paddle") {
			// Set current velocity to be angle between center of ball and center of paddle, converted to a unit vector and then accelerated to correct speed
			currentVelocity = new Vector3 ((this.transform.position.x - col.transform.position.x),
			                              (this.transform.position.y - col.transform.position.y),
			                              0).normalized * (speed + speedModifier / 1000);
		} else if (otherObjectName == "Brick") {
			// yPositionDistance is the vertical distance between the point of contact and the brick's center point
			float yPositionDistance = Mathf.Abs(col.contacts[0].point.y - col.gameObject.transform.position.y);

			// If the distance between the collision point and the brick's y-axis is less than than half the height of the ball...
			if (yPositionDistance < col.collider.bounds.size.y / 2) {
				currentVelocity.x = -currentVelocity.x;		// ... the ball contacted the brick's side; invert x-velocity
			} else {
				currentVelocity.y = -currentVelocity.y;		// Otherwise, the ball contacted the brick's top or bottom; invert y-velocity
			}

			// Finally, damage the brick!
			col.gameObject.SendMessage("hitBrick");
		}
	}
	// END UNITY METHODS ------------------------------------------

	// Increase Ball Speed by 1.5%
	void increaseSpeedModifier() {
		speedModifier += 15;
	}

	// Reset Ball Speed Modifier
	void resetSpeedModifier() {
		speedModifier = 0;
	}

	// Reset Ball Position & Velocity to initial values, called in Game Manager after player begins next level
	void resetBallPositionAndVelocity() {
		transform.position = initialPosition;
		currentVelocity = initialVelocity.normalized * ((speed + speedModifier)  / 1000);
	}

	// Reset of Ball position and velocity to respawn values. Occurs only after life is lost.
	void ballWasLost() {
		// Nullify velocity to eliminate unexpected behavior upon repositioning
		currentVelocity = Vector3.zero;
		// Alert game manager that the player lost a life
		gameManager.SendMessage ("lifeLost");

		transform.position = resetPosition;
		currentVelocity = resetVelocity.normalized * ((speed + speedModifier)  / 1000);
	}
	
	// Toggle Paused status
	void togglePaused() {
		paused = !paused;
	}
}