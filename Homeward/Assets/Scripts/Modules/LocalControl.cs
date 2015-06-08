using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

// attached to each module
// handle the module itself
public class LocalControl : MonoBehaviour
{

	public Sprite indoorSprite;
	public Sprite[] exteriors;
	private Sprite outdoorSprite;
	private Sprite noPowerSprite;
	private Sprite turnedOffSprite;

	public int powerConsumption;
	private SpriteRenderer spriteRenderer;
	public float minimumPowerLevel;
	private bool isPowered, isEnter;
	[HideInInspector]
	public float
			powerLevel;
	[HideInInspector]
	public List <GameObject>
			connections;
	[HideInInspector]
	public GameObject
			center;
	[HideInInspector]
	public bool
			centerLock = false;
	[HideInInspector]
	public int
			moduleID;
	[HideInInspector]
	public bool
			checkFlag = false;
	[HideInInspector]
	public bool
			isOn;
	public int durability;
	private DayNightController dayNightController;
	private float durabilityTimer;
	public float durabilityLossTime;
	public float durabilityLossSpeed;
	private bool isBroken, flag;
	private Text moduleStatusText;

	public Text ModuleStatusText {
			get { return moduleStatusText; }
			set { value = moduleStatusText; }
	}

	private KeyCode repairKey = KeyCode.F;
	private GameObject player;
	private float repairActionTime;
	private bool repairingFlag;
	private bool showTextFlag;
	private bool repairArrowQueueFlag;
	private GameObject repairArrowQueue;
	private AudioController audioController;
	private ItemName neededItem;
	private float selector;

	private LightScript[] lights;


	public bool ShowTextFlag {
			get { return showTextFlag; }
			set { value = showTextFlag; }
	}

	public bool IsEnter {
			get { return isEnter;}
			set { this.isEnter = value;}
	}

	public bool IsPowered {
			get { return isPowered;}
	}

	public bool IsBroken {
			get { return isBroken;}
	}

	private int powerIndicator;

	public int PowerIndicator {
			get { return powerIndicator;}
	}
	// Use this for initialization
	void Start ()
	{
			noPowerSprite = indoorSprite;
			turnedOffSprite = indoorSprite;
			repairArrowQueueFlag = false;

			connections = new List<GameObject> ();
			spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
			spriteRenderer.sprite = indoorSprite;
			spriteRenderer.sortingOrder = -3;
			CentralControl.isInside = true;
			isOn = true;
			moduleStatusText = gameObject.GetComponentInChildren<Text> ();
			dayNightController = GameObject.Find ("DayNightController").GetComponent<DayNightController> ();
			durability = 100;
			durabilityLossTime = (dayNightController.dayCycleLength * 2) / 100;
			flag = true;
			if (!GameObject.Find ("Module Building").GetComponent<Building> ().NewGameFlag)
					isEnter = true;
			else
					isEnter = false;

			player = GameObject.FindWithTag ("Player");
			repairingFlag = true;
			repairActionTime = 1f;

			player.SendMessage ("LocalModuleGenerated", gameObject);

			repairArrowQueue = GameObject.Find ("Canvas").transform.FindChild ("Repair Arrow Queue").gameObject;
			audioController = GameObject.Find ("AudioObject").GetComponent<AudioController> ();
			RepairItemSelect();

		if (gameObject.GetComponentsInChildren<LightScript>() != null)
		{
			lights = this.gameObject.GetComponentsInChildren<LightScript>();
		}
	}

