using UnityEngine;
using System.Collections;

namespace Universe {
	public class Terrain : MonoBehaviour {
		
		Mesh planetMesh;
		Vector3[] terrainVerts;
		PerlinNoise planetNoise;
		int planetSeed;
		double vert_x, vert_y, vert_z;
		GameObject cloudPrefab, cloud, treePrefab, tree;
		float terrainMap, cloudMap, treeMap;
		float terrainSpread = 0.75f;
		float terrainAmpl = 2.0f;
		float cloudSpread = 0.3f;
		float cloudAmpl = 0.25f;
		float treeAmpl = 5.0f;
		float treeSpread = 0.1f;
		
		// Use this for initialization
		void Start () {
			planetMesh = gameObject.GetComponent<MeshFilter> ().mesh;
			terrainVerts = planetMesh.vertices;
			cloudPrefab = (GameObject)Resources.Load ("Prefabs/Cloud");
			treePrefab = (GameObject)Resources.Load ("Prefabs/Tree");
			planetSeed = Random.Range (0, 100);
			planetNoise = new PerlinNoise (planetSeed);
			GenerateMesh ();
		}
		
		void GenerateMesh () {
			for (int i = 0; i < terrainVerts.Length; i++) {
				vert_x = (double)terrainVerts[i].x;
				vert_y = (double)terrainVerts[i].y;
				vert_z = (double)terrainVerts[i].z;
				terrainMap = Mathf.Clamp (terrainAmpl * (float)planetNoise.Noise (vert_x / terrainSpread, vert_y / terrainSpread, vert_z / terrainSpread) + 1.0f, 0.9f, 1.0f);
				terrainVerts[i] *= terrainMap;
				GenerateAtmosphere (terrainVerts[i]);
				PlantTrees (terrainVerts[i], terrainMap);
			}
			planetMesh.vertices = terrainVerts;
			planetMesh.RecalculateNormals ();
			planetMesh.Optimize ();
		}

		void GenerateAtmosphere (Vector3 vertPos) {
			vertPos *= 1.5f;
			cloudMap = cloudAmpl * (float)planetNoise.Noise (vertPos.x / cloudSpread, vertPos.y / cloudSpread, vertPos.z / cloudSpread);
			if (cloudMap > 0.05f) {
				cloud = (GameObject)Instantiate(cloudPrefab, vertPos, Quaternion.identity);
				cloud.transform.parent = transform;
			}
		}

		void PlantTrees (Vector3 vertPos, float terrainHeight) {
			vertPos *= 1.15f;
			treeMap = treeAmpl * (float)planetNoise.Noise (vertPos.x / treeSpread, vertPos.y / treeSpread, vertPos.z / treeSpread);
			if (terrainHeight == 1.0f && treeMap > 0.5f) {
				tree = (GameObject)Instantiate (treePrefab, vertPos, Quaternion.identity);
				tree.transform.parent = transform;
			}
		}
	}
}