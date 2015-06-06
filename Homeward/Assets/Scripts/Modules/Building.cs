using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Building class is for getting module building key, a "on-player-blueprint" will generate after player press the keys
public class Building : MonoBehaviour
{

    private GameObject habitatModuleDeploying;	// "on-player-blueprint" in prefabs
    private GameObject connectorModuleDeploying;		// "on-player-blueprint" in prefabs
    private GameObject refineryModuleDeploying;
    private GameObject foodModuleDeploying;
    private GameObject powerModuleDeploying;
    private GameObject healthStaminaModuleDeploying;
    private GameObject moduleControlModuleDeploying;
    private GameObject radarModuleDeploying;
    private GameObject storageModuleDeploying;
    private GameObject robotModuleDeploying;
    private GameObject airlockModuleDeploying;
    private GameObject oxygenModuleDeploying;
    private GameObject UIModuleDeploying;
	private GameObject buildingModuleDeploying;
	private GameObject miningProbeModuleDeploying;

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
    private Deployable airlockModuleDeployable;
    private Deployable oxygenModuleDeployable;
    private Deployable UIModuleDeployable;
	private Deployable buildingModuleDeployable;
	private Deployable miningProbeModuleDeployable;

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
    private KeyCode airlockModuleKey = KeyCode.F1;
    private KeyCode oxygenModuleKey = KeyCode.F2;
    private KeyCode UIModuleKey = KeyCode.F3;
	private KeyCode buildingModuleKey = KeyCode.F4;
	private KeyCode miningProbeModuleKey = KeyCode.F5;

    private Dictionary<string, bool> moduleDictionary;

    public static bool isDeploying;

	public static bool spawnHabitatModuleFlag;
	public static bool spawnConnectorModuleFlag;
	public static bool spawnFoodModuleFlag;
	public static bool spawnHealthStaminaModuleFlag;
	public static bool spawnModuleControlModuleFlag;
	public static bool spawnPowerModuleFlag;
	public static bool spawnRadarModuleFlag;
	public static bool spawnRefineryModuleFlag;
	public static bool spawnRobotModuleFlag;
	public static bool spawnStorageModuleFlag;
	public static bool spawnAirlockModuleFlag;
    public static bool spawnOxygenModuleFlag;
	public static bool spawnBuildingModuleFlag;
	public static bool spawnMiningProbeModuleFlag;

    public List<GameObject> initialModules = new List<GameObject>();
    private int i = 0;  // initialize in order

    private bool newGameFlag;

    public bool NewGameFlag
    {
        get
        {
            return newGameFlag;
        }
    }

