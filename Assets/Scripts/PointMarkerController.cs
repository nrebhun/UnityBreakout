/// <summary>
/// Point marker controller
/// --------------------
///     The Point Marker indicates to the player how many points were earned when a brick is broken. 
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PointMarkerController : MonoBehaviour {
	// Public Variables
	public Vector3 appearPosition;

	// Private Variables
	private bool shouldAppear, didAppear;
	private float timeStamp, timeScale = 1000;
	private Text pointsMarker;
	private float gameWorldWidthRatio = 15.66f, gameWorldHeightRatio = 9.15f;
	private float UIWidthRatio = 420.0f, UIHeightRatio = 260.0f;
	private int speedDivisor = 10, timeToDisappearInMilliseconds = 1500;
	// END GLOBAL VARIABLES ---------------------------------------

	void Start () {
		pointsMarker = GetComponent<Text> ();
		shouldAppear = false;
		didAppear = false;
		transform.localScale = Vector3.zero;
	}
	
	void Update () {
		// Mark a timestamp, position the text, make it visible, and toggle appearance bools
		if (shouldAppear) {
			timeStamp = Time.time * timeScale;
			transform.position = appearPosition;
			transform.localScale = Vector3.one;
			didAppear = true;
			shouldAppear = false;
		}

		// After appearing, translate the text upwards for 1.5 seconds before disappearing and toggling off didAppear
		if (didAppear && ((Time.time * timeScale) - timeStamp <= timeToDisappearInMilliseconds)) {
			transform.Translate(Vector3.up / speedDivisor);
		} else {
			transform.localScale = Vector3.zero;
			didAppear = false;
		}
	}
	// END UNITY METHODS ------------------------------------------

	// Prepare text content, should immediately precede call to primePointMarkerForAppearance()
	void updatePointMarkerText(int score) {
		pointsMarker.text = "+" + score;
	}

	// Prepare position and set shouldAppear to true (prime object for appearance)
	void primePointMarkerForAppearance(Vector3 newPosition) {
		appearPosition = new Vector3( UIWidthRatio * ((newPosition.x + gameWorldWidthRatio) / gameWorldWidthRatio), 
		                              UIHeightRatio * ((newPosition.y + gameWorldHeightRatio) / gameWorldHeightRatio), 
		                              0.0f);
		shouldAppear = true;
	}
}