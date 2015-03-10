using UnityEngine;
using System.Collections;

public class PCG_TriggerD : MonoBehaviour 
{

    [HideInInspector]
    public bool playerInsideCircleTriggerA3 = false;
    [HideInInspector]
    public bool playerExitedCircleTriggerA3 = false;

    private bool doOnce = false;

    void Start()
    {

        playerInsideCircleTriggerA3 = false;
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "MainPlayer")
        {
            playerInsideCircleTriggerA3 = true;
            playerExitedCircleTriggerA3 = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "MainPlayer")
        {
            playerInsideCircleTriggerA3 = false;
            playerExitedCircleTriggerA3 = true;
        }
    }
}
