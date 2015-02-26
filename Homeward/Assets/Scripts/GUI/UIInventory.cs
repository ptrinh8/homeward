using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour {

    private int nthBox;

    public static GameObject selectionBox;

    private static GameObject newBox;

    private float selectionBoxSize;

    private static Slot from, to;

    private int slots;

    private static GameObject playerInventory, moduleInventory;

    private bool firstTimeFlag;

    private int playerInventoryRows;

    private int playerInventorySlots;

    private static GameObject hoverObject;

    public GameObject iconPrefab; // icon to follow the cursor

    private List<GameObject> allSlots;

    public static void SetModuleInventory(GameObject moduleInv)
    {
        moduleInventory = moduleInv;
    }





	// Use this for initialization
	void Start () {
        allSlots = new List<GameObject>();
        firstTimeFlag = true;
        nthBox = 0;
        playerInventory = GameObject.Find("MainPlayer").GetComponent<PlayerController>().playerInventory;
        playerInventoryRows = playerInventory.GetComponent<Inventory>().rows;
        playerInventorySlots = playerInventory.GetComponent<Inventory>().slots;
        selectionBoxSize = playerInventory.GetComponent<Inventory>().slotSize + 3.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if (PlayerController.showPlayerInventory)
        {
            if (firstTimeFlag)
            {
                if (moduleInventory != null)
                {
                    slots = playerInventory.GetComponent<Inventory>().slots + moduleInventory.GetComponent<Inventory>().slots;
                    allSlots.AddRange(playerInventory.GetComponent<Inventory>().allSlots);
                    allSlots.AddRange(moduleInventory.GetComponent<Inventory>().allSlots);

                    playerInventory.GetComponent<Inventory>().DebugShowInventory();
                    Debug.Log("----");
                    moduleInventory.GetComponent<Inventory>().DebugShowInventory();
                }
                else
                {
                    slots = playerInventory.GetComponent<Inventory>().slots;
                    allSlots.AddRange(playerInventory.GetComponent<Inventory>().allSlots);
                }

                drawSelectionBox(0, playerInventory);
                firstTimeFlag = false;
}

            MoveSelectionBox();
            HandleKeyInput();
        }
        else
        {
            eraseSelectionBox();
            firstTimeFlag = true;
            allSlots.Clear();
            nthBox = 0;
        }

	}

    void drawSelectionBox(int nth, GameObject whichInventory)
    {
        newBox = Instantiate(Resources.Load("Inventory/SelectionBox")) as GameObject;
        RectTransform selectionBoxRect = newBox.GetComponent<RectTransform>();

        newBox.name = "SelectionBox" + nth;
        newBox.transform.SetParent(whichInventory.transform.parent);
        newBox.transform.localScale = new Vector3(1, 1, 1);

        selectionBoxRect.localPosition = whichInventory.GetComponent<Inventory>().inventoryRect.localPosition
            + new Vector3(nth % whichInventory.GetComponent<Inventory>().rows * (whichInventory.GetComponent<Inventory>().slotSize + whichInventory.GetComponent<Inventory>().slotPaddingLeft),
                (-1) * nth / whichInventory.GetComponent<Inventory>().rows * (whichInventory.GetComponent<Inventory>().slotSize + whichInventory.GetComponent<Inventory>().slotPaddingTop));
        selectionBoxRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, selectionBoxSize);
        selectionBoxRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, selectionBoxSize);
    }

    void eraseSelectionBox()
    {
        Destroy(newBox);
    }

    public void SetSelectionBoxActive(bool show)
    {
        if (newBox != null)
        {
            newBox.SetActive(show);
        }
    }

    private void MoveSelectionBox()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (nthBox < slots - 1)
            {
                eraseSelectionBox();
                nthBox++;

                if (moduleInventory == null)
                {
                    drawSelectionBox(nthBox, playerInventory);
                }
                else
                {
                    //if (nthBox % rows < playerInventory.GetComponent<Inventory>().rows)
                    //{
                    //    drawSelectionBox(nthBox - 4 * ((nthBox + playerInventoryRows) / rows), playerInventory);
                    //}
                    //else
                    //{
                    //    drawSelectionBox(nthBox - 4 * ((nthBox + moduleInventory.GetComponent<Inventory>().rows) / rows), moduleInventory);
                    //}
                    if (nthBox / playerInventorySlots < 1)
                    {
                        drawSelectionBox(nthBox, playerInventory);
                    }
                    else
                    {
                        drawSelectionBox(nthBox - playerInventorySlots, moduleInventory);
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (nthBox > 0)
            {
                eraseSelectionBox();
                nthBox--;

                if (moduleInventory == null)
                {
                    drawSelectionBox(nthBox, playerInventory);
                }
                else
                {
                    //if (nthBox % rows < playerInventoryRows)
                    //{
                    //    drawSelectionBox(nthBox - 4 * ((nthBox + playerInventoryRows) / rows), playerInventory);
                    //}
                    //else
                    //{
                    //    drawSelectionBox(nthBox - 4 * ((nthBox + moduleInventory.GetComponent<Inventory>().rows) / rows), moduleInventory);
                    //}
                    if (nthBox / playerInventorySlots < 1)
                    {
                        drawSelectionBox(nthBox, playerInventory);
                    }
                    else
                    {
                        drawSelectionBox(nthBox - playerInventorySlots, moduleInventory);
                    }
                }   
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (nthBox >= playerInventoryRows)
            {
                eraseSelectionBox();
                nthBox -= playerInventoryRows;

                if (moduleInventory == null)
                {
                    drawSelectionBox(nthBox, playerInventory);
                }
                else
                {
                    //if (nthBox % rows < playerInventoryRows)
                    //{
                    //    drawSelectionBox(nthBox - 4 * ((nthBox + playerInventoryRows) / rows) - playerInventoryRows, playerInventory);
                    //    nthBox -= rows;
                    //}
                    //else
                    //{
                    //    drawSelectionBox(nthBox - 4 * ((nthBox + playerInventoryRows) / rows) - playerInventoryRows, moduleInventory);
                    //    nthBox -= rows;
                    //}
                    if (nthBox / playerInventorySlots < 1)
                    {
                        drawSelectionBox(nthBox, playerInventory);
                    }
                    else
                    {
                        drawSelectionBox(nthBox - playerInventorySlots, moduleInventory);
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (nthBox < slots - 4) //MUST change 4 to something generic
            {
                eraseSelectionBox();
                nthBox += playerInventoryRows;

                if (moduleInventory == null)
                {
                    drawSelectionBox(nthBox, playerInventory);
                }
                else
                {
                    //if (nthBox % rows < playerInventory.GetComponent<Inventory>().rows)
                    //{
                    //    drawSelectionBox(nthBox - 4 * ((nthBox + playerInventoryRows) / rows) + playerInventoryRows, playerInventory);
                    //    nthBox += rows;
                    //}
                    //else
                    //{
                    //    drawSelectionBox(nthBox - 4 * ((nthBox + playerInventoryRows) / rows) + playerInventoryRows, moduleInventory);
                    //    nthBox += rows;
                    //}
                    if (nthBox / playerInventorySlots < 1)
                    {
                        drawSelectionBox(nthBox, playerInventory);
                    }
                    else
                    {
                        drawSelectionBox(nthBox - playerInventorySlots, moduleInventory);
                    }
                }
            }
        }
    }


    private void HandleKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            /*** search for slot ***/
            int i = 0;

            foreach (GameObject slot in allSlots)
            {
                if (i == nthBox)
                {
                    MoveItem(slot);
                }

                i++;
            }
        }

        //if (hoverObject != null)
        //{
        //    /*** make the item hover along to the cursor ***/
        //    Vector2 position;
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, newBox.transform.localPosition, canvas.worldCamera, out position);
        //    position.Set(position.x, position.y - hoverYOffset);
        //    hoverObject.transform.position = canvas.transform.TransformPoint(position);
        //}
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

                hoverObject = Instantiate(Resources.Load("Inventory/IconPrefab")) as GameObject;
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
