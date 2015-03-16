using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIModuleSelection : MonoBehaviour
{

    private bool firstTimeFlag = true;

    private static GameObject newBox;

    private float selectionBoxSize;

    private List<GameObject> allModuleSlots;

    private float moduleSelectionBoxWidth, moduleSelectionBoxHeight;

    private RectTransform moduleSelectionBoxRect;

    private int nthBox;

	private static int slots;

	private static int rows, columns;

    private GameObject moduleSelection;

	private static Slot from;

    private bool showDescriptionFlag;

    private Text description;

    private GameObject player;

    private ModuleItem habitatModule, connectorModule, foodModule, healthstaminaModule,
       moduleControlModule, powerModule, radarModule, refineryModule, robotModule,
       storageModule, airlockModule;


    void Start()
    {
		allModuleSlots = new List<GameObject>();
        player = GameObject.Find("MainPlayer");
		moduleSelection = player.GetComponent<PlayerController>().moduleSelection;
        description = player.GetComponent<PlayerController>().moduleDescription.GetComponentInChildren<Text>();
        RectTransform descriptionRect = description.GetComponent<RectTransform>();
        descriptionRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, moduleSelection.GetComponent<ModuleSelection>().moduleSelectionWidth - 10.0f);
        descriptionRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, moduleSelection.GetComponent<ModuleSelection>().moduleSelectionHeight - 5.0f);

        selectionBoxSize = moduleSelection.GetComponent<ModuleSelection>().moduleSlotSize + 7.0f;
		rows = moduleSelection.GetComponent<ModuleSelection>().moduleRows;
		columns = moduleSelection.GetComponent<ModuleSelection>().moduleColumns;
        showDescriptionFlag = true;

        habitatModule = GameObject.Find("UIHabitatModule").GetComponent<ModuleItem>();
        connectorModule = GameObject.Find("UIConnectorModule").GetComponent<ModuleItem>();
        foodModule = GameObject.Find("UIFoodModule").GetComponent<ModuleItem>();
        healthstaminaModule = GameObject.Find("UIHealthStaminaModule").GetComponent<ModuleItem>();
        moduleControlModule = GameObject.Find("UIModuleControlModule").GetComponent<ModuleItem>();
        powerModule = GameObject.Find("UIPowerModule").GetComponent<ModuleItem>();
        radarModule = GameObject.Find("UIRadarModule").GetComponent<ModuleItem>();
        refineryModule = GameObject.Find("UIRefineryModule").GetComponent<ModuleItem>();
        robotModule = GameObject.Find("UIRobotModule").GetComponent<ModuleItem>();
        storageModule = GameObject.Find("UIStorageModule").GetComponent<ModuleItem>();
        airlockModule = GameObject.Find("UIAirlockModule").GetComponent<ModuleItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.showModuleSelection)
        {
            if (firstTimeFlag)
            {
				ResetUIModuleSelection();
				slots = moduleSelection.GetComponent<ModuleSelection>().moduleSlots;
				allModuleSlots.AddRange(moduleSelection.GetComponent<ModuleSelection>().allModuleSlots);
                drawSelectionBox(0);
            }
            firstTimeFlag = false;
            showDescriptionFlag = true;
			MoveSelectionBox();
            ShowDescription();
			HandleKeyInput();
        }
        else
        {
			ResetUIModuleSelection();
        }

        showDescriptionFlag = false;
    }

	void ResetUIModuleSelection()
	{
		eraseSelectionBox();
		firstTimeFlag = true;
		allModuleSlots.Clear();
		nthBox = 0;
	}

	void eraseSelectionBox()
	{
		Destroy(newBox);
	}

    void drawSelectionBox(int nth)
    {
        newBox = Instantiate(Resources.Load("Inventory/SelectionBox")) as GameObject;
        RectTransform selectionBoxRect = newBox.GetComponent<RectTransform>();

        newBox.name = "SelectionBox" + nth;
		newBox.transform.SetParent(moduleSelection.transform);
        newBox.transform.localScale = new Vector3(1, 1, 1);

		selectionBoxRect.localPosition = 
			new Vector3(nth % columns * (moduleSelection.GetComponent<ModuleSelection>().moduleSlotSize + moduleSelection.GetComponent<ModuleSelection>().moduleSlotPaddingLeft),
			(-1) * nth / columns * (moduleSelection.GetComponent<ModuleSelection>().moduleSlotSize + moduleSelection.GetComponent<ModuleSelection>().moduleSlotPaddingTop));
        selectionBoxRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, selectionBoxSize);
        selectionBoxRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, selectionBoxSize);
    }

	
	private void MoveSelectionBox()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
            showDescriptionFlag = true;

			if (nthBox < slots - 1)
			{
				eraseSelectionBox();
				nthBox++;
				if (moduleSelection != null)
				{
					drawSelectionBox(nthBox);
				}
			}
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
            showDescriptionFlag = true;

			if (nthBox > 0)
			{
				eraseSelectionBox();
				nthBox--;

				if (moduleSelection != null)
				{
					drawSelectionBox(nthBox);
				}
			}
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
            showDescriptionFlag = true;

			if (nthBox >= columns)
			{
				eraseSelectionBox();
				nthBox -= columns;

				if (moduleSelection != null)
				{
					drawSelectionBox(nthBox);
				}
			}
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
            showDescriptionFlag = true;

			if (nthBox < slots - columns)
			{
				eraseSelectionBox();
				nthBox += columns;

				if (moduleSelection != null)
				{
					drawSelectionBox(nthBox);
				}
			}
		}
	}

    private void ShowDescription()
    {
        if (showDescriptionFlag)
        {
            if (nthBox == 0)
            {
                description.text = habitatModule.itemDescription;
            }
            else if (nthBox == 1)
            {
                description.text = connectorModule.itemDescription;
            }
            else if (nthBox == 2)
            {
                description.text = foodModule.itemDescription;
            }
            else if (nthBox == 3)
            {
                description.text = healthstaminaModule.itemDescription;
            }
            else if (nthBox == 4)
            {
                description.text = moduleControlModule.itemDescription;
            }
            else if (nthBox == 5)
            {
                description.text = powerModule.itemDescription;
            }
            else if (nthBox == 6)
            {
                description.text = refineryModule.itemDescription;
            }
            else if (nthBox == 7)
            {
                description.text = radarModule.itemDescription;
            }
            else if (nthBox == 8)
            {
                description.text = robotModule.itemDescription;
            }
            else if (nthBox == 9)
            {
                description.text = storageModule.itemDescription;
            }
            else if (nthBox == 10)
            {
                description.text = airlockModule.itemDescription;
            }
            else if (nthBox == 11)
            {
                description.text = "";
            }
            
        }
    }

    private void HandleKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            int i = 0;
            foreach (GameObject slot in allModuleSlots)
            {
                if (i == nthBox)
                {
                    if (GetSlot(slot) != null)
                    {
                        ModuleItem item = GetSlot(slot).GetItem();

                        if (item.itemName == ModuleItemName.HabitatModule)
                        {
                            Building.spawnHabitatModuleFlag = true;
                            Debug.Log("habitat module!");
                        }
                        else if (item.itemName == ModuleItemName.ConnectorModule)
                        {
                            Building.spawnConnectorModuleFlag = true;
                            Debug.Log("connector module!");
                        }
                        else if (item.itemName == ModuleItemName.FoodModule)
                        {
                            Building.spawnFoodModuleFlag = true;
                            Debug.Log("food module!");
                        }
                        else if (item.itemName == ModuleItemName.HealthStaminaModule)
                        {
                            Building.spawnHealthStaminaModuleFlag = true;
                            Debug.Log("health stamina module!");
                        }
                        else if (item.itemName == ModuleItemName.ModuleControlModule)
                        {
                            Building.spawnModuleControlModuleFlag = true;
                            Debug.Log("module control module!");
                        }
                        else if (item.itemName == ModuleItemName.PowerModule)
                        {
                            Building.spawnPowerModuleFlag = true;
                            Debug.Log("power module!");
                        }
                        else if (item.itemName == ModuleItemName.RefineryModule)
                        {
                            Building.spawnRefineryModuleFlag = true;
                            Debug.Log("refinery module!");
                        }
                        else if (item.itemName == ModuleItemName.RadarModule)
                        {
                            Building.spawnRadarModuleFlag = true;
                            Debug.Log("radar module!");
                        }
                        else if (item.itemName == ModuleItemName.RobotModule)
                        {
                            Building.spawnRobotModuleFlag = true;
                            Debug.Log("robot module!");
                        }
                        else if (item.itemName == ModuleItemName.StorageModule)
                        {
                            Building.spawnStorageModuleFlag = true;
                            Debug.Log("storage module!");
                        }
                        else if (item.itemName == ModuleItemName.AirlockModule)
                        {
                            Building.spawnAirlockModuleFlag = true;
                            Debug.Log("airlock module!");
                        }

                        PlayerController.showModuleSelection = false;
                    }
                }

                i++;
            }
        }
    }

	private ModuleSlot GetSlot(GameObject clicked)
	{
		//if (from == null)
		{
			if (!clicked.GetComponent<ModuleSlot>().IsEmpty)
			{
				return clicked.GetComponent<ModuleSlot>();
            }
        }
	
        return null;
    }
}
