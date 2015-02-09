using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class CentralControl : MonoBehaviour {

	public Sprite indoorSprite;
	public Sprite outdoorSprite;
	public static bool isInside; // This static variable is the general location of player(whether inside)
	private SpriteRenderer spriteRenderer;
	[HideInInspector]
	public bool isEnter;
	private int moduleID;
	private static bool[] visited;

	private List <GameObject> locals; // List of all the modules within the outpost

	private int durability;
	private DayNightController dayNightController;
	private float durabilityTimer;
	private float durabilityLossTime;
	public float durabilityLossSpeed;
	private bool isBroken;
	private Text moduleStatusText;
	private int pos; 

    /*** UI module flags by Takahide ***/
    public static bool healthStaminaModuleExists;
    public static bool moduleControlModuleExists;
    public static bool radarModuleExists;

	// Use this for initialization
	void Start () {
		locals = new List<GameObject>(); 

		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		isEnter = true;
		isInside = isEnter;

		moduleStatusText = gameObject.GetComponentInChildren<Text>();
		dayNightController = GameObject.Find ("DayNightController").GetComponent<DayNightController>();
		durability = 100;
		durabilityLossTime = (dayNightController.dayCycleLength * 4) / 100;
		durabilityLossSpeed = 1;

        /*** UI module flags ***/
        healthStaminaModuleExists = false;
        moduleControlModuleExists = false;
        radarModuleExists = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isEnter) {
			ShowInside();
		}
		else ShowOutside();
		
//		Debug.Log(powerInGeneral);
	}

	void ShowInside () {
		// Show inside sprite of habitat module
		spriteRenderer.sprite = indoorSprite;
		spriteRenderer.sortingOrder = -3;
		// Show inside sprite of all other modules within the outpost
		foreach (GameObject local in locals)
			local.SendMessage("ShowInside");
	}

	void ShowOutside () {
		spriteRenderer.sprite = outdoorSprite;
		spriteRenderer.sortingOrder = -1;
		foreach (GameObject local in locals)
			local.SendMessage("ShowOutside");
	}

	void DoorWayTriggered () {
		isEnter = !isEnter;
		isInside = isEnter;
	}

	void ChangeLocation (bool isEnter) {
		this.isEnter = isEnter;
	}

	void AddLocal (GameObject local) {
		local.GetComponent<LocalControl>().moduleID = locals.Count;
		locals.Add(local);
	}

	void BFS(int startModuleId, bool isWholeMap) {
		while (!locals[startModuleId].GetComponent<LocalControl>().isOn) {
			startModuleId++;
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
//		Debug.Log(locals[startModuleId]);
		while (Q.Count != 0) {
			int front = (int)Q.Peek();
//			Debug.Log(front);
			Q.Dequeue();
			foreach (GameObject connection in locals[front].GetComponent<LocalControl>().connections) {
//				Debug.Log(connection);
				if (connection.tag == "HabitatModule") {
					powerInGeneral += 2;
				} else if (!visited[connection.GetComponent<LocalControl>().moduleID] && connection.GetComponent<LocalControl>().isOn) {
					Debug.Log(connection);

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
//		Debug.Log(powerConsumptionInGeneral);
		for (int i = 0; i < locals.Count; i++) {
			if (justVisited[i])
				locals[i].GetComponent<LocalControl>().powerLevel = powerInGeneral / powerConsumptionInGeneral;
			if (!visited[i] && locals[i].GetComponent<LocalControl>().isOn && isWholeMap)
				BFS(i, false);
		}
	}

	void CheckPowerSupply() {
		BFS (0, true);
		foreach (GameObject local in locals)
			local.SendMessage("CheckPowerSupply");
	}

/**	void DurabilityLoss() {
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
			SwitchTriggered(false);
			checkFlag = true;
		}
	}
	**/
}
