// ========================================================================
// <file="Refining.cs" product="Homeward">
// <date>2014-11-11</date>
// ========================================================================


using UnityEngine;
using System.Collections;
using System;

public class Refining : MonoBehaviour 
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

    private bool showPlayerAndModuleInventory;
    public GameObject moduleInventory;
    private GameObject mainPlayer;

    private KeyCode keyToAddItemsFromMainPlayerInventory = KeyCode.K;
    private KeyCode keyToAddItemsDirectlyToModuleinventory = KeyCode.L;

	private AudioController audioController;
	public float distanceBetweenPlayerAndRefinery;
	private FMOD.Studio.EventInstance refineryMachine;
	private FMOD.Studio.ParameterInstance startingStopping;
	private FMOD.Studio.ParameterInstance distance;
	private FMOD.Studio.PLAYBACK_STATE refineryPlaybackState;
	private float refineryStartingStopping;
	private float refineryDistance;
	private bool refineryStarted;

    private void MineralsValidations()
    {
        if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral) == 0)
        {
            stopMineralsIntake = true;
        }
        else if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral) > 0)
        {
            stopMineralsIntake = false;
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
		refineryDistance = 0f;
		refineryStartingStopping = 0f;
		refineryStarted = false;
	}
	
	void Update () 
    {
		refineryDistance = Vector2.Distance(this.transform.position, playerController.transform.position);
		Debug.Log(distanceBetweenPlayerAndRefinery);
		refineryMachine.getPlaybackState(out refineryPlaybackState);
		startingStopping.setValue(refineryStartingStopping);
		distance.setValue(refineryDistance);
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
        
        if (moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral) == 2)
        {
            stopMineralsIntake = true;
            startTimer();
            updateNumberOfRefinedMinerals = false;
            refineryModuleSpriteRenderer.sprite = activeTexture;

            if (loadingUpdateTime == 500.0f)
            {
                stopTimer();
                refineryModuleSpriteRenderer.sprite = deactiveTexture;

                if (!updateNumberOfRefinedMinerals)
                {
                    addRefinedMineralOnce = false;
                    updateNumberOfRefinedMinerals = true;

                    if (!addRefinedMineralOnce)
                    {
                        addRefinedMineralOnce = true;
                        Item item = GameObject.Find("Material").GetComponent<Item>();
                        mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
                        moduleInventory.GetComponent<Inventory>().ClearSlot(ItemName.Mineral);
                        timerReached = false;
                    }
                }                
            }            
        }	
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

                    Item item = GameObject.Find("Water").GetComponent<Item>();
                    moduleInventory.GetComponent<Inventory>().AddItem(item);
                }

                if (Input.GetKeyDown(keyToAddItemsFromMainPlayerInventory))
                {
                    if ((mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral) > 0) && (stopMineralsIntake == false))
                    {
                        Item item = other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().GetItem(ItemName.Mineral);
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
    }

    void startTimer()
    {
		if (refineryStarted == false)
		{
			refineryMachine.start();
			refineryStartingStopping = 1.5f;
			refineryStarted = true;
		}
        if (!timerReached) { loadingUpdateTime = loadingStartTime++; }
        if (loadingUpdateTime == 500) { timerReached = true; }
    }

    void stopTimer()
	{
		if (refineryStarted == true)
		{
			refineryStartingStopping = 2.5f;
			refineryStarted = false;
		}
        if (timerReached == true) { loadingUpdateTime = 0.0f; loadingStartTime = 0.0f; }
    }

    void changeLoadingToPercent()
    {
        loadingPercent = loadingUpdateTime/5;
    }
}
