﻿using UnityEngine;

public class PlayerControl : MonoBehaviour {
	public float InnerValue = 0.5f;
	public float MiddleValue = 1f;
	public float OuterValue = 1.5f;

	public float ChangeSpeed = 2f;

	public RingType StartingRing = RingType.Inner;

	public RingType _currentRing {
		get;
		private set;
	}

	private float startTime;
	private float journeyLength;

	void Start() {
		_currentRing = StartingRing;
		startTime = Time.time;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			startTime = Time.time;
			switch (_currentRing) {
			case RingType.Inner:
				_currentRing = RingType.Middle;
				break;
			case RingType.Middle:
				_currentRing = RingType.Outer;
				break;
			case RingType.Outer:
				_currentRing = RingType.Inner;
				break;
			default:
				Debug.LogError ("Error, Ring Type unknown");
				break;
			}
		}

		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			startTime = Time.time;
			switch (_currentRing) {
			case RingType.Inner:
				_currentRing = RingType.Outer;
				break;
			case RingType.Middle:
				_currentRing = RingType.Inner;
				break;
			case RingType.Outer:
				_currentRing = RingType.Middle;
				break;
			default:
				Debug.LogError ("Error, Ring Type unknown");
				break;
			}
		}
	}

	float yVal;

	void FixedUpdate () {
		if (GameManager.hasStarted) {
			float distCovered = (Time.time - startTime) * ChangeSpeed;
			float fracJourney = 0;

			switch (_currentRing) {
			case RingType.Inner:
				journeyLength = Mathf.Abs(InnerValue - yVal);
				fracJourney = distCovered / journeyLength;
				yVal = Mathf.Lerp(yVal, InnerValue, fracJourney);
				break;
			case RingType.Middle:
				journeyLength = Mathf.Abs(MiddleValue - yVal);
				fracJourney = distCovered / journeyLength;
				yVal = Mathf.Lerp(yVal, MiddleValue, fracJourney);
				break;
			case RingType.Outer:
				journeyLength = Mathf.Abs(OuterValue - yVal);
				fracJourney = distCovered / journeyLength;
				yVal = Mathf.Lerp(yVal, OuterValue, fracJourney);
				break;
			default:
				Debug.LogError ("Error, Ring Type unknown");
				break;
			}

			transform.position = transform.up * yVal;
		}
	}
}

public enum RingType {
	Inner,
	Middle,
	Outer
}