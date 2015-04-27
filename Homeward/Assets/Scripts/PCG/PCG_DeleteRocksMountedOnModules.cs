// ==================================================================================
// <file="PCG_DeleteRocksMountedOnModules.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;

public class PCG_DeleteRocksMountedOnModules : MonoBehaviour
{
    [HideInInspector]
    public bool rockInsidePolygonTrigger = false;

    private bool doOnce = false;

    void Start() { rockInsidePolygonTrigger = false; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Rock1")
        {
            rockInsidePolygonTrigger = true;
            Destroy(other.gameObject);
        }
    }
}
