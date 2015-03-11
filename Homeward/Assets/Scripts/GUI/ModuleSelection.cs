using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ModuleSelection : MonoBehaviour
{

    private static GameObject newBox;

    private float selectionBoxSize;

    [HideInInspector] public  List<GameObject> allModuleSlots;

    private float moduleSelectionWidth, moduleSelectionHeight;

    public int moduleSlots; // set in inspector

    public int moduleRows;

    [HideInInspector] public int moduleColumns;

    public float moduleSlotPaddingLeft, moduleSlotPaddingTop;

    public float moduleSlotSize;

    public GameObject moduleSlotPrefab;

    private ModuleItem habitatModule, connectorModule, foodModule, healthstaminaModule,
        moduleControlModule, powerModule, radarModule, refineryModule, robotModule,
        storageModule;

    [HideInInspector] public RectTransform moduleSelectionRect;

    private int emptySlots;

    public bool IsEmpty
    {
        get { return emptySlots == 0; }
    }


    // Use this for initialization
    void Awake()
    {
        CreateLayout();
        SetModuleItems();

        DebugShowInventory();
    }




    void SetModuleItems()
    {
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
        
        AddItem(habitatModule);
        AddItem(connectorModule);
        AddItem(foodModule);
        AddItem(healthstaminaModule);
        AddItem(moduleControlModule);
        AddItem(powerModule);
        AddItem(radarModule);
        AddItem(refineryModule);
        AddItem(robotModule);
        AddItem(storageModule);
    }


    void CreateLayout()
    {
        allModuleSlots = new List<GameObject>();

        moduleSelectionWidth = (moduleSlots / moduleRows) * (moduleSlotSize + moduleSlotPaddingLeft) + moduleSlotPaddingLeft;
        moduleSelectionHeight = moduleRows * (moduleSlotSize + moduleSlotPaddingTop) + moduleSlotPaddingTop;

        moduleSelectionRect = GetComponent<RectTransform>();
        moduleSelectionRect.localScale = new Vector3(1, 1, 1);
        moduleSelectionRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, moduleSelectionWidth);
        moduleSelectionRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, moduleSelectionHeight);

        moduleColumns = moduleSlots / moduleRows;

        emptySlots = moduleSlots;

        for (int y = 0; y < moduleRows; y++)
        {
            for (int x = 0; x < moduleColumns; x++)
            {
                GameObject newSlot = Instantiate(moduleSlotPrefab) as GameObject;

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                newSlot.transform.SetParent(this.transform);
                newSlot.transform.localScale = new Vector3(1, 1, 1);

                slotRect.localPosition = moduleSelectionRect.localPosition + new Vector3(moduleSlotPaddingLeft * (x + 1) + (moduleSlotSize * x), -moduleSlotPaddingTop * (y + 1) - (moduleSlotSize * y));
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, moduleSlotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, moduleSlotSize);

                allModuleSlots.Add(newSlot);
            }
        }
    }




    public bool AddItem(ModuleItem moduleItem)
    {
        PlaceEmpty(moduleItem);
        return true;
    }

    private bool PlaceEmpty(ModuleItem moduleItem)
    {
        if (emptySlots > 0)
        {
            foreach (GameObject slot in allModuleSlots)
            {
                ModuleSlot tmp = slot.GetComponent<ModuleSlot>();

                if (tmp != null)
                {
                    if (tmp.IsEmpty)
                    {
                        tmp.AddItem(moduleItem);
                        emptySlots--;
                        return true;
                    }
                }
            }
        }

        return false;
    }


    public void DebugShowInventory()
    {
        if (IsEmpty)
        {
            Debug.Log("This Inventory is Empty");
        }
        else
        {
            Debug.Log("Num Occupied Slots = " + (moduleSlots - emptySlots));

            foreach (GameObject slot in allModuleSlots)
            {
                ModuleSlot tmp = slot.GetComponent<ModuleSlot>();

                if (tmp != null)
                {
                    if (!tmp.IsEmpty)
                    {
                        //Debug.Log(tmp.CurrentItem.itemName + ": " + tmp.Items.Count);
                    }
                }
            }
        }
    }

    public void SetModuleSlotsActive(bool show)
    {
        if (allModuleSlots != null)
        {
            foreach (GameObject slot in allModuleSlots)
            {
                slot.SetActive(show);
            }
        }
    }
}
