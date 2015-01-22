using UnityEngine;
using System.Collections;

namespace Universe {
	public class Plant : MonoBehaviour {
		Material treeMat, treeGreen, treeYellow, treeOrange, treeRed;
		float posY;
		bool drop = true;
		bool remove = true;
		Animation anim;
		// Use this for initialization
		void Start () {
			treeGreen = (Material)Resources.Load ("Materials/Tree_Green");
			treeYellow = (Material)Resources.Load ("Materials/Tree_Yellow");
			treeOrange = (Material)Resources.Load ("Materials/Tree_Orange");
			treeRed = (Material)Resources.Load ("Materials/Tree_Red");
			SetMaterial ();
			transform.LookAt (transform.position * 2);
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
				// Move tree
			}
		}
		void SetMaterial () {
			int rand = Random.Range (0, 5);
			switch (rand) {
			case 1:
				treeMat = treeGreen;
				break;
			case 2:
				treeMat = treeYellow;
				break;
			case 3:
				treeMat = treeOrange;
				break;
			case 4:
				treeMat = treeRed;
				break;
			default:
				treeMat = treeGreen;
				break;
			}
			transform.Find ("Tree").GetComponent<MeshRenderer> ().material = treeMat;
		}
	}
}

