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
    // Attributes
	public string itemName;
	public int itemID;
	public string itemDescription;
	public Texture2D itemIcon;
	
	public InstructionItem(string itemName, int itemID, string itemDescription, Texture2D itemIcon)
	{
		this.itemName = itemName;
		this.itemID = itemID;
		this.itemDescription = itemDescription;
		this.itemIcon = itemIcon;
	}

	public InstructionItem()
	{
		this.itemID = 0;
	}

}
