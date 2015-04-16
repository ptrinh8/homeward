﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class CentralControl : MonoBehaviour {

	public Sprite indoorSprite;
	public Sprite outdoorSprite;
	public static bool isInside; // This static variable is the general location of player(whether inside)
	private SpriteRenderer spriteRenderer;
	[HideInInspector]
	public bool isEnterOutpost, isEnter;
	private int moduleID;
	private static bool[] visited;
	private List <GameObject> locals; // List of all the modules within the outpost

    public List<GameObject> Locals
    {
        get { return locals; }
    }

	public int durability;
	private DayNightController dayNightController;
	private float durabilityTimer;
	public float durabilityLossTime;
	public float durabilityLossSpeed;
	private bool isBroken;
	private Text moduleStatusText;
	private int pos;

    public GameObject refineryModule;
    //public GameObject foodModule;
    public GameObject airlockModule;

    /*** UI module flags by Takahide ***/
    public static bool healthStaminaModuleExists;
    public static bool moduleControlModuleExists;
    public static bool radarModuleExists;
	private KeyCode repairKey = KeyCode.F;

    private GameObject player;
    private float repairActionTime;
    private bool repairingFlag;

	// Use this for initialization
	void Start () {
		locals = new List<GameObject>(); 
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		isEnter = true;
		isEnterOutpost = true;
		isInside = isEnterOutpost;
		ShowInside();
		moduleStatusText = gameObject.GetComponentInChildren<Text>();
		dayNightController = GameObject.Find ("DayNightController").GetComponent<DayNightController>();
		durability = 100;
		durabilityLossTime = (dayNightController.dayCycleLength * 4) / 100;

        /*** UI module flags ***/
        healthStaminaModuleExists = false;
        moduleControlModuleExists = false;
        radarModuleExists = false;

        player = GameObject.FindWithTag("Player");
        repairingFlag = true;
        repairActionTime = 0f;

        if (!GameObject.Find("Module Building").GetComponent<Building>().NewGameFlag)
            Invoke("InitializeRefineryModule", Time.deltaTime);

        player.SendMessage("OutpostGenerated", gameObject); // takahide added 04/02
        GameObject moduleControl = Instantiate(Resources.Load("ModuleControl/ModuleControl")) as GameObject;
        moduleControl.SendMessage("OutpostGenerated", gameObject);
		gameObject.GetComponent<SpriteRenderer>().material = GameObject.Find("DefaultSpriteMaterial").GetComponent<SpriteRenderer>().material;
	}

    void InitializeRefineryModule()
    {
        if (transform.rotation.eulerAngles.z == 0 || transform.rotation.eulerAngles.z == 360)
            refineryModule = Instantiate(refineryModule, new Vector3(transform.position.x + 2.85f, transform.position.y, 0), transform.rotation) as GameObject;
        else if (Mathf.Round(transform.rotation.eulerAngles.z) == 90)
            refineryModule = Instantiate(refineryModule, new Vector3(transform.position.x, transform.position.y + 2.85f, 0), transform.rotation) as GameObject;
        else if (transform.rotation.eulerAngles.z == 180)
            refineryModule = Instantiate(refineryModule, new Vector3(transform.position.x - 2.85f, transform.position.y, 0), transform.rotation) as GameObject;
        else if (transform.rotation.eulerAngles.z == 270)
            refineryModule = Instantiate(refineryModule, new Vector3(transform.position.x, transform.position.y - 2.85f, 0), transform.rotation) as GameObject;

        Invoke("InitializeRefineryModuleVariable", Time.deltaTime);
        Invoke("InitializeAirlockModule", Time.deltaTime);
    }

    void InitializeAirlockModule()
    {
        if (transform.rotation.eulerAngles.z == 0 || transform.rotation.eulerAngles.z == 360)
            airlockModule = Instantiate(airlockModule, new Vector3(transform.position.x + 4.7f, transform.position.y, 0), transform.rotation) as GameObject;
        else if (Mathf.Round(transform.rotation.eulerAngles.z) == 90)
            airlockModule = Instantiate(airlockModule, new Vector3(transform.position.x, transform.position.y + 4.7f, 0), transform.rotation) as GameObject;
        else if (transform.rotation.eulerAngles.z == 180)
            airlockModule = Instantiate(airlockModule, new Vector3(transform.position.x - 4.7f, transform.position.y, 0), transform.rotation) as GameObject;
        else if (transform.rotation.eulerAngles.z == 270)
            airlockModule = Instantiate(airlockModule, new Vector3(transform.position.x, transform.position.y - 4.7f, 0), transform.rotation) as GameObject;
        Invoke("InitializeAirlockModuleVariable", Time.deltaTime);
    }

    void InitializeRefineryModuleVariable()
    {
        refineryModule.GetComponent<LocalControl>().IsEnter = false;
    }

    void InitializeAirlockModuleVariable()
    {
        airlockModule.GetComponent<LocalControl>().IsEnter = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (isEnterOutpost) {
				ShowInside();
		}
		else ShowOutside();

		DurabilityLoss();
        DisplayText(ModuleControl.ShowModuleControl);
	}

	void ShowInside () {
		// Show inside sprite of habitat module
		spriteRenderer.sprite = indoorSprite;
		spriteRenderer.sortingOrder = -3;
		// Show inside sprite of all other modules within the outpost
		foreach (GameObject local in locals) {
			local.SendMessage("ShowInside");
			spriteRenderer.material = GameObject.Find("DefaultSpriteMaterial").GetComponent<SpriteRenderer>().material;
			local.GetComponent<SpriteRenderer>().material = GameObject.Find("DefaultSpriteMaterial").GetComponent<SpriteRenderer>().material;
		}
	}

	void ShowOutside () {
		spriteRenderer.sprite = outdoorSprite;
		spriteRenderer.sortingOrder = -1;
		foreach (GameObject local in locals) {
			local.SendMessage("ShowOutside");
			spriteRenderer.material = GameObject.Find("DayNightReactingMaterial").GetComponent<SpriteRenderer>().material;
			local.GetComponent<SpriteRenderer>().material = GameObject.Find("DayNightReactingMaterial").GetComponent<SpriteRenderer>().material;
		}
	}

	void DoorWayTriggered () {
		isEnterOutpost = !isEnterOutpost;
		isInside = isEnterOutpost; 
	}

	void HabitatModuleDoorWayTriggered (bool isDoorway) {
		if (isDoorway) {
			isEnterOutpost = !isEnterOutpost;
			isInside = isEnterOutpost;
		} 
		isEnter = !isEnter;
	}

	void ChangeLocation (bool isEnterOutpost) {
		this.isEnterOutpost = isEnterOutpost;
	}

	void AddLocal (GameObject local) {
		local.GetComponent<LocalControl>().moduleID = locals.Count;
		locals.Add(local);
	}

	void BFS(int startModuleId, bool isWholeMap) {
		if (locals.Count == 0)
			return;
		while (!locals[startModuleId].GetComponent<LocalControl>().isOn) {
			startModuleId++;
			if (startModuleId >= locals.Count)
				return;
		}
		bool[] justVisited = new bool[locals.Count];
		Queue Q = new Queue();
		float powerInGeneral = 0;
		float powerConsumptionInGeneral = 0;
		if (locals[startModuleId].GetComponent<LocalControl>().powerConsumption > 0)
			powerConsumptionInGeneral = locals[startModuleId].GetComponent<LocalControl>().powerConsumption;
		else powerInGeneral = -(locals[startModuleId].GetComponent<LocalControl>().powerConsumption);
		for (int i = 0; i < locals.Count; i++)
			justVisited[i] = false;
		if (isWholeMap) {
            healthStaminaModuleExists = false;
            moduleControlModuleExists = false;
            radarModuleExists = false;
			visited = new bool[locals.Count];
			for (int i = 0; i < locals.Count; i++)
				visited[i] = false;
		}
		justVisited[startModuleId] = true;
		visited[startModuleId] = true;
		Q.Enqueue(startModuleId);
		while (Q.Count != 0) {
			int front = (int)Q.Peek();
			Q.Dequeue();
			foreach (GameObject connection in locals[front].GetComponent<LocalControl>().connections) {
                if (connection.tag == "HabitatModule")
                {
                    //Debug.Log(locals[front]);
                    if (!isBroken)
                    {
                        powerInGeneral += 1;
                    }
                }
                else 
                    if (!visited[connection.GetComponent<LocalControl>().moduleID] && connection.GetComponent<LocalControl>().isOn) {
                    /*** Takahide Added From Here***/
                    if (connection.tag == "HealthStaminaModule")
                    {
                        healthStaminaModuleExists = true;
                    }
                    if (connection.tag == "ModuleControlModule")
                    {
                        moduleControlModuleExists = true;
                    }
                    if (connection.tag == "RadarModule")
                    {
                        radarModuleExists = true;
                    }
                    /*** Takahide code end ***/

					Q.Enqueue(connection.GetComponent<LocalControl>().moduleID);

					visited[connection.GetComponent<LocalControl>().moduleID] = true;

					justVisited[connection.GetComponent<LocalControl>().moduleID] = true;

					if (connection.GetComponent<LocalControl>().powerConsumption > 0)
						powerConsumptionInGeneral += connection.GetComponent<LocalControl>().powerConsumption;
					else 
						powerInGeneral -= connection.GetComponent<LocalControl>().powerConsumption;
				}
			}
		}
		for (int i = 0; i < locals.Count; i++) {
			if (justVisited[i])
				locals[i].GetComponent<LocalControl>().powerLevel = powerInGeneral / powerConsumptionInGeneral;
            if (!visited[i] && locals[i].GetComponent<LocalControl>().isOn && isWholeMap)
            {
				BFS(i, false);
            }
		}
	}

	void CheckPowerSupply() {
		BFS (0, true);
		foreach (GameObject local in locals)
			local.SendMessage("CheckPowerSupply");
	}
	
	void DurabilityLoss() {
		if (durability > 0)
		{
			durabilityTimer += Time.deltaTime * durabilityLossSpeed;
			if (durabilityTimer > durabilityLossTime)
			{
				durability -= 1;
				durabilityTimer = 0;
			}
		} else {
			isBroken = true;
			CheckPowerSupply();
		}

		if (!isBroken) {
			moduleStatusText.text = "+2, " + durability;
		}
		else
			moduleStatusText.text = "Broken";
		
		if (isEnter) {
            if (Input.GetKeyDown(repairKey))
            {
                if (player.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) > 0 && durability != 100)
                {
                    if (PlayerController.holdingRepairTool && PlayerController.toolUsingEnable)
                    {
                        if (repairingFlag)
                        {
                            repairingFlag = false;
                            RepairAction();
                            Invoke("TimeWait", repairActionTime);
                        }
                    }
                }
            }
		}
	}

    void RepairAction()
    {
        if (!isBroken)
        {
            if (durability + 10 <= 100)
            {
                durability += 10;
            }
            else durability = 100;
        }
        else
        {
            durability += 10;
            isBroken = false;
        }
        player.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
    }

    void TimeWait()
    {
        repairingFlag = true;
    }

    void DisplayText(bool flag)
    {
        if (flag)
        {
            moduleStatusText.enabled = true;
        }
        else
        {
            if (isEnter)
            {
                if (moduleStatusText.enabled == false)
                {
                    moduleStatusText.enabled = true;
                }
            }
            else
            {
                if (moduleStatusText.enabled == true)
                {
                    moduleStatusText.enabled = false;
                }
            }
        }
    }
}