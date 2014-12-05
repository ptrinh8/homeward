using UnityEngine;
using System.Collections;

// This class is for destroying the doorway(gameobject) when two finished modules are connected
public class DoorWayController : MonoBehaviour {

	private Transform childObject;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Connect Point") {
			childObject = transform.GetChild(0) as Transform;
			childObject.gameObject.SetActive(false);
		}
	}
}
