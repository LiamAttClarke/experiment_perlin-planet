using UnityEngine;
using System.Collections;

namespace Universe {
	public class Plant : MonoBehaviour {
		Material treeMat, tree1, tree2, tree3;
		// Use this for initialization
		void Start () {
			tree1 = (Material)Resources.Load ("Materials/Tree_01");
			tree2 = (Material)Resources.Load ("Materials/Tree_02");
			SetMaterial ();
			transform.LookAt (transform.position * 2);
			if (transform.position.magnitude < 0.98f) {
				transform.localScale = Vector2.zero;
			}
		}

		void SetMaterial () {
			int rand = Random.Range (0, 3);
			switch (rand) {
			case 1:
				treeMat = tree1;
				break;
			case 2:
				treeMat = tree2;
				break;
			default:
				treeMat = tree1;
				break;
			}
			gameObject.GetComponent<MeshRenderer> ().material = treeMat;
		}
		void Move () {
			RaycastHit hitInfo;
			if (Physics.Raycast (transform.position * 1.1f, -transform.forward, out hitInfo)) {
				if (hitInfo.collider.tag == "Terrain") {
					//transform.position *= hitInfo.point.magnitude;
				}
			}
		}
	}
}

