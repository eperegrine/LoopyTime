using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

	#if UNITY_ADS // If the Ads service is enabled...
	public bool ShowAds = true;
	public int GamesTillAdIsShown = 1;
	int gameCount = 0;
	#endif

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
	int lastScore = 0;

	bool hasPlayedThisSession;

	public static bool isPlaying = false;
	private static bool isCurrentGameBeingPlayed = false;

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

		#if !UNITY_EDITOR
		GodMode = false;
		#endif
		
		GamePickup.OnDetectPass = (RaycastHit2D hit) => {
			PlayerControl player = hit.transform.gameObject.GetComponent<PlayerControl>();
			if (player._currentRing != GamePickup._currentRing && !GodMode) {
				isPlaying = false;
				if (currentScore > highscore) {
					highscore = currentScore;
					PlayerPrefs.SetInt("HS", highscore);
				}
				isCurrentGameBeingPlayed = false;
				hasPlayedThisSession = true;
				lastScore = currentScore;
				currentScore = 0;
				MainSpinner.transform.rotation = Quaternion.identity;
				GamePickup.UpdatePos();

				#if UNITY_ADS
				gameCount++;
				if (GamesTillAdIsShown == gameCount) {
					ShowAd();
					gameCount = 0;
				}
				#endif
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
				BeginGame ();
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
		isCurrentGameBeingPlayed = true;
		isPlaying = true;
	}

	public void PauseGame() {
		isPlaying = false;
	}

	public void UpdateUI () {
		PausePanel.SetActive (!isPlaying);
		ControlPanel.SetActive (isPlaying);
		ScoreDisplay.gameObject.SetActive (isPlaying);
		EndGameDisplay.gameObject.SetActive (!isPlaying && hasPlayedThisSession && !isCurrentGameBeingPlayed);

		HighscoreDisplay.text = string.Format (HighScoreMessage, highscore); 
		ScoreDisplay.text = string.Format (ScoreDisplayMessage, currentScore, highscore);
		EndGameDisplay.text = string.Format (EndGameMessage, lastScore);
	}

	public void ShowAd()
	{
		#if UNITY_ADS
		if (ShowAds && Advertisement.IsReady("video"))
		{
			Advertisement.Show("video", new ShowOptions{
				resultCallback = ((ShowResult obj) => {
					Invoke("PauseGameAfterAd", 0.001f);
				})
			});
		}
		#endif
	}

	void PauseGameAfterAd() {
		isCurrentGameBeingPlayed = false;
		PauseGame();
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