	// Use this for initialization
	void Start () {
		// Instantiate the "on-player-blueprint", setActive(true) when needed
        habitatModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/Habitat Module/Habitat Module Deploying")) as GameObject;
		habitatModuleDeploying.SetActive(false);
        connectorModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/Connector Module/Connector Module Deploying")) as GameObject;
		connectorModuleDeploying.SetActive(false);
		refineryModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/Refinery Module/Refinery Module Deploying")) as GameObject;
		refineryModuleDeploying.SetActive(false);
		foodModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/Food Module/Food Module Deploying")) as GameObject;
		foodModuleDeploying.SetActive(false);
		powerModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/Power Module/Power Module Deploying")) as GameObject;
		powerModuleDeploying.SetActive(false);
        healthStaminaModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/HealthStaminaModule/HealthStamina Module Deploying")) as GameObject;
        healthStaminaModuleDeploying.SetActive(false);
        moduleControlModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/ModuleControlModule/ModuleControlModule Deploying")) as GameObject;
        moduleControlModuleDeploying.SetActive(false);
        radarModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/RadarModule/Radar Module Deploying")) as GameObject;
        radarModuleDeploying.SetActive(false);
        storageModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/Storage Module/Storage Module Deploying")) as GameObject;
        storageModuleDeploying.SetActive(false);
        robotModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/Robot Module/Robot Module Deploying")) as GameObject;
        robotModuleDeploying.SetActive(false);
        airlockModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/Airlock Module/Airlock Module Deploying")) as GameObject;
        airlockModuleDeploying.SetActive(false);
        oxygenModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/Oxygen Module/Oxygen Module Deploying")) as GameObject;
        oxygenModuleDeploying.SetActive(false);
        UIModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/Oxygen Module/Oxygen Module Deploying")) as GameObject;
        UIModuleDeploying.SetActive(false);
		buildingModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/Building Module/Building Module Deploying")) as GameObject;
		buildingModuleDeploying.SetActive(false);
		miningProbeModuleDeploying = Instantiate(Resources.Load("Prefabs/Modules/Mining Probe Module/Mining Probe Module Deploying")) as GameObject;
		miningProbeModuleDeploying.SetActive(false);

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
        airlockModuleDeployable = airlockModuleDeploying.GetComponent<Deployable>();
        oxygenModuleDeployable = oxygenModuleDeploying.GetComponent<Deployable>();
        UIModuleDeployable = UIModuleDeploying.GetComponent<Deployable>();
		buildingModuleDeployable = buildingModuleDeploying.GetComponent<Deployable>();
		miningProbeModuleDeployable = miningProbeModuleDeploying.GetComponent<Deployable>();

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
        moduleDictionary.Add("AirlockModule", true);
        moduleDictionary.Add("OxygenModule", true);
        moduleDictionary.Add("UIModule", true);
		moduleDictionary.Add("BuildingModule", true);
		moduleDictionary.Add("MiningProbeModule", true);
        isDeploying = false;

        spawnHabitatModuleFlag = false;
        spawnConnectorModuleFlag = false;
        spawnFoodModuleFlag = false;
        spawnHealthStaminaModuleFlag = false;
        spawnModuleControlModuleFlag = false;
        spawnPowerModuleFlag = false;
        spawnRadarModuleFlag = false;
        spawnRefineryModuleFlag = false;
        spawnRobotModuleFlag = false;
        spawnStorageModuleFlag = false;
        spawnAirlockModuleFlag = false;
        spawnOxygenModuleFlag = false;
		spawnBuildingModuleFlag = false;
		spawnMiningProbeModuleFlag = false;

        newGameFlag = true;
    }

    void NowBuilding(string module)
    {
        foreach (var key in moduleDictionary.Keys.ToList())
            moduleDictionary[key] = true;
        moduleDictionary[module] = false;
		/**
        if (connectorModuleDeployable.isDeploying && moduleDictionary["ConnectorModule"])
        {
            connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
            connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
        }
        if (refineryModuleDeployable.isDeploying && moduleDictionary["RefineryModule"])
        {
            refineryModuleDeployable.isDeploying = !refineryModuleDeployable.isDeploying;
            refineryModuleDeploying.SetActive(refineryModuleDeployable.isDeploying);
        }
        if (foodModuleDeployable.isDeploying && moduleDictionary["FoodModule"])
        {
            foodModuleDeployable.isDeploying = !foodModuleDeployable.isDeploying;
            foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
        }
        if (powerModuleDeployable.isDeploying && moduleDictionary["PowerModule"])
        {
            powerModuleDeployable.isDeploying = !powerModuleDeployable.isDeploying;
            powerModuleDeploying.SetActive(powerModuleDeployable.isDeploying);
        }
        if (healthStaminaModuleDeployable.isDeploying && moduleDictionary["HealthStaminaModule"])
        {
            healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable.isDeploying;
            healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
        }
        if (moduleControlModuleDeployable.isDeploying && moduleDictionary["ModuleControlModule"])
        {
            moduleControlModuleDeployable.isDeploying = !moduleControlModuleDeployable.isDeploying;
            moduleControlModuleDeploying.SetActive(moduleControlModuleDeployable.isDeploying);
        }
        if (radarModuleDeployable.isDeploying && moduleDictionary["RadarModule"])
        {
            radarModuleDeployable.isDeploying = !radarModuleDeployable.isDeploying;
            radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
        }
        if (storageModuleDeployable.isDeploying && moduleDictionary["StorageModule"])
        {
            storageModuleDeployable.isDeploying = !storageModuleDeployable.isDeploying;
            storageModuleDeploying.SetActive(storageModuleDeployable.isDeploying);
        }
        if (robotModuleDeployable.isDeploying && moduleDictionary["RobotModule"])
        {
            robotModuleDeployable.isDeploying = !robotModuleDeployable.isDeploying;
            robotModuleDeploying.SetActive(robotModuleDeployable.isDeploying);
        }
        if (habitatModuleDeployable.isDeploying && moduleDictionary["HabitatModule"])
        {
            habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
            habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
        }
        if (airlockModuleDeployable.isDeploying && moduleDictionary["AirlockModule"])
        {
            airlockModuleDeployable.isDeploying = !airlockModuleDeployable.isDeploying;
            airlockModuleDeploying.SetActive(airlockModuleDeployable.isDeploying);
        }
        if (oxygenModuleDeployable.isDeploying && moduleDictionary["OxygenModule"])
        {
            oxygenModuleDeployable.isDeploying = !oxygenModuleDeployable.isDeploying;
            oxygenModuleDeploying.SetActive(oxygenModuleDeployable.isDeploying);
        }
        if (UIModuleDeployable.isDeploying && moduleDictionary["UIModule"])
        {
            UIModuleDeployable.isDeploying = !UIModuleDeployable.isDeploying;
            UIModuleDeploying.SetActive(UIModuleDeployable.isDeploying);
        }
		if (buildingModuleDeployable.isDeploying && moduleDictionary["BuildingModule"])
		{
			buildingModuleDeployable.isDeploying = !buildingModuleDeployable.isDeploying;
			buildingModuleDeploying.SetActive(buildingModuleDeployable.isDeploying);
		}
		if (miningProbeModuleDeployable.isDeploying && moduleDictionary["MiningProbeModule"])
		{
			miningProbeModuleDeployable.isDeploying = !miningProbeModuleDeployable.isDeploying;
			miningProbeModuleDeploying.SetActive(miningProbeModuleDeployable.isDeploying);
		}
		**/
	}
	
