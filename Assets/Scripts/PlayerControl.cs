using UnityEngine;
using System.Collections;

namespace Universe {
	public class PlayerControl : MonoBehaviour {

		float zoom, mouseDeltaPosX, mouseDeltaPosY, timeMouseUp, initVelocityX, initVelocityY, velocityX, velocityY;
		float terrainMult = 0.9975f;
		float inputPrecision = 0.25f;
		float inertiaDuration = 1.5f;
		Vector3 mouseLastPos = Vector3.zero;
		RaycastHit target;
		bool isDragging, isReleased;
		GameObject planet, terrain;
		Mesh terrainMesh;
		int layerMask9;
        public float toolSize = 0.25f;
        Touch touchZero, touchOne;
        float zoomSensitivity = 0.015f;
        // initialize
		void Start () {
			planet = GameObject.Find ("Planet");
			terrain = GameObject.Find ("Terrain");
			terrainMesh = terrain.GetComponent<MeshFilter> ().mesh;
		}
        // update 
		void Update () {
            // set tool
			if (Input.GetKeyDown (KeyCode.Equals)) {
				terrainMult = 1.0075f;
			}
			if (Input.GetKeyDown (KeyCode.Minus)) {
				terrainMult = 0.9975f;
			}
			if (Input.GetMouseButton(1)) {
				RaiseLand ();
			}
            float minFOV = 13.5f;
            float maxFOV = 20f;

            // scroll zoom *****REMOVE FOR FINAL PRODUCT******
            zoom  = Mathf.Clamp(GetComponent<Camera>().fieldOfView + -Input.GetAxis("Mouse ScrollWheel"), minFOV, maxFOV);
            GetComponent<Camera>().fieldOfView = zoom;

            // pinch zoom
            if (Input.touchCount == 2) {
                touchZero = Input.GetTouch(0);
                touchOne = Input.GetTouch(1);

                Vector2 prevTouchZeroPos = touchZero.position - touchZero.deltaPosition;
                Vector2 prevTouchOnePos = touchOne.position - touchOne.deltaPosition;

                float prevTouchMag = (prevTouchZeroPos - prevTouchOnePos).magnitude;
                float touchMag = (touchZero.position - touchOne.position).magnitude;
                float touchDeltaMag = prevTouchMag - touchMag;

                GetComponent<Camera>().fieldOfView = Mathf.Clamp (GetComponent<Camera>().fieldOfView + touchDeltaMag * zoomSensitivity, minFOV, maxFOV);
            }

            // move planet
			if (Input.GetMouseButtonDown(0)) {
				if (Physics.Raycast (GetComponent<Camera>().ScreenPointToRay (Input.mousePosition), out target) && target.collider.tag == "Planet") {
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
		// physics
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
        // terraform
		void RaiseLand () {
			RaycastHit hitInfo;
			layerMask9 = 1 << 9;
            if (Physics.Raycast(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hitInfo, 10.0f, layerMask9))
            {
				Vector3[] terrainVerts = terrainMesh.vertices;
				for (int i = 0; i < terrainVerts.Length; i++) {
					Vector3 hitPoint = planet.transform.InverseTransformPoint (hitInfo.point);
					if (Vector3.Distance (terrainVerts[i], hitPoint) < toolSize) {
						terrainVerts[i] *= terrainMult;
					}
                }
				terrainMesh.vertices = terrainVerts;
				terrainMesh.RecalculateNormals ();
				terrainMesh.Optimize ();
				terrain.GetComponent<MeshCollider> ().sharedMesh = terrainMesh;
			}
		}
	}
}

