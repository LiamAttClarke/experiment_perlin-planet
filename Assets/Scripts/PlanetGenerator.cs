using UnityEngine;
using System.Collections;

namespace Universe {
	public class PlanetGenerator : MonoBehaviour {
		public float scale = 0.5f;
		public float amplitude = 2.0f;
		// Use this for initialization
		void Start () {
			GeneratePlanetMesh ();
		}
		
		// Update is called once per frame
		void Update () {

		}

		void GeneratePlanetMesh () {
			int seed = Random.Range (0, 100);
			Mesh planetMesh = gameObject.GetComponent<MeshFilter> ().mesh;
			Vector3[] vertices = planetMesh.vertices;
			PerlinNoise perlinNoise = new PerlinNoise (seed);
			for (int i = 0; i < vertices.Length; i++) {
				double vert_x = (double)vertices[i].x;
				double vert_y = (double)vertices[i].y;
				double vert_z = (double)vertices[i].z;
				float height = (amplitude * (float)perlinNoise.Noise (vert_x / scale, vert_y / scale, vert_z / scale) / 12.0f + 1.0f);
				if (height > 1) {
					height = 1;
				} else {
					height = 0.96f;
				}
				Vector3 vertPos = vertices[i] - transform.position;
				vertices[i] = vertPos * height;
				Debug.Log(height);
			}
			planetMesh.vertices = vertices;
			planetMesh.RecalculateNormals ();
			planetMesh.Optimize ();
		}
	}
}
