using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class LocalControl : MonoBehaviour {

	public Sprite indoorSprite;
	public Sprite outdoorSprite;
	public Sprite noPowerSprite;
	public Sprite turnedOffSprite;
	public int powerConsumption;
	public float minimumPowerLevel;
	[HideInInspector]
	public float powerLevel;
	[HideInInspector]
	public List <GameObject> connections;
	[HideInInspector]
	public GameObject center;
	[HideInInspector]
	public bool centerLock = false;
	[HideInInspector]
	public int moduleID;
	[HideInInspector]
	public bool checkFlag = false;
	private SpriteRenderer spriteRenderer;
	private bool isPowered;
	[HideInInspector]
	public bool isOn;
	private KeyCode turnKey = KeyCode.T;

	// Use this for initialization
	void Start () {
		connections = new List<GameObject>();
		CentralControl.isInside = true;
		isOn = true;
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log(powerLevel);
		if (checkFlag) {
			CheckPowerSupply();
			center.SendMessage("CheckPowerSupply");
			checkFlag = false;
		}
		if (Input.GetKeyDown(KeyCode.C))
			center.SendMessage("CheckPowerSupply");
	}

	void DoorWayTriggered () {
		center.SendMessage("DoorWayTriggered");
	}

	void ChangeLocation (bool isEnter) {
		center.SendMessage("ChangeLocation", isEnter);
	}

	void ShowInside () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();	
		if (!isOn) {
			spriteRenderer.sprite = turnedOffSprite;
		} else if (isPowered)
			spriteRenderer.sprite = indoorSprite;
		else spriteRenderer.sprite = noPowerSprite;

		spriteRenderer.sortingOrder = -3;
	}

	void ShowOutside () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = outdoorSprite;
		spriteRenderer.sortingOrder = -1;
	}

	void SetCenter (GameObject center) {
		if (!centerLock) {
			this.center = center;
			center.SendMessage("AddLocal", gameObject);
			centerLock = true;
//			CheckPowerSupply();
		}
	}

	void CheckPowerSupply () {
//		center.SendMessage("BFS", 0, SendMessageOptions.DontRequireReceiver);
		Text showPower;
		showPower = gameObject.GetComponentInChildren<Text>();
		if (powerConsumption > 0)
			showPower.text = Math.Round(powerLevel / minimumPowerLevel, 2).ToString();
		else if (powerConsumption == 0)
			showPower.text = " ";
		else
			showPower.text = "+" + -powerConsumption;
		if (!isOn) 
			showPower.text = "Off";
		else if (powerLevel >= minimumPowerLevel) {
			isPowered = true;
			foreach (Transform child in transform) 
			if (child.gameObject.tag == "Machine") {
				child.gameObject.SetActive(true);
			}
		} else {
			isPowered = false;
			foreach (Transform child in transform) {
				if (child.gameObject.tag == "Machine")
					child.gameObject.SetActive(false);
			}
		}
	}

	void SwitchTriggered () {
		isOn = !isOn;
		center.SendMessage("CheckPowerSupply");
		if (!isOn) {
			foreach (Transform child in transform) 
				if (child.gameObject.tag == "Machine") {
					child.gameObject.SetActive(false);
				}
			spriteRenderer.sprite = turnedOffSprite;
		} else if (isPowered){
			foreach (Transform child in transform) 
				if (child.gameObject.tag == "Machine") {
					child.gameObject.SetActive(true);
			}
			spriteRenderer.sprite = indoorSprite;
//			center.GetComponent<CentralControl>().powerInGeneral -= powerConsumption;
		} else spriteRenderer.sprite = noPowerSprite;
		spriteRenderer.sortingOrder = -3;
//		checkFlag = true;
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Player"){
			Debug.Log("Press T to turn on/off module");
			if (Input.GetKeyDown(turnKey)) {
				SwitchTriggered();
			}
		}
	}

	void AddConnection(GameObject other) {
		if (connections == null)
			connections = new List<GameObject>();
		connections.Add(other);
	}
}