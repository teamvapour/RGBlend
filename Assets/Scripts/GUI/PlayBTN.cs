using UnityEngine;
using System.Collections;

public class PlayBTN : MonoBehaviour {

	public Texture2D controlTexture;
	public Texture2D mainScreen;

	public string GameSceneName = "TestHome";

	void OnGUI () {
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), mainScreen);

		if (GUI.Button (new Rect (50, 650, 294, 82), controlTexture, new GUIStyle())) {
			Application.LoadLevel(GameSceneName);
		}





		if(Input.GetButton ("Fire1")) {
			Application.LoadLevel(GameSceneName);
		}

	}

	void OnUpdate() {

		if(Input.GetButton ("Fire1")) {
			Application.LoadLevel(GameSceneName);
		}


	}
}
