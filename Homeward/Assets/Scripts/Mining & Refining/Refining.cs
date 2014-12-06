// ==========================================================================================
// <file="Refining.cs" product="Homeward">
// <date>2014-11-29</date>
// <summary>Class to keep track of refining module and its corresponding variables </summary>
// ==========================================================================================

#region Header Files

using UnityEngine;
using System.Collections;
using System;

#endregion

public class Refining : MonoBehaviour 
{
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

        refineryModule = GameObject.Find("Refining");
        var refineryModuleSpriteRenderer = refineryModule.GetComponentInChildren<SpriteRenderer>();

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
                        inventory.AddItem(1);
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
	                itemDatabase.items[0].value -= 1;
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

    void OnGUI()
    {
        debugLoadingText = GameObject.FindGameObjectWithTag("LoadingText");
        debugLoadingText.GetComponent<GUIText>().text = "Refining Module Loading:";

        debugMineralsAddedText = GameObject.FindGameObjectWithTag("MineralsAddedText");
        debugMineralsAddedText.GetComponent<GUIText>().text = "Minerals Added [MAX of 2 PER ROUND]: ";

        debugMineralsRefinedText = GameObject.FindGameObjectWithTag("MineralsRefinedText");
        debugMineralsRefinedText.GetComponent<GUIText>().text = "Minerals Refined [OVERALL]: ";

        GUI.contentColor = Color.black;
        GUI.Label(new Rect(14, 80, 550, 550), "\t\t\t\t\t\t\t\t\t  " + loadingPercent.ToString() + "%\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t  " + mineralsDeposited + "\n\t\t\t\t\t\t\t\t\t\t\t " + refinedMineralsCreated);
        
    }
}
