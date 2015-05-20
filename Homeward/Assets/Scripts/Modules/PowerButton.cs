using UnityEngine;
using System.Collections;

public class PowerButton : MonoBehaviour {

	public Sprite powerButtonOn, powerButtonOff;
	private KeyCode turnKey = KeyCode.F;

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (Input.GetKeyDown(turnKey) && !transform.root.gameObject.GetComponent<LocalControl>().IsBroken) 
			{
				transform.root.gameObject.SendMessage("SwitchTriggered", !transform.root.gameObject.GetComponent<LocalControl>().isOn, SendMessageOptions.RequireReceiver);
				if (transform.root.gameObject.GetComponent<LocalControl>().isOn)
				{
					gameObject.GetComponent<SpriteRenderer>().sprite = powerButtonOn;
				}
				else
				{
					gameObject.GetComponent<SpriteRenderer>().sprite = powerButtonOff;
				}
			}
		}
	}
}
