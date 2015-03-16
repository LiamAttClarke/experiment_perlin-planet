using UnityEngine;
using System.Collections;

namespace Universe {
	public class Cloud : MonoBehaviour {

		// Use this for initialization
		void Start () {
			GameObject planet = GameObject.Find("Planet");
			transform.LookAt (transform.position * 2);
		}
	}
}
