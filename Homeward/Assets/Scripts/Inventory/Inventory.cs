/****
 * it is dangerous to use emptySlots if you are accessing directly to slot in Slot.cs
 * */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {
    
    private KeyCode invKey = KeyCode.I;

    public RectTransform inventoryRect;

    private float inventoryWidth, inventoryHeight;

    public int slots; // set in inspector

    public int rows;

	[HideInInspector] public int columns;

    public float slotPaddingLeft, slotPaddingTop;

    public float slotSize;

    public GameObject slotPrefab;

    public List<GameObject> allSlots;

    public GameObject iconPrefab; // icon to follow the cursor

    private static GameObject hoverObject;

    private Canvas canvas;

    private float hoverYOffset;

    private EventSystem eventSystem;

    private int emptySlots;

	private bool isEmpty;

    public int EmptySlots
    {
        get { return emptySlots; }
        set { emptySlots = value; }
    }

    public bool IsEmpty
    {
        get { return isEmpty; }
    }

    public bool IsFull
    {
        get { return emptySlots == 0; }
    }

	void Start () 
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        createLayout();
	}

    void createLayout()
    {
        allSlots = new List<GameObject>();

        hoverYOffset = slotSize * 0.01f;

        emptySlots = slots;

        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        inventoryRect = GetComponent<RectTransform>();
        inventoryRect.localScale = new Vector3(1, 1, 1);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        columns = slots / rows;

        

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newSlot = Instantiate(slotPrefab) as GameObject;

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                newSlot.name = "Slot";
                newSlot.transform.SetParent(this.transform.parent);
                newSlot.transform.localScale = new Vector3(1, 1, 1);

                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                allSlots.Add(newSlot);
            }
        }
    }

    

    
    
    
    
    
    /*****************************************************************************************************************/
	void Update () 
    {
		foreach (GameObject slot in allSlots)
		{
			Slot tmp = slot.GetComponent<Slot>();
			
			if (!tmp.IsEmpty)
			{
				isEmpty = false;
			}
		}
	}
    /*******************************************************************************************************************/






   
    public bool AddItem(Item item)
    {
        if (item.maxSize == 1)
        {
            PlaceEmpty(item);
            return true;
        }
        else
        {
            if (allSlots == null)
            {
                return false;
            }

            foreach (GameObject slot in allSlots)
			{
                Slot tmp = slot.GetComponent<Slot>();

                if (!tmp.IsEmpty)
                {
                    if (tmp.CurrentItem.itemName == item.itemName && tmp.IsAvailable)
                    {
                        tmp.AddItem(item);
                        return true;
                    }
                    //else if (tmp.CurrentItem.itemName == item.itemName && !tmp.IsAvailable)
                    //{
                    //    return false;
                    //}
                }
            }

            if (emptySlots > 0)
            {
                PlaceEmpty(item);
				return true;
            }
        }

        return false;
    }

    private bool PlaceEmpty(Item item)
    {
        if (emptySlots > 0)
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (tmp.IsEmpty)
                {
                    tmp.AddItem(item);
                    emptySlots--;
                    return true;
                }
            }
        }

        return false;
    }

    

    public void SetSlotsActive(bool show)
    {
        if (allSlots != null)
        {
            foreach (GameObject slot in allSlots)
            {
                slot.SetActive(show);
            }
        }
    }


    /*******************************************************************************************
     * Take care of the condition where this function returns empty item
     * *****************************************************************************************/
    public Item GetItem(ItemName itemName)
    {
        if (allSlots == null)
        {
            Debug.LogError("Inventory.cs: allSlots is null");
            return null;
        }
        else if (allSlots.Count == 0)
        {
            Debug.Log("Inventory.cs: there is no slot in this inventory");
        }
        else 
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (!tmp.IsEmpty)
                {
                    if (tmp.CurrentItem.itemName == itemName)
                    {
                        Item item = tmp.GetItem();

                        if (tmp.IsEmpty)
                        {
                            emptySlots++;
                        }

                        return item;
                    }
                    
                }
            }
        }

        Debug.LogError("Inventory.cs: Underflow - GetItem() is called when there is nothing to return.");
        return null;
    }

	public Item CheckItem(ItemName itemName)
	{
		if (allSlots == null)
		{
			Debug.LogError("Inventory.cs: allSlots is null");
			return null;
		}
		else if (allSlots.Count == 0)
		{
			Debug.Log("Inventory.cs: there is no slot in this inventory");
		}
		else 
		{
			foreach (GameObject slot in allSlots)
			{
				Slot tmp = slot.GetComponent<Slot>();
				
				if (!tmp.IsEmpty)
				{
					if (tmp.CurrentItem.itemName == itemName)
					{
						Item item = tmp.CheckItem();
						
						if (tmp.IsEmpty)
						{
							emptySlots++;
						}
						
						return item;
					}
					
				}
			}
		}
		
		Debug.LogError("Inventory.cs: Underflow - GetItem() is called when there is nothing to return.");
		return null;
	}

    /******************************************************************************************
     * Debug
     * ***************************************************************************************/
    public void DebugShowInventory()
    {
        if (IsEmpty)
        {
            Debug.Log("This Inventory is Empty");
        }
        else
        {
            foreach (GameObject slot in allSlots)
            {
                Debug.Log("Num occupied slots = " + (slots - emptySlots));

                Slot tmp = slot.GetComponent<Slot>();

                if(!tmp.IsEmpty)
                {
					Debug.Log(tmp.CurrentItem.itemName + ": " + tmp.Items.Count);
                }
            }
        }
    }

    public void ClearSlot(ItemName itemName)
    {
		Debug.Log (itemName);
        if (IsEmpty)
        {
            Debug.Log("This Inventory is Empty");
        }
        else
        {

            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (!tmp.IsEmpty && itemName == tmp.CurrentItem.itemName)
                {
                    tmp.ClearSlot();
                }
            }
        }
    }

    public int CountItems(ItemName itemName)
    {
		int itemCount = 0;
        if (isEmpty == true)
        {
            Debug.Log("This Inventory is Empty");
            return 0;
        }
        else
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (tmp != null)
                {
                    if (!tmp.IsEmpty && itemName == tmp.GetCurrentItem().itemName)
                    {
                        //Debug.Log(tmp.CurrentItem.itemName + ": " + tmp.Items.Count);
                        //return tmp.Items.Count;
						itemCount += tmp.Items.Count;
                    }
                }
            }
			return itemCount;
        }
    }





}
