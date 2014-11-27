// =======================================================================
// <file="CameraBehavior.cs" product="Homeward">
// <date>2014-11-12</date>
// <summary>Contains a base, abstract class for CameraBehavior</summary>
// =======================================================================

// Header Files
using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

	public Transform FollowThisGameObject;

	void Start () 
	{
	
	}

	void Update () 
	{
		// Follows the gameObject attached.
	 	transform.position = new Vector3 (FollowThisGameObject.position.x, FollowThisGameObject.position.y, - 10);
	}
}
