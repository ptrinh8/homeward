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
    private int itemsSlotWidth = 70;
    private int itemsSlotHeight = 70;
    private int itemsSlotsPadding = 12;
    private int instructionRowsCount = 1;
    private int instructionColsCount = 6;
    private Rect instructionBook = new Rect(50, 200, 0, 0); // Rect (x, y, width, height); Used to add bground to instruction, in consideration with values of rows & cols.

    public GUISkin GUIskin;
    private List<InstructionItem> instItems = new List<InstructionItem>(); // Holds instructions
    private InstructionDatabase database; // Stores items in DB
    private bool showInstructionItemDetails; // Each item information
    private string instructionItemDetailsText; // Item information text

    public Texture2D movePlayer;
    public Texture2D interaction;
    public Texture2D placingModule;
    public Texture2D rotatingModule;
    public Texture2D eating;
    public Texture2D inventory;

	// Use this for initialization
	void Start () {
        instructionDatabase = FindObjectOfType(typeof(InstructionDatabase)) as InstructionDatabase;

		//database = instItems.Add(new InstructionItem("Player Moving", 0, "'w,a,s,d'or arrow keys to move the player around", movePlayer));
        instItems.Add(new InstructionItem("Move Player", 0, "Press a, s, d, f, or arrow keys", movePlayer));
        instItems.Add(new InstructionItem("Interact with World", 1, "Press f to mine, refine, place modules", interaction));
        instItems.Add(new InstructionItem("Prepare Module", 2, "Num Key 1: Habitat Module \nNum Key 2: Connector Module \nNum Key 3: Refinary Module \nNum Key 4: Food Module", placingModule));
        instItems.Add(new InstructionItem("Rotate Module", 3, "Press r", rotatingModule));
        instItems.Add(new InstructionItem("Eat Foods", 4, "Press e", eating));
        instItems.Add(new InstructionItem("Look Inventory", 5, "Press i", inventory));

        GUIskin = Resources.Load<GUISkin>("InvGUIskin");
		showInstruction = false;

        // Set DB instance = DB.component
		database = GameObject.Find("Instruction Database").GetComponent<InstructionDatabase>();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(instKey))
        {
            if (Time.timeScale == 1) Time.timeScale = 0; // pause
            else if (Time.timeScale == 0) Time.timeScale = 1; // continue
                        
            showInstruction = !showInstruction;
        }
    }

    void OnGUI()
    {
        GUI.skin = GUIskin;

        // Set item information as blank
        instructionItemDetailsText = "";

        // is inventory visible?
        if (showInstruction)
        {
            DrawInstruction();

            // screen texture to make the screen dark when the gameplay is paused
            //screenTexture = new Texture2D(100, 100, TextureFormat.ARGB32, false);
            //screenTexture.SetPixel(0, 0, new Color(1.5f, 1.5f, 0, 1.5f)); // making it a little darker
            //screenTexture.Apply();

            // is mouse on item?, which makes showInstructionItemDetails = true
            if (showInstructionItemDetails)
                DrawInstructionDetails();
        }
    }

    void DrawInstruction()
    {
        int i = 0;

		// Calculate size of instruction window
        instructionBook.width = (itemsSlotWidth + itemsSlotsPadding) * instructionColsCount + itemsSlotsPadding;
        instructionBook.height = (itemsSlotHeight + itemsSlotsPadding) * instructionRowsCount + itemsSlotsPadding;

		// Draw background
		GUI.Box(instructionBook, "", GUIskin.GetStyle("Inventory Background"));

		// Current GUI input event stored in an Event variable
		Event currentGUIevent = Event.current;

		// Position and size of each item slot saved in a temp variable used for drawing the slots
        Rect slotRect = new Rect(instructionBook.x, instructionBook.y, itemsSlotWidth, itemsSlotHeight);

        for (int y = 0; y < instructionRowsCount; y++)
        {
            for (int x = 0; x < instructionColsCount; x++)
            {
                // Modify slotRect based on the position of the inventory window and the current item the loop is on
                slotRect.x = itemsSlotsPadding + instructionBook.x + x * (itemsSlotWidth + itemsSlotsPadding); // column position
                slotRect.y = itemsSlotsPadding + instructionBook.y + y * (itemsSlotHeight + itemsSlotsPadding); // row position

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
                        instructionItemDetailsText = CreateItemDetails(instItem);
                        showInstructionItemDetails = true;
                    }

                    if (instructionItemDetailsText == "")
                        showInstructionItemDetails = false;
                }
                i++;
            }
        }

        int headerWidth = 220;
        int headerHeight = 40;
        Rect instructionHeader = new Rect(instructionBook.x + instructionBook.width/2 - headerWidth/2, instructionBook.y - itemsSlotHeight/2, headerWidth, headerHeight);
        GUI.Label(instructionHeader, "Instruction Book ", GUIskin.GetStyle("Inventory Empty Slot"));
    }

    void DrawInstructionDetails()
    {
        //if (Event.current.mousePosition.x+10+300 < 'ScreenWidth') // currently working by Takahide : trying to make the box inside the screen
        float tooltipHeight = GUIskin.GetStyle("Inventory Tooltip").CalcHeight(new GUIContent(instructionItemDetailsText), 200);
        GUI.Box(new Rect(Event.current.mousePosition.x + 10, Event.current.mousePosition.y, 300, tooltipHeight), instructionItemDetailsText, GUIskin.GetStyle("Inventory Tooltip"));
    }

    private string CreateItemDetails(InstructionItem instItem)
    {
        instructionItemDetailsText = "";
        instructionItemDetailsText += "<color=#b8c7ff><b>" + instItem.itemName + "</b></color>\n\n" + instItem.itemDescription;
        return instructionItemDetailsText;
    }
}
