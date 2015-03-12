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
		}
		
		void Update () {
			ResizeCloud ();
		}
		
		void ResizeCloud () {
			planetCloudVect = transform.position - planet.transform.position;
			planetCameraVect = planet.transform.position - Camera.main.transform.position;
			cloudScale = Mathf.Clamp (Vector3.Dot(planetCloudVect.normalized, planetCameraVect.normalized) + cloudOverHang, 0, 0.2f) / 2.0f;
			transform.localScale = new Vector3 (cloudScale, cloudScale, cloudScale);
		}
	}
}
