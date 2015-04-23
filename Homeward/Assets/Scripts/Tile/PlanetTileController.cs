using UnityEngine;
using System.Collections;

public class PlanetTileController : MonoBehaviour {

    [HideInInspector]
    public static bool probeSpawnedFlag;
    [HideInInspector]
    public bool playerIsInside;

    
	void Start () 
    {
        probeSpawnedFlag = false;
	}
	
	void Update () 
    {
	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIsInside = true;
        }
    }
}
