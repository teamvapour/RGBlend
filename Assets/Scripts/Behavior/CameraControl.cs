using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	private Transform playerTransform;
	private Vector3 initialPosition;

	// Use this for initialization
	void Start () {
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(playerTransform != null) {
			Vector3 playerPosition = playerTransform.position;
			playerPosition.y = initialPosition.y;
			transform.position = playerPosition;
		}



	}
}
