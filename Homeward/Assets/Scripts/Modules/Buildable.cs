using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// This class is for handling detached blueprint
public class Buildable : MonoBehaviour 
{
    //private ItemDatabase itemDatabase;
    //private Inventory inventory;
	public GameObject module;	// Completed module(prefabs)
	public int materialsRequired;	// Materials required to complete the module
	private int buildingProgress;	// Number of materials spend to build this module
	private SpriteRenderer spriteRenderer;	// Change of sprite color when needed
	private KeyCode buildKey = KeyCode.F;
	private KeyCode cancelKey = KeyCode.E;
	private Color color;

	private bool building;

    private GameObject player;
    private float buildActionTime;
    private bool canBuild;

	private GameObject buildingBarBackground;
	private float fillspeed;
	private float buildingBarFillAmount;
	private int progress;

	private Color buildingBarBackgroundDefaultColor;
	private Color buildingBarBackgroundFlashColor;
	private float buildingBarBackgroundFlashTimer;
	private float buildingBarBackgroundFlashTime;
	private int buildingBarBackgroundFlashCount;
	private int buildingBarBackgroundFlashNumber;
	private bool buildingBarBackgroundFlashing;
	private bool buildingBarBackgroundFlashAlternate;

	private AudioController audioController;
	private bool soundPlayed = false;

