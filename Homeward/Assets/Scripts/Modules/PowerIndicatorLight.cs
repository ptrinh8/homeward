using UnityEngine;
using System.Collections;

public class PowerIndicatorLight : MonoBehaviour {

	public Sprite lightOn, lightOff;


	void Update()
	{
		if (transform.root.gameObject.GetComponent<LocalControl>().isOn)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = lightOn;
		}
		else
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = lightOff;
		}
	}

	void LightTriggered(bool flag)
	{
		if (flag)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = lightOn;
		}
		else
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = lightOff;
		}
	}
}
