using UnityEngine;
using System.Collections;

namespace Universe {
	public class Plant : MonoBehaviour {
		GameObject terrain;
		Mesh terrainMesh;
		public int vertexNum = 0;
		// Use this for initialization
		void Start () {
			transform.LookAt (transform.position * 2);
			terrain = GameObject.Find ("Terrain");
			terrainMesh = terrain.GetComponent<MeshFilter> ().mesh;
			MovePlant ();
		}

		void Update () {
			if (Input.GetMouseButton (1)) {
				MovePlant ();
			}
		}

		void MovePlant () {
			transform.position = transform.position.normalized * terrainMesh.vertices[vertexNum].magnitude;
		}
	}
}

