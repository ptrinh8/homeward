using UnityEngine;
using System.Collections;

public class LightScript : MonoBehaviour {

	public Color lightColor;
	private Light light;
	private DayNightController dayNightController;

	// Use this for initialization
	void Start () {

		light = this.gameObject.GetComponent<Light>();
		dayNightController = GameObject.Find("DayNightController").GetComponent<DayNightController>();
		lightColor = Color.white;
	
	}
	
	// Update is called once per frame
	void Update () {

		light.color = lightColor;
		if (Input.GetKey(KeyCode.Space))
		{
			LightSwitch(2);
		}
	
	}

	public void LightSwitch(int onOff)
	{
		if (onOff == 1)
		{
			lightColor = Color.white;
			light.color = lightColor;
		}
		else if (onOff == 2)
		{
			lightColor= Color.black;
			light.color = lightColor;
		}
	}
}
