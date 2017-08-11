using UnityEngine;

public class Spin : MonoBehaviour {

	public float Speed = 180;

	void FixedUpdate () {
		if (GameManager.isPlaying) {
			Vector3 currentRot = transform.eulerAngles;
			float RotateAmount = Speed * Time.fixedDeltaTime;
			transform.rotation = Quaternion.Euler (currentRot.x, currentRot.y, currentRot.z + RotateAmount);
		}
	}
}
