using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnhancedRadar : MonoBehaviour {

    private List<GameObject> trackedObjects_habitat, trackedObjects_connector, trackedObjects_refinery,
        trackedObjects_food, trackedObjects_airlock, trackedObjects_power;

    private List<GameObject> radarObjects_habitat, radarObjects_connector, radarObjects_refinery,
        radarObjects_food, radarObjects_airlock, radarObjects_power;

    private GameObject radarPrefab_habitat, radarPrefab_connector, radarPrefab_refinery,
        radarPrefab_food, radarPrefab_airlock, radarPrefab_power;

    private float switchDistance;

    public static bool showEnhancedRadar;

    private Camera mainCamera;

    private Camera enhancedRadarCamera;

    public static float fieldView;

    void OutpostGenerated(GameObject habitatModule)
    {
        trackedObjects_habitat.Add(habitatModule);
        DestroyRadarObjects(radarObjects_habitat);
        CreateRadarObjects(radarObjects_habitat, trackedObjects_habitat, radarPrefab_habitat);
    }

    void LocalModuleGenerated(GameObject localModule)
    {
        if (localModule.name == "Connector Module")
        {
            trackedObjects_connector.Add(localModule);
            DestroyRadarObjects(radarObjects_connector);
            CreateRadarObjects(radarObjects_connector, trackedObjects_connector, radarPrefab_connector);
        }
        else if (localModule.name == "Refinery Module")
        {
            trackedObjects_refinery.Add(localModule);
            DestroyRadarObjects(radarObjects_refinery);
            CreateRadarObjects(radarObjects_refinery, trackedObjects_refinery, radarPrefab_refinery);
        }
        else if (localModule.name == "Food Module")
        {
            trackedObjects_food.Add(localModule);
            DestroyRadarObjects(radarObjects_food);
            CreateRadarObjects(radarObjects_food, trackedObjects_food, radarPrefab_food);
        }
        else if (localModule.name == "Airlock Module")
        {
            trackedObjects_airlock.Add(localModule);
            DestroyRadarObjects(radarObjects_airlock);
            CreateRadarObjects(radarObjects_airlock, trackedObjects_airlock, radarPrefab_airlock);
        }
        else if (localModule.name == "Power Module")
        {
            trackedObjects_power.Add(localModule);
            DestroyRadarObjects(radarObjects_power);
            CreateRadarObjects(radarObjects_power, trackedObjects_power, radarPrefab_power);
        }
    }

	// Use this for initialization
	void Start () 
    {
        showEnhancedRadar = false;
        switchDistance = 5.5f;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        enhancedRadarCamera = GameObject.Find("EnhancedRadarCamera").GetComponent<Camera>();
        mainCamera.enabled = true;
        enhancedRadarCamera.enabled = false;

        radarObjects_habitat = new List<GameObject>();
        radarObjects_connector = new List<GameObject>();
        radarObjects_refinery = new List<GameObject>();
        radarObjects_food = new List<GameObject>();
        radarObjects_airlock = new List<GameObject>();
        radarObjects_power = new List<GameObject>();

        trackedObjects_habitat = new List<GameObject>();
        trackedObjects_connector = new List<GameObject>();
        trackedObjects_refinery = new List<GameObject>();
        trackedObjects_food = new List<GameObject>();
        trackedObjects_airlock = new List<GameObject>();
        trackedObjects_power = new List<GameObject>();

        radarPrefab_habitat = Instantiate(Resources.Load("Radar/HabitatPositionOnEnhancedRadar")) as GameObject;
        radarPrefab_connector = Instantiate(Resources.Load("Radar/ConnectorPositionOnEnhancedRadar")) as GameObject;
        radarPrefab_refinery = Instantiate(Resources.Load("Radar/RefineryPositionOnEnhancedRadar")) as GameObject;
        radarPrefab_food = Instantiate(Resources.Load("Radar/FoodPositionOnEnhancedRadar")) as GameObject;
        radarPrefab_airlock = Instantiate(Resources.Load("Radar/AirlockPositionOnEnhancedRadar")) as GameObject;
        radarPrefab_power = Instantiate(Resources.Load("Radar/PowerPositionOnEnhancedRadar")) as GameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            showEnhancedRadar = !showEnhancedRadar;
            
            if (showEnhancedRadar)
            {
                mainCamera.enabled = false;
                enhancedRadarCamera.enabled = true;

                for (int i = 0; i < radarObjects_habitat.Count; i++)
                {
                    radarObjects_habitat[i].layer = LayerMask.NameToLayer("EnhancedRadar");
                }
                for (int i = 0; i < radarObjects_connector.Count; i++)
                {
                    radarObjects_connector[i].layer = LayerMask.NameToLayer("EnhancedRadar");
                }
                for (int i = 0; i < radarObjects_refinery.Count; i++)
                {
                    radarObjects_refinery[i].layer = LayerMask.NameToLayer("EnhancedRadar");
                }
                for (int i = 0; i < radarObjects_food.Count; i++)
                {
                    radarObjects_food[i].layer = LayerMask.NameToLayer("EnhancedRadar");
                }
                for (int i = 0; i < radarObjects_airlock.Count; i++)
                {
                    radarObjects_airlock[i].layer = LayerMask.NameToLayer("EnhancedRadar");
                }
                for (int i = 0; i < radarObjects_power.Count; i++)
                {
                    radarObjects_power[i].layer = LayerMask.NameToLayer("EnhancedRadar");
                }
            }
            else
            {
                mainCamera.enabled = true;
                enhancedRadarCamera.enabled = false;
            }
        }
	}

    void DestroyRadarObjects(List<GameObject> radarObjects)
    {
        foreach (GameObject i in radarObjects)
        {
            Destroy(i);
        }
    }

    void CreateRadarObjects(List<GameObject> radarObjects, List<GameObject> trackedObjects, GameObject k)
    {
        radarObjects.Clear();

        foreach (GameObject o in trackedObjects)
        {
            k.transform.position = new Vector3(o.transform.position.x, o.transform.position.y, 2.0f);
            radarObjects.Add(k);
        }
    }
}
