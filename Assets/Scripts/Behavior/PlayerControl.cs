
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {


	public float speed = 100f;

	private float lastHTranslation = 0.0f;
	private float lastVTranslation = 0.0f;

	private float lastRotation = 0.0f;

	public int numberOfFollowers = 0;
	public List<Transform> npcFollowers;

	public float enemyRadius = 1000.0f;
	public float enemyRadiusMin = 1000.0f;
	public float enemyRadiusMax = 3000.0f; 

	private float textureOffset = 0.25f;
	private int numSprites = 3;

	public static bool isReady = false;

	private bool isMoving = false;



	// Use this for initialization
	void Start () {
		isReady = false;
		StartCoroutine(UpdateSpriteAnimation());
	}


	// base the radius on how many enemies are chasing the player
	public float GetEnemyRadius() {
		float radius =  enemyRadius + 200.0f*(numberOfFollowers+1);
		radius = Mathf.Clamp(radius, enemyRadiusMin, enemyRadiusMax);
		return radius;
	}

	void ScanForEnemies() {


	}

	public void addFollower(Transform o) {
		npcFollowers.Add(o);
	}

	public void removeFollower(Transform o) {
		npcFollowers.Remove(o);
	}
	
	// Update is called once per frame
	void Update () {


		float hTranslation = Input.GetAxis("Horizontal");
		float vTranslation = Input.GetAxis("Vertical");

		Debug.Log (hTranslation);

		Vector3 vel = rigidbody.velocity;
		vel.y = 0.0f;
		vel.x = hTranslation * speed * Time.deltaTime;
		vel.z = vTranslation * speed * Time.deltaTime;

		Vector3 velN = vel.normalized;

		if((Mathf.Abs(hTranslation) < 0.001f) && (Mathf.Abs(vTranslation) < 0.001f)) {
			isMoving = false;
			rigidbody.velocity = Vector3.zero;
			transform.eulerAngles = new Vector3(90,(lastRotation*180.0f)/Mathf.PI,0);
		} else {
			isMoving = true;
			float rotation = Mathf.Atan2(velN.x, velN.z);
			rigidbody.velocity = vel;
			transform.eulerAngles = new Vector3(90,(rotation*180.0f)/Mathf.PI,0);
			lastRotation = rotation;
			UpdateSpriteAnimation();
		}

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
	
	void OnCollisionEnter(Collision col) {
		
	}
}

