// ========================================================================
// <file="Food.cs" product="Homeward">
// <date>2014-11-11</date>
// ========================================================================


using UnityEngine;
using System.Collections;
using System;

public class Food : MonoBehaviour
{
    private PlayerController playerController = new PlayerController();
    private Mining minerals = new Mining();
    private Inventory inventory = new Inventory();

    private int foodCreated;

    private Vector2 worldSpacePos;
    private float loadingStartTime;
    private float loadingUpdateTime;
    private float loadingPercent;
    private bool timerReached = false;

    private bool stopMaterialsIntake = false;
    private bool updateNumberOfFoodItems = false;
    private bool addFoodOnce = false;

    private GameObject debugLoadingText;
    private GameObject debugMineralsAddedText;
    private GameObject debugMineralsRefinedText;

    public Sprite activeTexture;
    public Sprite deactiveTexture;
    public Sprite noPowerSupplyTexture;
    private GameObject refineryModule;

    private bool showPlayerAndModuleInventory;
    public GameObject moduleInventory;
    private GameObject mainPlayer;

    private KeyCode keyToAddItemsFromMainPlayerInventory = KeyCode.K;
    private KeyCode keyToAddItemsDirectlyToModuleinventory = KeyCode.L;

    private void MineralsValidations()
    {
        if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) == 0)
        {
            stopMaterialsIntake = true;
        }
        else if (mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) > 0)
        {
            stopMaterialsIntake = false;
        }
    }

    void Start()
    {
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        minerals = FindObjectOfType(typeof(Mining)) as Mining;
        inventory = FindObjectOfType(typeof(Inventory)) as Inventory;

        moduleInventory = Instantiate(moduleInventory) as GameObject;
        moduleInventory.transform.SetParent(GameObject.Find("Canvas").transform);
        moduleInventory.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        showPlayerAndModuleInventory = false;
        moduleInventory.SetActive(showPlayerAndModuleInventory);

        mainPlayer = GameObject.Find("MainPlayer");
        refineryModule = gameObject;
        worldSpacePos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
    }

    void Update()
    {
        var refineryModuleSpriteRenderer = refineryModule.GetComponent<SpriteRenderer>();
        if (!gameObject.transform.root.gameObject.GetComponent<LocalControl>().IsPowered)
        {
            refineryModuleSpriteRenderer.sprite = noPowerSupplyTexture;
        }
        else
        {
            refineryModuleSpriteRenderer.sprite = deactiveTexture;
        }

        changeLoadingToPercent();
        MineralsValidations();

        if (moduleInventory.GetComponent<Inventory>().CountItems(ItemName.Material) == 2)
        {
            stopMaterialsIntake = true;
            startTimer();
            updateNumberOfFoodItems = false;
            refineryModuleSpriteRenderer.sprite = activeTexture;

            if (loadingUpdateTime == 500.0f)
            {
                stopTimer();
                refineryModuleSpriteRenderer.sprite = deactiveTexture;

                if (!updateNumberOfFoodItems)
                {
                    addFoodOnce = false;
                    updateNumberOfFoodItems = true;

                    if (!addFoodOnce)
                    {
                        addFoodOnce = true;
                        Item item = GameObject.Find("Food1").GetComponent<Item>();
                        mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
                        moduleInventory.GetComponent<Inventory>().ClearSlot(ItemName.Material);
                        timerReached = false;
                    }
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.transform.root.gameObject.GetComponent<LocalControl>().IsPowered)
            {
                showPlayerAndModuleInventory = true;
                PlayerController.KeyCode_I_Works = !showPlayerAndModuleInventory;
                PlayerController.ShowPlayerInventory = showPlayerAndModuleInventory;
                moduleInventory.SetActive(true);
                moduleInventory.GetComponent<Inventory>().SetSlotsActive(showPlayerAndModuleInventory);
            }
            else
            {
                showPlayerAndModuleInventory = false;
                moduleInventory.SetActive(showPlayerAndModuleInventory);
                moduleInventory.GetComponent<Inventory>().SetSlotsActive(showPlayerAndModuleInventory);

                PlayerController.ShowPlayerInventory = showPlayerAndModuleInventory;
                PlayerController.KeyCode_I_Works = !showPlayerAndModuleInventory;
            }

            if (showPlayerAndModuleInventory)
            {
                if (Input.GetKeyDown(keyToAddItemsDirectlyToModuleinventory))
                {

                    Item item = GameObject.Find("Water").GetComponent<Item>();
                    moduleInventory.GetComponent<Inventory>().AddItem(item);
                }

                if (Input.GetKeyDown(keyToAddItemsFromMainPlayerInventory))
                {
                    if ((mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) > 0) && (stopMaterialsIntake == false))
                    {
                        Item item = other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
                        moduleInventory.GetComponent<Inventory>().AddItem(item);
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        showPlayerAndModuleInventory = false;
        moduleInventory.SetActive(showPlayerAndModuleInventory);
        moduleInventory.GetComponent<Inventory>().SetSlotsActive(showPlayerAndModuleInventory);
        PlayerController.ShowPlayerInventory = showPlayerAndModuleInventory;
        PlayerController.KeyCode_I_Works = !showPlayerAndModuleInventory;
    }

    void startTimer()
    {
        if (!timerReached) { loadingUpdateTime = loadingStartTime++; }
        if (loadingUpdateTime == 500) { timerReached = true; }
    }

    void stopTimer()
    {
        if (timerReached == true) { loadingUpdateTime = 0.0f; loadingStartTime = 0.0f; }
    }

    void changeLoadingToPercent()
    {
        loadingPercent = loadingUpdateTime / 5;
    }
}
