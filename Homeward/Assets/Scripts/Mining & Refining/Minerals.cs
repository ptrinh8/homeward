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
    private bool isPlayerUsingPickaxe = false;
    private bool isPlayerMining = false;
    private int keyPressesCouter = 2;

	void Start () 
    {
		mainPlayer = GameObject.Find ("MainPlayer");
		playerController = mainPlayer.GetComponent<PlayerController>();
        mineralStatus = mainPlayer.GetComponent<MineralsStatus>();

       // numberOfMineralQuantityText = GameObject.Find("Mineral Quantity");
      //  numberOfMineralQuantity = numberOfMineralQuantityText.GetComponent<GUIText>();
      //  numberOfMineralQuantity.text = "Minerals left in this mine: [GET CLOSE TO A ROCK]";

        // Generated randomMineralsQuantity based on RND range
        randomMineralsQuantity = Random.Range(minimumMineralsThatCanBeExtracted, maximumMineralsThatCanBeExtracted);

	}
	
    void Update () 
    {
        PlayerHoldingMiningPickaxe();

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

        if (isPlayerUsingPickaxe == true)
        {
            // Add mineral in inventory when 
            if (playerInMiningPosition && isPlayerMining)
            {
                if (mineralStatus.mineralsInInventory < mineralStatus.maxMineralsLoadAllowed)
                {
                    mineralStatus.mineralsInInventory++;
                    randomMineralsQuantity--;

                    if (!hasMineralBeenExtracted)
                    {

                        hasMineralBeenExtracted = true;
                    }
                }
                isPlayerMining = false;
            }
        }
        else
        {
        }

        // Press F to activate mining state
        if ((Input.GetKeyDown(KeyCode.F)) && (playerController.miningTimer == 0) && playerInMiningPosition == true)
        {
            isPlayerMining = !isPlayerMining;
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
		if (other.gameObject.tag == "Player") {
			playerInMiningPosition = true;
			// Show mine's randomMineralsQuantity
	     //   numberOfMineralQuantity.text = "Minerals left in this mine: " + randomMineralsQuantity.ToString();
		}
	}
    
	void OnTriggerExit2D(Collider2D other) 
    {
		if (other.gameObject.tag == "Player") {
			playerInMiningPosition = false;
			// Show default text when player walks away
	      //  numberOfMineralQuantity.text = "Minerals left in this mine: [GET CLOSE TO A ROCK]";
		}
	}

    void PlayerHoldingMiningPickaxe()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isPlayerUsingPickaxe = !isPlayerUsingPickaxe;
            if (keyPressesCouter % 2 == 0)
            {
                keyPressesCouter += 1;
            }
            else if (keyPressesCouter % 2 != 0)
            {
                keyPressesCouter += 1;
            }
        }
    }
}
