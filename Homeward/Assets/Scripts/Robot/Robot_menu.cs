// ==========================================================================
// <file="Robot_menu.cs" product="Homeward">
// <date>04-02-2014</date>
// ==========================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot_menu : MonoBehaviour 
{
    private PlayerController playerController = new PlayerController();
    private Robot_main robot_main = new Robot_main();

    private GameObject[] robotMenuObjects;

	void Start () 
    {
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        robot_main = FindObjectOfType(typeof(Robot_main)) as Robot_main;

        playerController.robotMenuActive = false;
	}
	
	void Update () 
    {
        if (robot_main.playerInteractWithRobot)
        {
            if (playerController.robotMenuActive)
            {
                this.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            }
            if (!playerController.robotMenuActive)
            {
                this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
        else if (!robot_main.playerInteractWithRobot)
        {
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            playerController.robotMenuActive = false;
        }
	}
}
