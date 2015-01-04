using UnityEngine;
using System.Collections;

namespace Universe {
	public class Cloud : MonoBehaviour {

		GameObject planet;
		Vector3 planetCloudVect, planetCameraVect;
		float cloudScale;
		float cloudOverHang = 0.75f;

		// Use this for initialization
		void Start () {
			planet = GameObject.Find("Planet");
			transform.LookAt (transform.position * 2);
			int seed = Random.Range (0, 100);
			PerlinNoise cloudNoise = new PerlinNoise(seed);
			Mesh cloudMesh = gameObject.GetComponent<MeshFilter> ().mesh;
			Vector3[] cloudVerts = cloudMesh.vertices;
			float spread = 0.1f;
			float amplitude = 0.75f;
			for (int i = 0; i < cloudVerts.Length; i++) {
				float cloudMap = amplitude * (float)cloudNoise.Noise ((double)cloudVerts[i].x / spread, (double)cloudVerts[i].y / spread, (double)cloudVerts[i].z / spread) + 1.0f;
				cloudVerts[i] *= cloudMap;
			}
			cloudMesh.vertices = cloudVerts;
			cloudMesh.RecalculateNormals ();
			cloudMesh.Optimize ();
		}
		
		void Update () {
			ResizeCloud ();
		}
		
		void ResizeCloud () {
			planetCloudVect = transform.position - planet.transform.position;
			planetCameraVect = planet.transform.position - Camera.main.transform.position;
			cloudScale = Mathf.Clamp (Vector3.Dot(planetCloudVect.normalized, planetCameraVect.normalized) + cloudOverHang, 0, 0.2f) / 1.25f;
			transform.localScale = new Vector3 (cloudScale, cloudScale, cloudScale);
		}
	}
}
