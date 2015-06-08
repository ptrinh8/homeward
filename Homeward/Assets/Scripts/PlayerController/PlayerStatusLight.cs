using UnityEngine;
using System.Collections;

public class PlayerStatusLight : MonoBehaviour {

	private Light statusLight;
	private PlayerController playerController;
	private Vector3 lightPosition;
	private float lightStep;
	private bool lightStrobeUp;
	private Color lightColor;

	// Use this for initialization
	void Start () {

		statusLight = GameObject.Find ("StatusLight").GetComponent<Light>();
		playerController = GameObject.Find ("MainPlayer").GetComponent<PlayerController>();
		lightPosition = new Vector3(-.478f, .937f, -.025f);
		statusLight.transform.localPosition = lightPosition;
		lightStrobeUp = true;
		statusLight.range = .4f;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (playerController.currentHealth >= 50)
		{
			lightColor = new Color(0f, 1f, 0f);
			lightStep = 3f;
			statusLight.color = lightColor;
		}
		else if (playerController.currentHealth >= 25 && playerController.currentHealth < 50)
		{
			lightColor = new Color(1f, 1f, 0f);
			lightStep = 6f;
			statusLight.color = lightColor;
		}
		else if (playerController.currentHealth < 25)
		{
			lightColor = new Color(1f, 0f, 0f);
			lightStep = 8f;
			statusLight.color = lightColor;
		}

		if (playerController.wasMovingNorth == true)
		{
			lightPosition = new Vector3(0.01f, .84f, -.025f);
			statusLight.transform.localPosition = lightPosition;
		}
		else if (playerController.wasMovingNorthEast == true)
		{
			lightPosition = new Vector3(-.593f, 1.033f, -.025f);
			statusLight.transform.localPosition = lightPosition;
		}
		else if (playerController.wasMovingEast == true)
		{
			lightPosition = new Vector3(-.478f, .937f, -.025f);
			statusLight.transform.localPosition = lightPosition;
			//statusLight.range = .3f;
		}
		else if (playerController.wasMovingSouthEast == true)
		{
			lightPosition = new Vector3(-.316f, 1.195f, -.025f);
			statusLight.transform.localPosition = lightPosition;
		}
		else if (playerController.wasMovingSouth == true)
		{
			lightPosition = new Vector3(0.047f, 1.195f, -.025f);
			statusLight.transform.localPosition = lightPosition;
		}
		else if (playerController.wasMovingSouthWest == true)
		{
			lightPosition = new Vector3(0.382f, 1.224f, -.025f);
			statusLight.transform.localPosition = lightPosition;
		}
		else if (playerController.wasMovingWest == true)
		{
			lightPosition = new Vector3(0.42f, 1.023f, -.025f);
			statusLight.transform.localPosition = lightPosition;
			//statusLight.range = .3f;
		}
		else if (playerController.wasMovingNorthWest == true)
		{
			lightPosition = new Vector3(0.143f, 1.023f, -.025f);
			statusLight.transform.localPosition = lightPosition;
		}

		if (lightStrobeUp == true)
		{
			if (statusLight.intensity < 8)
			{
				statusLight.intensity += lightStep * Time.deltaTime;
			}
			else if (statusLight.intensity >= 8)
			{
				lightStrobeUp = false;
			}
		}
		else if (lightStrobeUp == false)
		{
			if (statusLight.intensity > 0)
			{
				statusLight.intensity -= lightStep * Time.deltaTime;
			}
			else if (statusLight.intensity <= 0)
			{
				lightStrobeUp = true;
			}
		}
	
	}
}
