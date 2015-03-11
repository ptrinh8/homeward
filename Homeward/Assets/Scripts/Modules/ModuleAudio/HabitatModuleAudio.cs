using UnityEngine;
using System.Collections;

public class HabitatModuleAudio : MonoBehaviour {

	private FMOD.Studio.EventInstance habitatModuleAudio;
	public FMOD.Studio.ParameterInstance airlockPressure;
	public FMOD.Studio.ParameterInstance distance;

	public float pressure;
	public float playerDistance;

	private PlayerController player;

	// Use this for initialization
	void Start () {

		habitatModuleAudio = FMOD_StudioSystem.instance.GetEvent("event:/HabitatModuleAmbience");
		habitatModuleAudio.getParameter("AirlockPressure", out airlockPressure);
		habitatModuleAudio.getParameter("Distance", out distance);

		player = GameObject.Find("MainPlayer").GetComponent<PlayerController>();
		playerDistance = Vector2.Distance(this.transform.position, player.transform.position);
	
	}
	
	// Update is called once per frame
	void Update () {
		if (CentralControl.isInside == true)
		{
			habitatModuleAudio.start();


		}
	}
}
