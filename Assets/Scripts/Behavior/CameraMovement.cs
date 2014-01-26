using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	private Vector3 initialPosition;

	private Transform playerTransform;

	private Vector3 playerPosition;

	public static Vector3 homePosition;

	public float duration = 3.0f;
	private float startTime;

	private int direction = 0;

	// Use this for initialization
	void Start () {

		initialPosition = transform.position;
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

		// set the camera on the player
		playerPosition = playerTransform.position;
		playerPosition.y = initialPosition.y;
		transform.position = playerPosition;

		startTime = Time.time;
		direction = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if(homePosition != null) {

			if(homePosition.y != initialPosition.y) {
				homePosition.y = initialPosition.y;
			}

			// go to home
			if(direction == 0 && !transform.position.Equals(homePosition)) {
				transform.position = Vector3.Lerp(playerPosition, homePosition, (Time.time - startTime) / duration);
			}
			// go back to player
			else {
				if(direction == 0) {
					direction = 1;
					startTime = Time.time;
				}

				if(!transform.position.Equals(playerPosition)) {
					transform.position = Vector3.Lerp(homePosition, playerPosition, (Time.time - startTime) / duration);
				}
			}

			// ready to start
			if(direction == 1 && transform.position.Equals(playerPosition)) {
				PlayerControl.isCameraReady = true;
			}

			// follow the player once its ready
			if(PlayerControl.isCameraReady) {
				playerPosition = playerTransform.position;
				playerPosition.y = initialPosition.y;
				transform.position = playerPosition;
			}

		}


	}


}