// ===========================================================================================
// <file="MineralsStatus.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Class to keep track of Minerals in correspondence with Minerals.cs class</summary>
// ===========================================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

public class MineralsStatus : MonoBehaviour {

    // Max number of minerals allowed in backpack
	public int maxMineralsLoadAllowed;
    // For Debugging - shows displays number of minerals in inventory
    public GUIText numberOfMineralsInInventory;	
    // Mumber of minerals extracted and placed in backpack
    [HideInInspector]
	public int mineralsInInventory; 	
    [HideInInspector]
    public bool maxMineralsHaveReached = false;

	void Start () 
    {
		mineralsInInventory = 0;
	}
	
	void Update () 
    {
        maxMineralsHaveReacherCondition();
        numberOfMineralsInInventory.text = "<b>// Debugging Console - Mining & Refining //</b> \n\n Minerals In Inventory: " + mineralsInInventory.ToString();
	}

    void maxMineralsHaveReacherCondition()
    {
        if (mineralsInInventory == maxMineralsLoadAllowed)
        {
            maxMineralsHaveReached = true;
        }
        else
        {
            maxMineralsHaveReached = false;
        }
    }
}
