// ==================================================================================
// <file="ILevelLoader.cs" product="Homeward">
// <date>2015-01-11</date>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;
using System;

#endregion

public interface ILevelLoader 
{
	void RegisterPreAction(Action action, string message);
	void RegisterAction(Action action, string message);
	void RegisterPostAction(Action action, string message);
	void ReSize();
	void LoadLevel(string level);
	void LoadLevel(int levelID);
	void LoadLevel(string level, float minimumDurationSeconds);
	void LoadLevel(int levelID, float minimumDurationSeconds);
	void AssignFinishedLoadingCallback(Action finishedLoadingAction);
}
