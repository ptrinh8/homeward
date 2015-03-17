// ========================================================================
// <file="ModuleIntakeSystem.cs" product="Homeward">
// <date>2015-03-16</date>
// ========================================================================

using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class ModuleIntakeSystem : MonoBehaviour
{
    PlayerController playerController = new PlayerController();
    Mining minerals = new Mining();
    Inventory inventory = new Inventory();

    public bool RefiningModule, FoodModule, OxygenModule;

    int refinedMineralsCreated;
    Vector2 worldSpacePos;

    public float loadingStartTime;
    public float loadingUpdateTime;
    float loadingPercent;
    bool timerReached = false;

    bool stopMineralsIntake = false;
    bool updateNumberOfRefinedMinerals = false;
    bool addRefinedMineralOnce = false;

    GameObject debugLoadingText;
    GameObject debugMineralsAddedText;
    GameObject debugMineralsRefinedText;

    public Sprite activeTexture;
    public Sprite deactiveTexture;
    public Sprite noPowerSupplyTexture;
    GameObject refineryModule;

    public bool showPlayerAndModuleInventory; // takahide made it public to use this outside
    public GameObject moduleInventory;
    GameObject mainPlayer;

    KeyCode keyToAddItemsFromMainPlayerInventory = KeyCode.K;
    KeyCode keyToAddItemsDirectlyToModuleinventory = KeyCode.L;

    public AudioController audioController;
    public float distanceBetweenPlayerAndRefinery;
    FMOD.Studio.EventInstance refineryMachine;
    FMOD.Studio.ParameterInstance startingStopping;
    FMOD.Studio.ParameterInstance distance;
    FMOD.Studio.ParameterInstance airlockPressure;
    FMOD.Studio.PLAYBACK_STATE refineryPlaybackState;
    float refineryStartingStopping;
    float refineryDistance;
    bool refineryStarted;
    bool startRefiningProcess = false;
    public float time = 0.0F;
    public float refinerySoundPressure;

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
        loadingPercent = loadingUpdateTime / 5;
    }

    void MineralsValidations()
    {
        if (RefiningModule)
        {
            if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral) == 0) { stopMineralsIntake = true; }
            else if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral) > 0) { stopMineralsIntake = false; }
        }
        if (FoodModule || OxygenModule)
        {
            if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) == 0) { stopMineralsIntake = true; }
            else if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) > 0) { stopMineralsIntake = false; }
        }
    }

    void RefiningProcess(SpriteRenderer refineryModuleSpriteRenderer)
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
                    if (RefiningModule)
                    {
                        Item item = GameObject.Find("Material").GetComponent<Item>();
                        moduleInventory.GetComponent<Inventory>().AddItem(item);
                        moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Mineral);
                        moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Mineral);

                    }
                    if (FoodModule)
                    {
                        Item item = GameObject.Find("Food1").GetComponent<Item>();
                        moduleInventory.GetComponent<Inventory>().AddItem(item);
                        moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
                        moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
                    }
                    if (OxygenModule)
                    {
                        Item item = GameObject.Find("Oxygen").GetComponent<Item>();
                        moduleInventory.GetComponent<Inventory>().AddItem(item);
                        moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
                        moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
                    }
                    timerReached = false;
                }
            }
        }
    }

    void Start()
    {
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        minerals = FindObjectOfType(typeof(Mining)) as Mining;
        inventory = FindObjectOfType(typeof(Inventory)) as Inventory;

        moduleInventory = Instantiate(moduleInventory) as GameObject;
        moduleInventory.transform.SetParent(GameObject.Find("Canvas").transform);
        moduleInventory.GetComponent<Inventory>().slots = 4; //tak added 3/11
        moduleInventory.GetComponent<Inventory>().rows = 1; //tak added 3/11
        moduleInventory.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        showPlayerAndModuleInventory = false;
        moduleInventory.SetActive(showPlayerAndModuleInventory);

        mainPlayer = GameObject.Find("MainPlayer");
        refineryModule = gameObject;
        worldSpacePos = Camera.main.WorldToViewportPoint(gameObject.transform.position);

        audioController = GameObject.Find("AudioObject").GetComponent<AudioController>();
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
        gameObject.GetComponent<SpriteRenderer>().material = gameObject.transform.root.gameObject.GetComponent<SpriteRenderer>().material;

        refineryDistance = Vector2.Distance(this.transform.position, playerController.transform.position);
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

        if (RefiningModule) { if (moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral) == 10) { stopMineralsIntake = true; } }
        if (FoodModule || OxygenModule) { if (moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Material) == 10) { stopMineralsIntake = true; } }

        if (RefiningModule)
        {
            int _mineralCount = moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral);
            if (_mineralCount >= 2) { RefiningProcess(refineryModuleSpriteRenderer); }
            if (_mineralCount >= 2)
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
        }
        else if (FoodModule || OxygenModule)
        {
            int _mineralCount = moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Material);
            if (_mineralCount >= 2) { RefiningProcess(refineryModuleSpriteRenderer); }

            if (_mineralCount >= 2)
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
		}

        if (!addRefinedMineralOnce)
        {
            if (RefiningModule)
            {
                addRefinedMineralOnce = true;
                Item item = GameObject.Find("Material").GetComponent<Item>();
                moduleInventory.GetComponent<Inventory>().AddItem(item);
                //mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
                moduleInventory.GetComponent<Inventory>().ClearSlot(ItemName.Mineral);
                timerReached = false;
            }
            if (FoodModule)
            {
                addRefinedMineralOnce = true;
                Item item = GameObject.Find("Food1").GetComponent<Item>();
                moduleInventory.GetComponent<Inventory>().AddItem(item);
                //mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
                moduleInventory.GetComponent<Inventory>().ClearSlot(ItemName.Material);
                timerReached = false;
            }
            if (OxygenModule)
            {
                addRefinedMineralOnce = true;
                Item item = GameObject.Find("Oxygen").GetComponent<Item>();
                moduleInventory.GetComponent<Inventory>().AddItem(item);
                //mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
                moduleInventory.GetComponent<Inventory>().ClearSlot(ItemName.Material);
                timerReached = false;
            }
        }

        if (startRefiningProcess == true)        {        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        UIInventory.SetModuleInventory(moduleInventory); // takahide added
        //Taylor
        if (other.gameObject.tag == "Player")
        {
            PlayerController.toolUsingEnable = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.transform.root.gameObject.GetComponent<LocalControl>().IsPowered && !gameObject.transform.root.gameObject.GetComponent<LocalControl>().IsBroken &&
            gameObject.transform.root.gameObject.GetComponent<LocalControl>().isOn)
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
                    if (RefiningModule)
                    {
                        if (moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Material) > 0)
                        {
                            Item item = GameObject.Find("Material").GetComponent<Item>();
                            moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
                            other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
                        }
                    }
                    if (FoodModule)
                    {
                        if (moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Food1) > 0)
                        {
                            Item item = GameObject.Find("Food1").GetComponent<Item>();
                            moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Food1);
                            other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
                        }
                    }
                    if (OxygenModule)
                    {
                        if (moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Oxygen) > 0)
                        {
                            Item item = GameObject.Find("Oxygen").GetComponent<Item>();
                            moduleInventory.GetComponent<Inventory>().GetItem(ItemName.Oxygen);
                            other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
                        }
                    }
                }

                if (Input.GetKeyDown(keyToAddItemsFromMainPlayerInventory))
                {
                    if (RefiningModule)
                    {
                        if ((mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral) > 0) && (stopMineralsIntake == false))
                        {
                            Item item = other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().GetItem(ItemName.Mineral);
                            moduleInventory.GetComponent<Inventory>().AddItem(item);
                        }
                    }
                    if (FoodModule || OxygenModule)
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
        UIInventory.SetModuleInventory(null);

        // Taylor
        if (other.gameObject.tag == "Player")
        {
            PlayerController.toolUsingEnable = true;
        }
    }
}
