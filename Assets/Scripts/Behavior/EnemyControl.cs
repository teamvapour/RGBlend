using UnityEngine;
using System.Collections;


enum EnemyState {
	INITIAL,
	GO_HOME,
	CHASE_PLAYER
}

public class EnemyControl : MonoBehaviour {


	private NavMeshAgent agent;
	private GameObject player;

	private float lastRotation = 0.0f;
	private EnemyState state;
	private bool isSeenByCamera;
	private Vector3 startingPosition;
	private Vector3 startRotation;

	public float minHomeDistance = 3.0f;
	public float rayCastRadius = 60.0f;
	void Awake() {

	}

	// Use this for initialization
	void Start () {
		state = EnemyState.INITIAL;
		agent = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player");
		startingPosition = transform.position;
		startRotation = transform.eulerAngles;
		agent.SetDestination(startingPosition);
	}

		
	private void UpdateFaceOrientation() {
		if(state != EnemyState.INITIAL) {
			float rotation = Mathf.Atan2(agent.velocity.x, agent.velocity.z);
			lastRotation = rotation;
			transform.eulerAngles = new Vector3(90,(lastRotation*180.0f)/Mathf.PI,0);

			Vector3 pos = transform.position;
			pos.y = 0.5f;
			transform.position = pos;
		} else {
			transform.eulerAngles = new Vector3(90, startRotation.y,0);
		}
	}

	private void UpdateState() {
		// we go at the player, if we see it

		Vector3 forward = transform.TransformDirection(Vector3.up);

		RaycastHit hit;
		if(Physics.Raycast(transform.position, forward, out hit, rayCastRadius)) {
			Debug.Log ("There is something in fornt: "+hit.collider.name);
			if(hit.collider.name == "Player") {
				// Get Angry!
				state = EnemyState.CHASE_PLAYER;
			} 
		} else {
			if(state != EnemyState.INITIAL)
				state = EnemyState.GO_HOME;
		}

		Debug.DrawLine(transform.position, transform.position + forward*100.0f);

	}

	private void UpdateTarget() {
		switch(state) {
			case(EnemyState.GO_HOME):

				// if we are close to home, just stop
				// this way everytime the NPC will be back 
				// closer to his initial position, but not exactly on it

				if(agent.remainingDistance < minHomeDistance) {
					agent.Stop ();
				} else {
					agent.SetDestination(startingPosition);
				}
			break;

			case(EnemyState.CHASE_PLAYER):
				agent.SetDestination(player.transform.position);
			break;
		}

	}

	// Update is called once per frame
	void Update () {
		UpdateFaceOrientation();
		UpdateState();
		UpdateTarget();
	}
}
