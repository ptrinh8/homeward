using UnityEngine;
using System.Collections;

// Building class is for getting module building key, a "on-player-blueprint" will generate after player press the keys
public class Building : MonoBehaviour {

	public GameObject habitatModuleDeploying;	// "on-player-blueprint" in prefabs
	public GameObject connectorModuleDeploying;		// "on-player-blueprint" in prefabs
	public GameObject refineryModuleDeploying;
	public GameObject foodModuleDeploying;
	private HabitatModuleDeployable habitatModuleDeployable;	// Script in Habitat Module Deploying(prefabs)
	private Deployable connectorModuleDeployable;	// Script in any Module Deploying(prefabs)
	private Deployable refineryModuleDeployable;
	private Deployable foodModuleDeployable;
	private KeyCode habitatModuleKey = KeyCode.Alpha1;
	private KeyCode connectorModuleKey = KeyCode.Alpha2;
	private KeyCode refineryModuleKey = KeyCode.Alpha3;
	private KeyCode foodModuleKey = KeyCode.Alpha4;

	// Use this for initialization
	void Start () {
		// Instantiate the "on-player-blueprint", setActive(true) when needed
		habitatModuleDeploying = Instantiate(habitatModuleDeploying) as GameObject;
		habitatModuleDeploying.SetActive(false);
		connectorModuleDeploying = Instantiate(connectorModuleDeploying) as GameObject;
		connectorModuleDeploying.SetActive(false);
		refineryModuleDeploying = Instantiate(refineryModuleDeploying) as GameObject;
		refineryModuleDeploying.SetActive(false);
		foodModuleDeploying = Instantiate(foodModuleDeploying) as GameObject;
		foodModuleDeploying.SetActive(false);

		habitatModuleDeployable = habitatModuleDeploying.GetComponent<HabitatModuleDeployable>();
		connectorModuleDeployable = connectorModuleDeploying.GetComponent<Deployable>();
		refineryModuleDeployable = refineryModuleDeploying.GetComponent<Deployable>();
		foodModuleDeployable = foodModuleDeploying.GetComponent<Deployable>();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(habitatModuleKey)) {
			// Handle condition which other modules are deploying
			if (connectorModuleDeployable.isDeploying) {
				connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
				connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
			}
			if (refineryModuleDeployable.isDeploying) {
				refineryModuleDeployable.isDeploying = !refineryModuleDeployable;
				refineryModuleDeploying.SetActive(refineryModuleDeployable.isDeploying);
			}
			if (foodModuleDeployable.isDeploying) {
				foodModuleDeployable.isDeploying = !foodModuleDeployable;
				foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
			}
			// isDeploying means whether this module is deploying
			habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
			habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
			
		}

		if (Input.GetKeyDown(connectorModuleKey)) {
			if (habitatModuleDeployable.isDeploying){
				habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
				habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
			}
			if (refineryModuleDeployable.isDeploying) {
				refineryModuleDeployable.isDeploying = !refineryModuleDeployable;
				refineryModuleDeploying.SetActive(refineryModuleDeployable.isDeploying);
			}
			if (foodModuleDeployable.isDeploying) {
				foodModuleDeployable.isDeploying = !foodModuleDeployable;
				foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
			}
			connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
			connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
		}

		if (Input.GetKeyDown(refineryModuleKey)) {
			if (habitatModuleDeployable.isDeploying){
				habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
				habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
			}
			if (connectorModuleDeployable.isDeploying) {
				connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
				connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
			}
			if (foodModuleDeployable.isDeploying) {
				foodModuleDeployable.isDeploying = !foodModuleDeployable;
				foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
			}
			refineryModuleDeployable.isDeploying = !refineryModuleDeployable.isDeploying;
			refineryModuleDeploying.SetActive(refineryModuleDeployable.isDeploying);
		}

		if (Input.GetKeyDown(foodModuleKey)) {
			if (habitatModuleDeployable.isDeploying){
				habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
				habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
			}
			if (connectorModuleDeployable.isDeploying) {
				connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
				connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
			}
			if (refineryModuleDeployable.isDeploying) {
				refineryModuleDeployable.isDeploying = !refineryModuleDeployable;
				refineryModuleDeploying.SetActive(refineryModuleDeployable.isDeploying);
			}
			foodModuleDeployable.isDeploying = !foodModuleDeployable.isDeploying;
			foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
		}

	}
}
