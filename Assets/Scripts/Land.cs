using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Universe {
	public class Land : MonoBehaviour {

		Mesh terrainMesh;
		Vector3[] terrainVerts;
		PerlinNoise planetNoise;
		int planetSeed;
		double vert_x, vert_y, vert_z;
		float terrainMap;
		// Land
		float terrainAmpl = 0.75f;
		float terrainSpread = 0.75f;

		// Use this for initialization
		void Start () {

			// Terrain mesh
			terrainMesh = gameObject.GetComponent<MeshFilter> ().mesh;
			terrainVerts = terrainMesh.vertices;

			// Noise
			planetSeed = Random.Range (0, 100);
			planetNoise = new PerlinNoise (planetSeed);

			// Generate Land
			GenerateTerrain ();
		}
		
		void GenerateTerrain () {

			// Modulate terrain
			for (int i = 0; i < terrainVerts.Length; i++) {
				vert_x = (double)terrainVerts[i].x;
				vert_y = (double)terrainVerts[i].y;
				vert_z = (double)terrainVerts[i].z;
				// Perlin Noise for terrain elevation
				terrainMap = Mathf.Clamp (terrainAmpl * (float)planetNoise.Noise (vert_x / terrainSpread, vert_y / terrainSpread, vert_z / terrainSpread) + 1.0f, 0.85f, 1.0f);
				terrainVerts[i] *= terrainMap;
			}

			// Recalculate Mesh
			terrainMesh.vertices = terrainVerts;
			terrainMesh.RecalculateNormals ();
			terrainMesh.Optimize ();

			gameObject.GetComponent<MeshCollider> ().sharedMesh = null;
			gameObject.GetComponent<MeshCollider> ().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
		}
	}
}