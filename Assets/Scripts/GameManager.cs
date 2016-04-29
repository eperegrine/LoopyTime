using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public PlayerControl Player;
	public Spin MainSpinner;
	public SpinSpeeds Speeds = new SpinSpeeds(180, 180, 180);
	public Pickup GamePickup;

	public GameObject PausePanel;
	public Button PauseButton;
	public Button MoveUpButton;
	public Button MoveDownButton;

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

		if (PauseButton == null) {
			Debug.LogError ("PausePanel is not assigned in the game manager, destroying...");
			Destroy (this.gameObject);
		}

		if (MoveUpButton == null) {
			Debug.LogError ("PausePanel is not assigned in the game manager, destroying...");
			Destroy (this.gameObject);
		}

		if (MoveDownButton == null) {
			Debug.LogError ("PausePanel is not assigned in the game manager, destroying...");
			Destroy (this.gameObject);
		}
		
		GamePickup.OnDetectPass = (RaycastHit2D hit) => {
			PlayerControl player = hit.transform.gameObject.GetComponent<PlayerControl>();
			if (player._currentRing != GamePickup._currentRing) {
				isPlaying = false;
				SceneManager.LoadScene(0, LoadSceneMode.Single);
			}
		};
	}

	void Update () {
		PausePanel.SetActive (!isPlaying);

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

	public void BeginGame() {
		isPlaying = true;
	}

	public void PauseGame() {
		isPlaying = false;
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