	// Use this for initialization
	void Start () 
    {
        //itemDatabase = FindObjectOfType(typeof(ItemDatabase)) as ItemDatabase;
        //inventory  = FindObjectOfType(typeof(Inventory)) as Inventory;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		color = spriteRenderer.color;
		color.a = 0.5f;
		spriteRenderer.color = color;
		buildingProgress = 0;

        player = GameObject.FindWithTag("Player");
        canBuild = true;
        buildActionTime = 0f;
		building = false;

		// building bar
		buildingBarBackground = GameObject.Find("Canvas").transform.FindChild("Building Bar Background").gameObject;
		fillspeed = 0.02f;
		buildingBarFillAmount = 0;
		progress = 0;

		buildingBarBackgroundFlashNumber = 3;
		buildingBarBackgroundFlashTime = .2f;

		audioController = GameObject.Find ("AudioObject").GetComponent<AudioController>();
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (buildingBarBackgroundFlashing == true && building == false)
		{
			buildingBarBackgroundFlashTimer += Time.deltaTime;
			StartCoroutine(FlashBuildingBar());
		}

		if (buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>() != null)
		{
			if (!building && buildingBarBackgroundFlashing == false)
			{
				Invoke ("Release", .5f);
			}

			if (buildingBarFillAmount >= 0 && buildingBarFillAmount <= 0.85f)
			{
				buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = Color.red;
			}
			else if (buildingBarFillAmount > 0.85f && buildingBarFillAmount <= 0.93f)
			{
				buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = Color.yellow;
				buildingBarBackgroundFlashColor = Color.yellow;
			}
			else if (buildingBarFillAmount > 0.93f && buildingBarFillAmount <= 0.98f)
			{
				buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = Color.green;
				buildingBarBackgroundFlashColor = Color.green;
			}
			else if (buildingBarFillAmount > 0.98f && buildingBarFillAmount <= 1f)
			{
				buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = Color.blue;
				buildingBarBackgroundFlashColor = Color.blue;
			}
			buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().fillAmount = buildingBarFillAmount;
			// else fail?
		}

		if (building == true)
		{
			if (!buildingBarBackground.activeSelf)
			{
				if (canBuild)
				{
					buildingBarBackground.SetActive(true);
					buildingBarBackground.GetComponent<Image>().color = Color.gray;
					buildingBarBackground.transform.position = GameObject.Find("Canvas").transform.position + new Vector3(8, -10, 0);
					buildingBarFillAmount = 0;
				}
			}
			else
			{
				buildingBarBackground.transform.position = GameObject.Find("Canvas").transform.position + new Vector3(8, -10, 0);
				if (buildingBarFillAmount <= 1)
				{
					buildingBarFillAmount +=  fillspeed;
				}
				else
				{
					buildingBarBackground.SetActive(false);
				}
			}
		}
		else if (building == false && Input.GetKey(buildKey) == false)
		{
			if (canBuild == true && buildingBarBackgroundFlashing == false) 
			{
				//audioController.PlayBuildingSound(2);
				buildingBarBackgroundFlashing = true;
				if (buildingBarFillAmount >= 0 && buildingBarFillAmount <= 0.85f)
				{
					buildingBarBackgroundFlashNumber = 0;
					progress = 0;
				}
				else if (buildingBarFillAmount > 0.85f && buildingBarFillAmount <= 0.93f)
				{
					buildingBarBackgroundFlashNumber = 3;
					progress = 1;
				}
				else if (buildingBarFillAmount > 0.93f && buildingBarFillAmount <= 0.98f)
				{
					buildingBarBackgroundFlashNumber = 6;
					progress = 2;
				}
				else if (buildingBarFillAmount > 0.98f && buildingBarFillAmount <= 1f)
				{
					buildingBarBackgroundFlashNumber = 9;
					progress = 3;
				}
			}
		}

		if (buildingProgress == materialsRequired) 
        {
			Instantiate(module, gameObject.transform.position, gameObject.transform.rotation);
			Destroy(gameObject);
        }
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
		{
			if (PlayerController.holdingBuildingTool && PlayerController.toolUsingEnable)
			{
				if (other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) > 0)
				{
					if (Input.GetKey(buildKey))
					{
						building = true;
						canBuild = false;
					}
					else
					{
						building = false;
						canBuild = true;
					}
				}
			}

            if (Input.GetKeyDown(cancelKey)){
                Destroy(gameObject);
				// return the materials?
			}
        }
    }

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			Reset();
			buildingBarBackground.SetActive(false);
		}
	}

    void BuildAction()
    {
		Debug.Log ("build");
		//audioController.PlayBuildingSound(2);
		if (canBuild == false)
		{
			if (buildingProgress + progress < materialsRequired)
			{
				buildingProgress += progress;
			}
			else 
			{
				progress = materialsRequired - buildingProgress;
				buildingProgress = materialsRequired;
			}
	        color.a += (0.4f / materialsRequired) * progress;
	        spriteRenderer.color = color;
			for (int i = 0; i < progress; i++)
			{
				player.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
			}
		}
		//buildingBarBackgroundFlashing = true;
		canBuild = true;
		Reset();
    }

	void Reset()
	{
		buildingBarFillAmount = 0;
		buildingBarBackground.SetActive(false);
		progress = 0;
	}

	IEnumerator FlashBuildingBar()
	{
		if (buildingBarBackgroundFlashCount < buildingBarBackgroundFlashNumber)
		{
			if (buildingBarBackgroundFlashTimer > buildingBarBackgroundFlashTime)
			{
				if (buildingBarBackgroundFlashAlternate == true)
				{
					buildingBarBackground.GetComponent<Image>().color = buildingBarBackgroundFlashColor;
					buildingBarBackgroundFlashAlternate = false;
				}
				else if (buildingBarBackgroundFlashAlternate == false)
				{
					buildingBarBackground.GetComponent<Image>().color = Color.black;
					buildingBarBackgroundFlashAlternate = true;
				}
				buildingBarBackgroundFlashTimer = 0f;
				buildingBarBackgroundFlashCount++;
			}
		}
		else
		{
			buildingBarBackgroundFlashTimer = 0f;
			buildingBarBackgroundFlashCount = 0;
			buildingBarBackgroundFlashing = false;
		}
		yield return null;
	}

	void Release()
	{
		//soundPlayed = false;
		if (buildingBarFillAmount > 0)
		{
			canBuild = false;
			buildingBarFillAmount = 0;
			//if (buildingBarFillAmount < 0) {buildingBarFillAmount = 0;}
		}
		else 
		{
			if (canBuild == false)
			{
				BuildAction();
			}
			else
			{
				Reset ();
			}
		}
	}
}
