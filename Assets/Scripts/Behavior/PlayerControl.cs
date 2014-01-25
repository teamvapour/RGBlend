
using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


	public float speed = 100f;

	private float lastHTranslation = 0.0f;
	private float lastVTranslation = 0.0f;

	private float lastRotation = 0.0f;

	public int numberOfFollowers = 0;

	public float enemyRadius = 1000.0f;
	public float enemyRadiusMin = 1000.0f;
	public float enemyRadiusMax = 3000.0f; 

	// Use this for initialization
	void Start () {
	
	}


	// base the radius on how many enemies are chasing the player
	public float GetEnemyRadius() {
		float radius =  enemyRadius + 200.0f*(numberOfFollowers+1);
		radius = Mathf.Clamp(radius, enemyRadiusMin, enemyRadiusMax);
		return radius;
	}

	void ScanForEnemies() {
	//	GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");


	}
	
	// Update is called once per frame
	void Update () {
		float hTranslation = Input.GetAxis("Horizontal");
		float vTranslation = Input.GetAxis("Vertical");

		Vector3 vel = rigidbody.velocity;
		vel.y = 0.0f;
		vel.x = hTranslation * speed * Time.deltaTime;
		vel.z = vTranslation * speed * Time.deltaTime;

		Vector3 velN = vel.normalized;

		if((Mathf.Abs(hTranslation) < 0.001f) && (Mathf.Abs(vTranslation) < 0.001f)) {
			rigidbody.velocity = Vector3.zero;
			transform.eulerAngles = new Vector3(90,(lastRotation*180.0f)/Mathf.PI,0);
		} else {
			float rotation = Mathf.Atan2(velN.x, velN.z);
			rigidbody.velocity = vel;
			transform.eulerAngles = new Vector3(90,(rotation*180.0f)/Mathf.PI,0);
			lastRotation = rotation;
		}

	}
	
	void OnCollisionEnter(Collision col) {
	//	Debug.Log ("Bang!");
	}
}

