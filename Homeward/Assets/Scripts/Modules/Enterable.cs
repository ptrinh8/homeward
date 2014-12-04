using UnityEngine;
using System.Collections;

public class Enterable : MonoBehaviour {

	public bool xEnter;
	private GameObject mainPlayer;
	private PlayerController playerController;
	private float x, y;

	// Use this for initialization
	void Start () {
		mainPlayer = GameObject.Find("MainPlayer");
		playerController = mainPlayer.GetComponent<PlayerController>();
		x = y = 0;
		int rotation = (int) gameObject.transform.root.rotation.eulerAngles.z;
		if (rotation == 90 || rotation == 270)
			xEnter = !xEnter;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			x = playerController.x;
			y = playerController.y;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (xEnter) {
				if (playerController.x == x)
					SpriteController.isEnter = !SpriteController.isEnter;
			} else if (playerController.y == y)
				SpriteController.isEnter = !SpriteController.isEnter;
		}
	}	
}
