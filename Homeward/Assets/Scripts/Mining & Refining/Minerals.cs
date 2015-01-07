// ========================================================================
// <file="Mineral.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Class to keep track of amount for minerals extracted</summary>
// ========================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

public class Minerals : MonoBehaviour 
{
    // Instances of other classes
    private Inventory inventory;
    private ItemDatabase itemDatabase;
    private PlayerController playerController;
    private MineralsStatus mineralStatus;

    public int minimumMineralsThatCanBeExtracted;
    public int maximumMineralsThatCanBeExtracted;
    [HideInInspector]
	public int randomMineralsQuantity;	

	private GameObject mainPlayer;	
	private GameObject numberOfMineralQuantityText;
    
    private GUIText numberOfMineralQuantity;
    [HideInInspector]
	public bool playerInMiningPosition = false;
    private bool hasMineralBeenExtracted = false; 

	void Start () 
    {
        // Initialize monobehavior of other initialized classes
        inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
        itemDatabase = FindObjectOfType(typeof(ItemDatabase)) as ItemDatabase;

		mainPlayer = GameObject.Find ("MainPlayer");
		playerController = mainPlayer.GetComponent<PlayerController>();
        mineralStatus = mainPlayer.GetComponent<MineralsStatus>();

        numberOfMineralQuantityText = GameObject.Find("Mineral Quantity");
        numberOfMineralQuantity = numberOfMineralQuantityText.GetComponent<GUIText>();
        numberOfMineralQuantity.text = "Minerals left in this mine: [GET CLOSE TO A ROCK]";

        // Generated randomMineralsQuantity based on RND range
        randomMineralsQuantity = Random.Range(minimumMineralsThatCanBeExtracted, maximumMineralsThatCanBeExtracted);	
	}
	
    void Update () 
    {
		// Destroy mine when exhausted
		if ((randomMineralsQuantity == 0) && !animation.isPlaying && gameObject) 
        {
			StartCoroutine (DestroyMine());
		}

        if (mineralStatus.mineralsInInventory < 0)
        {
            mineralStatus.mineralsInInventory = 0;
        }

        if (randomMineralsQuantity < 0)
        {
            randomMineralsQuantity = 0;
        }
	
		// Add mineral in inventory when 
		if (playerInMiningPosition && playerController.PlayerIsMiningNow) {
            if (mineralStatus.mineralsInInventory < mineralStatus.maxMineralsLoadAllowed)
            {
                mineralStatus.mineralsInInventory++;
				randomMineralsQuantity --;

                if (!hasMineralBeenExtracted)
                {
                    if ((inventory.inventory[1].itemName == null) && (inventory.inventory[2].itemName == null)
                     && (inventory.inventory[3].itemName == null) && (inventory.inventory[4].itemName == null) && (inventory.inventory[5].itemName == null)
                     && (inventory.inventory[6].itemName == null) && (inventory.inventory[7].itemName == null) && (inventory.inventory[8].itemName == null))
                    {                        
                        inventory.AddItem(0);
                    }

                    else
                    {
                        // Don't do anything
                    }

                    hasMineralBeenExtracted = true;
                }
                itemDatabase.items[0].value += 1;
			}
			playerController.PlayerIsMiningNow = false;
		}

        if (itemDatabase.items[0].value == 1)
        {
            itemDatabase.items[0].itemIcon = itemDatabase.items[0].itemIconReplace;
        }

        // Press F to activate mining state
        if ((Input.GetKeyDown(KeyCode.F)) && (playerController.miningTimer == 0) && playerInMiningPosition == true)
        {
            playerController.isMining = true;
        }
	} 

	IEnumerator DestroyMine ()
	{
			// Animation of mine's destruction
			animation.Play();
			yield return new WaitForSeconds(0.5f);
			Destroy (gameObject);
	}

	void OnTriggerStay2D(Collider2D other) 
    {
		//Taylor
		if (other.gameObject.tag == "Player") {
			playerInMiningPosition = true;
			// Show mine's randomMineralsQuantity
	        numberOfMineralQuantity.text = "Minerals left in this mine: " + randomMineralsQuantity.ToString();
		}
	}
    
	void OnTriggerExit2D(Collider2D other) 
    {
		if (other.gameObject.tag == "Player") {
			playerInMiningPosition = false;
			// Show default text when player walks away
	        numberOfMineralQuantity.text = "Minerals left in this mine: [GET CLOSE TO A ROCK]";
		}
	}
}
