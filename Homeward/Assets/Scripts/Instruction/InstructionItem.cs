// ==================================================================================
// <file="InstructionItem.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Creates an InstructionItem class for Instruction</summary>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

public enum InstructionItemName
{
    instructionItemName,
    columnNum,
    instructionItemDescription,
    instructionItemSprite
}

[System.Serializable]
public class InstructionItem 
{
    // Attributes
	public string instructionItemName;
    public int columnNum;
	public string instructionItemDescription;
	public Sprite instructionItemSprite;
	
	
}
