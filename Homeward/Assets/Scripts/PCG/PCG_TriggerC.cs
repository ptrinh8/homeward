using UnityEngine;
using System.Collections;

public class PCG_TriggerC : MonoBehaviour 
{

    [HideInInspector]
    public bool playerInsideCircleTriggerA2 = false;
    [HideInInspector]
    public bool playerExitedCircleTriggerA2 = false;

    private bool doOnce = false;

    void Start()
    {

        playerInsideCircleTriggerA2 = false;
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        playerInsideCircleTriggerA2 = true;
        playerExitedCircleTriggerA2 = false;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        playerInsideCircleTriggerA2 = false;
        playerExitedCircleTriggerA2 = true;
    }
}
