using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This class is for destroying the doorway(gameobject) when two finished modules are connected
public class DoorWayController : MonoBehaviour {

	private Transform childObject;
	private CentralControl center;
	private LocalControl local;
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
			// Situations whether "other" module is habitat module or other modules
			if (gameObject.transform.root.gameObject.tag != "HabitatModule") {
				center = other.gameObject.transform.root.gameObject.GetComponent<CentralControl>();
				local = other.gameObject.transform.root.gameObject.GetComponent<LocalControl>();
				if (center != null) 
					gameObject.SendMessageUpwards("SetCenter", other.gameObject.transform.root.gameObject, SendMessageOptions.DontRequireReceiver);
				if (local != null) {
					gameObject.SendMessageUpwards("SetCenter", local.center, SendMessageOptions.DontRequireReceiver);
				} 	
				gameObject.SendMessageUpwards("AddConnection", other.gameObject.transform.root.gameObject);
	//			if ((gameObject.transform.root.gameObject.GetComponent<LocalControl>().moduleID 
	//			    > other.gameObject.transform.root.gameObject.GetComponent<LocalControl>().moduleID))
					gameObject.transform.root.gameObject.GetComponent<LocalControl>().checkFlag = true;
	//			gameObject.SendMessageUpwards("CheckPowerSupply", SendMessageOptions.DontRequireReceiver);
			}
			gameObject.SendMessageUpwards("ChangeLocation", true);
		}
	}
}
