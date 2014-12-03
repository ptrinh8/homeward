// ==================================================================================
// <file="InstructionDatabase.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Create a Database for items in the inventory</summary>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#endregion

[System.Serializable]
public class InstructionDatabase : MonoBehaviour
{

    public List<InstructionItem> instItems = new List<InstructionItem>();

    public InstructionItem GetItem(int id)
    {
        foreach (InstructionItem instItem in instItems)
        {
            if (instItem.itemID == id)
            {
                return instItem;
            }
        }
        return null;
    }

}
