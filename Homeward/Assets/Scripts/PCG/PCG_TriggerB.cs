using UnityEngine;
using System.Collections;

public class PCG_TriggerB : MonoBehaviour 
{

    [HideInInspector]
    public bool playerInsideCircleTriggerA1 = false;
    [HideInInspector]
    public bool playerExitedCircleTriggerA1 = false;

    private bool doOnce = false;

    void Start()
    {

        playerInsideCircleTriggerA1 = false;
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        playerInsideCircleTriggerA1 = true;
        playerExitedCircleTriggerA1 = false;
        Debug.Log("Enter");
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        playerInsideCircleTriggerA1 = false;
        playerExitedCircleTriggerA1 = true;
    }
}
