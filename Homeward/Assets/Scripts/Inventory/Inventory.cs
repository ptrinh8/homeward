using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {
    
    private KeyCode invKey = KeyCode.I;

    private RectTransform inventoryRect;

    private float inventoryWidth, inventoryHeight;

    private bool showInventory;

    public int slots; // set in inspector

    public int rows;

    public float slotPaddingLeft, slotPaddingTop;

    public float slotSize;

    public GameObject slotPrefab;

    private List<GameObject> allSlots;

    private static Slot from, to;

    public GameObject iconPrefab; // icon to follow the cursor

    private static GameObject hoverObject;

    private Canvas canvas;

    private float hoverYOffset;

    private EventSystem eventSystem;

    private int emptySlots;

    public int EmptySlots
    {
        get { return emptySlots; }
        set { emptySlots = value; }
    }

    public bool IsEmpty
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

        int columns = slots / rows;

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

	void Update () 
    {
        //HandleMouseInput();
	}

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
                    else if (tmp.CurrentItem.itemName == item.itemName && !tmp.IsAvailable)
                    {
                        return false;
                    }
                }
            }

            if (emptySlots > 0)
            {
                PlaceEmpty(item);
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

    /******************************************************************************************
     * Debug
     * ***************************************************************************************/
    public void DebugShowInventory()
    {
        if (IsEmpty)
        {
         //   Debug.Log("This Inventory is Empty");
        }
        else
        {
            // Debug.Log("Num Occupied Slots = " + (slots - emptySlots));

            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if(!tmp.IsEmpty)
                {
                    // Debug.Log(tmp.CurrentItem.itemName + ": " + tmp.Items.Count);
                }
            }
        }
    }

    public void ClearSlot(ItemName itemName)
    {
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
        if (IsEmpty)
        {
            Debug.Log("This Inventory is Empty");
            return 0;
        }
        else
        {
           // Debug.Log("Num Occupied Slots = " + (slots - emptySlots));

            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (!tmp.IsEmpty && itemName == tmp.CurrentItem.itemName)
                {
                    //Debug.Log(tmp.CurrentItem.itemName + ": " + tmp.Items.Count);
                    return tmp.Items.Count;
                }
                
            }
            return 0;
        }
    }

    /**************************************************************************
     * use this until I figure out GUI handling with arrow keys
     * ***************************************************************************/
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonUp(0)) // 0 = left click // moveitem
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null) // -1 = mouse pointer
            {
                from.GetComponent<Image>().color = Color.white;
                from.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                to = null;
                from = null;
                hoverObject = null;
            }
        }

        //if (Input.GetMouseButtonUp(1)) // 1 = right click // use item
        //{
        //    //Debug.Log("asdfasdfasdfasdf");
        //    if (!eventSystem.IsPointerOverGameObject(-1) && from != null) // -1 = mouse pointer
        //    {
        //        Debug.Log("asdfasdfasdfasdf");
        //        from.GetComponent<Slot>().GetItem();
        //    }
        //}

        if (hoverObject != null)
        {
            /*** make the item hover along to the cursor ***/
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y - hoverYOffset);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }


    }

    /****************************************************************************
     * Moves item when left clicked
     * **************************************************************************/
    public void MoveItem(GameObject clicked)
    {
        if (from == null)
        {
            if (!clicked.GetComponent<Slot>().IsEmpty)
            {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.grey;

                hoverObject = Instantiate(iconPrefab) as GameObject;
                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                hoverObject.name = "Hover";

                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
                hoverObject.transform.localScale = from.gameObject.transform.localScale;

            }
        }
        else if (to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }

        if (to != null && from != null)
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items);

            to.AddItems(from.Items);

            if (tmpTo.Count == 0)
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tmpTo);
            }

            from.GetComponent<Image>().color = Color.white;

            to = null;
            from = null;
            hoverObject = null;
        }
    }
}
