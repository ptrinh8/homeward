using UnityEngine;
using System.Collections;

public class Footprint : MonoBehaviour {

	private float timer;
	private float despawnTime;
	private SpriteRenderer renderer;

	private Color tileColor;

	// Use this for initialization
	void Start () {

		despawnTime = GameObject.Find ("DayNightController").GetComponent<DayNightController>().dayCycleLength;
		renderer = GetComponent<SpriteRenderer>();
		tileColor = GameObject.Find ("MainPlayer").GetComponent<PlayerController>().tileColor;
		if ((tileColor.b > tileColor.r) && (tileColor.b > tileColor.g))
		{
			tileColor.r += .3f;
			tileColor.g += .3f;
		}
		else if ((tileColor.r > tileColor.b) && (tileColor.r > tileColor.g))
		{
			tileColor.g += .3f;
			tileColor.b += .3f;
		}
		else if ((tileColor.g > tileColor.b) && (tileColor.g > tileColor.r))
		{
			tileColor.r += .3f;
			tileColor.b += .3f;
		}
		renderer.color = tileColor;
		GameObject.Destroy(this.gameObject, despawnTime);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
