using UnityEngine;
using System.Collections;

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

    private GameObject player;
    private float buildActionTime;
    private bool buildingFlag;

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
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (buildingProgress >= materialsRequired) 
        {
			Instantiate(module, gameObject.transform.position, gameObject.transform.rotation);
			Destroy(gameObject);
        }
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player"){
            if (Input.GetKeyDown(cancelKey))
                Destroy(gameObject);

            if (Input.GetKeyDown(buildKey))
            {
			    if(other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) > 0)
			    {
                        if (PlayerController.holdingBuildingTool && PlayerController.toolUsingEnable)
                        {
                            if (buildingFlag) {
                                buildingFlag = false;
                                BuildAction();
                                Invoke("TimeWait", buildActionTime);
                        }
                    }
			    }
            }
        }
    }

    void BuildAction()
    {
        buildingProgress++;
        color.a += 0.4f / materialsRequired;
        spriteRenderer.color = color;
        player.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
    }

    void TimeWait()
    {
        buildingFlag = true;
    }
}
