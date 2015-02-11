using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Building class is for getting module building key, a "on-player-blueprint" will generate after player press the keys
public class Building : MonoBehaviour {

	public GameObject habitatModuleDeploying;	// "on-player-blueprint" in prefabs
	public GameObject connectorModuleDeploying;		// "on-player-blueprint" in prefabs
	public GameObject refineryModuleDeploying;
	public GameObject foodModuleDeploying;
	public GameObject powerModuleDeploying;
    public GameObject healthStaminaModuleDeploying;
    public GameObject moduleControlModuleDeploying;
    public GameObject radarModuleDeploying;
	public GameObject storageModuleDeploying;
	public GameObject robotModuleDeploying;

	private HabitatModuleDeployable habitatModuleDeployable;	// Script in Habitat Module Deploying(prefabs)
	private Deployable connectorModuleDeployable;	// Script in any Module Deploying(prefabs)
	private Deployable refineryModuleDeployable;
	private Deployable foodModuleDeployable;
	private Deployable powerModuleDeployable;
    private Deployable healthStaminaModuleDeployable;
    private Deployable moduleControlModuleDeployable;
    private Deployable radarModuleDeployable;
	private Deployable storageModuleDeployable;
	private Deployable robotModuleDeployable;

	private KeyCode habitatModuleKey = KeyCode.Alpha1;
	private KeyCode connectorModuleKey = KeyCode.Alpha2;
	private KeyCode refineryModuleKey = KeyCode.Alpha3;
	private KeyCode foodModuleKey = KeyCode.Alpha4;
	private KeyCode powerModuleKey = KeyCode.Alpha5;
    private KeyCode healthStaminaModuleKey = KeyCode.Alpha6;
    private KeyCode moduleControlModuleKey = KeyCode.Alpha7;
    private KeyCode radarModuleKey = KeyCode.Alpha8;
	private KeyCode storageModuleKey = KeyCode.Alpha9;
	private KeyCode robotModuleKey = KeyCode.Alpha0;

	private Dictionary<string, bool> moduleDictionary;

	public GameObject habitatModule;
	public GameObject refineryModule;

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
		powerModuleDeploying = Instantiate(powerModuleDeploying) as GameObject;
		powerModuleDeploying.SetActive(false);
        healthStaminaModuleDeploying = Instantiate(healthStaminaModuleDeploying) as GameObject;
        healthStaminaModuleDeploying.SetActive(false);
        moduleControlModuleDeploying = Instantiate(moduleControlModuleDeploying) as GameObject;
        moduleControlModuleDeploying.SetActive(false);
        radarModuleDeploying = Instantiate(radarModuleDeploying) as GameObject;
        radarModuleDeploying.SetActive(false);
		storageModuleDeploying = Instantiate(storageModuleDeploying) as GameObject;
		storageModuleDeploying.SetActive(false);
		robotModuleDeploying = Instantiate(robotModuleDeploying) as GameObject;
		robotModuleDeploying.SetActive(false);

		habitatModuleDeployable = habitatModuleDeploying.GetComponent<HabitatModuleDeployable>();
		connectorModuleDeployable = connectorModuleDeploying.GetComponent<Deployable>();
		refineryModuleDeployable = refineryModuleDeploying.GetComponent<Deployable>();
		foodModuleDeployable = foodModuleDeploying.GetComponent<Deployable>();
		powerModuleDeployable = powerModuleDeploying.GetComponent<Deployable>();
        healthStaminaModuleDeployable = healthStaminaModuleDeploying.GetComponent<Deployable>();
        moduleControlModuleDeployable = moduleControlModuleDeploying.GetComponent<Deployable>();
        radarModuleDeployable = radarModuleDeploying.GetComponent<Deployable>();
		storageModuleDeployable = storageModuleDeploying.GetComponent<Deployable>();
		robotModuleDeployable = robotModuleDeploying.GetComponent<Deployable>();

		moduleDictionary = new Dictionary<string, bool>(); 
		moduleDictionary.Add("HabitatModule", true);
		moduleDictionary.Add("ConnectorModule", true);
		moduleDictionary.Add("RefineryModule", true);
		moduleDictionary.Add("FoodModule", true);
		moduleDictionary.Add("PowerModule", true);
		moduleDictionary.Add("HealthStaminaModule", true);
		moduleDictionary.Add("ModuleControlModule", true);
		moduleDictionary.Add("RadarModule", true);
		moduleDictionary.Add("StorageModule", true);
		moduleDictionary.Add("RobotModule", true);


