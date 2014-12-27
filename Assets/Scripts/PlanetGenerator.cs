using UnityEngine;
using System.Collections;

namespace Universe {
	public class PlanetGenerator : MonoBehaviour {
		public float scale = 0.5f;
		public float amplitude = 12.0f;
		int terrainSeed;
		int atmosphereSeed;
		float rotateSpeed = 1.0f;
		// Use this for initialization
		void Start () {
			atmosphereSeed = Random.Range (0, 100);
			terrainSeed = Random.Range (0, 100);
			GeneratePlanetMesh ();
		}
		
		// Update is called once per frame
		void Update () {
			if (Input.GetKey (KeyCode.LeftArrow)) {
				transform.Rotate (new Vector3 (0, 0, rotateSpeed));
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				transform.Rotate (new Vector3 (0, 0, -rotateSpeed));
			}
		}

		void GeneratePlanetMesh () {
			Mesh planetMesh = gameObject.GetComponent<MeshFilter> ().mesh;
			Vector3[] vertices = planetMesh.vertices;
			PerlinNoise terrainNoise = new PerlinNoise (terrainSeed);
			for (int i = 0; i < vertices.Length; i++) {
				double vert_x = (double)vertices[i].x;
				double vert_y = (double)vertices[i].y;
				double vert_z = (double)vertices[i].z;
				float terrainMap = amplitude * (float)terrainNoise.Noise (vert_x / scale, vert_y / scale, vert_z / scale) / 12.0f + 1.0f;
				if (terrainMap > 1.0f) {
					terrainMap = 1.0f;
				} else {
					terrainMap = 0;
				}
				vertices[i] -= transform.position;
				vertices[i] *= terrainMap;
				GenerateAtmosphere (vertices[i]);
			}
			planetMesh.vertices = vertices;
			planetMesh.RecalculateNormals ();
			planetMesh.Optimize ();
		}


		void GenerateAtmosphere (Vector3 skyPoint) {
			GameObject cloudPrefab = (GameObject)Resources.Load ("Prefabs/Cloud");
			skyPoint *= 1.25f;
			PerlinNoise atmosphereNoise = new PerlinNoise (atmosphereSeed);
			float cloudSpread = 0.25f;
			float cloudMap = amplitude * (float)atmosphereNoise.Noise (skyPoint.x / cloudSpread, skyPoint.y / cloudSpread, skyPoint.z / cloudSpread) / 12.0f + 1.0f;
			if (cloudMap > 1.1f) {
				GameObject cloud = (GameObject)Instantiate(cloudPrefab, skyPoint, Quaternion.identity);
				cloud.transform.parent = transform;
			}
		}
	}
}
