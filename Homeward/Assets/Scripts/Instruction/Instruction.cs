// ==================================================================================
// <file="Instruction.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Contains a base, abstract class for Instruction Management</summary>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#endregion

public class Instruction : MonoBehaviour {
    private InstructionDatabase instructionDatabase;

    [HideInInspector]
    public bool showInstruction;
    private KeyCode instKey = KeyCode.B;
    private int itemsSlotsSize = 100;
    private int itemsSlotsPadding = 12;
    private int backpackRowsCount = 3;
    private int backpackColsCount = 3;
    private Rect inventorySize = new Rect(100, 200, 0, 0); // Used to add bground to backpack, in consideration with values of rows & cols.

    public GUISkin GUIskin;
    public List<InstructionItem> instItems = new List<InstructionItem>(); // Holds instructions
    private InstructionDatabase database; // Stores items in DB
    private bool showInstructionItemDetails; // Each item information
    private string inventoryItemDetailsText; // Item information text
    [HideInInspector]
    public bool isItemBeingDragged;
    private Item thisItemIsBeingDragged;

    // Coordinate of the click to be able to draw the sprite at the correct off-set to the mouse
    private Vector2 mouseDragCoordinates;

	// Use this for initialization
	void Start () {
        instructionDatabase = FindObjectOfType(typeof(InstructionDatabase)) as InstructionDatabase;

		// Loop to add an inventory slot for each slot based on the result of XxY
        for (int i = 0; i < (backpackRowsCount * backpackColsCount); i++)
        {
            instItems.Add(new InstructionItem());
        }

        GUIskin = Resources.Load<GUISkin>("InvGUIskin");
		showInstruction = false;

        // Set DB instance = DB.component
		database = GameObject.Find("Instruction Database").GetComponent<InstructionDatabase>();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(instKey))
            showInstruction = !showInstruction;
    }

    void OnGUI()
    {
        GUI.skin = GUIskin;

        // Set item information as blank
        inventoryItemDetailsText = "";

        // is inventory visible?
        if (showInstruction)
        {
            DrawInstruction();

            // is mouse on item?, which makes showInventoryItemDetails = true
            if (showInstructionItemDetails)
                DrawInstructionDetails();

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

    void DrawInstruction()
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

                InstructionItem instItem = instItems[i];

                GUI.Box(slotRect, "", GUIskin.GetStyle("Box"));

                // Does slot have a name in it?
                if (instItem.itemName != null)
                {
                    // Yes, draw the icon
                    if (instItem.itemIcon != null)
                    {
                        GUI.DrawTexture(slotRect, instItem.itemIcon);
                    }

                    // Is mouse position within the slot?
                    if (slotRect.Contains(Event.current.mousePosition))
                    {
                        // Is left-click pressed? also, is item not being dragged?
                        if (currentGUIevent.isMouse && currentGUIevent.button == 0 && currentGUIevent.type == EventType.mouseDown && !isItemBeingDragged)
                        {
                            
                        }
                    }
                }
                i++;
            }
        }
    }

    void DrawInstructionDetails()
    {

    }
}