		Instantiate(habitatModule, new Vector3(7, 3, 0),  Quaternion.identity);
		Invoke("InitialState", Time.deltaTime);
	}

	void InitialState() {
		refineryModule = Instantiate(refineryModule, new Vector3(9.85f, 3, 0),  Quaternion.identity) as GameObject;
		Invoke ("InitialVar", Time.deltaTime);
	}

	void InitialVar() {
		LocalControl localControlScript = refineryModule.GetComponent<LocalControl>();
		localControlScript.IsEnter = false;
	}


	void NowBuilding(string module) {
		foreach (var key in moduleDictionary.Keys.ToList())
			moduleDictionary[key] = true;
		moduleDictionary[module] = false;

		if (connectorModuleDeployable.isDeploying && moduleDictionary["ConnectorModule"]) {
			connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
			connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
		}
		if (refineryModuleDeployable.isDeploying && moduleDictionary["RefineryModule"]) {
			refineryModuleDeployable.isDeploying = !refineryModuleDeployable;
			refineryModuleDeploying.SetActive(refineryModuleDeployable.isDeploying);
		}
		if (foodModuleDeployable.isDeploying && moduleDictionary["FoodModule"]) {
			foodModuleDeployable.isDeploying = !foodModuleDeployable;
			foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
		}
		if (powerModuleDeployable.isDeploying && moduleDictionary["PowerModule"]) {
			powerModuleDeployable.isDeploying = !powerModuleDeployable;
			powerModuleDeploying.SetActive(powerModuleDeployable.isDeploying);
		}
		if (healthStaminaModuleDeployable.isDeploying && moduleDictionary["HealthStaminaModule"]) {
			healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable;
			healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
		}
		if (moduleControlModuleDeployable.isDeploying && moduleDictionary["ModuleControlModule"])
		{
			moduleControlModuleDeployable.isDeploying = !moduleControlModuleDeployable;
			moduleControlModuleDeploying.SetActive(moduleControlModuleDeployable.isDeploying);
		}
		if (radarModuleDeployable.isDeploying && moduleDictionary["RadarModule"]) {
			radarModuleDeployable.isDeploying = !radarModuleDeployable;
			radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
		}
		if (storageModuleDeployable.isDeploying && moduleDictionary["StorageModule"]) {
			storageModuleDeployable.isDeploying = !storageModuleDeployable;
			storageModuleDeploying.SetActive(storageModuleDeployable.isDeploying);
		}
		if (robotModuleDeployable.isDeploying && moduleDictionary["RobotModule"]) {
			robotModuleDeployable.isDeploying = !robotModuleDeployable;
			robotModuleDeploying.SetActive(robotModuleDeployable.isDeploying);
		}
		if (habitatModuleDeployable.isDeploying && moduleDictionary["HabitatModule"]){
			habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
			habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(habitatModuleKey)) {
			// Handle condition which other modules are deploying
			NowBuilding("HabitatModule");
			// isDeploying means whether this module is deploying
			habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
			habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
		}

		if (Input.GetKeyDown(connectorModuleKey)) {
			NowBuilding("ConnectorModule");
			connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
			connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
		}

		if (Input.GetKeyDown(refineryModuleKey)) {
			NowBuilding("RefineryModule");
			refineryModuleDeployable.isDeploying = !refineryModuleDeployable.isDeploying;
			refineryModuleDeploying.SetActive(refineryModuleDeployable.isDeploying);
		}

		if (Input.GetKeyDown(foodModuleKey)) {
			NowBuilding("FoodModule");
			foodModuleDeployable.isDeploying = !foodModuleDeployable.isDeploying;
			foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
		}

		if (Input.GetKeyDown(powerModuleKey)) {
			NowBuilding("PowerModule");
			powerModuleDeployable.isDeploying = !powerModuleDeployable.isDeploying;
			powerModuleDeploying.SetActive(powerModuleDeployable.isDeploying);
		}
		
        if (Input.GetKeyDown(healthStaminaModuleKey))
        {
			NowBuilding("HealthStaminaModule");
			healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable.isDeploying;
            healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
        }

        if (Input.GetKeyDown(moduleControlModuleKey))
        {
			NowBuilding("ModuleControlModule");
			radarModuleDeployable.isDeploying = !radarModuleDeployable.isDeploying;
            radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
        }

		if (Input.GetKeyDown(storageModuleKey)) {
			NowBuilding("StorageModule");
			storageModuleDeployable.isDeploying = !storageModuleDeployable.isDeploying;
			storageModuleDeploying.SetActive(storageModuleDeployable.isDeploying);
		}

		if (Input.GetKeyDown(robotModuleKey)) {
			NowBuilding("RobotModule");
			robotModuleDeployable.isDeploying = !robotModuleDeployable;
			robotModuleDeploying.SetActive(robotModuleDeployable);
		}
	}
}
