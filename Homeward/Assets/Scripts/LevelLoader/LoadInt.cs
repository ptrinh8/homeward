// ==================================================================================
// <file="LoadInt.cs" product="Homeward">
// <date>2015-01-11</date>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

public class LoadInt : ILevelToLoad 
{
	
	private int idToLoad;

	public LoadInt(int LevelID)
	{
		idToLoad = LevelID;
	}

	public void LoadLevel ()
	{
		Application.LoadLevel(idToLoad);
	}
}
