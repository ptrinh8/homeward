using UnityEngine;
using System.Collections;

public class AirPressureIndicator : MonoBehaviour {

	public Sprite[] indicator;
	private float airPressureBarFillAmount;

	// Update is called once per frame
	void Update () {
		airPressureBarFillAmount = transform.root.gameObject.GetComponent<AirControl>().airPressureBar;
		if (airPressureBarFillAmount >= 0.8)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = indicator[5];
		}
		else if (airPressureBarFillAmount >= 0.6 && airPressureBarFillAmount < 0.8)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = indicator[4];
		}
		else if (airPressureBarFillAmount >= 0.4 && airPressureBarFillAmount < 6)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = indicator[3];
		}
		else if (airPressureBarFillAmount >= 0.2 && airPressureBarFillAmount < 0.4)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = indicator[2];
		}
		else if (airPressureBarFillAmount > 0 && airPressureBarFillAmount < 0.2)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = indicator[1];
		}
		else if (airPressureBarFillAmount == 0)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = indicator[0];
		}
	}
}
