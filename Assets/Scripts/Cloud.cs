using UnityEngine;
using System.Collections;
namespace Universe {
	public class Cloud : MonoBehaviour {
		GameObject planet;
		Vector3 planetCloudVect;
		Vector3 planetCameraVect;
		float cloudScale;
		// Use this for initialization
		void Start () {
			planet = GameObject.Find("Planet");
			ResizeCloud ();
		}
		
		void Update () {
			if (planet.GetComponent<Planet> ().isDragging) {
				ResizeCloud ();
			}
		}
		
		void ResizeCloud () {
			planetCloudVect = transform.position - planet.transform.position;
			planetCameraVect = planet.transform.position - Camera.main.transform.position;
			cloudScale = Mathf.Clamp (Vector3.Dot(planetCloudVect.normalized, planetCameraVect.normalized) + 0.85f, 0, 0.2f);
			transform.localScale = new Vector3 (cloudScale, cloudScale, cloudScale);
			transform.LookAt (transform.position * 2);
		}
	}
}
