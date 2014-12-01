using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

	public GameObject habitatModuleDeploying;
	public GameObject connectorModuleDeploying;
	private KeyCode habitatModuleKey = KeyCode.Alpha1;
	private KeyCode connectorModuleKey = KeyCode.Alpha2;

	[HideInInspector]
	public bool isDeploying;

	// Use this for initialization
	void Start () {
		isDeploying = false;
		habitatModuleDeploying = Instantiate(habitatModuleDeploying) as GameObject;
		habitatModuleDeploying.SetActive(false);
		connectorModuleDeploying = Instantiate(connectorModuleDeploying) as GameObject;
		connectorModuleDeploying.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(habitatModuleKey) && isDeploying == false) {
			habitatModuleDeploying.SetActive(true);
			isDeploying = true;
		}

		if (Input.GetKeyDown(connectorModuleKey) && isDeploying == false) {
			connectorModuleDeploying.SetActive(true);
			isDeploying = true;
		}

	}
}
