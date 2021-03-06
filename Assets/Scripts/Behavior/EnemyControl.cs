﻿using UnityEngine;
using System.Collections;


enum EnemyState {
	INITIAL,
	GO_HOME,
	CHASE_NPC,
	CHASE_PLAYER
}

public enum EnemyType {
	ENEMY_POLICE,
	ENEMY_RIOT,
	ENEMY_HIPPY
}

public class EnemyControl : MonoBehaviour {


	private NavMeshAgent agent;
	private GameObject player;

	private float lastRotation = 0.0f;
	private EnemyState state;
	private bool isSeenByCamera;
	private Vector3 startingPosition;
	private Vector3 startRotation;

	public EnemyType enemyType;

	public AudioClip fxHit;

	public float enemyLife = 100.0f;
	public float enemyDamage = 10.0f;
	public float minHomeDistance = 3.0f;
	public float rayCastRadius = 60.0f;
	public float enemyReallyCloseRadius = 100.0f;
	public float enemyKillRadius = 30.0f;

	private bool risedPlayerFollowers;
	private bool lowerPlayerFollowers;

	public Color enemyColor;
	public float distanceToPlayer = 0.0f;
	public float myRadius = 0.0f;
	public float minPlayerColorDistanceToStayAlive = 100.0f;


	private GameObject guiManager;
	private Transform enemyTarget = null;

	private int numSprites = 4;
	private float textureOffset = 0.25f;
	private bool isMoving = false;
	private bool isDead = false;

	private FXPlayer fx = null;

	void Awake() {

	}

	// Use this for initialization
	void Start () {

		fx = GameObject.Find ("FXPlayer").GetComponent<FXPlayer>();

		isMoving = false;
		isDead = false;
		RandomRotate();
	
		guiManager = GameObject.Find ("GUIManager");
		state = EnemyState.INITIAL;
		agent = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player");
		startingPosition = transform.position;
		startRotation = transform.eulerAngles;
		agent.SetDestination(startingPosition);

		risedPlayerFollowers = false;
		lowerPlayerFollowers = false;


		StartCoroutine(UpdateSpriteAnimation());
	}

	public void HitMeWithABat(float damage)  {
		enemyLife -= damage;
		if(enemyLife < 0) {
			enemyLife = 0;
			isDead = true;
		}
	}


	private void RandomRotate() {
		Vector3 rotation = transform.eulerAngles;
		rotation.y = Random.Range(0,360);
		
		transform.eulerAngles = rotation;
	}

	IEnumerator UpdateSpriteAnimation() {

		while(true) {
			if(isMoving) {
				Vector2 tv = renderer.material.GetTextureOffset("_MainTex");
				
				tv.x += textureOffset;
				
				if(tv.x> textureOffset*(numSprites-1)) {
					tv.x = 0.0f;
				}
				renderer.material.SetTextureOffset("_MainTex",tv);
			}
			yield return new WaitForSeconds(0.25f);
		}
		
	}


