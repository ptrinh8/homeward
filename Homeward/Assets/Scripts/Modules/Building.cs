using UnityEngine;
using System.Collections;

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
	private HabitatModuleDeployable habitatModuleDeployable;	// Script in Habitat Module Deploying(prefabs)
	private Deployable connectorModuleDeployable;	// Script in any Module Deploying(prefabs)
	private Deployable refineryModuleDeployable;
	private Deployable foodModuleDeployable;
	private Deployable powerModuleDeployable;
    private Deployable healthStaminaModuleDeployable;
    private Deployable moduleControlModuleDeployable;
    private Deployable radarModuleDeployable;
	private KeyCode habitatModuleKey = KeyCode.Alpha1;
	private KeyCode connectorModuleKey = KeyCode.Alpha2;
	private KeyCode refineryModuleKey = KeyCode.Alpha3;
	private KeyCode foodModuleKey = KeyCode.Alpha4;
	private KeyCode powerModuleKey = KeyCode.Alpha5;
    private KeyCode healthStaminaModuleKey = KeyCode.Alpha6;
    private KeyCode moduleControlModuleKey = KeyCode.Alpha7;
    private KeyCode radarModuleKey = KeyCode.Alpha8;

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

		habitatModuleDeployable = habitatModuleDeploying.GetComponent<HabitatModuleDeployable>();
		connectorModuleDeployable = connectorModuleDeploying.GetComponent<Deployable>();
		refineryModuleDeployable = refineryModuleDeploying.GetComponent<Deployable>();
		foodModuleDeployable = foodModuleDeploying.GetComponent<Deployable>();
		powerModuleDeployable = powerModuleDeploying.GetComponent<Deployable>();
        healthStaminaModuleDeployable = healthStaminaModuleDeploying.GetComponent<Deployable>();
        moduleControlModuleDeployable = moduleControlModuleDeploying.GetComponent<Deployable>();
        radarModuleDeployable = radarModuleDeploying.GetComponent<Deployable>();
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
			if (powerModuleDeployable.isDeploying) {
				powerModuleDeployable.isDeploying = !powerModuleDeployable;
				powerModuleDeploying.SetActive(powerModuleDeployable.isDeploying);
			}
            if (healthStaminaModuleDeployable.isDeploying) {
                healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable;
                healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
            }
            if (moduleControlModuleDeployable.isDeploying)
            {
                moduleControlModuleDeployable.isDeploying = !moduleControlModuleDeployable;
                moduleControlModuleDeploying.SetActive(moduleControlModuleDeployable.isDeploying);
            }
            if (radarModuleDeployable.isDeploying)
            {
                radarModuleDeployable.isDeploying = !radarModuleDeployable;
                radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
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
			if (powerModuleDeployable.isDeploying) {
				powerModuleDeployable.isDeploying = !powerModuleDeployable;
				powerModuleDeploying.SetActive(powerModuleDeployable.isDeploying);
			}
            if (healthStaminaModuleDeployable.isDeploying)
            {
                healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable;
                healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
            }
            if (moduleControlModuleDeployable.isDeploying)
            {
                moduleControlModuleDeployable.isDeploying = !moduleControlModuleDeployable;
                moduleControlModuleDeploying.SetActive(moduleControlModuleDeployable.isDeploying);
            }
            if (radarModuleDeployable.isDeploying)
            {
                radarModuleDeployable.isDeploying = !radarModuleDeployable;
                radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
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
			if (powerModuleDeployable.isDeploying) {
				powerModuleDeployable.isDeploying = !powerModuleDeployable;
				powerModuleDeploying.SetActive(powerModuleDeployable.isDeploying);
			}
            if (healthStaminaModuleDeployable.isDeploying)
            {
                healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable;
                healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
            }
            if (moduleControlModuleDeployable.isDeploying)
            {
                moduleControlModuleDeployable.isDeploying = !moduleControlModuleDeployable;
                moduleControlModuleDeploying.SetActive(moduleControlModuleDeployable.isDeploying);
            }
            if (radarModuleDeployable.isDeploying)
            {
                radarModuleDeployable.isDeploying = !radarModuleDeployable;
                radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
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
			if (powerModuleDeployable.isDeploying) {
				powerModuleDeployable.isDeploying = !powerModuleDeployable;
				powerModuleDeploying.SetActive(powerModuleDeployable.isDeploying);
			}
            if (healthStaminaModuleDeployable.isDeploying)
            {
                healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable;
                healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
            }
            if (moduleControlModuleDeployable.isDeploying)
            {
                moduleControlModuleDeployable.isDeploying = !moduleControlModuleDeployable;
                moduleControlModuleDeploying.SetActive(moduleControlModuleDeployable.isDeploying);
            }
            if (radarModuleDeployable.isDeploying)
            {
                radarModuleDeployable.isDeploying = !radarModuleDeployable;
                radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
            }
			foodModuleDeployable.isDeploying = !foodModuleDeployable.isDeploying;
			foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
		}

		if (Input.GetKeyDown(powerModuleKey)) {
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
			if (foodModuleDeployable.isDeploying) {
				foodModuleDeployable.isDeploying = !foodModuleDeployable;
				foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
			}
            if (healthStaminaModuleDeployable.isDeploying)
            {
                healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable;
                healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
            }
            if (moduleControlModuleDeployable.isDeploying)
            {
                moduleControlModuleDeployable.isDeploying = !moduleControlModuleDeployable;
                moduleControlModuleDeploying.SetActive(moduleControlModuleDeployable.isDeploying);
            }
            if (radarModuleDeployable.isDeploying)
            {
                radarModuleDeployable.isDeploying = !radarModuleDeployable;
                radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
            }
			powerModuleDeployable.isDeploying = !powerModuleDeployable.isDeploying;
			powerModuleDeploying.SetActive(powerModuleDeployable.isDeploying);
		}

        if (Input.GetKeyDown(healthStaminaModuleKey))
        {
            if (habitatModuleDeployable.isDeploying)
            {
                habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
                habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
            }
            if (connectorModuleDeployable.isDeploying)
            {
                connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
                connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
            }
            if (refineryModuleDeployable.isDeploying)
            {
                refineryModuleDeployable.isDeploying = !refineryModuleDeployable;
                refineryModuleDeploying.SetActive(refineryModuleDeployable.isDeploying);
            }
            if (foodModuleDeployable.isDeploying)
            {
                foodModuleDeployable.isDeploying = !foodModuleDeployable;
                foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
            }
            if (healthStaminaModuleDeployable.isDeploying)
            {
                healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable;
                healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
            }
            if (moduleControlModuleDeployable.isDeploying)
            {
                moduleControlModuleDeployable.isDeploying = !moduleControlModuleDeployable;
                moduleControlModuleDeploying.SetActive(moduleControlModuleDeployable.isDeploying);
            }
            if (radarModuleDeployable.isDeploying)
            {
                radarModuleDeployable.isDeploying = !radarModuleDeployable;
                radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
            }
            healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable.isDeploying;
            healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
        }

        if (Input.GetKeyDown(moduleControlModuleKey))
        {
            if (habitatModuleDeployable.isDeploying)
            {
                habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
                habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
            }
            if (connectorModuleDeployable.isDeploying)
            {
                connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
                connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
            }
            if (refineryModuleDeployable.isDeploying)
            {
                refineryModuleDeployable.isDeploying = !refineryModuleDeployable;
                refineryModuleDeploying.SetActive(refineryModuleDeployable.isDeploying);
            }
            if (foodModuleDeployable.isDeploying)
            {
                foodModuleDeployable.isDeploying = !foodModuleDeployable;
                foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
            }
            if (healthStaminaModuleDeployable.isDeploying)
            {
                healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable;
                healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
            }
            if (moduleControlModuleDeployable.isDeploying)
            {
                moduleControlModuleDeployable.isDeploying = !moduleControlModuleDeployable;
                moduleControlModuleDeploying.SetActive(moduleControlModuleDeployable.isDeploying);
            }
            if (radarModuleDeployable.isDeploying)
            {
                radarModuleDeployable.isDeploying = !radarModuleDeployable;
                radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
            }
            moduleControlModuleDeployable.isDeploying = !moduleControlModuleDeployable.isDeploying;
            moduleControlModuleDeploying.SetActive(moduleControlModuleDeployable.isDeploying);
        }

        if (Input.GetKeyDown(radarModuleKey))
        {
            if (habitatModuleDeployable.isDeploying)
            {
                habitatModuleDeployable.isDeploying = !habitatModuleDeployable.isDeploying;
                habitatModuleDeploying.SetActive(habitatModuleDeployable.isDeploying);
            }
            if (connectorModuleDeployable.isDeploying)
            {
                connectorModuleDeployable.isDeploying = !connectorModuleDeployable.isDeploying;
                connectorModuleDeploying.SetActive(connectorModuleDeployable.isDeploying);
            }
            if (refineryModuleDeployable.isDeploying)
            {
                refineryModuleDeployable.isDeploying = !refineryModuleDeployable;
                refineryModuleDeploying.SetActive(refineryModuleDeployable.isDeploying);
            }
            if (foodModuleDeployable.isDeploying)
            {
                foodModuleDeployable.isDeploying = !foodModuleDeployable;
                foodModuleDeploying.SetActive(foodModuleDeployable.isDeploying);
            }
            if (healthStaminaModuleDeployable.isDeploying)
            {
                healthStaminaModuleDeployable.isDeploying = !healthStaminaModuleDeployable;
                healthStaminaModuleDeploying.SetActive(healthStaminaModuleDeployable.isDeploying);
            }
            if (moduleControlModuleDeployable.isDeploying)
            {
                moduleControlModuleDeployable.isDeploying = !moduleControlModuleDeployable;
                moduleControlModuleDeploying.SetActive(moduleControlModuleDeployable.isDeploying);
            }
            if (radarModuleDeployable.isDeploying)
            {
                radarModuleDeployable.isDeploying = !radarModuleDeployable;
                radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
            }
            radarModuleDeployable.isDeploying = !radarModuleDeployable.isDeploying;
            radarModuleDeploying.SetActive(radarModuleDeployable.isDeploying);
        }
	}
}
