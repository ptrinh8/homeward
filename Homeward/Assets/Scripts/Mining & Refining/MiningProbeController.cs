using UnityEngine;
using System.Collections;

public class MiningProbeController : MonoBehaviour {

    GameObject player;

    bool spawnFlag;

    public GameObject probeInventory;

	void Start () 
    {
        player = GameObject.Find("MainPlayer");
        spawnFlag = true;

        gameObject.transform.SetParent(player.transform);
        gameObject.transform.position = player.transform.position;
        gameObject.renderer.enabled = false;

        probeInventory = Instantiate(probeInventory) as GameObject;
        probeInventory.transform.position = gameObject.transform.position;
        probeInventory.GetComponent<Inventory>().slots = 4;
        probeInventory.GetComponent<Inventory>().rows = 1;
        probeInventory.AddComponent<CanvasGroup>();
        probeInventory.GetComponent<CanvasGroup>().alpha = 0;
        probeInventory.AddComponent<UIInventory>();
        probeInventory.SetActive(false);
        probeInventory.GetComponent<Inventory>().SetSlotsActive(false);
        probeInventory.transform.SetParent(GameObject.Find("Canvas").transform);
        UIInventory.SetModuleInventory(null);
	}
	
	void Update () 
    {

        if (Input.GetKeyDown(KeyCode.H) && spawnFlag)
        {
            spawnFlag = false;

            gameObject.transform.SetParent(player.transform.parent);
            gameObject.renderer.enabled = true;

            InvokeRepeating("Mine", 3, 3); // mines once in ten seconds
        }
	}

    void Mine()
    {
        Item item = GameObject.Find("Material").GetComponent<Item>();
        probeInventory.GetComponent<Inventory>().AddItem(item);
        probeInventory.GetComponent<Inventory>().DebugShowInventory();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            probeInventory.SetActive(true);
            probeInventory.GetComponent<CanvasGroup>().alpha = 1;
            probeInventory.GetComponent<Inventory>().SetSlotsActive(true);
            UIInventory.SetModuleInventory(probeInventory);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            probeInventory.GetComponent<CanvasGroup>().alpha = 0;
            probeInventory.GetComponent<Inventory>().SetSlotsActive(false);
            UIInventory.SetModuleInventory(null);
        }
    }
}
