// ========================================================================
// <file="Refining.cs" product="Homeward">
// <date>2014-11-11</date>
// ========================================================================


using UnityEngine;
using System.Collections;
using System;

public class BuildingModule : MonoBehaviour 
{
	private PlayerController playerController;
	private Mining minerals = new Mining();
	private Inventory inventory = new Inventory();
	
	private Vector2 worldSpacePos;
	private float loadingStartTime;
	public float loadingUpdateTime;
	private float loadingPercent;
	private bool timerReached = false;
	
	private bool stopMineralsIntake = false;
	private bool updateNumberOfRefinedMinerals = false;
	private bool addRefinedMineralOnce = false;
	
	private GameObject debugLoadingText;
	private GameObject debugMineralsAddedText;
	private GameObject debugMineralsRefinedText;
	
	public Sprite activeTexture;
	public Sprite deactiveTexture;
	public Sprite noPowerSupplyTexture;
	private GameObject refineryModule;
	
	private int mineralType1Count;
	private int mineralType2Count;
	private int mineralType3Count;

	private bool hasMineralType1;
	private bool hasMineralType2;
	private bool hasMineralType3;
	
	public bool showPlayerAndModuleInventory; // takahide made it public to use this outside
	public GameObject moduleInventory;
	private GameObject mainPlayer;
	
	private KeyCode keyToAddItemsFromMainPlayerInventory = KeyCode.K;
	private KeyCode keyToAddItemsDirectlyToModuleinventory = KeyCode.L;
	
	public AudioController audioController;
	public float distanceBetweenPlayerAndRefinery;
	private FMOD.Studio.EventInstance refineryMachine;
	private FMOD.Studio.ParameterInstance startingStopping;
	private FMOD.Studio.ParameterInstance distance;
	private FMOD.Studio.ParameterInstance airlockPressure;
	private FMOD.Studio.PLAYBACK_STATE refineryPlaybackState;
	private float refineryStartingStopping;
	private float refineryDistance;
	private bool refineryStarted;
	private bool startRefiningProcess = false;
	public float time = 0.0F;
	public float refinerySoundPressure;
	
	private void MineralsValidations()
	{
		/*
        if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral) == 0)
        {
           stopMineralsIntake = true;
        }
        else if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral) > 0)
        {
            stopMineralsIntake = false;
        }*/
	}
	
