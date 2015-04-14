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

	private Vector2 position;
	private int quadrant;

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
		quadrant = Random.Range(0, 3);
		if (quadrant == 0)
		{
			position.x = Random.Range(350f, 400f);
			position.y = Random.Range(350f, 400f);
		}
		else if (quadrant == 1)
		{
			position.x = Random.Range(0f, 250f);
			position.y = Random.Range(350f, 400f);
		}
		else if (quadrant == 3)
		{
			position.x = Random.Range(0, 250f);
			position.y = Random.Range(0f, 250f);
		}
		else if (quadrant == 4)
		{
			position.x = Random.Range(350f, 400f);
			position.y = Random.Range(0f, 250f);
		}

		this.transform.position = position;
    }

    void Update()
    {
        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;
		
        distance = Vector2.Distance(transform.position, mainPlayerPos);
    }
}
