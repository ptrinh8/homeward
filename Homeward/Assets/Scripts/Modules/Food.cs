using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	// Instances of other classes
	private PlayerController playerController;
	//private ItemDatabase itemDatabase;
	private MineralsStatus mineralStatus;
	private Minerals minerals;
	//private Inventory inventory;
	
	private int mineralsDeposited = 0;
	private int totalMineralsDeposited = 0;
	private int refinedMineralsCreated = 0;
	
	private float loadingStartTime = 0;
	private float loadingUpdateTime = 0;
	private float loadingPercent = 0;
	
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

	
	void Start () 
	{
		// Initialize monobehavior of other initialized classes
		playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
		//itemDatabase = FindObjectOfType(typeof(ItemDatabase)) as ItemDatabase;
		mineralStatus = FindObjectOfType(typeof(MineralsStatus)) as MineralsStatus;
		minerals = FindObjectOfType(typeof(Minerals)) as Minerals;
		//inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
		
		loadingStartTime = 0;

        /*** module inventory ***/
        moduleInventory = Instantiate(moduleInventory) as GameObject;
        moduleInventory.transform.SetParent(GameObject.Find("Canvas").transform);
        moduleInventory.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        
        showPlayerAndModuleInventory = false;
        moduleInventory.SetActive(showPlayerAndModuleInventory);
	}
	
	void Update () 
	{
		changeLoadingToPercent();
		// Taylor
		refineryModule = gameObject;
		var refineryModuleSpriteRenderer = refineryModule.GetComponent<SpriteRenderer>();

        /***********************************************************************
         * Takahide added
         * *********************************************************************/
        if (!gameObject.transform.root.gameObject.GetComponent<LocalControl>().IsPowered)
        {
            refineryModuleSpriteRenderer.sprite = noPowerSupplyTexture;
        }
        else
        {
            refineryModuleSpriteRenderer.sprite = deactiveTexture;
        }

        /***********************************************************************
         * Tak end
         * **********************************************************************/

		if (mineralStatus.mineralsInInventory == 0)
		{
			stopMineralsIntake = true; 
		}
		
		else if (mineralStatus.mineralsInInventory > 0)
		{
			stopMineralsIntake = false;     
			
			if (mineralsDeposited == 2)
			{
				stopMineralsIntake = true; 
			}
		}
		
		// mineral despoted = 2?
		if (mineralsDeposited == 2)
		{
			startTimer();
			updateNumberOfRefinedMinerals = false;
			refineryModuleSpriteRenderer.sprite = activeTexture;
			// timer = 500?
			if (loadingUpdateTime == 500.0f)
			{
				stopTimer();
				refineryModuleSpriteRenderer.sprite = deactiveTexture;
				if (!updateNumberOfRefinedMinerals)
				{
					refinedMineralsCreated++;
					mineralsDeposited = 0;
					updateNumberOfRefinedMinerals = true;
					
					if (!addRefinedMineralOnce)
					{
						//inventory.AddItem(2);
						addRefinedMineralOnce = true;
					}
					//itemDatabase.items[1].value += 1;
				}                
			}            
		}	
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
        {
            /*******************************************
            * Inventory
            * *****************************************/
            if (gameObject.transform.root.gameObject.GetComponent<LocalControl>().IsPowered) // if the module is powered, show both player and module inventory
            {
                showPlayerAndModuleInventory = true;
                PlayerController.KeyCode_I_Works = !showPlayerAndModuleInventory;
                PlayerController.ShowPlayerInventory = showPlayerAndModuleInventory;
                moduleInventory.SetActive(true);
                moduleInventory.GetComponent<Inventory>().SetSlotsActive(showPlayerAndModuleInventory);

                //if (showPlayerAndModuleInventory) moduleInventory.GetComponent<Inventory>().DebugShowInventory();
            }
            else
            {
                showPlayerAndModuleInventory = false;
                moduleInventory.SetActive(showPlayerAndModuleInventory);
                moduleInventory.GetComponent<Inventory>().SetSlotsActive(showPlayerAndModuleInventory);

                PlayerController.ShowPlayerInventory = showPlayerAndModuleInventory;
                PlayerController.KeyCode_I_Works = !showPlayerAndModuleInventory;
            }

            if (showPlayerAndModuleInventory) // if inventories are visible
            {
                if (Input.GetKeyDown(KeyCode.L)) // L is temporary. Delete this once you find how to add item.
                {

                    Item item = GameObject.Find("Water").GetComponent<Item>();
                    moduleInventory.GetComponent<Inventory>().AddItem(item);
                }

                if (Input.GetKeyDown(KeyCode.K)) // K is temporary. Delete this once you find how to add item.
                {
                    Item item = GameObject.Find("RepairingTool").GetComponent<Item>();
                    moduleInventory.GetComponent<Inventory>().AddItem(item);
                }
            }
                
            /*******************************************
            * Inventory END
            * *****************************************/

            if ((Input.GetKeyDown(KeyCode.F)) == true) // Press F to activate refining module
            {
                if (stopMineralsIntake == false)
                {
                    mineralStatus.mineralsInInventory--;
                    //itemDatabase.items[1].value -= 1; // removes the value of material, if itemID = 1
                    if (mineralStatus.mineralsInInventory > -1)
                    {
                        mineralsDeposited++;
                        totalMineralsDeposited++;
                    }
                    else
                    {
                        // Do nothing
                    }
                }
                else
                {

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
		loadingUpdateTime = loadingStartTime++;
	}
	
	void stopTimer()
	{
		loadingUpdateTime = 0.0f;
		loadingStartTime = 0.0f;
	}
	
	void changeLoadingToPercent()
	{
		loadingPercent = loadingUpdateTime/5;
	}
}
