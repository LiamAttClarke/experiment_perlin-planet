using UnityEngine;
using System.Collections;

namespace Universe {
	public class Plant : MonoBehaviour {
		float posY;
		bool drop = true;
		bool remove = true;
		Animation anim;
		// Use this for initialization
		void Start () {
			transform.LookAt (transform.position * 2);
			MovePlant ();
			if (transform.position.magnitude < 0.98f) {
				transform.localScale = Vector2.zero;
			}
			posY = transform.position.y;
			anim = gameObject.GetComponent<Animation> ();
		}

		void Update () {
			if (Time.time > (-posY + 1.0f) && drop) {
				anim.Play ("Drop");
				drop = false;
			}
			if (transform.position.magnitude < 0.98f && remove) {
				anim.Play ("Remove");
				remove = false;
			}
			if (Input.GetMouseButton (1)) {
				MovePlant ();
			}
		}

		void MovePlant () {
			//transform.position = transform.position.normalized * terrainMesh.vertices[vertexNum].magnitude;
		}
	}
}

