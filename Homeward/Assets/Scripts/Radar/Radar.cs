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

    void OutpostGenerated(GameObject habitatModule) // called from sendmessage() in DistantHabitatModule.cs
    {
        trackedObjects.Add(habitatModule);
        DestroyRadarObjects();
        CreateRadarObjects();
    }

    
    
    void Start()
    {
        switchDistance = 5.5f;
        trackedObjects = new List<GameObject>();
        radarObjects = new List<GameObject>();
        borderObjects = new List<GameObject>();
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
    }

    void DestroyRadarObjects()
    {
        foreach (GameObject i in radarObjects)
        {
            Destroy(i);
        }

        foreach (GameObject k in borderObjects)
        {
            Destroy(k);
        }
    }

    void CreateRadarObjects()
    {
        radarObjects.Clear();
        borderObjects.Clear();

        foreach (GameObject o in trackedObjects)
        {
            GameObject k = Instantiate(radarPrefab, o.transform.position, Quaternion.identity) as GameObject;
            k.transform.position = new Vector3(k.transform.position.x, k.transform.position.y, 2.0f);
            radarObjects.Add(k);
            GameObject j = Instantiate(radarPrefab, o.transform.position, Quaternion.identity) as GameObject;
            j.transform.position = new Vector3(j.transform.position.x, j.transform.position.y, 2.0f);
            borderObjects.Add(j);
        }
    }

    
}
