// ==================================================================================
// <file="LoadString.cs" product="Homeward">
// <date>2015-01-11</date>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

public class LoadString : ILevelToLoad 
{	
	private string nameToLoad;

	public LoadString(string LevelName)
	{
		nameToLoad = LevelName;
	}

	public void LoadLevel ()
	{
		Application.LoadLevel(nameToLoad);
	}
}
