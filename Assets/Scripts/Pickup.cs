using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class Pickup : MonoBehaviour {

	public float InnerValue = 0.5f;
	public float MiddleValue = 1f;
	public float OuterValue = 1.5f;

	public float CheckDistance = .5f;
	public float ChangeSpeed = 2f;

	public LayerMask WhatIsPlayer;

	[HideInInspector]
	public float startTime;

	[HideInInspector]
	public RingType _currentRing = RingType.Middle;

	float yVal;
	float journeyLength;
	float fracJourney;

	void Awake () {
		UpdatePos();
	}

	void UpdatePos() {
		int angle = UnityEngine.Random.Range(90, 340);
		transform.rotation = Quaternion.Euler(0, 0, GameManager._instance.MainSpinner.transform.localEulerAngles.z + angle);
		
		int ringTypeRandom = UnityEngine.Random.Range(0, 3);
		RingType newRing = _currentRing;
		switch (ringTypeRandom)
		{
			case 0:
				newRing = RingType.Inner;
				break;
			case 1:
				newRing = RingType.Middle;
				break;
			case 2:
				newRing = RingType.Outer;
				break;
			default:
				newRing = RingType.Inner;
				break;
		}
		
		_currentRing = newRing;
	}

	void FixedUpdate () {
		switch (_currentRing) {
		case RingType.Inner:
			yVal = InnerValue;
			break;
		case RingType.Middle:
			yVal = MiddleValue;
			break;
		case RingType.Outer:
			yVal = OuterValue;
			break;
		default:
			Debug.LogError ("Error, Ring Type unknown");
			break;
		}

		transform.position = transform.up * yVal;

		RaycastHit2D rightCheck = Physics2D.Raycast (transform.position, transform.up, CheckDistance, WhatIsPlayer);
		RaycastHit2D leftCheck = Physics2D.Raycast (transform.position, -transform.up, CheckDistance, WhatIsPlayer);

		Debug.DrawRay (transform.position, transform.up * CheckDistance, rightCheck ? Color.red : Color.green);
		Debug.DrawRay (transform.position, -transform.up * CheckDistance, leftCheck ? Color.red : Color.green);

		if (rightCheck)
			OnDetectPass.Invoke (rightCheck);

		if (leftCheck)
			OnDetectPass.Invoke (leftCheck);
	}

	public void OnCollisionEnter2D (Collision2D coll) {
		UpdatePos ();
		GameManager._instance.AddToScore ();
	}
		
	public Action<RaycastHit2D> OnDetectPass = (RaycastHit2D coll) => {
		//DoStuff
	};
}
