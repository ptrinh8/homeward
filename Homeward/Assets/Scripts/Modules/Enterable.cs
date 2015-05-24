using UnityEngine;
using System.Collections;

// This script is attached to doorway(gameobject in any finished module) to detect whether the player is entered
public class Enterable : MonoBehaviour {

	public bool xEnter;	// whether the doorway open for x axis
	private GameObject mainPlayer;
	private PlayerController playerController;
	private float x, y;	// Record the direction when player enters the trigger
	[HideInInspector]
	public bool isDoorway;

	// Use this for initialization
	void Start () {
		mainPlayer = GameObject.Find("MainPlayer");
		playerController = mainPlayer.GetComponent<PlayerController>();
		x = y = 0;
		isDoorway = true;
		// Change the xEnter to meet the rotation
		int rotation = (int) gameObject.transform.root.rotation.eulerAngles.z;
		if (rotation == 90 || rotation == 270)
			xEnter = !xEnter;
	}
	
	void OnTriggerEnter2D (Collider2D other) {
		// Record the direction when player enters the trigger
		if (other.gameObject.tag == "Player") {
			x = mainPlayer.GetComponent<Rigidbody2D>().velocity.x;
			y = mainPlayer.GetComponent<Rigidbody2D>().velocity.y;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			// should not show indoor if player enters and exits the trigger in a "z" route
			if (xEnter) 
			{
				// should not show indoor if player enters and exits the trigger in same direction
				if (mainPlayer.GetComponent<Rigidbody2D>().velocity.x * x > 0)
				{
					if (gameObject.transform.root.gameObject.tag == "HabitatModule")
					{
						gameObject.SendMessageUpwards("HabitatModuleDoorWayTriggered", isDoorway, SendMessageOptions.RequireReceiver);
					}
					else 
					{
						gameObject.SendMessageUpwards("DoorWayTriggered", isDoorway, SendMessageOptions.RequireReceiver);
					}
				}
			} 
			else if (mainPlayer.GetComponent<Rigidbody2D>().velocity.y * y > 0)
			{
				if (gameObject.transform.root.gameObject.tag == "HabitatModule")
				{
					gameObject.SendMessageUpwards("HabitatModuleDoorWayTriggered", isDoorway, SendMessageOptions.RequireReceiver);
				}
				else 
				{
					gameObject.SendMessageUpwards("DoorWayTriggered", isDoorway, SendMessageOptions.RequireReceiver);
				}
			}
		}
	}	
}
