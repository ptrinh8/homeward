// ==========================================================================
// <file="Robot_wps.cs" product="Homeward">
// <date>04-02-2014</date>
// ==========================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Robot_wps : MonoBehaviour 
{
    private Robot_main robot_main = new Robot_main();
    private PlayerController playerController = new PlayerController();

    public GameObject wayPointSymbol;
    private GameObject[] wayPoints = new GameObject[100];
    private int WP_count = 0;

    public Vector3[] robotLocation = new Vector3[30];
    public bool addWP, bothWPset;
    private bool traverseToWP1, traverseToWP2, followBehaviorReactivate;
    private bool _isFollowBehaviorActiveForButtonA, _isRobotMovingForButtonA, _isFollowBehaviorActiveForButtonB, _isRobotMovingForButtonB;
    private int _WPcounter = 0;

    public bool RobotWayPoints
    {
        set { addWP = value; }
        get { return addWP; }
    }

    public Vector3[] RobotLocation
    {
        set { robotLocation = value; }
        get { return robotLocation; }
    }

    public void RobotTraverseTowardsWP1_ButtonClick()
    {
        if (bothWPset)
        traverseToWP1 = true;
        _isFollowBehaviorActiveForButtonA = false;
        _isRobotMovingForButtonA = true;
    }
    
    private void RobotTraverseTowardsWP1_Event()
    {
        if (traverseToWP1 && bothWPset)
        {
            if (!_isFollowBehaviorActiveForButtonA)
            {
                _isFollowBehaviorActiveForButtonA = true;
                robot_main.robotFollowBehavior = false;
            }

            if (transform.position != robotLocation[0])
            {
                if (_isRobotMovingForButtonA)
                {
                    transform.position = Vector2.MoveTowards(transform.position, robotLocation[0], Time.deltaTime * 1.0F);
                }
            }
            else if(transform.position == robotLocation[0])
            {
                _isRobotMovingForButtonA = false;
            }
        }
    }

    public void RobotTraverseTowardsWP2_ButtonClick()
    {
        if (bothWPset)
            traverseToWP2 = true;
        _isFollowBehaviorActiveForButtonB = false;
        _isRobotMovingForButtonB = true;
    }

    private void RobotTraverseTowardsWP2_Event()
    {
        if (traverseToWP2 && bothWPset)
        {
            if (!_isFollowBehaviorActiveForButtonB)
            {
                _isFollowBehaviorActiveForButtonB = true;
                robot_main.robotFollowBehavior = false;
            }

            if (transform.position != robotLocation[1])
            {
                if (_isRobotMovingForButtonB)
                {
                    transform.position = Vector2.MoveTowards(transform.position, robotLocation[1], Time.deltaTime * 1.0F);
                }
            }
            else if (transform.position == robotLocation[1])
            {
                _isRobotMovingForButtonB = false;
            }
        }
    }

    public void FollowBehaviorReactivate_ButtonClick()
    {
        if (!robot_main.robotFollowBehavior)
        {
            followBehaviorReactivate = true;
        }
    }

    private void FollowBehaviorReactivate_Event()       
    {
        if (followBehaviorReactivate && !robot_main.robotFollowBehavior)
        {
            if (_isRobotMovingForButtonA == false || _isRobotMovingForButtonB == false)
            robot_main.robotFollowBehavior = true;
            followBehaviorReactivate = false;
        }
    }

    void AddWPsymbol()
    {
        wayPoints[WP_count] = Instantiate(wayPointSymbol, transform.position, transform.rotation) as GameObject;
        WP_count++;
    }

    void RemoveWPsymbol()
    {
        for (int i = 3; i < 100; i++)
        {
            if (WP_count == i) { Destroy(wayPoints[i - 3]); }
        }
    } 

	void Start () 
    {
        robot_main = FindObjectOfType(typeof(Robot_main)) as Robot_main;
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        playerController.addRobotWP = false;
        addWP = false;
        bothWPset = false;
        traverseToWP1 = false;
        _isFollowBehaviorActiveForButtonA = false;
        followBehaviorReactivate = false;
        _isRobotMovingForButtonA = true;
	}

    void AddRobotWaypoints()
    {
        if (playerController.addRobotWP)
        {
            if (!addWP)
            {
                addWP = true;
                if (_WPcounter % 2 == 0)
                {
                    robotLocation[0] = transform.position;
                    AddWPsymbol();
                }
                else if (_WPcounter % 2 != 0)
                {
                    robotLocation[1] = transform.position;
                    AddWPsymbol();
                    bothWPset = true;
                }
                _WPcounter++;
            }
        }
        else if (!playerController.addRobotWP)
        {
            addWP = false;
        }
    }
	
	void Update () 
    {
        AddRobotWaypoints(); 
        RemoveWPsymbol();
        RobotTraverseTowardsWP1_Event();
        RobotTraverseTowardsWP2_Event();
        FollowBehaviorReactivate_Event();
	}
}

