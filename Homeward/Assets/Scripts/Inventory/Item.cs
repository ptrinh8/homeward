﻿// ==================================================================================
// <file="Item.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Creates an Item class for Inventory</summary>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

public enum ItemName
{
    MiningTool,
    RepairingTool,
    BuildingTool,
	Mineral,
    Mineral1,
	Mineral2,
	Mineral3,
    Water,
    Food1,
    Food2,
    Food3,
    Material,
    Oxygen,
	MiningToolEquipped,
	BuildingToolEquipped,
	RepairingToolEquipped,
	Wire,
	Screw,
	Metal,
	MiningProbe
}

[System.Serializable]
public class Item : MonoBehaviour
{
    // Attributes
    public ItemName itemName;
    public string itemDescription;
    public Sprite spriteNeutral;
    public Sprite spriteHighLighted;
    public int maxSize;

    public void Use()
    {
        switch (itemName)
        {
            case ItemName.Food1:
                break;
            case ItemName.Food2:
                break;
            case ItemName.Food3:
                break;
            case ItemName.Water:
                break;
			case ItemName.Mineral:
				break;
            case ItemName.Mineral1:
                break;
			case ItemName.Mineral2:
				break;
			case ItemName.Mineral3:
                break;
            case ItemName.MiningTool:
                break;
            case ItemName.BuildingTool:
                break;
            case ItemName.Material:
                break;
            case ItemName.RepairingTool:
                //PlayerController.holdingRepairTool = true;
                //Debug.Log("RepairingTool Equiped");
                break;
			case ItemName.BuildingToolEquipped:
				break;
			case ItemName.RepairingToolEquipped:
				break;
			case ItemName.MiningToolEquipped:
				break;
			case ItemName.Wire:
				break;
			case ItemName.Screw:
				break;
			case ItemName.Metal:
				break;
			case ItemName.MiningProbe:
				break;
        }
    }
}
