using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// This class is for handling detached blueprint
public class Buildable : MonoBehaviour 
{
	public GameObject module;	// Completed module(prefabs)
	public int materialsRequired;	// Materials required to complete the module
	private int buildingProgress;	// Number of materials spend to build this module
	private SpriteRenderer spriteRenderer;	// Change of sprite color when needed
	private KeyCode buildKey = KeyCode.F;
	private KeyCode cancelKey = KeyCode.E;
	private Color color;

    private GameObject player;
    private float buildActionTime;
    private bool buildingFlag;

	private GameObject buildingBarBackground;
	private float fillspeed;
	private float buildingBarFillAmount;
	private int progress;

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
        buildingFlag = true;
        buildActionTime = 0f;

		// building bar
		buildingBarBackground = GameObject.Find("Canvas").transform.FindChild("Building Bar Background").gameObject;
		fillspeed = 0.02f;
		buildingBarFillAmount = 0;
		progress = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>() != null)
		{
			if (!buildingFlag)
			{
				buildingBarBackground.GetComponent<Image>().color = Color.black;
				Invoke("Release", 0.5f);
			}

			if (buildingBarFillAmount >= 0 && buildingBarFillAmount <= 0.85f)
			{
				buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = Color.red;
			}
			else if (buildingBarFillAmount > 0.85f && buildingBarFillAmount <= 0.93f)
			{
				buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = Color.yellow;
			}
			else if (buildingBarFillAmount > 0.93f && buildingBarFillAmount <= 0.98f)
			{
				buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = Color.green;
			}
			else if (buildingBarFillAmount > 0.98f && buildingBarFillAmount <= 1f)
			{
				buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().color = Color.blue;
			}
			buildingBarBackground.transform.FindChild("BuildingBar").gameObject.GetComponent<Image>().fillAmount = buildingBarFillAmount;
			// else fail?
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
						if (!buildingBarBackground.activeSelf)
						{
							if (buildingFlag)
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
					else
					{
						if (buildingFlag) 
						{
							buildingFlag = false;
							if (buildingBarFillAmount >= 0 && buildingBarFillAmount <= 0.85f)
							{
								progress = 0;
							}
							else if (buildingBarFillAmount > 0.85f && buildingBarFillAmount <= 0.93f)
							{
								progress = 1;
							}
							else if (buildingBarFillAmount > 0.93f && buildingBarFillAmount <= 0.98f)
							{
								progress = 2;
							}
							else if (buildingBarFillAmount > 0.98f && buildingBarFillAmount <= 1f)
							{
								progress = 3;
							}
						}
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
		buildingFlag = true;
		Reset();
    }

	void Reset()
	{
		buildingBarFillAmount = 0;
		buildingBarBackground.SetActive(false);
		progress = 0;
	}

	void Release()
	{
		if (buildingBarFillAmount > 0)
		{
			buildingBarFillAmount -= fillspeed;
			if (buildingBarFillAmount < 0) {buildingBarFillAmount = 0;}
		}
		else 
		{
			BuildAction();
		}
	}
}
