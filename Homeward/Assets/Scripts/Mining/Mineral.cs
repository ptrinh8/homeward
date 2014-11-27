// =========================================================================
// <file="Mineral.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Class to keep track of mining amount for each resource</summary>
// =========================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

public class Mineral : MonoBehaviour 
{
	public int minQuantity, maxQuantity;	// rangge of quantity
	private int quantity;	// how many minerals contain in this mine
	public GameObject mainPlayer;	
	public GameObject textFinder;
	public PlayerController playerC;	// player‘s input
	public PlayerStatus playerS;	// record and return how many minerals player owned

    public Item.ItemType type;
    public Texture2D icon;
    public GameObject obj;
	
	private GUIText showQuantity;	// show quantity at right left corner(when player stand near a mine)
	private bool playerInMiningPosition = false;

	// Use this for initialization
	void Start () {

		mainPlayer = GameObject.Find ("MainPlayer");
		playerC = mainPlayer.GetComponent<PlayerController>();
		playerS = mainPlayer.GetComponent<PlayerStatus>();

		textFinder = GameObject.Find ("Mineral Quantity");
		showQuantity = textFinder.GetComponent<GUIText>();
		showQuantity.text = "";

		quantity = Random.Range(minQuantity, maxQuantity);	// generate quantity base on range

	}
	
    void Update () {

		// destory mine if exhausted
		if (quantity == 0 && !animation.isPlaying && gameObject) {
			StartCoroutine (DestroyMine());
		}	
	
		// add mineral to player when he press F near a mine
		if (playerInMiningPosition && playerC.MiningNow) {
			if (playerS.mineralsOwned < playerS.maxLoad) {
				playerS.mineralsOwned ++;
				quantity --;
			}
			playerC.MiningNow = false;
		}
	} 

	IEnumerator DestroyMine ()
	{
			// animation of mine's destruction
			animation.Play();
			yield return new WaitForSeconds(1.0f);
			Destroy (gameObject);
	}

	void OnTriggerStay2D(Collider2D other) {
		playerInMiningPosition = true;
		// show mine's quantity
		showQuantity.text = quantity.ToString();
	}


	void OnTriggerExit2D(Collider2D other) {
		playerInMiningPosition = false;
		// show nothing if player go away
		showQuantity.text = "";
	}
}
