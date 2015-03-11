using UnityEngine;
using System.Collections;

public class AirlockModuleAudio : MonoBehaviour {
	
	private FMOD.Studio.EventInstance airlockModuleAudio;
	public FMOD.Studio.ParameterInstance airlockPressure;
	public FMOD.Studio.ParameterInstance distance;
	
	public float pressure;
	public float playerDistance;
	
	private PlayerController player;
	
	private bool audioStarted;
	
	private AudioController audioController;
	
	// Use this for initialization
	void Start () {
		
		airlockModuleAudio = FMOD_StudioSystem.instance.GetEvent("event:/AirlockModuleAmbience");
		airlockModuleAudio.getParameter("AirlockPressure", out airlockPressure);
		airlockModuleAudio.getParameter("Distance", out distance);
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
				airlockModuleAudio.start();
				audioStarted = true;
			}
		}
		else
		{
			if (audioStarted == true)
			{
				airlockModuleAudio.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				audioStarted = false;
			}
		}
		
		playerDistance = Vector2.Distance(this.transform.position, player.transform.position);
		pressure = audioController.controllerSoundPressure;
		
		distance.setValue(playerDistance);
		airlockPressure.setValue(audioController.controllerSoundPressure);
	}
}
