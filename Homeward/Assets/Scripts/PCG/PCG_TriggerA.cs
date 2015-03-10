using UnityEngine;
using System.Collections;

public class PCG_TriggerA : MonoBehaviour 
{

    [HideInInspector]
    public bool playerInsideCircleTriggerA = false;
    [HideInInspector]
    public bool playerExitedCircleTriggerA = false;

    private bool doOnce = false;

    void Start()
    {

        playerInsideCircleTriggerA = false;
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        playerInsideCircleTriggerA = true;
        playerExitedCircleTriggerA = false;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        playerInsideCircleTriggerA = false;
        playerExitedCircleTriggerA = true;
    }
}
