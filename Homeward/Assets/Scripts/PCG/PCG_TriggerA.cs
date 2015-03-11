// ==================================================================================
// <file="PCG_TriggerA.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;

public class PCG_TriggerA : MonoBehaviour 
{   
    [HideInInspector]
    public bool playerInsideTriggerA = false;
    [HideInInspector]
    public bool playerExitedTriggerA = false;

    private bool doOnce = false;

	void Start () 
    {

        playerInsideTriggerA = false;
	}

	void Update () 
    {

	}

    void OnTriggerEnter2D(Collider2D other)
    {
        playerInsideTriggerA = true;
        playerExitedTriggerA = false;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        playerInsideTriggerA = false;
        playerExitedTriggerA = true;
    }
}
