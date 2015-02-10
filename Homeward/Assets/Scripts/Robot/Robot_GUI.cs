// ==========================================================================
// <file="Robot_GUI.cs" product="Homeward">
// <date>04-02-2014</date>
// ==========================================================================

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Robot_GUI : MonoBehaviour 
{
    private Robot_main robot_main = new Robot_main(); 
    private Robot_wps robot_wps = new Robot_wps();
    private GameObject WP1_Text, WP2_Text;
    private Text WP1_text, WP2_text;

    public void WPbuttonA()
    {
        robot_wps.RobotTraverseTowardsWP1_ButtonClick();
    }

    public void WPbuttonB()
    {
        robot_wps.RobotTraverseTowardsWP2_ButtonClick();
    }

    public void ReactivateButton()
    {
        robot_wps.FollowBehaviorReactivate_ButtonClick();
    }

	void Start () 
    {
        robot_main = FindObjectOfType(typeof(Robot_main)) as Robot_main; 
        robot_wps = FindObjectOfType(typeof(Robot_wps)) as Robot_wps;
        WP1_Text = GameObject.FindGameObjectWithTag("Waypoint1Text");
        WP2_Text = GameObject.FindGameObjectWithTag("Waypoint2Text");


        WP1_text = WP1_Text.GetComponent<Text>();
        WP2_text = WP2_Text.GetComponent<Text>();

        WP1_text.text = "WY1 Location: ";
        WP2_text.text = "WY1 Location: ";

        GameObject buttonA = GameObject.FindGameObjectWithTag("WP1Button");
        buttonA.GetComponent<Button>().onClick.AddListener(() => { WPbuttonA(); });
        GameObject buttonB = GameObject.FindGameObjectWithTag("WP2Button");
        buttonB.GetComponent<Button>().onClick.AddListener(() => { WPbuttonB(); });
        GameObject buttonC = GameObject.FindGameObjectWithTag("ReactivateButton");
        buttonC.GetComponent<Button>().onClick.AddListener(() => { ReactivateButton(); });
        
	}
	
	void Update () 
    {
        if (robot_wps.addWP)
        {
            WP1_text.text = " WY1 Location" + robot_wps.robotLocation[0];
            WP2_text.text = " WY2 Location" + robot_wps.robotLocation[1];
        }
	}
}
