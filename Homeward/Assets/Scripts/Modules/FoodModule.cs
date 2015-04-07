// ========================================================================
// <file="Refining.cs" product="Homeward">
// <date>2014-11-11</date>
// ========================================================================


using UnityEngine;
using System.Collections;
using System;

public class FoodModule : MonoBehaviour 
{
    private PlayerController playerController = new PlayerController();
    private Mining minerals = new Mining();
    private Inventory inventory = new Inventory();

    private int refinedMineralsCreated;

    private Vector2 worldSpacePos;
    private float loadingStartTime;
    private float loadingUpdateTime;
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
    private float time = 0.0F;
	public float refinerySoundPressure;

    private void MineralsValidations()
    {
		/*
        if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) == 0)
        {
            stopMineralsIntake = true;
        }
        else if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) > 0)
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
                    addRefinedMineralOnce = true;
                    Item item = GameObject.Find("Food1").GetComponent<Item>();
                    moduleInventory.GetComponent<Inventory>().AddItem(item);
                    moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
                    moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
                    timerReached = false;
                }
            }
        }
    }

	void Start () 
    {
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        minerals = FindObjectOfType(typeof(Mining)) as Mining;
        inventory = FindObjectOfType(typeof(Inventory)) as Inventory;

        moduleInventory = Instantiate(moduleInventory) as GameObject;
        moduleInventory.transform.SetParent(GameObject.Find("Canvas").transform);
        moduleInventory.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        showPlayerAndModuleInventory = false;
        moduleInventory.SetActive(showPlayerAndModuleInventory);

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
		refineryDistance = Vector2.Distance(this.transform.position, playerController.transform.position);
		//Debug.Log(distanceBetweenPlayerAndRefinery);
		refineryMachine.getPlaybackState(out refineryPlaybackState);
		startingStopping.setValue(refineryStartingStopping);
		distance.setValue(refineryDistance);
		airlockPressure.setValue(GameObject.Find ("AudioObject").GetComponent<AudioController>().controllerPressure);
        var refineryModuleSpriteRenderer = refineryModule.GetComponent<SpriteRenderer>();
        if (!gameObject.transform.root.gameObject.GetComponent<LocalControl>().IsPowered)
        {
            refineryModuleSpriteRenderer.sprite = noPowerSupplyTexture;
        }
        else
        {
            refineryModuleSpriteRenderer.sprite = deactiveTexture;
        }

        changeLoadingToPercent();
        MineralsValidations();

        if (moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Material) == 10)
        {
            Debug.Log("This happened");
            stopMineralsIntake = true;
        }

        int _mineralCount = moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Material);

        if (_mineralCount == 2 || _mineralCount == 3 || _mineralCount == 4 || _mineralCount == 5 || _mineralCount == 6 ||
            _mineralCount == 7 || _mineralCount == 8 || _mineralCount == 9 || _mineralCount == 10)
        {
            RefiningProcess(refineryModuleSpriteRenderer);
        }

		if (_mineralCount > 0)
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
            Item item = GameObject.Find("Food1").GetComponent<Item>();
            moduleInventory.GetComponent<Inventory>().AddItem(item);
            //mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
            moduleInventory.GetComponent<Inventory>().ClearSlot(ItemName.Material);
            timerReached = false;
        }                

		if(startRefiningProcess == true)
		{
			
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        UIInventory.SetModuleInventory(moduleInventory); // takahide added
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.transform.root.gameObject.GetComponent<LocalControl>().IsPowered)
            {
                showPlayerAndModuleInventory = true;
                PlayerController.KeyCode_I_Works = !showPlayerAndModuleInventory;
                PlayerController.ShowPlayerInventory = showPlayerAndModuleInventory;
                moduleInventory.SetActive(true);
                moduleInventory.GetComponent<Inventory>().SetSlotsActive(showPlayerAndModuleInventory);
            }
            else
            {
                showPlayerAndModuleInventory = false;
                moduleInventory.SetActive(showPlayerAndModuleInventory);
                moduleInventory.GetComponent<Inventory>().SetSlotsActive(showPlayerAndModuleInventory);

                PlayerController.ShowPlayerInventory = showPlayerAndModuleInventory;
                PlayerController.KeyCode_I_Works = !showPlayerAndModuleInventory;
            }

            if (showPlayerAndModuleInventory) 
            {
                if (Input.GetKeyDown(keyToAddItemsDirectlyToModuleinventory))
                {
                    if ( moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Food1) > 0)
                    {
                    Item item = GameObject.Find("Food1").GetComponent<Item>();
                    moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Food1);
                    other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
                    }

                }

                if (Input.GetKeyDown(keyToAddItemsFromMainPlayerInventory))
                {
                    if ((mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) > 0) && (stopMineralsIntake == false))
                    {
                        Item item = other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
                        moduleInventory.GetComponent<Inventory>().AddItem(item);
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        showPlayerAndModuleInventory = false;
        moduleInventory.SetActive(showPlayerAndModuleInventory);
        moduleInventory.GetComponent<Inventory>().SetSlotsActive(showPlayerAndModuleInventory);
        PlayerController.ShowPlayerInventory = showPlayerAndModuleInventory;
        PlayerController.KeyCode_I_Works = !showPlayerAndModuleInventory;
        //UIInventory.SetModuleInventory(null);
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
