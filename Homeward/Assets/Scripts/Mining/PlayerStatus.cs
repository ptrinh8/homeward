// ===================================================================================================
// <file="PlayerStatus.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Class to keep track of Player's status in correspondence with Minerals extracted</summary>
// ===================================================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

public class PlayerStatus : MonoBehaviour {

	public int maxLoad;		// max value of minerals of player's backpack
	public int mineralsOwned;	// amount of minerals player have
	public GUIText showMineralOwned;	// show that amount at left right corner


	// Use this for initialization
	void Start () {
		mineralsOwned = 0;
		maxLoad = 20;
	}
	
	// Update is called once per frame
	void Update () {	
		SetMineralsOwnedText();
	}

	void SetMineralsOwnedText() {
		// add "full" at the end of the amount if the backpack is overload
		if (mineralsOwned == maxLoad)
			showMineralOwned.text = "Minerals: " + mineralsOwned.ToString() + " (full)";
		else 
			showMineralOwned.text = "Minerals: " + mineralsOwned.ToString();
	}
}
