using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	private const float menuWidth = 300f;
	private const float menuHeight = 300f;

	private const float buttonWidth = 150f;
	private const float buttonHeight = 50f;
	private const float buttonDistance = 10f;
	
	private Rect mainMenu;
	private bool isPaused = false;

	public string TitleScreenName = "TitleScreen";
	public string GameScreenName = "TestPauseMenu";

	// Use this for initialization
	void Start () {
		float menuX = (Screen.width - menuWidth) * 0.5f;
		float menuY = (Screen.height - menuHeight) * 0.5f;
		mainMenu = new Rect(menuX, menuY, menuWidth, menuHeight);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("escape pressed");
		if(Input.GetKeyDown(KeyCode.Escape)) {
			isPaused = !isPaused;

			if(isPaused) {
				Time.timeScale = 0;

			}
			else {
				Time.timeScale = 1;
			}

		}

	}


	void OnGUI() {

		if(isPaused) {
			GUI.Window(0, mainMenu, displayMenu, "Menu");
		}

	}


	void displayMenu(int id) {

		float btnStartOffset = 50;

		if(GUI.Button(new Rect((menuWidth - buttonWidth) * 0.5f, btnStartOffset, buttonWidth, buttonHeight), "Resume")) {
			Time.timeScale = 1;
			isPaused = false;
		}

		if(GUI.Button(new Rect((menuWidth - buttonWidth) * 0.5f, btnStartOffset + buttonHeight + buttonDistance, buttonWidth, buttonHeight), "Restart")) {
			Time.timeScale = 1;
			Application.LoadLevel(GameScreenName);
		}

		if(GUI.Button(new Rect((menuWidth - buttonWidth) * 0.5f, btnStartOffset + buttonHeight*2 + buttonDistance*2, buttonWidth, buttonHeight), "Quit")) {
			Time.timeScale = 1;
			Application.LoadLevel(TitleScreenName);
		}

	}

}
