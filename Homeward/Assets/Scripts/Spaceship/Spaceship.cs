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

    public static float distance;

    public bool DemoEnds
    {
        set { endDemo = value; }
        get { return endDemo; }
    }

    void HideUI()
    {
        GameObject[] bar = GameObject.FindGameObjectsWithTag("Bar");
        foreach(GameObject item in bar)
        {
        item.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "MainPlayer")
        {
            HideUI();
            playerInsideSpaceship = true;
            playerExitedSpaceship = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "MainPlayer")
        {
            playerInsideSpaceship = false;
            playerExitedSpaceship = true;
        }
    }

    void Start()
    {
    }

    void Update()
    {
        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;

        distance = Vector2.Distance(transform.position, mainPlayerPos);
    }
}
