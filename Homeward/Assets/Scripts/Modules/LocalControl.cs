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
	private SpriteRenderer spriteRenderer;
	public float minimumPowerLevel;
	private bool isPowered, isEnter;
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
	[HideInInspector]
	public bool isOn;
	private KeyCode turnKey = KeyCode.T;

	public int durability;
	private DayNightController dayNightController;
	private float durabilityTimer;
	public float durabilityLossTime;
	public float durabilityLossSpeed;
	private bool isBroken, flag;
	private Text moduleStatusText;

    public Text ModuleStatusText
    {
        get { return moduleStatusText; }
        set { value = moduleStatusText; }
    }

	private int pos; 
	private KeyCode repairKey = KeyCode.F;

	private GameObject player;
    private float repairActionTime;
    private bool repairingFlag;
    private bool showTextFlag;
	private bool repairArrowQueueFlag;

	private GameObject repairArrowQueue;

	private AudioController audioController;

    public bool ShowTextFlag
    {
        get { return showTextFlag; }
        set { value = showTextFlag; }
    }

	public bool IsEnter 
	{
		get {
			return isEnter;
		}
		set {
			this.isEnter = value;
		}
	}
	public bool IsPowered 
	{
		get {
			return isPowered;
		}
	}

    public bool IsBroken
    {
        get
        {
            return isBroken;
        }
    }

	// Use this for initialization
	void Start () {
		repairArrowQueueFlag = false;

		connections = new List<GameObject>();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = indoorSprite;
		spriteRenderer.sortingOrder = -3;
		CentralControl.isInside = true;
		isOn = true;
		moduleStatusText = gameObject.GetComponentInChildren<Text>();
		dayNightController = GameObject.Find ("DayNightController").GetComponent<DayNightController>();
		durability = 100;
		durabilityLossTime = (dayNightController.dayCycleLength * 4) / 100;
		flag = true;
        if (!GameObject.Find("Module Building").GetComponent<Building>().NewGameFlag)
            isEnter = true;
        else isEnter = false;

		player = GameObject.FindWithTag("Player");
        repairingFlag = true;
        repairActionTime = 1f;

        player.SendMessage("LocalModuleGenerated", gameObject);

		repairArrowQueue = GameObject.Find("Canvas").transform.FindChild("Repair Arrow Queue").gameObject;
		audioController = GameObject.Find ("AudioObject").GetComponent<AudioController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (checkFlag) {
			center.SendMessage("CheckPowerSupply");
			checkFlag = false;
		}
        if (isEnter)
        {
            if (GameObject.FindWithTag("Player").GetComponent<PlayerController>().EnvironmentalAir != gameObject.GetComponent<AirControl>().Air)
            {
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().EnvironmentalAir = gameObject.GetComponent<AirControl>().Air;
            }
		}
		/**
		if (CentralControl.isInside) {
			if (spriteRenderer.material != GameObject.Find("DefaultSpriteMaterial").GetComponent<SpriteRenderer>().material){
				center.SendMessage("ChangeMaterialToDefault", true, SendMessageOptions.RequireReceiver);
			}
		} else {
			if (spriteRenderer.material != GameObject.Find("DayNightReactingMaterial").GetComponent<SpriteRenderer>().material){
				center.SendMessage("ChangeMaterialToDefault", false, SendMessageOptions.RequireReceiver);
			}
		}
		**/

		DurabilityLoss();
        DisplayText(ModuleControl.ShowModuleControl);

	}

	/**
	void ChangeMaterialToDefault(bool toDefault) {
		Debug.Log(toDefault);
		if (toDefault){
			spriteRenderer.material = GameObject.Find("DefaultSpriteMaterial").GetComponent<SpriteRenderer>().material;
			foreach (Transform child in transform){
				if (child.gameObject.GetComponent<SpriteRenderer>() != null)
					child.gameObject.GetComponent<SpriteRenderer>().material = GameObject.Find("DefaultSpriteMaterial").GetComponent<SpriteRenderer>().material;
			}
		} else {
			spriteRenderer.material = GameObject.Find("DayNightReactingMaterial").GetComponent<SpriteRenderer>().material;
			foreach (Transform child in transform){
				if (child.gameObject.GetComponent<SpriteRenderer>() != null)
					child.gameObject.GetComponent<SpriteRenderer>().material = GameObject.Find("DayNightReactingMaterial").GetComponent<SpriteRenderer>().material;
			}
		}
	}
	**/

	void DoorWayTriggered (bool isDoorway) {
		if (isDoorway)
			center.SendMessage("DoorWayTriggered", SendMessageOptions.RequireReceiver);
		isEnter = !isEnter;
	}

	void ChangeLocation (bool isEnter) {
		center.SendMessage("ChangeLocation", isEnter);
	}

	void ShowInside () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();	
		if (!isOn || isBroken) {
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
		}
	}

	void CheckPowerSupply () {
		if (isBroken) {
			// do nothing
		} else {
			if (powerConsumption > 0)
				moduleStatusText.text = Math.Round(powerLevel / minimumPowerLevel, 2).ToString();
			else if (powerConsumption == 0)
				moduleStatusText.text = " ";
			else
				moduleStatusText.text = "+" + -powerConsumption;
			if (!isOn) 
				moduleStatusText.text = "Off";
			else if (powerLevel >= minimumPowerLevel) {
				isPowered = true;
                //foreach (Transform child in transform) 
                //if (child.gameObject.tag == "Machine") {
                //    child.gameObject.SetActive(true);
                //}
			} else {
				isPowered = false;
                //foreach (Transform child in transform) {
                //    if (child.gameObject.tag == "Machine")
                //        child.gameObject.SetActive(false);
                //}
			}
		}
		if (powerConsumption != 0 && !isBroken)
			moduleStatusText.text += ", ";
		pos = moduleStatusText.text.Length;
	}

	void SwitchTriggered (bool flag) {
		isOn = flag;
		center.SendMessage("CheckPowerSupply");
		if (!isOn) {
            //foreach (Transform child in transform) 
            //    if (child.gameObject.tag == "Machine") {
            //        child.gameObject.SetActive(false);
            //    }
			if (center.GetComponent<CentralControl>().isEnterOutpost)
				spriteRenderer.sprite = turnedOffSprite;
		} else if (isPowered){
            //foreach (Transform child in transform)
            //    if (child.gameObject.tag == "Machine")
            //    {
            //        child.gameObject.SetActive(true);
            //    }
			spriteRenderer.sprite = indoorSprite;
		} else spriteRenderer.sprite = noPowerSprite;
		spriteRenderer.sortingOrder = -3;
	}

	void OnTriggerStay2D(Collider2D other)
    {
		if (other.gameObject.tag == "Player"){
			//Debug.Log("Press T to turn on/off module");
			if (Input.GetKeyDown(turnKey) && !isBroken) {
				SwitchTriggered(!isOn);
			}
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.toolUsingEnable = false;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.toolUsingEnable = true;
        }
    }

	void AddConnection(GameObject other) {
		if (connections == null)
			connections = new List<GameObject>();
		connections.Add(other);
	}

	void DurabilityLoss() {
		if (durability > 0)
		{
			durabilityTimer += Time.deltaTime * durabilityLossSpeed;
			if (durabilityTimer > durabilityLossTime)
			{
				durability -= 1;
				durabilityTimer = 0;
			}
		} else {
			isBroken = true;
			if (flag) {
				SwitchTriggered(false);
				flag = false;
			}
		}

		if (isEnter) 
		{
	        if (player.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) > 0 && durability != 100)
	        {
	        	if (PlayerController.holdingRepairTool && PlayerController.toolUsingEnable)
	            {
	            	if (repairingFlag)
	                {
						if (Input.GetKeyDown(repairKey))
						{
							audioController.PlayRepairSound(2);
							repairArrowQueueFlag = !repairArrowQueueFlag;
							PlayerController.showRepairArrows = repairArrowQueueFlag;
							repairArrowQueue.SendMessage("Reset");
							repairArrowQueue.SetActive(repairArrowQueueFlag);
						}
						if (repairArrowQueue.GetComponent<ArrowQueueControl>().CorrectInput)
						{
							repairArrowQueue.GetComponent<ArrowQueueControl>().CorrectInput = false;
							repairingFlag = false;
							Invoke("TimeWait", repairActionTime);
						}
	                }
	            }
	        }
		}
		else
		{
			//repairArrowQueue.SetActive(false);
		}

		if (durability > 0) {
			if (pos < moduleStatusText.text.Length)
				moduleStatusText.text = moduleStatusText.text.Remove(pos);
			moduleStatusText.text += durability.ToString();
		}else
			moduleStatusText.text = "Broken";
	}

    void RepairAction()
    {
        if (!isBroken)
        {
            if (durability + 10 <= 100)
            {
                durability += 10;
            }
            else 
			{
				durability = 100;
				repairArrowQueueFlag = false;
				PlayerController.showRepairArrows = false;
				repairArrowQueue.SendMessage("Reset");
				repairArrowQueue.SetActive(false);
			}
        }
        else
        {
            durability += 10;
            isBroken = false;
            SwitchTriggered(true);
            flag = true;
			// arrow reset
			repairArrowQueue.SendMessage("Reset");
        }
		audioController.PlayRepairSound(3);
        player.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
    }

    void TimeWait()
    {
        repairingFlag = true;
		RepairAction();
    }

    void DisplayText(bool flag)
    {
        if (flag)
        {
            moduleStatusText.enabled = true;
        }
        else
        {
            if (isEnter)
            {
                //Debug.Log();
                if (moduleStatusText.enabled == false)
                {
                    moduleStatusText.enabled = true;
                }
            }
            else
            {
                if (moduleStatusText.enabled == true)
                {
                    moduleStatusText.enabled = false;
                }
            }
        }
    }

}