using UnityEngine;
using System.Collections;

namespace Universe {
	public class Terrain : MonoBehaviour {
		
		Mesh terrainMesh;
		Vector3[] terrainVerts, sphereVerts;
		PerlinNoise planetNoise;
		int planetSeed;
		double vert_x, vert_y, vert_z;
		GameObject cloudPrefab, cloud, oakTreePrefab, spruceTreePrefab, tree, treeType, planet;
		float terrainMap, cloudMap, treeMap;
		float terrainSpread = 0.75f;
		float terrainAmpl = 0.75f;
		float cloudSpread = 0.3f;
		float cloudAmpl = 0.25f;
		float treeAmpl = 5.0f;
		float treeSpread = 0.1f;
		
		// Use this for initialization
		void Start () {
			terrainMesh = gameObject.GetComponent<MeshFilter> ().mesh;
			terrainVerts = terrainMesh.vertices;
			cloudPrefab = (GameObject)Resources.Load ("Prefabs/Cloud");
			oakTreePrefab = (GameObject)Resources.Load ("Prefabs/Tree_Oak");
			spruceTreePrefab = (GameObject)Resources.Load ("Prefabs/Tree_Spruce");
			planet = GameObject.Find ("Planet");
			sphereVerts = GameObject.Find ("Icosphere").GetComponent<MeshFilter> ().mesh.vertices;
			planetSeed = Random.Range (0, 100);
			planetNoise = new PerlinNoise (planetSeed);
			GenerateMesh ();
			SetVertex ();
		}
		
		void GenerateMesh () {
			for (int i = 0; i < terrainVerts.Length; i++) {
				vert_x = (double)terrainVerts[i].x;
				vert_y = (double)terrainVerts[i].y;
				vert_z = (double)terrainVerts[i].z;
				terrainMap = Mathf.Clamp (terrainAmpl * (float)planetNoise.Noise (vert_x / terrainSpread, vert_y / terrainSpread, vert_z / terrainSpread) + 1.0f, 0.85f, 1.0f);
				terrainVerts[i] *= terrainMap;
			}
			terrainMesh.vertices = terrainVerts;
			terrainMesh.RecalculateNormals ();
			terrainMesh.Optimize ();
		}
		void SetVertex () {
			for (int i = 0; i < sphereVerts.Length; i++) {
				terrainMap = Mathf.Clamp (terrainAmpl * (float)planetNoise.Noise (sphereVerts[i].x / terrainSpread, sphereVerts[i].y / terrainSpread, sphereVerts[i].z / terrainSpread) + 1.0f, 0.85f, 1.0f);
				GenerateAtmosphere (sphereVerts[i]);
				PlantTrees (sphereVerts[i], terrainMap);
			}
		}

		void GenerateAtmosphere (Vector3 vertPos) {
			vertPos *= 1.5f;
			cloudMap = cloudAmpl * (float)planetNoise.Noise (vertPos.x / cloudSpread, vertPos.y / cloudSpread, vertPos.z / cloudSpread);
			if (cloudMap > 0.05f) {
				cloud = (GameObject)Instantiate(cloudPrefab, vertPos, Quaternion.identity);
				cloud.transform.parent = planet.transform;
			}
		}

		void PlantTrees (Vector3 vertPos, float terrainHeight) {
			vertPos *= 1.15f;
			int rand = Random.Range (0,2);
			if (rand == 0) {
				treeType = oakTreePrefab;
			} else {
				treeType = spruceTreePrefab;
			}
			treeMap = treeAmpl * (float)planetNoise.Noise (vertPos.x / treeSpread, vertPos.y / treeSpread, vertPos.z / treeSpread);
			if (terrainHeight == 1.0f && treeMap > 0.5f) {
				tree = (GameObject)Instantiate (treeType, vertPos, Quaternion.identity);
				tree.transform.parent = planet.transform;
			}
		}
	}
}