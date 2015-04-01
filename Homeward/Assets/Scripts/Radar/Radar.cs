/*****
 * need to implement outpost destruction
 * 
 * ****/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Radar : MonoBehaviour {

    private List<GameObject> trackedObjects; // habitat modules (the center of each outpost)

    private List<GameObject> radarObjects;

    public GameObject radarPrefab; // Assets/Prefab/Radar/ObjectPositionOnRadar

    private List<GameObject> borderObjects;

    private float switchDistance;

    public Transform helpTransform;

    private GameObject radarObject;

    void OutpostGenerated(GameObject habitatModule) // called from sendmessage() in DistantHabitatModule.cs
    {
        trackedObjects.Add(habitatModule);
        CreateRadarObjects();
        Debug.Log("tracked object num = " + trackedObjects.Count);
    }

    
    
    void Start()
    {
        switchDistance = 5.5f;
        //GameObject habitatModule = GameObject.FindWithTag("ModuleBuilding").GetComponent<Building>().initialModules[0];
        trackedObjects = new List<GameObject>();
        //trackedObjects.Add(habitatModule);
        radarObject = GameObject.Find("RadarBackground");
        radarObject.AddComponent<CanvasGroup>();
        radarObject.GetComponent<CanvasGroup>().alpha = 1;

        radarObjects = new List<GameObject>();
        borderObjects = new List<GameObject>();
        //CreateRadarObjects();
    }



    void Update()
    {
        for (int i = 0; i < radarObjects.Count; i++)
        {
            if (Vector3.Distance(radarObjects[i].transform.position, transform.position) > switchDistance)
            {
                helpTransform.LookAt(radarObjects[i].transform);
                borderObjects[i].transform.position = transform.position + switchDistance * helpTransform.forward;
                borderObjects[i].layer = LayerMask.NameToLayer("Radar");
                radarObjects[i].layer = LayerMask.NameToLayer("Invisible");
            }
            else
            {
                borderObjects[i].layer = LayerMask.NameToLayer("Invisible");
                radarObjects[i].layer = LayerMask.NameToLayer("Radar");
            }
        }

        if (!CentralControl.radarModuleExists)
        {
            radarObject.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            radarObject.GetComponent<CanvasGroup>().alpha = 1;
        }
    }


    void CreateRadarObjects()
    {
        radarObjects.Clear();
        borderObjects.Clear();

        Debug.Log("num trackedObjects = " + trackedObjects.Count);

        foreach (GameObject o in trackedObjects)
        {
            GameObject k = Instantiate(radarPrefab, o.transform.position, Quaternion.identity) as GameObject;
            k.transform.position = new Vector3(k.transform.position.x, k.transform.position.y, 2.0f);
            radarObjects.Add(k);
            GameObject j = Instantiate(radarPrefab, o.transform.position, Quaternion.identity) as GameObject;
            j.transform.position = new Vector3(j.transform.position.x, j.transform.position.y, 2.0f);
            borderObjects.Add(j);
            Debug.Log("1111111111111111111");
        }
    }
}
