using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Universe {
	public class Terrain : MonoBehaviour {

		Mesh terrainMesh;
		Vector3[] terrainVerts;
		PerlinNoise planetNoise;
		int planetSeed, randTree;
		double vert_x, vert_y, vert_z;
		GameObject cloudPrefab, cloud, treePrefab, tree, planet;
		float terrainMap, cloudMap, treeMap;
		List<Vector3> vertList;
		// Terrain
		float terrainSpread = 0.75f;
		float terrainAmpl = 0.75f;
		// Atmosphere
		float cloudSpread = 0.3f;
		float cloudAmpl = 0.25f;
		// Surface objects
		float treeAmpl = 5.0f;
		float treeSpread = 0.1f;

		// Use this for initialization
		void Start () {
			// Initialize gameObjects
			cloudPrefab = (GameObject)Resources.Load ("Prefabs/Cloud");
			treePrefab = (GameObject)Resources.Load ("Prefabs/Tree");
			planet = GameObject.Find ("Planet");
			// Terrain mesh
			terrainMesh = gameObject.GetComponent<MeshFilter> ().mesh;
			terrainVerts = terrainMesh.vertices;
			vertList = new List<Vector3> ();
			// Noise
			planetSeed = Random.Range (0, 100);
			planetNoise = new PerlinNoise (planetSeed);

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
			// Add all terrain vertices to a list
			for (int i = 0; i < terrainVerts.Length; i++) {
				vertList.Add (terrainVerts[i]);
			}
			// Remove redundant/ overlapping vertices from list
			vertList = vertList.Distinct().ToList();
			// Place ojects and clouds per vertex
			for (int i = 0; i < vertList.Count; i++) {
				GenerateAtmosphere (vertList[i]);
				PlantTrees (vertList[i]);
			}
			// Recalculate Mesh
			terrainMesh.vertices = terrainVerts;
			terrainMesh.RecalculateNormals ();
			terrainMesh.Optimize ();
		}

		void GenerateAtmosphere (Vector3 vertPos) {
			// Place cloud at a fixed distance from planet
			vertPos *= 1.5f;
			// Perlin Noise for cloud distrubution
			cloudMap = cloudAmpl * (float)planetNoise.Noise (vertPos.x / cloudSpread, vertPos.y / cloudSpread, vertPos.z / cloudSpread);
			if (cloudMap > 0.05f) {
				cloud = (GameObject)Instantiate(cloudPrefab, vertPos, Quaternion.identity);
				cloud.transform.parent = planet.transform;
			}
		}

		void PlantTrees (Vector3 vertPos) {
			// Perlin Noise for tree distribution
			treeMap = treeAmpl * (float)planetNoise.Noise (vertPos.x / treeSpread, vertPos.y / treeSpread, vertPos.z / treeSpread);
			if (treeMap > 0.5f) {
				tree = (GameObject)Instantiate (treePrefab, vertPos, Quaternion.identity);
				tree.transform.parent = planet.transform;
			}
		}
	}
}