	// Update is called once per frame
	void Update ()
	{

		if (CentralControl.isInside == false)
		{
			spriteRenderer.sortingOrder = 1;
		}

		if (CentralControl.isInside == true && dayNightController.currentPhase == DayNightController.DayPhase.Dusk)
		{
			if (lights != null)
			{
				for (int i = 0; i < lights.Length; i++)
				{
					lights[i].LightSwitch(1);
				}
			}
		}
		else if (dayNightController.currentPhase == DayNightController.DayPhase.Dawn || CentralControl.isInside == false)
		{
			if (lights != null)
			{
				for (int i = 0; i < lights.Length; i++)
				{
					lights[i].LightSwitch(2);
				}
			}
		}
			if (checkFlag) {
					center.SendMessage ("CheckPowerSupply");
					checkFlag = false;
			}
			if (isEnter) {
				Debug.Log(gameObject + " " + durability);
					if (GameObject.FindWithTag ("Player").GetComponent<PlayerController> ().EnvironmentalAir != gameObject.GetComponent<AirControl> ().Air) {
							GameObject.FindWithTag ("Player").GetComponent<PlayerController> ().EnvironmentalAir = gameObject.GetComponent<AirControl> ().Air;
					}
			}
			DurabilityLoss ();
			DisplayText (ModuleControl.ShowModuleControl);
	}

	void DoorWayTriggered (bool isDoorway)
	{
			if (isDoorway)
					center.SendMessage ("DoorWayTriggered", SendMessageOptions.RequireReceiver);
			isEnter = !isEnter;
	}

	void ChangeLocation (bool isEnter)
	{
			center.SendMessage ("ChangeLocation", isEnter);
	}

	void ShowInside ()
	{
			spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();	
			if (!isOn || isBroken) {
					spriteRenderer.sprite = turnedOffSprite;
					gameObject.SendMessage ("LightTriggered", false, SendMessageOptions.DontRequireReceiver);
			} else if (isPowered) {
					spriteRenderer.sprite = indoorSprite;
					gameObject.SendMessage ("LightTriggered", true, SendMessageOptions.DontRequireReceiver);
			} else {
					spriteRenderer.sprite = noPowerSprite;
					gameObject.SendMessage ("LightTriggered", false, SendMessageOptions.DontRequireReceiver);
			}

			spriteRenderer.sortingOrder = -3;
	}

	void ShowOutside ()
	{
			spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
			spriteRenderer.sprite = outdoorSprite;
			spriteRenderer.sortingOrder = -1;
	}

	void SetCenter (GameObject center)
	{
			if (!centerLock) {
					this.center = center;
					center.SendMessage ("AddLocal", gameObject);
					centerLock = true;
			}
	}

	void CheckPowerSupply ()
	{
			if (isBroken) {
					// do nothing
			} else {
					if (powerConsumption > 0) {
							powerIndicator = (int)(powerLevel / minimumPowerLevel);
					} else {
							powerIndicator = -1;
					}

					if (!isOn) 
							moduleStatusText.text = "";
					else if (powerLevel >= minimumPowerLevel) {
							isPowered = true;
					} else {
							isPowered = false;
					}
			}
	}

	void SwitchTriggered (bool flag)
	{
			isOn = flag;
			center.SendMessage ("CheckPowerSupply");
			if (!isOn) {
					if (center.GetComponent<CentralControl> ().isEnterOutpost) {
							spriteRenderer.sprite = turnedOffSprite;
					}
					gameObject.SendMessage ("LightTriggered", false, SendMessageOptions.DontRequireReceiver);
			} else {
					if (isPowered) {
							spriteRenderer.sprite = indoorSprite;
							gameObject.SendMessage ("LightTriggered", true, SendMessageOptions.DontRequireReceiver);
					} else {
							spriteRenderer.sprite = noPowerSprite;
							gameObject.SendMessage ("LightTriggered", false, SendMessageOptions.DontRequireReceiver);
					}
			}

			spriteRenderer.sortingOrder = -3;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
			if (other.gameObject.tag == "Player") {
					PlayerController.toolUsingEnable = false;
			}
	}

	void OnTriggerExit2D (Collider2D other)
	{
			if (other.gameObject.tag == "Player") {
					PlayerController.toolUsingEnable = true;
			}
	}

	void AddConnection (GameObject other)
	{
			if (connections == null)
					connections = new List<GameObject> ();
			connections.Add (other);
	}

