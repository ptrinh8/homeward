using UnityEngine;
using System.Collections;

// This class hanles the snap trigger
public class Detector : MonoBehaviour {

	[HideInInspector] public Vector3 relation;	// Position change when snap
	[HideInInspector] public bool matched;	// Whether the trigger is matched
	// Use this for initialization
	void Start () {
		matched = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject.tag == "Connect Point") {
			relation = other.gameObject.transform.position - gameObject.transform.position; //Calculate the position change
			matched = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Connect Point") {
			relation = new Vector3(0, 0, 0);
			matched = false;
		}
	}
}
