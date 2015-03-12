using UnityEngine;
using System.Collections;

public class Topper : MonoBehaviour {
	public GameObject tree, cloud;
	public int treeCount, cloudCount;
	float planetRadius = 1f;
	float atmosphereRadius = 1.4f;
	float longitude, latitude, x, y, z;
	// Use this for initialization
	void Start () {
		Plot (tree, treeCount, planetRadius);
		Plot (cloud, cloudCount, atmosphereRadius);
	}
	
	void Plot (GameObject obj, int objCount, float r) {
		for (int i = 0; i < objCount; i++) {
			longitude = Random.Range (0, Mathf.PI * 2);
			//latitude = Random.Range (0, Mathf.PI);
			float abc = Random.Range (0,1f);
			latitude = Mathf.Acos (2 * abc - 1);
			x = r * Mathf.Sin (latitude) * Mathf.Cos (longitude);
			y = r * Mathf.Sin (latitude) * Mathf.Sin (longitude);
			z = r * Mathf.Cos (latitude);
			GameObject topping = (GameObject)GameObject.Instantiate (obj, new Vector3 (x,y,z), Quaternion.identity);
			topping.transform.parent = transform;
		}
	}
}