	void DurabilityLoss ()
	{
			if (durability > 0) {
					durabilityTimer += Time.deltaTime * durabilityLossSpeed;
					if (durabilityTimer > durabilityLossTime) {
							durability -= 1;
							durabilityTimer = 0;
					}
			} else {
					isBroken = true;
					if (flag) {
							SwitchTriggered (false);
							flag = false;
					}
			}

			if (durability <= 0)
			{
				outdoorSprite = exteriors[4];
			}
			else if (durability > 0 && durability <= 25)
			{
				outdoorSprite = exteriors[3];
			}
			else if (durability > 25 && durability <= 50)
			{
				outdoorSprite = exteriors[2];
			}
			else if (durability > 50 && durability <= 75)
			{
				outdoorSprite = exteriors[1];
			}
			else if (durability > 75 && durability <= 100)
			{
				outdoorSprite = exteriors[0];
			}

			if (!CentralControl.isInside)
			{
				spriteRenderer.sprite = outdoorSprite;
			}	

			if (isEnter) 
			{
					if (player.gameObject.GetComponent<PlayerController> ().playerInventory.GetComponent<Inventory> ().CountItems (neededItem) > 0 && durability != 100) 
					{
							if (PlayerController.holdingRepairTool && PlayerController.toolUsingEnable) 
							{
							// do exterior sprite
							GameObject.FindWithTag("RepairScreen").GetComponent<SpriteRenderer>().enabled = true;
							GameObject.FindWithTag("RepairScreen").GetComponent<SpriteRenderer>().sprite = outdoorSprite;
							GameObject.FindWithTag("RepairScreen").transform.position = transform.position;
									if (repairingFlag) 
									{
											if (Input.GetKeyDown (repairKey)) 
											{
													audioController.PlayRepairSound (2);
													repairArrowQueueFlag = !repairArrowQueueFlag;
													PlayerController.showRepairArrows = repairArrowQueueFlag;
													repairArrowQueue.SendMessage ("Reset", SendMessageOptions.DontRequireReceiver);
													repairArrowQueue.SetActive (repairArrowQueueFlag);
											}
											if (repairArrowQueue.GetComponent<ArrowQueueControl> ().CorrectInput) 
											{
													repairArrowQueue.GetComponent<ArrowQueueControl> ().CorrectInput = false;
													repairingFlag = false;
													Invoke ("TimeWait", repairActionTime);
											}
									}
							}
							else GameObject.FindWithTag("RepairScreen").GetComponent<SpriteRenderer>().enabled = false;
							
					}
			}

			if (durability > 0) {
					moduleStatusText.text = "";
			} else
					moduleStatusText.text = "";
	}

	void RepairAction ()
	{
		if (!isBroken) {
				if (durability + 10 <= 100) {
						durability += 10;
				} else {
						durability = 100;
				}
		} else {
				durability += 10;
				isBroken = false;
				SwitchTriggered (true);
				flag = true;
				// arrow reset
		}
		audioController.PlayRepairSound (3);
		repairArrowQueueFlag = false;
		PlayerController.showRepairArrows = false;
		repairArrowQueue.SendMessage ("Reset", SendMessageOptions.DontRequireReceiver);
		repairArrowQueue.SetActive (false);
		player.gameObject.GetComponent<PlayerController> ().playerInventory.GetComponent<Inventory> ().GetItem (neededItem);
		RepairItemSelect();
	}

	private void RepairItemSelect()
	{
		selector = UnityEngine.Random.Range(1f, 10f);
		if (selector < 5f)
		{
			neededItem = ItemName.Wire;
		}
		else if (selector >= 5f && selector < 8.5f)
		{
			neededItem = ItemName.Screw;
		}
		else if (selector >= 8.5f)
		{
			neededItem = ItemName.Metal;
		}
	}

	void TimeWait ()
	{
			repairingFlag = true;
			RepairAction ();
	}

	void DisplayText (bool flag)
	{
			if (flag) {
					moduleStatusText.enabled = true;
			} else {
					if (isEnter) {
							if (moduleStatusText.enabled == false) {
									moduleStatusText.enabled = true;
							}
					} else {
							if (moduleStatusText.enabled == true) {
									moduleStatusText.enabled = false;
							}
					}
			}
	}
}