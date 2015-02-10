using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CentralControl : MonoBehaviour {

	public Sprite indoorSprite;
	public Sprite outdoorSprite;
	public int powerInGeneral; 
	public static bool isInside; // This static variable is the general location of player(whether inside)
	private SpriteRenderer spriteRenderer;
	private bool isEnter;

	private List <GameObject> locals; // List of all the modules within the outpost

	// Use this for initialization
	void Start () {
		locals = new List<GameObject>(); 
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		isEnter = true;
		isInside = isEnter;
	}
	
	// Update is called once per frame
	void Update () {
		if (isEnter) {
			ShowInside();
		}
		else ShowOutside();
		Debug.Log(powerInGeneral);
	}

	void ShowInside () {
		// Show inside sprite of habitat module
		spriteRenderer.sprite = indoorSprite;
		spriteRenderer.sortingOrder = -3;
		// Show inside sprite of all other modules within the outpost
		foreach (GameObject local in locals)
			local.SendMessage("ShowInside");
	}

	void ShowOutside () {
		spriteRenderer.sprite = outdoorSprite;
		spriteRenderer.sortingOrder = -1;
		foreach (GameObject local in locals)
			local.SendMessage("ShowOutside");
	}

	void DoorWayTriggered () {
		isEnter = !isEnter;
		isInside = isEnter;
	}

	void ChangeLocation (bool isEnter) {
		this.isEnter = isEnter;
	}

	void AddLocal (GameObject local) {
		locals.Add(local);

	}
}
