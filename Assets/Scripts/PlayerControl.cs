using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	Vector3 mouseLastPos = Vector3.zero;
	float mouseDeltaPosX;
	float mouseDeltaPosY;
	float inputPrecision = 0.5f;
	RaycastHit hitInfo;
	public bool isDragging;
	float timeMouseUp;
	float initVelocityX;
	float initVelocityY;
	float velocityX;
	float velocityY;
	float inertiaDuration = 1.5f;
	enum spinState {down, up};

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Ray rayOrigin = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (rayOrigin, out hitInfo) && hitInfo.collider.tag == "Planet") {
				mouseLastPos = Input.mousePosition;
				isDragging = true;
			}
		}
		if (Input.GetMouseButton(0) && isDragging) {
			mouseDeltaPosX = Input.mousePosition.x - mouseLastPos.x;
			mouseDeltaPosY = Input.mousePosition.y - mouseLastPos.y;
			hitInfo.collider.transform.Rotate (mouseDeltaPosY * inputPrecision, -mouseDeltaPosX * inputPrecision, 0, Space.World);
		}
		if (Input.GetMouseButtonUp(0)) {
			initVelocityX = mouseDeltaPosX / Time.deltaTime;
			initVelocityY = mouseDeltaPosY / Time.deltaTime;
			timeMouseUp = Time.time;
			isDragging = false;

		}
		mouseLastPos = Input.mousePosition;
	}

	void FixedUpdate () {
		// Inertia
		if (!isDragging) {
			float t = (Time.time - timeMouseUp) / inertiaDuration;
			velocityX = Mathf.Lerp (initVelocityX, 0, t);
			velocityY = Mathf.Lerp (initVelocityY, 0, t);
			hitInfo.collider.transform.Rotate (Mathf.Clamp (velocityY * 0.01f, -20.0f, 20.0f), Mathf.Clamp (-velocityX * 0.01f, -20.0f, 20.0f), 0, Space.World);
		}
	}
}
