using UnityEngine;
using System.Collections;

public class Bed : MonoBehaviour {

	private PlayerController player;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "MainPlayer")
		{
			Debug.Log ("playerhere");
			player = collision.gameObject.GetComponent<PlayerController>();
			player.canSleep = true;
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.name == "MainPlayer")
		{
			Debug.Log ("playerleft");
			player = collision.gameObject.GetComponent<PlayerController>();
			player.canSleep = false;
		}
	}
}
