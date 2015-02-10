using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalControl : MonoBehaviour {

	public Sprite indoorSprite;
	public Sprite outdoorSprite;
	public Sprite indoorSpriteNoPower;
	public Sprite outdoorSpriteNoPower;
	public int powerConsumption;
	[HideInInspector]
	public GameObject center;
	[HideInInspector]
	public bool centerLock = false;
	private SpriteRenderer spriteRenderer;
	private bool isPowered;

	// Use this for initialization
	void Start () {
		CentralControl.isInside = true;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void DoorWayTriggered () {
		center.SendMessage("DoorWayTriggered");
	}

	void ChangeLocation (bool isEnter) {
		center.SendMessage("ChangeLocation", isEnter);
	}

	void ShowInside () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();	
		if (isPowered)
			spriteRenderer.sprite = indoorSprite;
		else spriteRenderer.sprite = indoorSpriteNoPower;
		spriteRenderer.sortingOrder = -3;
	}

	void ShowOutside () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		if (isPowered)
			spriteRenderer.sprite = outdoorSprite;
		else spriteRenderer.sprite = outdoorSpriteNoPower;
		spriteRenderer.sortingOrder = -1;
	}

	void SetCenter (GameObject center) {
		if (!centerLock) {
			this.center = center;
			center.SendMessage("AddLocal", gameObject);
			centerLock = true;
			CheckPowerSupply();
		}
	}

	void CheckPowerSupply () {
		if (center.GetComponent<CentralControl>().powerInGeneral - powerConsumption >= 0) {
			center.GetComponent<CentralControl>().powerInGeneral -= powerConsumption;
			isPowered = true;

		} else {
			isPowered = false;
			foreach (Transform child in transform) {
				if (child.gameObject.tag != "Connect Point")
					child.gameObject.SetActive(false);
			}
		}
	}
}