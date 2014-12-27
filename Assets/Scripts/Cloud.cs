using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {
	GameObject planet;
	// Use this for initialization
	void Start () {
		planet = GameObject.Find("Planet");
	}
	
	// Update is called once per frame
	void Update () {
		ResizeCloud ();
	}

	void ResizeCloud () {
		Vector3 planetCloudVect = transform.position - planet.transform.position;
		Vector3 cameraPlanetVect = planet.transform.position - Camera.main.transform.position;
		float cloudCameraScal = Vector3.Dot(planetCloudVect.normalized, cameraPlanetVect.normalized) + 0.85f;
		if (cloudCameraScal < 0) {
			cloudCameraScal = 0;
		} else if (cloudCameraScal > 0.2f) {
			cloudCameraScal = 0.2f;
		}
		transform.localScale = new Vector3 (cloudCameraScal, cloudCameraScal, cloudCameraScal);
		transform.LookAt (transform.position * 2);
	}
}
	