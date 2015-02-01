using UnityEngine;
using System.Collections;

namespace Universe {
	public class Plant : MonoBehaviour {
		Material treeMat, tree1, tree2, tree3;
		float posY;
		bool drop = true;
		Animation anim;
		// Use this for initialization
		void Start () {
			tree1 = (Material)Resources.Load ("Materials/Tree_01");
			tree2 = (Material)Resources.Load ("Materials/Tree_02");
			SetMaterial ();
			transform.LookAt (transform.position * 2);
			if (transform.position.magnitude < 0.98f) {
				transform.localScale = Vector2.zero;
			}
			posY = transform.position.y;
			anim = gameObject.GetComponent<Animation> ();
		}

		void Update () {
			//Debug.DrawRay (transform.position, -transform.forward);

			if (Time.time > (-posY + 1.0f) && drop) {
				anim.Play ("Drop");
				drop = false;
			}
			if (Input.GetMouseButton (1)) {
				Move ();
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
			transform.Find ("Tree").GetComponent<MeshRenderer> ().material = treeMat;
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

