// ==========================================================================
// <file="Robot_main.cs" product="Homeward">
// <date>04-02-2014</date>
// ==========================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot_main : MonoBehaviour
{
    private PlayerController playerController = new PlayerController();
    private float speed = 1.0F;

    [HideInInspector]
    public bool robotFollowBehavior, playerInteractWithRobot;

    public bool RobotFollowBehavior
    {
        set { robotFollowBehavior = value; }
        get { return robotFollowBehavior; }
    }

    public bool PlayerInteractionWithRobot
    {
        set { playerInteractWithRobot = value; }
        get { return playerInteractWithRobot; }
    }
    
	void Start () 
    {
       playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
       robotFollowBehavior = true;
       playerInteractWithRobot = false;       
	}
	
	void Update () 
    {        
        if (robotFollowBehavior)
        {
            if (Vector2.Distance(transform.position, playerController.playerPosition) > 1.0F)
            {
                this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, playerController.playerPosition, speed * Time.deltaTime);
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") { playerInteractWithRobot = true; }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") { playerInteractWithRobot = false; }
    }
}


