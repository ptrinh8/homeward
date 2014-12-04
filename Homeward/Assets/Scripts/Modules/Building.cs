using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

	public GameObject habitatModuleDeploying;
	public GameObject connectorModuleDeploying;
	private HabitatModuleDeployable habitatModuleDeployable;
	private Deployable connectorModuleDeployable;
	private KeyCode habitatModuleKey = KeyCode.Alpha1;
	private KeyCode connectorModuleKey = KeyCode.Alpha2;

	// Use this for initialization
	void Start () {

		habitatModuleDeploying = Instantiate(habitatModuleDeploying) as GameObject;
		habitatModuleDeploying.SetActive(false);
		connectorModuleDeploying = Instantiate(connectorModuleDeploying) as GameObject;
		connectorModuleDeploying.SetActive(false);
		habitatModuleDeployable = habitatModuleDeploying.GetComponent<HabitatModuleDeployable>();
		connectorModuleDeployable = connectorModuleDeploying.GetComponent<Deployable>();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(habitatModuleKey)) {
			if (connectorModuleDeployable.isDeploying) {
				connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
				connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
			}
			habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
			habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
			
		}

		if (Input.GetKeyDown(connectorModuleKey)) {
			if (habitatModuleDeployable.isDeploying){
				habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
				habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
			}
			connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
			connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
		}

	}
}
