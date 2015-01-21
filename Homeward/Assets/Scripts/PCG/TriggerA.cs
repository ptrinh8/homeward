// ==================================================================================
// <file="TriggerA.cs" product="Homeward">
// <date>2014-12-11</date>
// ==================================================================================

using UnityEngine;
using System.Collections;

public class TriggerA : MonoBehaviour 
{
    [HideInInspector]
    public bool playerInsideTriggerA = false;

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
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        playerInsideTriggerA = false;
    }
}
