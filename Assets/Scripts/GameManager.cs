using UnityEngine;

public class GameManager : MonoBehaviour {

	public PlayerControl Player;
	public Spin MainSpinner;
	public SpinSpeeds Speeds = new SpinSpeeds(180, 90, 45);
	public Pickup GamePickup;

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
		
		GamePickup.OnDetectPass = (bool left) => {
			Debug.Log("Passed");
		};
	}

	void Update () {
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