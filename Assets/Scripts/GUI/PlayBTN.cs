using UnityEngine;
using System.Collections;

public class PlayBTN : MonoBehaviour {

	public Texture2D controlTexture;

	public string GameSceneName = "TestPauseMenu";

	void OnGUI () {

		if (GUI.Button (new Rect (356, 360, 294, 82), controlTexture, new GUIStyle())) {
			Application.LoadLevel(GameSceneName);
		}

	}


}
