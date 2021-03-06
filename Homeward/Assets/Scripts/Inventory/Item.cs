﻿// ==================================================================================
// <file="Item.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Creates an Item class for Inventory</summary>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

[System.Serializable]
public class Item 
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
	public GameObject itemObject;
	
	public Item(string itemName, int itemID, string itemDescription, ItemType itemType, Texture2D itemIcon, GameObject itemObject)
	{
		this.itemName = itemName;
		this.itemID = itemID;
		this.itemDescription = itemDescription;
		this.itemType = itemType;
		this.itemIcon = itemIcon;
		this.itemObject = itemObject;
	}

	public Item()
	{
		this.itemID = 0;
	}

	public Item(ItemType itemType)
	{
		this.itemID = 0;
		this.itemType = itemType;
	}
}
