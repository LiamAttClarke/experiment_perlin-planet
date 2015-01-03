using UnityEngine;
using System.Collections;

namespace Universe {
	public class PlayerControl : MonoBehaviour {

		float mouseDeltaPosX, mouseDeltaPosY, timeMouseUp, initVelocityX, initVelocityY, velocityX, velocityY;
		float inputPrecision = 0.25f;
		float inertiaDuration = 1.5f;
		Vector3 mouseLastPos = Vector3.zero;
		RaycastHit hitInfo;
		bool isDragging, isReleased;
		GameObject planet;

		void Start () {
			planet = GameObject.Find ("Planet");
		}

		void Update () {
			if (Input.GetMouseButton(1)) {
				RaiseLand ();
			}
			if (Input.GetMouseButtonDown(0)) {
				Ray rayOrigin = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (rayOrigin, out hitInfo) && hitInfo.collider.tag == "Planet") {
					mouseLastPos = Input.mousePosition;
					isDragging = true;
					isReleased = false;
				}
			}
			if (Input.GetMouseButton(0) && isDragging) {
				mouseDeltaPosX = Input.mousePosition.x - mouseLastPos.x;
				mouseDeltaPosY = Input.mousePosition.y - mouseLastPos.y;
				planet.transform.Rotate (mouseDeltaPosY * inputPrecision, -mouseDeltaPosX * inputPrecision, 0, Space.World);
			}
			if (Input.GetMouseButtonUp(0)) {
				initVelocityX = mouseDeltaPosX / Time.deltaTime;
				initVelocityY = mouseDeltaPosY / Time.deltaTime;
				timeMouseUp = Time.time;
				isDragging = false;
				isReleased = true;
			}
			mouseLastPos = Input.mousePosition;
		}
		
		void FixedUpdate () {
			// Inertia
			if (isReleased) {
				float t = (Time.time - timeMouseUp) / inertiaDuration;
				velocityX = Mathf.Lerp (initVelocityX, 0, t);
				velocityY = Mathf.Lerp (initVelocityY, 0, t);
				planet.transform.Rotate (Mathf.Clamp (velocityY * 0.01f, -20.0f, 20.0f), Mathf.Clamp (-velocityX * 0.01f, -20.0f, 20.0f), 0, Space.World);
			}
			if (velocityX == 0 && velocityY == 0) {
				isReleased = false;
			}
		}
		void RaiseLand () {
			RaycastHit[] rayCastAll = Physics.RaycastAll (Camera.main.ScreenPointToRay (Input.mousePosition));
			for (int i = 0; i < rayCastAll.Length; i++) {
				if (rayCastAll[i].collider.tag == "Terrain") {
					// terraform
				}
			}
		}
	}
}
