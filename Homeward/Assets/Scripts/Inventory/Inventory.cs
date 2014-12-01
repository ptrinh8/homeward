// ==================================================================================
// <file="Inventory.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Contains a base, abstract class for Inventory Management</summary>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#endregion

public class Inventory : MonoBehaviour 
{
    private MineralsStatus mineralsStatus;
    private ItemDatabase itemDatabase;

	[HideInInspector]
	public bool showInventory; 
	private KeyCode invKey = KeyCode.I; 
	private int itemsSlotsSize = 50; 
	private int itemsSlotsPadding = 12; 
	private int backpackRowsCount = 3; 
	private int backpackColsCount = 3; 
	private Rect inventorySize = new Rect(100, 200, 0, 0); // Used to add bground to backpack, in consideration with values of rows & cols.

	public GUISkin GUIskin; 
    [HideInInspector]
	public List<Item> inventory = new List<Item>(); // Holds player's items
	private ItemDatabase database; // Stores items in DB
	private bool showInventoryItemDetails; // Each item information
	private string inventoryItemDetailsText; // Item information text
    [HideInInspector]
	public bool isItemBeingDragged; 
	private Item thisItemIsBeingDragged; 

	// Coordinate of the click to be able to draw the sprite at the correct off-set to the mouse
    private Vector2 mouseDragCoordinates;
	
	void Start () 
    {
        mineralsStatus = FindObjectOfType(typeof(MineralsStatus)) as MineralsStatus;
        itemDatabase = FindObjectOfType(typeof(ItemDatabase)) as ItemDatabase;

		// Loop to add an inventory slot for each slot based on the result of XxY
        for (int i = 0; i < (backpackRowsCount * backpackColsCount); i++)
        {
            inventory.Add(new Item());
        }

        GUIskin = Resources.Load<GUISkin>("InvGUIskin");
		showInventory = false;

        // Set DB instance = DB.component
		database = GameObject.Find("Item Database").GetComponent<ItemDatabase>();

		// Add items with following IDs
	    //	AddItem (0);
	    //	AddItem (1);

	}

	void Update()
	{
		if (Input.GetKeyDown(invKey))
			showInventory = !showInventory;
	}

	void OnGUI()
	{
		GUI.skin = GUIskin;

		// Set item information as blank
		inventoryItemDetailsText = "";

        // is inventory visible?
		if (showInventory)
		{
			DrawInventory();
            
            // is mouse on item?, which makes showInventoryItemDetails = true
			if (showInventoryItemDetails)
				DrawItemDetails();
            
            // is item being dragged? make icon follow mouse
			if (isItemBeingDragged)
				GUI.DrawTexture(new Rect(Event.current.mousePosition.x - mouseDragCoordinates.x, Event.current.mousePosition.y 
                - mouseDragCoordinates.y, itemsSlotsSize, itemsSlotsSize), thisItemIsBeingDragged.itemIcon);
		}

		else if (isItemBeingDragged)
		{
			isItemBeingDragged = false;
			thisItemIsBeingDragged = null;
		}
	}

