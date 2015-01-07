using UnityEngine;
using System.Collections;

public class HabitatModule : MonoBehaviour {

	private DayNightController dayNightController;

	public int durability;
	private float durabilityTimer;
	private float durabilityLossTime;

	// Use this for initialization
	void Start () {
		dayNightController = GameObject.Find ("DayNightController").GetComponent<DayNightController>();

		durability = 100;
		durabilityLossTime = (dayNightController.dayCycleLength * 4) / 100;
	}
	
	// Update is called once per frame
	void Update () {

		if (durability > 0)
		{
			durabilityTimer += Time.deltaTime;
			if (durabilityTimer > durabilityLossTime)
			{
				durability -= 1;
				durabilityTimer = 0;
			}
		}

	}
}
