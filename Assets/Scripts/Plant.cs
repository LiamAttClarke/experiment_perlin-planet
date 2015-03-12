using UnityEngine;
using System.Collections;

namespace Universe {
	public class Plant : MonoBehaviour {
		// Use this for initialization
		void Start () {
			transform.LookAt (transform.position * 2);
			if (transform.position.magnitude < 0.98f) {
				transform.localScale = Vector2.zero;
			}
			Move ();
		}

		void Update () {
			if (Input.GetMouseButton (1)) {
				Move ();
			}
		}

		void Move () {
			RaycastHit hitInfo;
			if (Physics.Raycast (transform.position, -transform.forward, out hitInfo)) {
				if (hitInfo.collider.tag == "Terrain" && hitInfo.distance < 0.5f) {
					if (hitInfo.point.magnitude > 0.97f) {
						transform.position = hitInfo.point;
					} else {
						GameObject.Destroy (gameObject);
					}
				}
			}
		}
	}
}