	void DrawInventory()
	{
		int i = 0;

		// Calculate size of inventory window
        inventorySize.width = (itemsSlotsSize + itemsSlotsPadding) * backpackColsCount + itemsSlotsPadding;
        inventorySize.height = (itemsSlotsSize + itemsSlotsPadding) * backpackRowsCount + itemsSlotsPadding;

		// Draw background
		GUI.Box(inventorySize, "", GUIskin.GetStyle("Inventory Background"));

		// Current GUI input event stored in an Event variable
		Event currentGUIevent = Event.current;

		// Position and size of each item slot saved in a temp variable used for drawing the slots
        Rect slotRect = new Rect(inventorySize.x, inventorySize.y, itemsSlotsSize, itemsSlotsSize);
		
        for (int y = 0; y < backpackRowsCount; y++)
		{
            for (int x = 0; x < backpackColsCount; x++)
			{
				// Modify slotRect based on the position of the inventory window and the current item the loop is on
				slotRect.x = itemsSlotsPadding + inventorySize.x + x * (itemsSlotsSize + itemsSlotsPadding); // column position
				slotRect.y = itemsSlotsPadding + inventorySize.y + y * (itemsSlotsSize + itemsSlotsPadding); // row position

				Item item = inventory[i];

				GUI.Box(slotRect, "", GUIskin.GetStyle("Box"));
                
                // Does slot have a name in it?
				if (item.itemName != null)
				{
					// Yes, draw the icon
                    if (item.itemIcon != null)
                    {
                        GUI.DrawTexture(slotRect, item.itemIcon);
                    }

					// Is mouse position within the slot?
					if (slotRect.Contains(Event.current.mousePosition))
					{
                        // Is left-click pressed? also, is item not being dragged?
                        if (currentGUIevent.isMouse && currentGUIevent.button == 0 && currentGUIevent.type == EventType.mouseDown && !isItemBeingDragged)
						{
                            // Start dragging
                            mouseDragCoordinates = GetIconCoordDifference(currentGUIevent.mousePosition, slotRect);
							isItemBeingDragged = true;
							thisItemIsBeingDragged = item;
							inventory[i] = new Item();
						}

                        // Is no button pressed while item being dragged?
                        if (currentGUIevent.isMouse && currentGUIevent.type == EventType.mouseUp && isItemBeingDragged)
						{
                            // Replace items
							Item tempItem = item;
							inventory[i] = thisItemIsBeingDragged;
							thisItemIsBeingDragged = tempItem;
						}

                        // Is item being dragged?
						if (!isItemBeingDragged)
						{
                            // No. Show item information
							inventoryItemDetailsText = CreateItemDetails(item);
							showInventoryItemDetails = true;
						}
					} 
					// If item information text = "", show nothing
					if (inventoryItemDetailsText == "")
						showInventoryItemDetails = false;
				}

				else 
				{
                    // Is no button pressed while item being dragged?
                    if (currentGUIevent.isMouse && currentGUIevent.type == EventType.mouseUp && isItemBeingDragged)
					{
						// If on top of a slot
                        if (slotRect.Contains(currentGUIevent.mousePosition))
						{
							inventory[i] = thisItemIsBeingDragged;
							isItemBeingDragged = false;
							thisItemIsBeingDragged = null;
						}
					}
				}

				i++;
			}
		}

        if (itemDatabase.items[0].value == 0)
        {
            itemDatabase.items[0].itemIcon = itemDatabase.items[0].itemIconEmpty;
        }
        else
        {
            itemDatabase.items[0].itemIcon = itemDatabase.items[0].itemIconReplace;
        }
        if (itemDatabase.items[1].value == 0)
        {
            itemDatabase.items[1].itemIcon = itemDatabase.items[1].itemIconEmpty;
        }
        else
        {
            itemDatabase.items[1].itemIcon = itemDatabase.items[1].itemIconReplace;
        }

		// Discard item in trashcan
		Rect trashcan = new Rect(inventorySize.x + inventorySize.width - 170, inventorySize.y + inventorySize.height - 4, 140, 30);
		GUI.Box (trashcan,"Discard Item", GUIskin.GetStyle("Inventory Empty Slot"));

        // If item[0] is being dragged and dropped in trashcan
        if (isItemBeingDragged && currentGUIevent.type == EventType.mouseUp && trashcan.Contains(currentGUIevent.mousePosition) && thisItemIsBeingDragged == itemDatabase.items[0]) 
		{
          
            mineralsStatus.mineralsInInventory = 0;
            itemDatabase.items[0].value = 0;

            itemDatabase.items[0].itemIcon = itemDatabase.items[0].itemIconEmpty;

            for (i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemName == null)
                {
                    inventory[i] = thisItemIsBeingDragged;
                    isItemBeingDragged = false;
                    break;
                }
            }
		}

        // If item[1] is being dragged and dropped in trashcan
        if (isItemBeingDragged && currentGUIevent.type == EventType.mouseUp && trashcan.Contains(currentGUIevent.mousePosition) && thisItemIsBeingDragged == itemDatabase.items[1])
        {

            itemDatabase.items[1].value = 0;

            itemDatabase.items[1].itemIcon = itemDatabase.items[1].itemIconEmpty;

            for (i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemName == null)
                {
                    inventory[i] = thisItemIsBeingDragged;
                    isItemBeingDragged = false;
                    break;
                }
            }            
        }

        Rect inventoryHeader = new Rect(inventorySize.x + inventorySize.width - 209, inventorySize.y + inventorySize.height - 233, 220, 40);
        GUI.Label(inventoryHeader, "Astronaut's Backpack ", GUIskin.GetStyle("Inventory Empty Slot"));
	}

	protected Vector2 GetIconCoordDifference(Vector2 m, Rect s)
	{
		Vector2 diff = new Vector2 (m.x - s.x, m.y - s.y);
		return diff;
	}

	private void DrawItemDetails()
	{
		float tooltipHeight = GUIskin.GetStyle("Inventory Tooltip").CalcHeight(new GUIContent(inventoryItemDetailsText), 200);
		GUI.Box(new Rect(Event.current.mousePosition.x + 10, Event.current.mousePosition.y, 300, tooltipHeight), inventoryItemDetailsText, GUIskin.GetStyle("Inventory Tooltip"));
	}

    private string CreateItemDetails(Item item)
    {
        inventoryItemDetailsText = "";
        inventoryItemDetailsText += "<color=#b8c7ff><b>" + item.itemName + "</b></color>\n\n" + item.itemDescription + " \n\nAmount Collected: " + item.value;

        if (mineralsStatus.maxMineralsHaveReached == true)
        {
            inventoryItemDetailsText += "\n\n<b><color=#ff0000>You can't carry more minerals.</color></b>";
        }

        return inventoryItemDetailsText;
    }

	// Add items from DB to player's inventory
    public void AddItem(int itemID)
    {
        for (int i = 0; i < database.items.Count; i++)
        {
            if (itemID == database.items[i].itemID)
            {
                for (int j = 0; j < inventory.Count; j++)
                {
                    if (inventory[j].itemName == null)
                    {
                        inventory[j] = database.items[i];
                        break;
                    }
                }
            }
        }
    }

    public void changeIntValue(int intValue)
    {
        intValue = 10;
    }
}

