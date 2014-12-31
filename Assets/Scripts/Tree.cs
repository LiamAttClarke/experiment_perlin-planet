using UnityEngine;
using System.Collections;

namespace Universe {
	public class Tree : MonoBehaviour {
		
		// Use this for initialization
		void Start () {
			transform.LookAt (transform.position * 2);
		}
	}
}

