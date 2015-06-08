using UnityEngine;
using System.Collections;

public class BuildingModuleAudio : MonoBehaviour {
	
	private FMOD.Studio.EventInstance buildingModuleAudio;
	public FMOD.Studio.ParameterInstance airlockPressure;
	public FMOD.Studio.ParameterInstance distance;
	
	public float pressure;
	public float playerDistance;
	
	private PlayerController player;
	
	private bool audioStarted;
	
	private AudioController audioController;
	
	// Use this for initialization
	void Start () {
		
		buildingModuleAudio = FMOD_StudioSystem.instance.GetEvent("event:/BuildingModuleAmbience");
		buildingModuleAudio.getParameter("AirlockPressure", out airlockPressure);
		buildingModuleAudio.getParameter("Distance", out distance);
		audioStarted = false;
		
		player = GameObject.Find("MainPlayer").GetComponent<PlayerController>();
		playerDistance = Vector2.Distance(this.transform.position, player.transform.position);
		
		audioController = GameObject.Find ("AudioObject").GetComponent<AudioController>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (CentralControl.isInside == true)
		{
			if (audioStarted == false)
			{
				buildingModuleAudio.start();
				audioStarted = true;
			}
		}
		else
		{
			if (audioStarted == true)
			{
				buildingModuleAudio.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				audioStarted = false;
			}
		}
		
		playerDistance = Vector2.Distance(this.transform.position, player.transform.position);
		pressure = audioController.controllerSoundPressure;
		
		distance.setValue(playerDistance);
		airlockPressure.setValue(audioController.controllerSoundPressure);
	}
}
