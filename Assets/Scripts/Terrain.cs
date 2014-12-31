﻿using UnityEngine;
using System.Collections;

namespace Universe {
	public class Terrain : MonoBehaviour {
		
		Mesh planetMesh;
		Vector3[] terrainVerts;
		PerlinNoise terrainNoise, atmosphereNoise;
		int terrainSeed, atmosphereSeed;
		double vert_x, vert_y, vert_z;
		GameObject cloudPrefab, cloud;
		float terrainMap, cloudMap;
		float terrainSpread = 0.75f;
		float terrainAmpl = 2.0f;
		float cloudSpread = 0.25f;
		float cloudAmpl = 5.0f;
		
		// Use this for initialization
		void Start () {
			planetMesh = gameObject.GetComponent<MeshFilter> ().mesh;
			terrainVerts = planetMesh.vertices;
			cloudPrefab = (GameObject)Resources.Load ("Prefabs/Cloud");
			atmosphereSeed = Random.Range (0, 100);
			terrainSeed = Random.Range (0, 100);
			GenerateMesh ();
		}
		
		void GenerateMesh () {
			terrainNoise = new PerlinNoise (terrainSeed);
			for (int i = 0; i < terrainVerts.Length; i++) {
				vert_x = (double)terrainVerts[i].x;
				vert_y = (double)terrainVerts[i].y;
				vert_z = (double)terrainVerts[i].z;
				terrainMap = Mathf.Clamp (terrainAmpl * (float)terrainNoise.Noise (vert_x / terrainSpread, vert_y / terrainSpread, vert_z / terrainSpread) + 1.0f, 0.9f, 1.0f);
				terrainVerts[i] *= terrainMap;
				GenerateAtmosphere (terrainVerts[i]);
			}
			planetMesh.vertices = terrainVerts;
			planetMesh.RecalculateNormals ();
			planetMesh.Optimize ();
		}
		
		
		void GenerateAtmosphere (Vector3 skyPoint) {
			skyPoint *= 1.25f;
			atmosphereNoise = new PerlinNoise (atmosphereSeed);
			cloudMap = cloudAmpl * (float)atmosphereNoise.Noise (skyPoint.x / cloudSpread, skyPoint.y / cloudSpread, skyPoint.z / cloudSpread) / 12.0f + 1.0f;
			if (cloudMap > 1.1f) {
				cloud = (GameObject)Instantiate(cloudPrefab, skyPoint, Quaternion.identity);
				cloud.transform.parent = transform;
			}
		}
	}
}