	private void RefiningProcess(SpriteRenderer refineryModuleSpriteRenderer)
	{
		startTimer();
		updateNumberOfRefinedMinerals = false;
		refineryModuleSpriteRenderer.sprite = activeTexture;
		
		if (loadingUpdateTime == time)
		{
			Debug.Log("ClockStarted");
			stopTimer();
			refineryModuleSpriteRenderer.sprite = deactiveTexture;
			
			if (!updateNumberOfRefinedMinerals)
			{
				addRefinedMineralOnce = false;
				updateNumberOfRefinedMinerals = true;
				
				if (!addRefinedMineralOnce)
				{
					if (hasMineralType3 == true)
					{
						Item item = GameObject.Find("Wire").GetComponent<Item>();
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						item = GameObject.Find("Screw").GetComponent<Item>();
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						item = GameObject.Find("Metal").GetComponent<Item>();
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Mineral3);
					}
					else if (hasMineralType2 == true)
					{
						Item item = GameObject.Find("Wire").GetComponent<Item>();
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						item = GameObject.Find("Metal").GetComponent<Item>();
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						item = GameObject.Find("Screw").GetComponent<Item>();
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Mineral2);
					}
					else if (hasMineralType1 == true)
					{
						Item item = GameObject.Find("Wire").GetComponent<Item>();
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						item = GameObject.Find("Metal").GetComponent<Item>();
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						item = GameObject.Find("Screw").GetComponent<Item>();
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().AddItem(item);
						moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Mineral1);
					}
					addRefinedMineralOnce = true;
					timerReached = false;
				}
			}
		}
	}
	
	void Start () 
	{
		playerController = GameObject.Find ("MainPlayer").GetComponent<PlayerController>();
		minerals = FindObjectOfType(typeof(Mining)) as Mining;
		inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
		
		moduleInventory = Instantiate(moduleInventory) as GameObject;
		moduleInventory.transform.SetParent(GameObject.Find("Canvas").transform);
		moduleInventory.GetComponent<Inventory>().slots = 4; //tak added 3/11
		moduleInventory.GetComponent<Inventory>().rows = 1; //tak added 3/11
		moduleInventory.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
		showPlayerAndModuleInventory = false;
		moduleInventory.SetActive(true);
		
		mainPlayer = GameObject.Find("MainPlayer");
		refineryModule = gameObject;
		worldSpacePos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
		
		audioController = GameObject.Find ("AudioObject").GetComponent<AudioController>();
		refineryMachine = FMOD_StudioSystem.instance.GetEvent("event:/RefineryMachine");
		refineryMachine.getPlaybackState(out refineryPlaybackState);
		refineryMachine.getParameter("StartingStopping", out startingStopping);
		refineryMachine.getParameter("Distance", out distance);
		refineryMachine.getParameter("AirlockPressure", out airlockPressure);
		refinerySoundPressure = 0f;
		refineryDistance = 0f;
		refineryStartingStopping = 0f;
		refineryStarted = false;
		time = 1000.0F;
		
	}
	
	void Update()
	{
		//		Debug.Log (showPlayerAndModuleInventory);
		// Taylor
		gameObject.GetComponent<SpriteRenderer>().material = transform.root.gameObject.GetComponent<SpriteRenderer>().material;
		// Taylor
		
		refineryDistance = Vector2.Distance(this.transform.position, playerController.transform.position);
		mineralType1Count = moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral1);
		mineralType2Count = moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral2);
		mineralType3Count = moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral3);
		//Debug.Log (mineralType1Count);
		//Debug.Log(distanceBetweenPlayerAndRefinery);
		refineryMachine.getPlaybackState(out refineryPlaybackState);
		startingStopping.setValue(refineryStartingStopping);
		distance.setValue(refineryDistance);
		airlockPressure.setValue(audioController.controllerSoundPressure);
		var refineryModuleSpriteRenderer = refineryModule.GetComponent<SpriteRenderer>();
		if (!gameObject.transform.root.gameObject.GetComponent<LocalControl>().IsPowered || gameObject.transform.root.gameObject.GetComponent<LocalControl>().IsBroken ||
		    !gameObject.transform.root.gameObject.GetComponent<LocalControl>().isOn)
		{
			refineryModuleSpriteRenderer.sprite = noPowerSupplyTexture;
		}
		else
		{
			refineryModuleSpriteRenderer.sprite = deactiveTexture;
		}
		
		changeLoadingToPercent();
		MineralsValidations();
		CheckMineralTypes();
		
		if (moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral1) == 10)
		{
			Debug.Log("This happened");
			stopMineralsIntake = true;
		}
		
		if (mineralType1Count >= 1 || mineralType2Count >= 1 || mineralType3Count >= 1)
		{
			RefiningProcess(refineryModuleSpriteRenderer);
		}
		
		if (mineralType1Count >= 1 || mineralType2Count >= 1 || mineralType3Count >= 1)
		{
			if (refineryStarted == false)
			{
				refineryMachine.start();
				refineryStartingStopping = 1.5f;
				refineryStarted = true;
			}
		}
		else
		{
			if (refineryStarted == true)
			{
				refineryStartingStopping = 2.5f;
				refineryStarted = false;
			}
		}
		
		if (!addRefinedMineralOnce)
		{
			addRefinedMineralOnce = true;
			//Item item = GameObject.Find("Material").GetComponent<Item>();
			//moduleInventory.GetComponent<Inventory>().AddItem(item);
			//mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
			moduleInventory.GetComponent<Inventory>().ClearSlot(ItemName.Mineral1);
			timerReached = false;
		}                
		
		if(startRefiningProcess == true)
		{
			
		}
		if (showPlayerAndModuleInventory == true)
		{
			//moduleInventory.SetActive(true);
			moduleInventory.GetComponent<CanvasRenderer>().SetAlpha(1);
			moduleInventory.GetComponent<Inventory>().SetSlotsActive(true);
		}
		else if (showPlayerAndModuleInventory == false)
		{
			moduleInventory.GetComponent<CanvasRenderer>().SetAlpha(0);
			//moduleInventory.SetActive(false);
			moduleInventory.GetComponent<Inventory>().SetSlotsActive(false);
		}
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		UIInventory.SetModuleInventory(moduleInventory); // takahide added
		//Taylor
		if (other.gameObject.tag == "Player")
		{
			Debug.Log ("entered");
			PlayerController.toolUsingEnable = false;
		}
	}

	void CheckMineralTypes()
	{
		if (mineralType1Count >= 1)
		{
			hasMineralType1 = true;
		}
		else
		{
			hasMineralType1 = false;
		}

		if (mineralType2Count >= 1)
		{
			hasMineralType2 = true;
		}
		else
		{
			hasMineralType2 = false;
		}

		if (mineralType3Count >= 1)
		{
			hasMineralType3 = true;
		}
		else
		{
			hasMineralType3 = false;
		}
	}
	
	void startTimer()
	{
		if (!timerReached) { loadingUpdateTime = loadingStartTime++; }
		if (loadingUpdateTime == time) { timerReached = true; }
	}
	
	void stopTimer()
	{
		if (timerReached == true) { loadingUpdateTime = 0.0f; loadingStartTime = 0.0f; }
	}
	
	void changeLoadingToPercent()
	{
		loadingPercent = loadingUpdateTime/5;
	}
}
