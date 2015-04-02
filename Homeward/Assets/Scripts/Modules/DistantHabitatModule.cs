using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DistantHabitatModule : MonoBehaviour {

    private Dictionary<GameObject, float> habitatModules;
    private GameObject player;
    private GameObject minObject;
    public static float min;

	// Use this for initialization
	void Start () {
        habitatModules = new Dictionary<GameObject, float>();
        player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        Refresh();
        min = habitatModules[minObject];
	}

    void Register(GameObject habitatModule)
    {
        habitatModules.Add(habitatModule, Vector2.Distance(habitatModule.transform.position, player.transform.position));
        minObject = habitatModule;
    }

    void Refresh()
    {
        var keys = new List<GameObject>(habitatModules.Keys);
        foreach (var key in keys)
        {
            habitatModules[key] = Vector2.Distance(key.transform.position, player.transform.position);
            if (habitatModules[key] < habitatModules[minObject])
            {
                minObject = key;
            }
        }

    }
}
