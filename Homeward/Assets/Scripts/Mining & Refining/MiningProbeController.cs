using UnityEngine;
using System.Collections;
using System;

public class MiningProbeController : MonoBehaviour
{

    GameObject player;

    public static bool spawnFlag;

    public GameObject probeInventory;

    int miningCount;

    int maxMiningCount;

    GameObject[] tiles;

    GameObject GameObject;

    int abc = 0;


    void Start()
    {
        player = GameObject.Find("MainPlayer");
        spawnFlag = false;
        miningCount = 0;
        maxMiningCount = 0;

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

        GameObject = GameObject.Find("GameObject");
    }


    void Update()
    {
        if (spawnFlag)
        {
            Debug.Log("kitakitakitakita");

            tiles = GameObject.FindGameObjectsWithTag("FinalTextures");

            PlanetTileController.probeSpawnedFlag = true;

            foreach (GameObject tile in tiles)
            {
                if (playerIsInside(tile))
                {
                    Debug.Log("player is inside of " + tile.name);
                    int[] extractableValues = GameObject.GetComponent<PCG_Tiles>().ExtractableValues;
                    string num = tile.name.Remove(0, 14);
                    maxMiningCount = extractableValues[Int32.Parse(num)];
                }
            }

            spawnFlag = false;

            gameObject.transform.SetParent(player.transform.parent);
            gameObject.renderer.enabled = true;

            InvokeRepeating("Mine", 3, 3); // mines once in 3 seconds
        }
    }

    bool playerIsInside(GameObject tile)
    {
        if (tile.GetComponent<PlanetTileController>().playerIsInside)
        {
            Debug.Log("PlayerIsInside");

            return true;
        }

        return false;
    }

    void Mine()
    {
        if (miningCount < maxMiningCount)
        {
            Item item = GameObject.Find("Mineral").GetComponent<Item>();
            probeInventory.GetComponent<Inventory>().AddItem(item);
            probeInventory.GetComponent<Inventory>().DebugShowInventory();
            miningCount++;
        }
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
