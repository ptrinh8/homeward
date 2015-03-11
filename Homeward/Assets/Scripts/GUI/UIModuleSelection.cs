using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    void Start()
    {
		allModuleSlots = new List<GameObject>();
		moduleSelection = GameObject.Find("MainPlayer").GetComponent<PlayerController>().moduleSelection;
		selectionBoxSize = moduleSelection.GetComponent<ModuleSelection>().moduleSlotSize - 20.0f;
		rows = moduleSelection.GetComponent<ModuleSelection>().moduleRows;
		columns = moduleSelection.GetComponent<ModuleSelection>().moduleColumns;

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
			MoveSelectionBox();
			HandleKeyInput();
        }
        else
        {
			ResetUIModuleSelection();
        }
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
		newBox.transform.SetParent(moduleSelection.transform.parent);
        newBox.transform.localScale = new Vector3(1, 1, 1);

		selectionBoxRect.localPosition = moduleSelection.GetComponent<ModuleSelection>().moduleSelectionRect.localPosition +
			new Vector3(nth % columns * moduleSelection.GetComponent<ModuleSelection>().moduleSlotSize + moduleSelection.GetComponent<ModuleSelection>().moduleSlotPaddingLeft,
			(-1) * nth / columns * moduleSelection.GetComponent<ModuleSelection>().moduleSlotSize + moduleSelection.GetComponent<ModuleSelection>().moduleSlotPaddingTop);
        selectionBoxRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, selectionBoxSize);
        selectionBoxRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, selectionBoxSize);
    }

	
	private void MoveSelectionBox()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
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
							
								//if (from != null)
							{
								if (item.itemName == ModuleItemName.HabitatModule)
								{
									Debug.Log("habitat module!");
									}
								else if (item.itemName == ModuleItemName.ConnectorModule)
								{
									Debug.Log("connector module!");
									}
								else if (item.itemName == ModuleItemName.AirlockModule)
								{
									Debug.Log("airlock module!");
									}
								else if (item.itemName == ModuleItemName.FoodModule)
								{
									Debug.Log("food module!");
									}
								else if (item.itemName == ModuleItemName.HealthStaminaModule)
								{
									Debug.Log("health stamina module!");
									}
								else if (item.itemName == ModuleItemName.ModuleControlModule)
								{
									Debug.Log("module control module!");
									}
								else if (item.itemName == ModuleItemName.PowerModule)
								{
									Debug.Log("power module!");
									}
								else if (item.itemName == ModuleItemName.RefineryModule)
								{
									Debug.Log("refinery module!");
									}
								else if (item.itemName == ModuleItemName.RadarModule)
								{
									Debug.Log("radar module!");
									}
								else if (item.itemName == ModuleItemName.RobotModule)
								{
									Debug.Log("robot module!");
									}
								else if (item.itemName == ModuleItemName.StorageModule)
								{
									Debug.Log("storage module!");
									}
									PlayerController.showModuleSelection = false;
								}
							}
						}
						i++;
					}
			}
		}

		private ModuleSlot GetSlot(GameObject clicked)
	{
		if (from == null)
		{
			if (!clicked.GetComponent<ModuleSlot>().IsEmpty)
			{
				return clicked.GetComponent<ModuleSlot>();
				}
			}
			return null;
		}
}
