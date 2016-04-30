using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public bool GodMode = false;

	public PlayerControl Player;
	public Spin MainSpinner;
	public SpinSpeeds Speeds = new SpinSpeeds(180, 180, 180);
	public Pickup GamePickup;

	public GameObject PausePanel;
	public GameObject ControlPanel;

	public Text ScoreDisplay;
	public string ScoreDisplayMessage = "Score: {0}\nHighScore: {1}";
	public Text HighscoreDisplay;
	public string HighScoreMessage = "Highscore: {0}";
	public Text EndGameDisplay;
	public string EndGameMessage = "You Scored {0}!\nTap to Play Again";
	int currentScore = 0;
	int highscore = 10;

	bool hasPlayedThisSession;

	public static bool isPlaying = false;

	public static GameManager _instance;

	public static Vector3 SpinnerUp {
		get { 
			return _instance.MainSpinner.transform.up;
		}
	}

	public static Vector3 SpinnerRight { 
		get { 
			return _instance.MainSpinner.transform.right;
		}
	}

	void Awake () {
		if (_instance == null) {
			_instance = this;
		} else {
			Destroy (gameObject);
		}

		if (Player == null) {
			Debug.LogError ("Player is not assigned in the game manager, destroying...");
			Destroy (this.gameObject);
		}

		if (MainSpinner == null) {
			Debug.LogError ("Main Spinner is not assigned in the game manager, destroying...");
			Destroy (this.gameObject);
		}
		
		if (GamePickup == null) {
			Debug.LogError ("Game Pickup is not assigned in the game manager, destroying...");
			Destroy (this.gameObject);
		}

		if (PausePanel == null) {
			Debug.LogError ("PausePanel is not assigned in the game manager, destroying...");
			Destroy (this.gameObject);
		}

		if (ControlPanel == null) {
			Debug.LogError ("Control Panel is not assigned in the game manager, destroying...");
			Destroy (this.gameObject);
		}

		if (ScoreDisplay == null) {
			Debug.LogError ("Score Display is not assigned in the game manager, destroying...");
			Destroy (this.gameObject);
		}

		if (HighscoreDisplay == null) {
			Debug.LogError ("Highscore Display is not assigned in the game manager, destroying...");
			Destroy (this.gameObject);
		}

		if (EndGameDisplay == null) {
			Debug.LogError ("Highscore Display is not assigned in the game manager, destroying...");
			Destroy (this.gameObject);
		}

		highscore = PlayerPrefs.GetInt ("HS");

		
		GamePickup.OnDetectPass = (RaycastHit2D hit) => {
			PlayerControl player = hit.transform.gameObject.GetComponent<PlayerControl>();
			if (player._currentRing != GamePickup._currentRing && !GodMode) {
				isPlaying = false;
				if (currentScore > highscore) {
					highscore = currentScore;
					PlayerPrefs.SetInt("HS", highscore);
				}
				hasPlayedThisSession = true;
				MainSpinner.transform.rotation = Quaternion.identity;
			}
		};
	}

	bool touchHandled;

	void Update () {
		UpdateUI ();

		if (isPlaying) {
			switch (Player._currentRing) {
				case RingType.Inner:
					MainSpinner.Speed = Speeds.InnerSpeed;
					break;
				case RingType.Middle:
					MainSpinner.Speed = Speeds.MiddleSpeed;
					break;
				case RingType.Outer:
					MainSpinner.Speed = Speeds.OuterSpeed;
					break;
				default:
					Debug.LogError ("Error, Ring Type unknown");
					break;
			}
		} 
		else {
			if (Input.GetKeyDown(KeyCode.Space)) {
				isPlaying = true;
			}
		}
		
	}

	void OnDrawGUI () {
		GUI.Label (new Rect (new Vector2 (10, 10), new Vector2 (250, 50)), Input.touchCount.ToString());
	}

	public void AddToScore() {
		currentScore++;
	}

	public void BeginGame() {
		isPlaying = true;
	}

	public void PauseGame() {
		isPlaying = false;
	}

	public void UpdateUI () {
		PausePanel.SetActive (!isPlaying);
		ControlPanel.SetActive (isPlaying);
		ScoreDisplay.gameObject.SetActive (isPlaying);
		EndGameDisplay.gameObject.SetActive (!isPlaying && hasPlayedThisSession);

		HighscoreDisplay.text = string.Format (HighScoreMessage, highscore); 
		ScoreDisplay.text = string.Format (ScoreDisplayMessage, currentScore, highscore);
		EndGameDisplay.text = string.Format (EndGameMessage, currentScore);
	}
}

[System.Serializable]
public struct SpinSpeeds {
	public float InnerSpeed;
	public float MiddleSpeed;
	public float OuterSpeed;

	public SpinSpeeds (float _inSpeed, float _midSpeed, float _outSpeed){
		InnerSpeed = _inSpeed;
		MiddleSpeed = _midSpeed;
		OuterSpeed = _outSpeed;
	} 
}