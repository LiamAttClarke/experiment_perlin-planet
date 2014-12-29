using UnityEngine;
using System.Collections;

namespace Universe {
	public class Planet : MonoBehaviour {
		
		Mesh planetMesh;
		Vector3[] terrainVerts;
		PerlinNoise terrainNoise, atmosphereNoise;
		int terrainSeed, atmosphereSeed;
		double vert_x, vert_y, vert_z;
		GameObject cloudPrefab;
		GameObject cloud;
		float terrainMap, cloudMap;
		float terrainSpread = 0.75f;
		float cloudSpread = 0.25f;
		float amplitude = 12.0f;
		float rotateSpeed = 1.0f;
		public bool isDragging = false;

		// Use this for initialization
		void Start () {
			planetMesh = gameObject.GetComponent<MeshFilter> ().mesh;
			terrainVerts = planetMesh.vertices;
			cloudPrefab = (GameObject)Resources.Load ("Prefabs/Cloud");
			atmosphereSeed = Random.Range (0, 100);
			terrainSeed = Random.Range (0, 100);
			GenerateMesh ();
		}

		Vector3 mouseDownPos = Vector3.zero;
		// Update is called once per frame
		void Update () {
			float hitDist = 0;
			if (Input.GetMouseButtonDown(0)) {
				Ray rayOrigin = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hitInfo;
				if (Physics.Raycast (rayOrigin, out hitInfo) && hitInfo.collider.tag == "Planet") {
					mouseDownPos = Input.mousePosition;
					isDragging = true;
				}
			}
			if (Input.GetMouseButton(0) && isDragging) {
				Vector3 mousePos = Input.mousePosition;
				Debug.Log (mousePos + " " + mouseDownPos);
				transform.Rotate ((mousePos.y - mouseDownPos.y) / 100, (-mousePos.x - -mouseDownPos.x) / 100, 0, Space.World);
			}
			if (Input.GetMouseButtonUp(0)) {
				isDragging = false;
			}
		}
		
		void GenerateMesh () {
			terrainNoise = new PerlinNoise (terrainSeed);
			for (int i = 0; i < terrainVerts.Length; i++) {
				vert_x = (double)terrainVerts[i].x;
				vert_y = (double)terrainVerts[i].y;
				vert_z = (double)terrainVerts[i].z;
				terrainMap = amplitude * (float)terrainNoise.Noise (vert_x / terrainSpread, vert_y / terrainSpread, vert_z / terrainSpread) / 12.0f + 1.0f;
				if (terrainMap > 1.0f) {
					terrainMap = 1.0f;
				} else {
					terrainMap = 0;
				}
				terrainVerts[i] -= transform.position;
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
			cloudMap = amplitude * (float)atmosphereNoise.Noise (skyPoint.x / cloudSpread, skyPoint.y / cloudSpread, skyPoint.z / cloudSpread) / 12.0f + 1.0f;
			if (cloudMap > 1.1f) {
				cloud = (GameObject)Instantiate(cloudPrefab, skyPoint, Quaternion.identity);
				cloud.transform.parent = transform;
			}
		}
	}
}