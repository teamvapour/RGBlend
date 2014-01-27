using UnityEngine;
using System.Collections;

public class PlayerGoal : MonoBehaviour {

	private const float endScrWidth = 300f;
	private const float endScrHeight = 300f;
	
	private const float buttonWidth = 150f;
	private const float buttonHeight = 50f;
	private const float buttonDistance = 10f;
	
	private Rect endScrRect;
	private static bool isEnded;
	private static bool isWin;
	private bool homeReject;
	
	private string TitleScreenName = "TitleScreen";
	private string GameScreenName = "TestHome";
	private string endMessage;

	// Use this for initialization
	void Start () {
		GameScreenName = Application.loadedLevelName;

		float menuX = (Screen.width - endScrWidth) * 0.5f;
		float menuY = (Screen.height - endScrHeight) * 0.5f;
		endScrRect = new Rect(menuX, menuY, endScrWidth, endScrHeight);
		isEnded = false;
		isWin = false;
		homeReject = false;
		Time.timeScale = 1;

		GameObject[] buildings = GameObject.FindGameObjectsWithTag("Buildings");

		ArrayList possibleHomes = new ArrayList();

		for (int i = 0; i < buildings.Length; i++) {
			GameObject bld = buildings[i];
			if(bld.name == "HomeRandom") {
				possibleHomes.Add(bld);
			}
		}

		int randomIndex = Random.Range(0, possibleHomes.Count -1);
		GameObject home = (GameObject) possibleHomes[randomIndex];
		home.name = "Home";
		home.renderer.material.color = Color.red;
		CameraMovement.homePosition = home.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(isEnded) {
			if(Input.GetButton("Fire1")) {
				Time.timeScale = 1;
				Application.LoadLevel(GameScreenName);
			}
		}
	}

	void OnGUI() {
		
		if(isEnded) {
			string scrTitle = isWin ? "You Have Won!" : "Game Over";

			if(isWin) {
				endMessage = "You got Home safely";
			}
			else {
				if(homeReject) {
					endMessage = "You got Rejected from your home for looking like someone else!";
				}
				else endMessage = "You got Beaten for not bellonging in the group!";
			}

			GUI.Window(2, endScrRect, displayEnd, scrTitle);
		}
		
	}

	public static void Fail() {
		Time.timeScale = 0;
		isWin = false;
		isEnded = true;
	}

	void displayEnd(int id) {
		
		float btnStartOffset = 50;

		GUI.Label(new Rect((endScrWidth - buttonWidth) * 0.5f, btnStartOffset, buttonWidth, buttonHeight), endMessage);

		if(GUI.Button(new Rect((endScrWidth - buttonWidth) * 0.5f, btnStartOffset + buttonHeight + buttonDistance, buttonWidth, buttonHeight), "Play Again?")) {
			Time.timeScale = 1;
			Application.LoadLevel(GameScreenName);
		}
		
		if(GUI.Button(new Rect((endScrWidth - buttonWidth) * 0.5f, btnStartOffset + (buttonHeight + buttonDistance)*2, buttonWidth, buttonHeight), "Quit")) {
			Time.timeScale = 1;
			Application.LoadLevel(TitleScreenName);
		}

		
	}

	void OnCollisionEnter(Collision col) {
		
		//	Debug.Log("player collided with"+col.gameObject.tag);
		
		if(col.gameObject.name.Equals("Home")) {

		//	ColourBarController controller = (ColourBarController) GameObject.Find("ColourBarController");
			GameObject guiManager = GameObject.Find ("GUIManager");
			ColourBarController barController = guiManager.GetComponent<ColourBarController>();

			if(barController.CanGetInHome()) {

				Debug.Log("You WIn");
				Time.timeScale = 0;
				isWin = true;
				isEnded = true;
			}
			else {
				homeReject = true;
				Fail();
			}
		}
	}
}
