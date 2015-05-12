﻿// ==================================================================================
// <file="PCG_DeleteElementsMountedByRocksANDModules.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;

public class PCG_DeleteElementsMountedByRocksANDModules : MonoBehaviour
{
    [HideInInspector]
    public bool rockInsidePolygonTrigger = false;

    private bool doOnce = false;

    void Start() { rockInsidePolygonTrigger = false; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Rock1") || (other.gameObject.tag == "Modules" ) || (other.gameObject.tag == "HabitatModule") || (other.gameObject.tag == "HealthStaminaModule") || (other.gameObject.tag == "RefineryModule"))
        {
            rockInsidePolygonTrigger = true;
            Destroy(gameObject);
        }
    }
}
