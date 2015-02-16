// ========================================================================
// <file="Mining.cs" product="Homeward">
// <date>2014-11-11</date>
// ========================================================================


using UnityEngine;
using System.Collections;

public class Mining : MonoBehaviour 
{
    private Inventory inventory = new Inventory();
    private PlayerController playerController = new PlayerController();

    public int minimumMineralsThatCanBeExtracted;
    public int maximumMineralsThatCanBeExtracted;
    [HideInInspector]
	public int randomMineralsQuantity;

    public GameObject moduleInventory;
	private GameObject mainPlayer;	
	private GameObject numberOfMineralQuantityText;
    
    [HideInInspector]
	public bool playerInMiningPosition = false;
    private bool isPlayerMining = false;
    private KeyCode miningKey = KeyCode.F;
	private AudioController audioController;

    private void PlayerMiningState()
    {
        if ((Input.GetKeyDown(miningKey)) && (playerController.miningTimer == 0) && playerInMiningPosition == true)
        {
			Debug.Log ("playing");
			audioController.PlayMiningSound();
            isPlayerMining = !isPlayerMining;
        }
    }

    private void StartDestoryMineCoroutine()
    {
        if ((randomMineralsQuantity == 0) && !animation.isPlaying && gameObject) { StartCoroutine(DestroyMine()); }
    }

    private void MineralsValidations()
    {
        if (randomMineralsQuantity < 0) { randomMineralsQuantity = 0; }
    }

	void Start () 
    {
        mainPlayer = GameObject.Find ("MainPlayer");
		playerController = mainPlayer.GetComponent<PlayerController>();
        inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
        randomMineralsQuantity = Random.Range(minimumMineralsThatCanBeExtracted, maximumMineralsThatCanBeExtracted);
		audioController = GameObject.Find ("AudioObject").GetComponent<AudioController>();
	}
	
    void Update () 
    {
        PlayerMiningState();
        StartDestoryMineCoroutine();
        MineralsValidations();

        if (playerInMiningPosition && isPlayerMining)
        {
            if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Mineral) < GameObject.Find("Mineral").GetComponent<Item>().maxSize)
            {
                randomMineralsQuantity--;
                Item item = GameObject.Find("Mineral").GetComponent<Item>();
                mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
            }
            isPlayerMining = false;
        }
	} 

	IEnumerator DestroyMine ()
	{
			animation.Play();
			yield return new WaitForSeconds(0.5f);
			Destroy (gameObject);
	}

	void OnTriggerStay2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player") { playerInMiningPosition = true; }
	}
    
	void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player") { playerInMiningPosition = false; }
	}
}
