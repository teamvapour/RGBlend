using UnityEngine;
using System.Collections;

public class ColourBarController : MonoBehaviour {
	
	
	public Texture RedTexture;
	public Texture GreenTexture;
	public Texture BlueTexture;
	public Texture WhiteTexture;
	public Texture FrameTexture;
	public GameObject PlayerShadow;
	
	
	public float fRed = 100;
	public float fGreen = 100;
	public float fBlue = 100;
	
	public float fBarSpeed = 2;
	public float fColorStickiness = 0.1f; // how much the minimum gets bumped.... about 1/20 of fBarSpeed?
	public float fBarHeight = 20;
	
	public float fRedMin = 0.0f;
	public float fBlueMin = 0.0f;
	public float fGreenMin = 0.0f;
	
	public Transform spotlight;
	float fBaseBoost = 20.0f;
	float fCooldown = 50.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButton ("Fire1")) {
			fRed += fBarSpeed;
			fRedMin += fColorStickiness;
			}
		else
		{
			fRed -= fBarSpeed;
		}
		
		if (Input.GetButton ("Fire2")) {
			fGreen += fBarSpeed;
			fGreenMin += fColorStickiness;
		}
		else
		{
			fGreen -= fBarSpeed;

		}
		if (Input.GetButton ("Fire3")) {
			fBlue += fBarSpeed;
			fBlueMin += fColorStickiness;
		}
		else
		{
			fBlue -= fBarSpeed;
		}
		
		
		fRed = Mathf.Clamp(fRed, fRedMin, 255.0f);
		fBlue = Mathf.Clamp(fBlue, fBlueMin, 255.0f);
		fGreen = Mathf.Clamp(fGreen, fGreenMin, 255.0f);
		
		Color mixColor = new Color(fRed/255.0f,(fGreen/255.0f),(fBlue/255.0f), (fRed+fGreen+fBlue)
		                           /(3*255.0f));
		PlayerShadow.renderer.material.color = mixColor;

	}

	public void ColourBaseBoost(EnemyType et) {
		switch(et)
		{
		case EnemyType.ENEMY_HIPPY:
			fGreenMin += fBaseBoost;
			break;
			
		case EnemyType.ENEMY_POLICE:
			fBlueMin += fBaseBoost;
			break;
			
		case EnemyType.ENEMY_RIOT:
			fRedMin += fBaseBoost;
			break;
			
		}

		fRedMin = Mathf.Clamp(fRedMin,0.0f,255.0f);
		fGreenMin = Mathf.Clamp(fGreenMin,0.0f,255.0f);
		fBlueMin = Mathf.Clamp(fBlueMin, 0.0f,255.0f);

		fRed = Mathf.Clamp(fRed, fRedMin, 255.0f);
		fBlue = Mathf.Clamp(fBlue, fBlueMin, 255.0f);
		fGreen = Mathf.Clamp(fGreen, fGreenMin, 255.0f);
	}

	public void ColourKnockDown(EnemyType et) {
		switch(et)
		{
		case EnemyType.ENEMY_HIPPY:
			fGreen -= fCooldown;

			break;

		case EnemyType.ENEMY_POLICE:
			fBlue -= fCooldown;
			break;

		case EnemyType.ENEMY_RIOT:
			fRed -= fCooldown;

			break;

		}
		fRed = Mathf.Clamp(fRed, fRedMin, 255.0f);
		fBlue = Mathf.Clamp(fBlue, fBlueMin, 255.0f);
		fGreen = Mathf.Clamp(fGreen, fGreenMin, 255.0f);

	}



	
	void OnGUI() {
		float fBarSpacing = fBarHeight/4;
		
		Color mixColor = new Color(fRed/255.0f,(fGreen/255.0f),(fBlue/255.0f));
		Color curColour;
		float top = 12.5f;
		float left = 12.5f;
		
		GUI.DrawTexture(new Rect(left,top,fRed,fBarHeight),RedTexture);
		top += fBarHeight;
		top += fBarSpacing;
		GUI.DrawTexture(new Rect(left,top,fGreen,fBarHeight),GreenTexture);
		top += fBarHeight;
		top += fBarSpacing;
		GUI.DrawTexture(new Rect(left,top,fBlue,fBarHeight),BlueTexture);
		top += fBarHeight;
		top += fBarSpacing;
		
		curColour = GUI.color;
		GUI.color = mixColor;
		//		GUI.DrawTexture(new Rect(10,top,100,fBarHeight), WhiteTexture);
		GUI.DrawTexture(new Rect(left-12.5f,0,FrameTexture.width, FrameTexture.height), FrameTexture);
		
		GUI.color = curColour;
		top += fBarHeight;
		
		
	}
}
