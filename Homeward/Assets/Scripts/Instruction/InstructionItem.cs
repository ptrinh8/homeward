// ==================================================================================
// <file="InstructionItem.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Creates an InstructionItem class for Instruction</summary>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

[System.Serializable]
public class InstructionItem 
{
	public enum ItemType 
    {
		Consumable,
		Mineral
	}

    // Attributes
	public string itemName;
	public int itemID;
	public string itemDescription;
	public ItemType itemType;
	public Texture2D itemIcon;
    public Texture2D itemIconEmpty;
    public Texture2D itemIconReplace;
	public GameObject itemObject;

    [HideInInspector]
    public int value;
	
	public InstructionItem(string itemName, int itemID, string itemDescription, ItemType itemType, Texture2D itemIcon, Texture2D itemIconEmpty, Texture2D itemIconReplace, GameObject itemObject, int value)
	{
		this.itemName = itemName;
		this.itemID = itemID;
		this.itemDescription = itemDescription;
		this.itemType = itemType;
		this.itemIcon = itemIcon;
        this.itemIconEmpty = itemIconEmpty;
        this.itemIconReplace = itemIconReplace;
		this.itemObject = itemObject;
        this.value = value;
	}

	public InstructionItem()
	{
		this.itemID = 0;
	}

	public InstructionItem(ItemType itemType)
	{
		this.itemID = 0;
		this.itemType = itemType;
	}
}
