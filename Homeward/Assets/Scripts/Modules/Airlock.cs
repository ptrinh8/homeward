﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// class handles the airlock(door to inside and outside)
public class Airlock : MonoBehaviour {
    public bool xEnter;	// whether the doorway open for x axis
	private GameObject mainPlayer;
	private PlayerController playerController;
	private float x, y;	// Record the direction when player enters the trigger
	[HideInInspector]
	public bool isDoorway;
    private bool playerEnterCheckFlag;

    private SpriteRenderer spriteRenderer;
    private List<BoxCollider2D> colliders = new List<BoxCollider2D>();
    private AirControl airControl;
    private bool isToOutside;
    private KeyCode manuallyOperateKey = KeyCode.F;
    private LocalControl moduleControl;

    public bool IsToOustside
    {
        get { return isToOutside;}
        set { this.isToOutside = value;}
    }

    void Awake()
    {
        isDoorway = true;
        isToOutside = true;
    }

	// Use this for initialization
	void Start () {
		mainPlayer = GameObject.FindGameObjectWithTag("Player");
		playerController = mainPlayer.GetComponent<PlayerController>();
		x = y = 0;
		
		// Change the xEnter to meet the rotation
		int rotation = (int) gameObject.transform.root.rotation.eulerAngles.z;
		if (rotation == 90 || rotation == 270)
			xEnter = !xEnter;
        playerEnterCheckFlag = true;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        colliders = gameObject.GetComponents<BoxCollider2D>().ToList();
        airControl = gameObject.transform.root.gameObject.GetComponent<AirControl>();
        moduleControl = gameObject.transform.root.gameObject.GetComponent<LocalControl>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector2.Distance(mainPlayer.transform.position, transform.position) < transform.root.localScale.x * 0.35f)
        {
            AirCheck();
        }
        else
        {
            AirlockTriggered(true);
        }
		gameObject.GetComponent<SpriteRenderer>().material = transform.root.gameObject.GetComponent<SpriteRenderer>().material;
	}

	// Record the direction when player enters the trigger
	void OnTriggerStay2D (Collider2D other) 
    {
		if (other.gameObject.tag == "Player") {
            if (playerEnterCheckFlag)
            {
				x = mainPlayer.GetComponent<Rigidbody2D>().velocity.x;
				y = mainPlayer.GetComponent<Rigidbody2D>().velocity.y;
                playerEnterCheckFlag = false;
            }
        }
    }

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			// should not show indoor if player enters and exits the trigger in a "z" route
			if (xEnter) 
			{
				// should not show indoor if player enters and exits the trigger in same direction
				if (mainPlayer.GetComponent<Rigidbody2D>().velocity.x * x > 0)
				{
					if (gameObject.transform.root.gameObject.tag == "HabitatModule")
					{
						gameObject.SendMessageUpwards("HabitatModuleDoorWayTriggered", isDoorway, SendMessageOptions.RequireReceiver);
					}
					else 
					{
						gameObject.SendMessageUpwards("DoorWayTriggered", isDoorway, SendMessageOptions.RequireReceiver);
					}
				}
			} 
			else if (mainPlayer.GetComponent<Rigidbody2D>().velocity.y * y > 0)
			{
				if (gameObject.transform.root.gameObject.tag == "HabitatModule")
				{
					gameObject.SendMessageUpwards("HabitatModuleDoorWayTriggered", isDoorway, SendMessageOptions.RequireReceiver);
				}
				else 
				{
					gameObject.SendMessageUpwards("DoorWayTriggered", isDoorway, SendMessageOptions.RequireReceiver);
				}
			}
            playerEnterCheckFlag = true;
		}
    }

    void AirInManually()
    {
        if (!moduleControl.isOn || !moduleControl.IsPowered || moduleControl.IsBroken)
        {
            if (Input.GetKeyDown(manuallyOperateKey))
            {
                airControl.Flag = -1;
                airControl.Timer += Time.deltaTime * airControl.manuallyOperateDifficulty;
                airControl.airPressureBar = airControl.Timer / airControl.duration;
            }
        }
    }

    void AirOutManually()
    {
        if (!moduleControl.isOn || !moduleControl.IsPowered || moduleControl.IsBroken)
        {
            if (Input.GetKeyDown(manuallyOperateKey))
            {
                airControl.Flag = 1;
                airControl.Timer -= Time.deltaTime * airControl.manuallyOperateDifficulty;
                airControl.airPressureBar = airControl.Timer / airControl.duration;
            }
        }
    }

    void AirlockTriggered(bool disabled)
    {
        if (spriteRenderer.enabled != disabled)
        {
            spriteRenderer.enabled = disabled;
			foreach (BoxCollider2D collider in colliders)
			{
				if (!collider.isTrigger)
					collider.enabled = disabled;
			}
        }
    }

	// method handles the situation when players go inside or outside
    void AirCheck()
    {
        if (CentralControl.isInside)
        {
            if (gameObject.GetComponentInParent<DoorWayController>().ConnectedTo != null)
            {
                if (gameObject.GetComponentInParent<DoorWayController>().ConnectedTo.GetComponentInParent<AirControl>().Air)
                {
                    if (airControl.Air && airControl.airPressureBar == 1)
                    {
                        AirlockTriggered(false);
                    }
                    else
                    {
                        if (airControl.Timer < airControl.duration)
                        {
                            AirInManually();
                            if (moduleControl.isOn && moduleControl.IsPowered && !moduleControl.IsBroken)
                            {
                                if (Input.GetKeyDown(manuallyOperateKey))
								{
                                    gameObject.SendMessageUpwards("AirModuleActivite");
								}
                            }
                        }
                        else
                        {
                            AirlockTriggered(false);
                            airControl.airPressureBar = 1;
                            airControl.Air = true;
                            airControl.Flag = 0;
                        }
                    }
                }
                else
                {
                    if (airControl.Air && airControl.airPressureBar == 1)
                    {
                        if (airControl.Timer < airControl.duration)
                        {
                            AirInManually();
                            if (moduleControl.isOn && moduleControl.IsPowered && !moduleControl.IsBroken)
                            {
                                if (Input.GetKeyDown(manuallyOperateKey))
								{
                                    gameObject.SendMessageUpwards("AirModuleActivite");
								}
                            }
                        }
                        else
                        {
                            AirlockTriggered(false);
                            airControl.airPressureBar = 1;
                            airControl.Air = true;
                            airControl.Flag = 0;
                        }
                    }
                    else
                    {
                        AirlockTriggered(false);
                    }
                }
            }
            else
            {
                if (airControl.Air)
                {
                    if (airControl.Timer > 0)
                    {
                        AirOutManually();
                    }
                    else
                    {
                        AirlockTriggered(false);
                        airControl.airPressureBar = 0;
                        airControl.Air = false;
                        airControl.Flag = 0;
                    }
                }
                else if (airControl.airPressureBar == 0)
                {
                    AirlockTriggered(false);
                }
            }
        }
        else
        {
            if (!airControl.Air)
            {
                AirlockTriggered(false);
            }
            else
            {
                if (airControl.Timer > 0)
                {
                    AirOutManually();
                    if (moduleControl.isOn && moduleControl.IsPowered && !moduleControl.IsBroken)
                    {
                        if (Input.GetKeyDown(manuallyOperateKey))
                            gameObject.SendMessageUpwards("AirModuleActivite");
                    }
                }
                else
                {
                    AirlockTriggered(false);
                    airControl.airPressureBar = 0;
                    airControl.Air = false;
                    airControl.Flag = 0;
                }
            }
        }
    }
}