	private void RisePlayerFollowers() {

		if(!risedPlayerFollowers) {
			PlayerControl playerController = player.GetComponent<PlayerControl>();
			playerController.addFollower(transform);
			playerController.numberOfFollowers++;
			risedPlayerFollowers = true;
			lowerPlayerFollowers = false;
		}
	}
	private void LowerPlayerFollowers() {

		if(!lowerPlayerFollowers) {
			PlayerControl playerController = player.GetComponent<PlayerControl>();
			playerController.removeFollower(transform);
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

	private float GetDistanceToPlayer() {
		return 0.0f;
	}

	private float GetDistanceToTargetEnemy() {

		Vector3 distanceVector = transform.position - enemyTarget.transform.position;
		return distanceVector.sqrMagnitude;
	}

	private void HitEnemyTarget() {

		EnemyControl enemyTargetControl = enemyTarget.GetComponent<EnemyControl>();

		if(enemyType == EnemyType.ENEMY_POLICE) {
			fx.PlayFxEnemyHit();	
		}
		
		if(enemyTargetControl.isDead) {
			PlayerControl playerController = player.GetComponent<PlayerControl>();
			playerController.removeFollower(enemyTarget);
			Destroy(enemyTarget.gameObject);
						
			// boost our minimum level
			ColourBarController barController = guiManager.GetComponent<ColourBarController>();
			barController.ColourBaseBoost(enemyType);
		}

		enemyTargetControl.HitMeWithABat(enemyDamage);

	}

	private void UpdateState() {


		float enemyIsClose = 300;
		float enemyInRange = 50;
		// we go at the player, if we see it
		if(state == EnemyState.CHASE_NPC) {
			// chase the npc forever

			if(enemyTarget == null)
			{
				// check if the player is close enough

				// if not, go home

				state = EnemyState.GO_HOME;
				return;
			}


			float distanceToEnemy = GetDistanceToTargetEnemy();

			if(distanceToEnemy < enemyKillRadius)
			{
				// do stuff when the enemy is in range to attack
				HitEnemyTarget();
			}
			else if(distanceToEnemy < enemyReallyCloseRadius) 
			{
				// do stuff when the enemy is close enough.
				// For example, alert the enemy and set them to running away!
				
				//Debug.Log("Enemy in range: " + transform.name +" at "+transform.position.ToString()+"  is chasing "+enemyTarget.transform.name+" at "+ enemyTarget.transform.position.ToString() + ": "+ distanceToEnemy.ToString() + " to go");
				
			}	else {
			//	Debug.Log("Enemy in pursuit: " + transform.name +" at "+transform.position.ToString()+"  is chasing "+enemyTarget.transform.name+" at "+ enemyTarget.transform.position.ToString() + ": "+ distanceToEnemy.ToString() + " to go");

			}
			
			
			
		} else {
			PlayerControl playerController = player.GetComponent<PlayerControl>();
			Vector3 distanceVector = transform.position - player.transform.position;

			distanceToPlayer = distanceVector.sqrMagnitude;
			// check if player is close enough
			// to do anything at all
			float enemyRadius = playerController.GetEnemyRadius();
			myRadius = enemyRadius;
			if(distanceVector.sqrMagnitude < enemyRadius) {
		//		Debug.Log (transform.name + " -> Players is with the enemy radius, my state is "+state);
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

					//	Debug.Log ("Ray Cast Hit!");
					} else if (distanceVector.sqrMagnitude < enemyReallyCloseRadius) {
				
						RisePlayerFollowers();
						state = EnemyState.CHASE_PLAYER;

						// the NPC is at critical distance to the player
					}
				// if we are already angry, check if we are close to the player
				} else {

					float colorDistance = GetPlayerColorDistance();

					if((colorDistance > minPlayerColorDistanceToStayAlive) && (distanceVector.sqrMagnitude < enemyKillRadius)) {
						if(!playerController.isDead) {
							Debug.Log ("Player got killed by:"+enemyType+" Color Distance:"+colorDistance);
							playerController.DieLikeAMan();
						}

					} else {
						// you are friend of the enemy
						// so the enemy will check for some policeman or rioters to kill
						PlayerControl playerControl = player.GetComponent<PlayerControl>();

						foreach(Transform enemy in playerControl.npcFollowers) {

							if(enemy == transform) continue;

							if(enemyType != enemy.GetComponent<EnemyControl>().enemyType) {
								LowerPlayerFollowers();
								enemyTarget = enemy.transform;
								state = EnemyState.CHASE_NPC;

								// Knock back the player's colour bar. 
							//	ColourBarController barController = guiManager.GetComponent<ColourBarController>();
							//	barController.ColourKnockDown(enemyType);

								break;
							}
						}
			
					}
				}
			} else {
				if(state != EnemyState.INITIAL) {
					LowerPlayerFollowers();
					state = EnemyState.GO_HOME;
				}
			}
		}
	}

	private float GetPlayerColorDistance() {
		ColourBarController barController = guiManager.GetComponent<ColourBarController>();

		Vector3 playerColorVector = new Vector3(barController.fRed, barController.fGreen, barController.fBlue);
		Vector3 enemyColorVector = new Vector3(enemyColor.r*255.0f, enemyColor.g*255.0f, enemyColor.b*255.0f);
		float d = Vector3.Distance(playerColorVector, enemyColorVector);

		return d;
	}

	private void UpdateTarget() {
		switch(state) {
			case(EnemyState.GO_HOME):
				
				// if we are close to home, just stop
				// this way everytime the NPC will be back 
				// closer to his initial position, but not exactly on it

				if(agent.remainingDistance < minHomeDistance) {
					isMoving = false;
					agent.Stop ();
				} else {
					isMoving = true;
					agent.SetDestination(startingPosition);
				}
			break;

			case(EnemyState.CHASE_PLAYER):
				isMoving = true;
				agent.SetDestination(player.transform.position);
			break;
			case(EnemyState.CHASE_NPC):
				isMoving = true;
				agent.SetDestination(enemyTarget.position);
			break;
		}

	}

	// Update is called once per frame
	void Update () {
		UpdateFaceOrientation();
		UpdateState();
		UpdateTarget();
		/*
		if(agent.hasPath) {
			Debug.Log("Drawing line from "+transform.position.ToString()+" to "+agent.transform.position.ToString());
			Debug.DrawLine(transform.position, agent.transform.position,Color.magenta,0,false);
		}

*/
	}
}
