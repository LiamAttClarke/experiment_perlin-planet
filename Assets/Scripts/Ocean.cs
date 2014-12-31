using UnityEngine;
using System.Collections;
namespace Universe {
	public class Ocean : MonoBehaviour {
		PerlinNoise oceanNoise;
		Mesh oceanMesh;
		Vector3[] oceanVerts, waveVerts;
		float oceanMap;
		float wave = 0;
		float current = 0.01f;
		double vert_x, vert_y, vert_z;
		float oceanAmpl = 0.1f;
		float oceanSpread = 0.25f;

		// Use this for initialization
		void Start () {
			oceanMesh = gameObject.GetComponent<MeshFilter> ().mesh;
			oceanVerts = oceanMesh.vertices;
			waveVerts = oceanMesh.vertices;
			oceanNoise = new PerlinNoise (99);
		}
		
		// Update is called once per frame
		void Update () {
			for (int i = 0; i < waveVerts.Length; i++) {
				vert_x = (double)waveVerts[i].x;
				vert_y = (double)waveVerts[i].y;
				vert_z = (double)waveVerts[i].z;
				oceanMap = oceanAmpl * (float)oceanNoise.Noise (vert_x / oceanSpread + wave, vert_y / oceanSpread, vert_z / oceanSpread) + 1.0f;
				waveVerts[i] = oceanVerts[i] * oceanMap;
			}
			oceanMesh.vertices = waveVerts;
			oceanMesh.RecalculateNormals ();
			oceanMesh.Optimize ();
			wave += current;
		}
	}
}

