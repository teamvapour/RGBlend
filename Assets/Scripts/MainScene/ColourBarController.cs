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
	public float fBarHeight = 20;
	
	public float fRedMin = 0.0f;
	public float fBlueMin = 0.0f;
	public float fGreenMin = 0.0f;
	
	public Transform spotlight;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// off for now
		//spotlight.GetComponent<Light>().color = new Color(fRed, fGreen, fBlue);
		
		if (Input.GetButton ("Fire1")) {
			fRed += fBarSpeed;
			if(fRed > 255) fRed = 255;
		}
		else
		{
			fRed -= fBarSpeed;
			if(fRed < 0) fRed = 0;
		}
		
		if (Input.GetButton ("Fire2")) {
			fGreen += fBarSpeed;
			if(fGreen > 255) fGreen = 255;
		}
		else
		{
			fGreen -= fBarSpeed;
			if(fGreen < 0) fGreen = 0;
		}
		if (Input.GetButton ("Fire3")) {
			fBlue += fBarSpeed;
			if(fBlue > 255) fBlue = 255;
		}
		else
		{
			fBlue -= fBarSpeed;
			if(fBlue < 0) fBlue = 0;
		}
		
		
		fRed = Mathf.Clamp(fRed, fRedMin, 255.0f);
		fBlue = Mathf.Clamp(fBlue, fBlueMin, 255.0f);
		fGreen = Mathf.Clamp(fGreen, fGreenMin, 255.0f);
		
		Color mixColor = new Color(fRed/255.0f,(fGreen/255.0f),(fBlue/255.0f), (fRed+fGreen+fBlue)
		                           /(3*255.0f));
		PlayerShadow.renderer.material.color = mixColor;
		
		
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
