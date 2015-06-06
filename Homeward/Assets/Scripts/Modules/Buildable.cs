﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// This class is for handling detached blueprint
public class Buildable : MonoBehaviour 
{
	public GameObject module;	// Completed module(prefabs)
	public int materialsRequired;	// Materials required to complete the module
	private int buildingProgress;	// Number of materials spend to build this module
	private SpriteRenderer spriteRenderer;	// Change of sprite color when needed
	private KeyCode buildKey = KeyCode.F;
	private KeyCode cancelKey = KeyCode.E;
	private Color color;
	private Color flashColor;

    private GameObject player;
    private float buildActionTime;
    private bool buildingFlag;
	private bool buildingNow; // whether the building action is conducting right now
	private float flashTimer;
	private bool flashSwitch;

	private int wiresRequired;
	private int screwsRequired;
	private int metalRequired;

	private int wiresSpent;
	private int screwsSpent;
	private int metalSpent;

	private int wiresLeft;
	private int screwsLeft;
	private int metalLeft;

	private bool wiresStillRequired;
	private bool screwsStillRequired;
	private bool metalStillRequired;

	private GameObject buildingBarBackground;
	private float fillspeed;
	private float buildingBarFillAmount;
	private int progress;

	private bool built = false;

	private PlayerController playerController;

	// Use this for initialization
	void Start () 
    {
        //itemDatabase = FindObjectOfType(typeof(ItemDatabase)) as ItemDatabase;
        //inventory  = FindObjectOfType(typeof(Inventory)) as Inventory;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		color = spriteRenderer.color;
		color.a = 0.5f;
		spriteRenderer.color = color;
		buildingProgress = 0;

        player = GameObject.FindWithTag("Player");
        buildingFlag = true;
        buildActionTime = .5f;
		flashTimer = 0;
		flashSwitch = true;
		buildingNow = false;

		// building bar
		buildingBarBackground = GameObject.Find("Canvas").transform.FindChild("Building Bar Background").gameObject;
		fillspeed = 0.02f;
		buildingBarFillAmount = 0;
		progress = 0;
		
		wiresRequired = 3;
		screwsRequired = 3;
		metalRequired = 3;

		wiresLeft = wiresRequired;
		screwsLeft = screwsRequired;
		metalLeft = metalRequired;

		wiresStillRequired = true;
		screwsStillRequired = true;
		metalStillRequired = true;

		
		playerController = GameObject.Find ("MainPlayer").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>() != null)
		{
			if (!buildingFlag)
			{
				if (buildingBarFillAmount >= 0.85f && buildingBarFillAmount <= 1f)
				{
					buildingBarBackground.GetComponent<Image>().color = Color.black;
					flashTimer += (0.05f + Time.deltaTime);
					Invoke("FlashBuildingBar", flashTimer);
					if (!buildingNow)
					{
						buildingNow = true;
						Invoke("BuildAction", buildActionTime);
						playerController.PlayBuildingAnimation();// play animation here!!!

					}
				}
				else
				{
					Reset();
				}
			}
			else
			{
				if (buildingBarFillAmount >= 0 && buildingBarFillAmount <= 0.85f)
				{
					buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = Color.red;
				}
				else if (buildingBarFillAmount > 0.85f && buildingBarFillAmount <= 0.93f)
				{
					buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = Color.yellow;
				}
				else if (buildingBarFillAmount > 0.93f && buildingBarFillAmount <= 0.98f)
				{
					buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = Color.green;
				}
				else if (buildingBarFillAmount > 0.98f && buildingBarFillAmount <= 1f)
				{
					buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = Color.blue;
				}
				buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().fillAmount = buildingBarFillAmount;
				// else fail?
			}
		}
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
		{
			if (PlayerController.holdingBuildingTool && PlayerController.toolUsingEnable)
			{
				if (other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Wire) > 0)
				{
					if (Input.GetKey(buildKey))
					{
						if (!buildingBarBackground.activeSelf)
						{
							if (buildingFlag)
							{
								buildingBarBackground.SetActive(true);
								buildingBarBackground.GetComponent<Image>().color = Color.gray;
								buildingBarBackground.transform.position = GameObject.Find("Canvas").transform.position + new Vector3(10, -10, 0);
								buildingBarFillAmount = 0;
							}
						}
						else if (buildingFlag)
						{
							buildingBarBackground.transform.position = GameObject.Find("Canvas").transform.position + new Vector3(10, -10, 0);
							if (buildingBarFillAmount <= 1)
							{
								buildingBarFillAmount +=  fillspeed;
							}
							else
							{
								Reset();
							}
						}
					}
					else
					{
						if (buildingFlag) 
						{
							buildingFlag = false;
							if (buildingBarFillAmount >= 0 && buildingBarFillAmount <= 0.85f)
							{
								progress = 0;
							}
							else if (buildingBarFillAmount > 0.85f && buildingBarFillAmount <= 0.93f)
							{
								progress = 1;
							}
							else if (buildingBarFillAmount > 0.93f && buildingBarFillAmount <= 0.98f)
							{
								progress = 2;
							}
							else if (buildingBarFillAmount > 0.98f && buildingBarFillAmount <= 1f)
							{
								progress = 3;
							}
						}
					}
				}
			}