	// Update is called once per frame
	void Update () {
        if (i < initialModules.Count)
        {
            initialModules[i].SetActive(true);
            i++;
        } 
        else
        {
            if (newGameFlag != false)
                newGameFlag = false;
        }

        if (!CentralControl.isInside)
        {
            if (Input.GetKeyDown(habitatModuleKey) || spawnHabitatModuleFlag)
            {
                // Handle condition which other modules are deploying
                NowBuilding("HabitatModule");
                // isDeploying means whether this module is deploying
                habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
                habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
                isDeploying = habitatModuleDeployable.isDeploying;
            }

            if (Input.GetKeyDown(connectorModuleKey) || spawnConnectorModuleFlag)
            {
                NowBuilding("ConnectorModule");
                connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
                connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
                isDeploying = connectorModuleDeployable.isDeploying;
            }

            if (Input.GetKeyDown(refineryModuleKey) || spawnRefineryModuleFlag)
            {
                NowBuilding("RefineryModule");
                refineryModuleDeployable.isDeploying = !refineryModuleDeployable.isDeploying;
                refineryModuleDeploying.SetActive(refineryModuleDeployable.isDeploying);
                isDeploying = refineryModuleDeployable.isDeploying;
            }

            if (Input.GetKeyDown(foodModuleKey) || spawnFoodModuleFlag)
            {
                NowBuilding("FoodModule");
                foodModuleDeployable.isDeploying = !foodModuleDeployable.isDeploying;
                foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
                isDeploying = foodModuleDeployable.isDeploying;
            }

            if (Input.GetKeyDown(powerModuleKey) || spawnPowerModuleFlag)
            {
                NowBuilding("PowerModule");
                powerModuleDeployable.isDeploying = !powerModuleDeployable.isDeploying;
                powerModuleDeploying.SetActive(powerModuleDeployable.isDeploying);
                isDeploying = powerModuleDeployable.isDeploying;
            }

            if (Input.GetKeyDown(healthStaminaModuleKey) || spawnHealthStaminaModuleFlag)
            {
                NowBuilding("HealthStaminaModule");
                healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable.isDeploying;
                healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
                isDeploying = healthStaminaModuleDeployable.isDeploying;
            }

            if (Input.GetKeyDown(moduleControlModuleKey) || spawnModuleControlModuleFlag)
            {
                NowBuilding("ModuleControlModule");
                moduleControlModuleDeployable.isDeploying = !moduleControlModuleDeployable.isDeploying;
                moduleControlModuleDeploying.SetActive(moduleControlModuleDeployable.isDeploying);
                isDeploying = moduleControlModuleDeployable.isDeploying;
            }

            if (Input.GetKeyDown(radarModuleKey) || spawnRadarModuleFlag)
            {
                NowBuilding("RadarModule");
                radarModuleDeployable.isDeploying = !radarModuleDeployable.isDeploying;
                radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
                isDeploying = radarModuleDeployable.isDeploying;
            }

            if (Input.GetKeyDown(storageModuleKey) || spawnStorageModuleFlag)
            {
                NowBuilding("StorageModule");
                storageModuleDeployable.isDeploying = !storageModuleDeployable.isDeploying;
                storageModuleDeploying.SetActive(storageModuleDeployable.isDeploying);
                isDeploying = storageModuleDeployable.isDeploying;
            }

            if (Input.GetKeyDown(robotModuleKey) || spawnRobotModuleFlag)
            {
                NowBuilding("RobotModule");
                robotModuleDeployable.isDeploying = !robotModuleDeployable.isDeploying;
                robotModuleDeploying.SetActive(robotModuleDeployable.isDeploying);
                isDeploying = robotModuleDeployable.isDeploying;
            }

            if (Input.GetKeyDown(airlockModuleKey) || spawnAirlockModuleFlag)
            {
                NowBuilding("AirlockModule");
                airlockModuleDeployable.isDeploying = !airlockModuleDeployable.isDeploying;
                airlockModuleDeploying.SetActive(airlockModuleDeployable.isDeploying);
                isDeploying = airlockModuleDeployable.isDeploying;
            }

			if (Input.GetKeyDown(oxygenModuleKey) || spawnOxygenModuleFlag)
			{
				NowBuilding("OxygenModule");
				oxygenModuleDeployable.isDeploying = !oxygenModuleDeployable.isDeploying;
				oxygenModuleDeploying.SetActive(oxygenModuleDeployable.isDeploying);
				isDeploying = oxygenModuleDeployable.isDeploying;
			}

            if (Input.GetKeyDown(UIModuleKey))
            {
                NowBuilding("UIModule");
                UIModuleDeployable.isDeploying = !UIModuleDeployable.isDeploying;
                UIModuleDeploying.SetActive(UIModuleDeployable.isDeploying);
                isDeploying = UIModuleDeployable.isDeploying;
            }

			if (Input.GetKeyDown(buildingModuleKey))
			{
				NowBuilding("BuildingModule");
				buildingModuleDeployable.isDeploying = !buildingModuleDeployable.isDeploying;
				buildingModuleDeploying.SetActive(buildingModuleDeployable.isDeploying);
				isDeploying = buildingModuleDeployable.isDeploying;
			}

			if (Input.GetKeyDown(miningProbeModuleKey))
			{
				NowBuilding("MiningProbeModule");
				miningProbeModuleDeployable.isDeploying = !miningProbeModuleDeployable.isDeploying;
				miningProbeModuleDeploying.SetActive(miningProbeModuleDeployable.isDeploying);
				isDeploying = miningProbeModuleDeployable.isDeploying;
			}

            spawnHabitatModuleFlag = false;
            spawnConnectorModuleFlag = false;
            spawnFoodModuleFlag = false;
            spawnHealthStaminaModuleFlag = false;
            spawnModuleControlModuleFlag = false;
            spawnPowerModuleFlag = false;
            spawnRadarModuleFlag = false;
            spawnRefineryModuleFlag = false;
            spawnRobotModuleFlag = false;
            spawnStorageModuleFlag = false;
            spawnAirlockModuleFlag = false;
            spawnOxygenModuleFlag = false;
			spawnBuildingModuleFlag = false;
			spawnMiningProbeModuleFlag = false;
        }
    }
}
