// ==================================================================================
// <file="ItemDatabase.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Create a Database for items in the inventory</summary>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#endregion

[System.Serializable]
public class ItemDatabase : MonoBehaviour 
{

	public List<Item> items = new List<Item>();

	public Item GetItem(int id)
	{
		foreach(Item item in items)
		{
			if (item.itemID == id)
			{
				return item;
			}
		}
		return null;
	}

}
