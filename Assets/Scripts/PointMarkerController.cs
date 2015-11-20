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
	private float timeStamp;
	private Text pointsMarker;
	private float width = 15.66f, height = 9.15f;
	void Start () {
		pointsMarker = GetComponent<Text> ();
		shouldAppear = false;
		transform.localScale = Vector3.zero;
	}
	
	void Update () {
		// Mark a, position the text, make it visible, and toggle appearance bools
		if (shouldAppear) {
			timeStamp = Time.time * 1000;
			transform.position = appearPosition;
			transform.localScale = Vector3.one;
			didAppear = true;
			shouldAppear = false;
		}

		// After appearing, translate the text upwards for 1.5 seconds before disappearing and toggling off didAppear
		if (didAppear && ((Time.time * 1000) - timeStamp <= 1500)) {
			transform.Translate(Vector3.up / 10);
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
		appearPosition = new Vector3( 420.0f * ((newPosition.x + width) / width), 
		                              260.0f * ((newPosition.y + height) / height), 
		                              0.0f);
		shouldAppear = true;
	}
}