using UnityEngine;
using System.Collections;

public class Spaceship : MonoBehaviour 
{
    [HideInInspector]
    public bool endDemo = false;
    [HideInInspector]
    public bool playerInsideSpaceship = false;
    [HideInInspector]
    public bool playerExitedSpaceship = false;

    public bool DemoEnds
    {
        set { endDemo = value; }
        get { return endDemo; }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        playerInsideSpaceship = true;
        playerExitedSpaceship = false;
        Debug.Log("Inside");
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        playerInsideSpaceship = false;
        playerExitedSpaceship = true;
        Debug.Log("Outside");
    }

    void Start()
    {
    }

    void Update()
    {
        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;

        var distance = Vector2.Distance(transform.position, mainPlayerPos);
        Debug.Log("Distance to spaceship: " + distance);
    }
}
