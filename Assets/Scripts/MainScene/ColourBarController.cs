﻿using UnityEngine;
using System.Collections;

public class ColourBarController : MonoBehaviour {


	public Texture RedTexture;
	public Texture GreenTexture;
	public Texture BlueTexture;
	public Texture WhiteTexture;
	public Texture FrameTexture;
	public float fRed = 100;
	public float fGreen = 100;
	public float fBlue = 100;
	public float fBarSpeed = 2;
	public float fBarHeight = 20;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		if (Input.GetKey(KeyCode.Alpha1)) {
			fRed += fBarSpeed;
			if(fRed > 255) fRed = 255;
		}
		else
		{
			fRed -= fBarSpeed;
			if(fRed < 0) fRed = 0;
		}

		if (Input.GetKey(KeyCode.Alpha2)) {
			fGreen += fBarSpeed;
			if(fGreen > 255) fGreen = 255;
		}
		else
		{
			fGreen -= fBarSpeed;
			if(fGreen < 0) fGreen = 0;
		}
		if (Input.GetKey(KeyCode.Alpha3)) {
			fBlue += fBarSpeed;
			if(fBlue > 255) fBlue = 255;
		}
		else
		{
			fBlue -= fBarSpeed;
			if(fBlue < 0) fBlue = 0;
		}


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

		Debug.Log("Bar Height = "+top+" with spacing of "+fBarSpacing);
	}
}