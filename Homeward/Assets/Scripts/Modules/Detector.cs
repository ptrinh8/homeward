using UnityEngine;
using System.Collections;

public class Detector : MonoBehaviour {

	[HideInInspector] public Vector3 relation;
	[HideInInspector] public bool matched;
	// Use this for initialization
	void Start () {
//		relation = new Vector3();
		matched = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject.tag == "Connect Point") {
			relation = other.gameObject.transform.position - gameObject.transform.position;
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
