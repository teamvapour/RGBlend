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
	public float enemyReallyCloseRadius = 100.0f;

	private bool risedPlayerFollowers;
	private bool lowerPlayerFollowers;

	public float distanceToPlayer = 0.0f;

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

		risedPlayerFollowers = false;
		lowerPlayerFollowers = false;
	}

	private void RisePlayerFollowers() {

		if(!risedPlayerFollowers) {
			PlayerControl playerController = player.GetComponent<PlayerControl>();
			playerController.numberOfFollowers++;
			risedPlayerFollowers = true;
			lowerPlayerFollowers = false;
		}
	}
	private void LowerPlayerFollowers() {

		if(!lowerPlayerFollowers) {
			PlayerControl playerController = player.GetComponent<PlayerControl>();
			playerController.numberOfFollowers--;
			lowerPlayerFollowers = true;
			risedPlayerFollowers = false;
		}
	}




	private void UpdateFaceOrientation() {

		if(state != EnemyState.INITIAL) {
			float rotation = Mathf.Atan2(agent.velocity.x, agent.velocity.z);
			lastRotation = rotation;
			transform.eulerAngles = new Vector3(90,(lastRotation*180.0f)/Mathf.PI,0);


		} else {
			transform.eulerAngles = new Vector3(90, startRotation.y,0);
		}

		// keep the enemy at constant y pos
		Vector3 pos = transform.position;
		pos.y = 0.5f;
		transform.position = pos;
	}

	private void UpdateState() {
		// we go at the player, if we see it

		PlayerControl playerController = player.GetComponent<PlayerControl>();
		Vector3 distanceVector = transform.position - player.transform.position;

		distanceToPlayer = distanceVector.sqrMagnitude;
		// check if player is close enough
		// to do anything at all
		float enemyRadius = playerController.GetEnemyRadius();
		if(distanceVector.sqrMagnitude < enemyRadius) {
			// if we are not angry, check if we see the player
			// we get angry if we do
			if(state != EnemyState.CHASE_PLAYER) {
				Vector3 forward = transform.TransformDirection(Vector3.up);
				RaycastHit hit;
				// we get angry if we see the player, or if he is really close to us - 6th sense!
				if(Physics.Raycast(transform.position, forward, out hit, rayCastRadius)) {
					if(hit.collider.name == "Player") {
						// Get Angry!
						RisePlayerFollowers();
						state = EnemyState.CHASE_PLAYER;
					}
				}

				if (distanceVector.sqrMagnitude < enemyReallyCloseRadius) {
					RisePlayerFollowers();
					state = EnemyState.CHASE_PLAYER;
				}
			// if we are already angry, check if we are close to the player
			} 
		} else {
			if(state != EnemyState.INITIAL) {
				LowerPlayerFollowers();
				state = EnemyState.GO_HOME;
			}
		}



		
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
