using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	// Instances of other classes
	private PlayerController playerController;
	private ItemDatabase itemDatabase;
	private MineralsStatus mineralStatus;
	private Minerals minerals;
	private Inventory inventory;
	
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
	private GameObject refineryModule;
	
	void Start () 
	{
		// Initialize monobehavior of other initialized classes
		playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
		itemDatabase = FindObjectOfType(typeof(ItemDatabase)) as ItemDatabase;
		mineralStatus = FindObjectOfType(typeof(MineralsStatus)) as MineralsStatus;
		minerals = FindObjectOfType(typeof(Minerals)) as Minerals;
		inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
		
		loadingStartTime = 0;
	}
	
	void Update () 
	{
		changeLoadingToPercent();
		// Taylor
		refineryModule = gameObject;
		var refineryModuleSpriteRenderer = refineryModule.GetComponent<SpriteRenderer>();
		
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
						inventory.AddItem(2);
						addRefinedMineralOnce = true;
					}
					itemDatabase.items[1].value += 1;
				}                
			}            
		}	
	}
	
	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
			// Press F to activate refining module
			if ((Input.GetKeyDown(KeyCode.F)) == true)
		{
			if (stopMineralsIntake == false)
			{
				mineralStatus.mineralsInInventory--;
				itemDatabase.items[1].value -= 1; // removes the value of material, if itemID = 1
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