            if (Input.GetKeyDown(cancelKey)){
                Destroy(gameObject);
				// return the materials?
			}
        }
		/*
		if (other.gameObject.tag == "Airlock" || other.gameObject.tag == "ConnectorModule" || other.gameObject.tag == "HealthStaminaModule" || other.gameObject.tag == "HabitatModule" || other.gameObject.tag == "ModuleControlModule"
		         || other.gameObject.tag == "PowerModule" || other.gameObject.tag == "RadarModule" || other.gameObject.tag == "RefineryModule" || other.gameObject.tag == "BuildingModule" || other.gameObject.tag == "MiningProbeModule"
		         || other.gameObject.tag == "OxygenModule" || other.gameObject.tag == "StorageModule")
		{
			if (wiresLeft == 0 && screwsLeft == 0 && metalLeft == 0)
			{
				if (built == true)
				{
					Debug.Log("destroying");
					Destroy(this.gameObject);
				}
			}
		}*/
    }

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			Reset();
		}
	}

    void BuildAction()
    {
		Debug.Log("Been here");

		if (wiresStillRequired == true)
		{
			if (wiresSpent + progress < wiresRequired)
			{
				wiresSpent += progress;
			}
			else
			{
				progress = wiresRequired - wiresSpent;
				wiresSpent = wiresRequired;
				wiresStillRequired = false;
			}
		}
		else if (screwsStillRequired == true)
		{
			if (screwsSpent + progress < screwsRequired)
			{
				screwsSpent += progress;
			}
			else
			{
				progress = screwsRequired - screwsSpent;
				screwsSpent = screwsRequired;
				screwsStillRequired = false;
			}
		}
		else if (metalStillRequired == true)
		{
			if (metalSpent + progress < metalRequired)
			{
				metalSpent += progress;
			}
			else
			{
				progress = metalRequired - metalSpent;
				metalSpent = metalRequired;
				metalStillRequired = false;
			}
		}
        color.a += (0.4f / materialsRequired) * progress;
        spriteRenderer.color = color;
		for (int i = 0; i < progress; i++)
		{
			if (wiresLeft > 0)
			{
				player.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().GetItem(ItemName.Wire);
				wiresLeft--;
			}
			else if (screwsLeft > 0)
			{
				player.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().GetItem(ItemName.Screw);
				screwsLeft--;
			}
			else if (metalLeft > 0)
			{
				player.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().GetItem(ItemName.Metal);
				metalLeft--;
			}
		}
		/*
		if (buildingProgress + progress < materialsRequired)
		{
			buildingProgress += progress;
		}
		else 
		{
			progress = materialsRequired - buildingProgress;
			buildingProgress = materialsRequired;
		}*/
		CheckBuild();
		Reset();
    }

	void Reset()
	{
		buildingFlag = true;
		flashSwitch = true;
		buildingBarFillAmount = 0;
		progress = 0;
		flashTimer = 0;
		buildingBarBackground.SetActive(false);
		buildingNow = false;
	}

	void CheckBuild()
	{
		if (wiresLeft == 0 && screwsLeft == 0 && metalLeft == 0) 
		{
			if (built == false)
			{
				built = true;
				Debug.Log ("building");
				Instantiate(module, gameObject.transform.position, Quaternion.identity);
				Destroy(this.gameObject);
			}
		}
	}

	void FlashBuildingBar()
	{
		flashColor = buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color;
		if (flashSwitch)
		{
			// change color to light
			flashColor.a = 0.5f;
		}
		else
		{
			// change color to dark
			flashColor.a = 1.0f;
		}
		buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = flashColor; 
		flashSwitch = !flashSwitch;
	}
}
