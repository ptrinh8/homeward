using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
	private FMOD.Studio.EventInstance music1;
	private FMOD.Studio.EventInstance music2;
	private FMOD.Studio.EventInstance music3;
	private FMOD.Studio.EventInstance music4;
	private FMOD.Studio.EventInstance leftFootMetal;
	private FMOD.Studio.EventInstance rightFootMetal;
	private FMOD.Studio.EventInstance leftFootSand;
	private FMOD.Studio.EventInstance rightFootSand;
	private FMOD.Studio.EventInstance mining;
	private FMOD.Studio.EventInstance drone;
	private FMOD.Studio.EventInstance refineryMachine;


	public FMOD.Studio.ParameterInstance stemTrigger1;
	public FMOD.Studio.ParameterInstance stemTrigger2;
	public FMOD.Studio.ParameterInstance stemTrigger3;
	public FMOD.Studio.ParameterInstance stemTrigger4;
	private FMOD.Studio.ParameterInstance leftFootMetalSelector;
	private FMOD.Studio.ParameterInstance rightFootMetalSelector;
	private FMOD.Studio.ParameterInstance leftFootSandSelector;
	private FMOD.Studio.ParameterInstance rightFootSandSelector;
	private FMOD.Studio.ParameterInstance leftFootMetalInsideOutside;
	private FMOD.Studio.ParameterInstance rightFootMetalInsideOutside;
	private FMOD.Studio.ParameterInstance leftFootSandInsideOutside;
	private FMOD.Studio.ParameterInstance rightFootSandInsideOutside;
	private FMOD.Studio.ParameterInstance leftFootMetalAirlockPressure;
	private FMOD.Studio.ParameterInstance rightFootMetalAirlockPressure;
	private FMOD.Studio.ParameterInstance refineryAirlockPressure;
	private FMOD.Studio.ParameterInstance miningSelector;
	private FMOD.Studio.ParameterInstance miningInsideOutside;
	private FMOD.Studio.ParameterInstance droneVolume;
	private FMOD.Studio.ParameterInstance startingStopping;

	public FMOD.Studio.PLAYBACK_STATE dronePlaybackState;
	private FMOD.Studio.PLAYBACK_STATE refineryPlaybackState;
	public float stemTriggerValue1;
	public float stemTriggerValue2;
	public float stemTriggerValue3;
	public float stemTriggerValue4;
	public float droneVolumeValue;
	private float refineryStartingStopping;
	private PlayerController player;

	private bool refineryStarted;

	public bool refineryMachineWorking;

	public int songSelectNumber;
	public float songTriggerValue;
	public bool songPlaying;
	public FMOD.Studio.PLAYBACK_STATE song1PlaybackState;
	public FMOD.Studio.PLAYBACK_STATE song2PlaybackState;
	public FMOD.Studio.PLAYBACK_STATE song3PlaybackState;
	public FMOD.Studio.PLAYBACK_STATE song4PlaybackState;
	private FMOD.Studio.EventInstance currentSong;

	private AirControl airControl;
	public float controllerSoundPressure;
	public float controllerPressure;

	//airlock stuff
	private FMOD.Studio.EventInstance airlockSound;
	public FMOD.Studio.ParameterInstance airlockPressure;
	public FMOD.Studio.ParameterInstance airlockTransition;
	public FMOD.Studio.PLAYBACK_STATE airlockPlaybackState;
	public bool pressureRisingFalling;
	public float audioPressureTimer;
	public float airlockDuration;
	public float audioTimer;
	public bool airlockActivated;
	public bool airlockSoundStarted;
	public bool playerLeft;
	// Use this for initialization
	void Start () {
		music1 = FMOD_StudioSystem.instance.GetEvent("event:/Music1");
		music2 = FMOD_StudioSystem.instance.GetEvent("event:/Music2");
		music3 = FMOD_StudioSystem.instance.GetEvent("event:/Music3");
		music4 = FMOD_StudioSystem.instance.GetEvent("event:/Music4");
		drone = FMOD_StudioSystem.instance.GetEvent("event:/Drone");
		refineryMachine = FMOD_StudioSystem.instance.GetEvent("event:/RefineryMachine");
		//music.start();
		music1.getParameter("MusicTrigger", out stemTrigger1);
		music2.getParameter("MusicTrigger", out stemTrigger2);
		music3.getParameter("MusicTrigger", out stemTrigger3);
		music4.getParameter("MusicTrigger", out stemTrigger4);
		drone.getParameter("Volume", out droneVolume);
		drone.getPlaybackState(out dronePlaybackState);
		refineryMachine.getPlaybackState(out refineryPlaybackState);
		refineryMachine.getParameter("StartingStopping", out startingStopping);
		stemTriggerValue1 = 0f;
		stemTriggerValue2 = 0f;
		stemTriggerValue3 = 0f;
		stemTriggerValue4 = 0f;
		songTriggerValue = 0f;
		droneVolumeValue = 0f;
		refineryStartingStopping = 0f;
		refineryStarted = false;
		player = GameObject.Find ("MainPlayer").GetComponent<PlayerController>();
		songPlaying = false;
		controllerSoundPressure = 0f;

		airlockSound = FMOD_StudioSystem.instance.GetEvent("event:/Airlock");
		airlockSound.getParameter("AirlockPressure", out airlockPressure);
		airlockSound.getParameter("AirlockTransition", out airlockTransition);
		playerLeft = false;
		airlockSoundStarted = false;
		audioPressureTimer = 0f;
	}
	// Update is called once per frame
	void Update () {
		//stemTrigger.setValue(stemTriggerValue);
		drone.getPlaybackState(out dronePlaybackState);
		refineryMachine.getPlaybackState(out refineryPlaybackState);
		music1.getPlaybackState(out song1PlaybackState);
		music2.getPlaybackState(out song2PlaybackState);
		music3.getPlaybackState(out song3PlaybackState);
		music4.getPlaybackState(out song4PlaybackState);
		airlockSound.getPlaybackState(out airlockPlaybackState);


		//controllerSoundPressure = GameObject.Find ("Airlock Module(Clone)").GetComponent<AirControl>().soundPressure;

		if (songPlaying == true)
		{
			if (songTriggerValue <= 1.51f)
			{
				songTriggerValue += .001f;
				switch(songSelectNumber)
				{
				case 1:
					stemTrigger1.setValue(songTriggerValue);
					//Debug.Log ("setting value 1");
					break;
				case 2:
					stemTrigger2.setValue(songTriggerValue);
					//Debug.Log ("setting value 2");
					break;
				case 3:
					stemTrigger3.setValue(songTriggerValue);
					//Debug.Log ("setting value 3");
					break;
				case 4:
					stemTrigger4.setValue(songTriggerValue);
					//Debug.Log ("setting value 4");
					break;
				}
			}
			else if (songTriggerValue >= 1.51f)
			{
				songTriggerValue += .001f;
				switch(songSelectNumber)
				{
				case 1:
					stemTrigger1.setValue(songTriggerValue);
					//Debug.Log ("setting value 1");
					break;
				case 2:
					stemTrigger2.setValue(songTriggerValue);
					//Debug.Log ("setting value 2");
					break;
				case 3:
					stemTrigger3.setValue(songTriggerValue);
					//Debug.Log ("setting value 3");
					break;
				case 4:
					stemTrigger4.setValue(songTriggerValue);
					//Debug.Log ("setting value 4");
					break;
				}
			}
		}
		else if (songPlaying == false)
		{
			if (songTriggerValue >= 0f)
			{
				songTriggerValue -= .001f;
				switch(songSelectNumber)
				{
				case 1:
					stemTrigger1.setValue(songTriggerValue);
					break;
				case 2:
					stemTrigger2.setValue(songTriggerValue);
					break;
				case 3:
					stemTrigger3.setValue(songTriggerValue);
					break;
				case 4:
					stemTrigger4.setValue(songTriggerValue);
					break;
				}
			}
			else if (songTriggerValue < 0f)
			{
				switch(songSelectNumber)
				{
				case 1:
					music1.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
					break;
				case 2:
					music2.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
					break;
				case 3:
					music3.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
					break;
				case 4:
					music4.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
					break;
				}
			}
		}

		if (airlockActivated == true && airlockSoundStarted == false)
		{
			if (airlockPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
			{
				airlockSound.start();
				airlockSoundStarted = true;
			}
		}

		if (audioPressureTimer < 0)
		{
			pressureRisingFalling = false;
			audioPressureTimer = 0;
		}

		if (pressureRisingFalling == true && playerLeft == false)
		{
			audioTimer += Time.deltaTime;
			audioPressureTimer = audioTimer / airlockDuration;
			controllerSoundPressure = Mathf.Lerp(0f, 10f, audioPressureTimer);
			airlockPressure.setValue(controllerSoundPressure);
			airlockTransition.setValue(2.5f);
		}

		else if (pressureRisingFalling == true && playerLeft == true)
		{
			Debug.Log ("pressure rising");
			audioTimer -= Time.deltaTime;
			audioPressureTimer = audioTimer / airlockDuration;
			controllerSoundPressure = Mathf.Lerp(0f, 10f, audioPressureTimer);
			airlockPressure.setValue(controllerSoundPressure);
			airlockTransition.setValue(2.5f);

		}
		else if (pressureRisingFalling == false)
		{
			airlockTransition.setValue(3.5f);
			if (controllerSoundPressure > 5)
			{
				playerLeft = true;
			}
			else if (controllerSoundPressure < 5)
			{
				playerLeft = false;
			}
		}

	}
	void OnDisable()
	{
		music1.release();
		music2.release();
		music3.release();
		music4.release();
	}

	public void MusicControl(int controlNumber, int songNumber)
	{
		songSelectNumber = songNumber;
		// 1 = play, 2 = stop
		if (controlNumber == 1)
		{
			Debug.Log("song playing");
			if (songPlaying == false)
			{
				switch(songSelectNumber)
				{
				case 1:
					music1.start();
					break;
				case 2:
					music2.start();
					break;
				case 3:
					music3.start();
					break;
				case 4:
					music4.start();
					break;
				}
				songPlaying = true;
			}
		}
		else if (controlNumber == 2)
		{
			if (songPlaying == true)
			{
				songPlaying = false;
			}
		}
	}

	public void DroneControl(int controlNumber)
	{
		if (controlNumber == 0)
		{
			if (dronePlaybackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
			{
				drone.start();
			}

			if (dronePlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
			{
				if (droneVolumeValue < 10f)
				{
					droneVolumeValue += .5f;
					droneVolume.setValue(droneVolumeValue);
				}
				else
				{
					droneVolume.setValue(10f);
				}
			}
		}
		else if (controlNumber == 1)
		{
			if (droneVolumeValue > 0f)
			{
				droneVolumeValue -= .5f;
				droneVolume.setValue(droneVolumeValue);
			}
			
			if (droneVolumeValue <= 0f)
			{
				if (dronePlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
				{
					drone.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				}
			}
		}
	}


	public void PlayFootstep(int footstep)
	{
		// Debug.Log ("playing footstep");
		if (footstep == 0)
		{
			if (CentralControl.isInside == true)
			{
				leftFootMetal = FMOD_StudioSystem.instance.GetEvent("event:/LeftFootMetal");
				leftFootMetal.getParameter("Selector", out leftFootMetalSelector);
				leftFootMetal.getParameter("InsideOutside", out leftFootMetalInsideOutside);
				leftFootMetal.getParameter("AirlockPressure", out leftFootMetalAirlockPressure);
				leftFootMetalSelector.setValue(Random.Range (1f, 11f));
				leftFootMetalInsideOutside.setValue(.5f);
				leftFootMetalAirlockPressure.setValue(controllerSoundPressure);
				leftFootMetal.start();
				leftFootMetal.release ();
			}
			else
			{
				leftFootSand = FMOD_StudioSystem.instance.GetEvent("event:/LeftFootSand");
				leftFootSand.getParameter("Selector", out leftFootSandSelector);
				leftFootSand.getParameter("InsideOutside", out leftFootSandInsideOutside);
				leftFootSandSelector.setValue(Random.Range (1f, 9f));
				leftFootSandInsideOutside.setValue(1.75f);
				leftFootSand.start();
				leftFootSand.release();
			}
		}
		else if (footstep == 1)
		{
			if (CentralControl.isInside == true)
			{
				rightFootMetal = FMOD_StudioSystem.instance.GetEvent("event:/RightFootMetal");
				rightFootMetal.getParameter("Selector", out rightFootMetalSelector);
				rightFootMetal.getParameter("InsideOutside", out rightFootMetalInsideOutside);
				rightFootMetal.getParameter("AirlockPressure", out rightFootMetalAirlockPressure);
				rightFootMetalSelector.setValue(Random.Range(1f, 11f));
				rightFootMetalInsideOutside.setValue(.5f);
				rightFootMetalAirlockPressure.setValue(controllerSoundPressure);
				rightFootMetal.start();
				rightFootMetal.release ();
			}
			else
			{
				rightFootSand = FMOD_StudioSystem.instance.GetEvent("event:/RightFootSand");
				rightFootSand.getParameter("Selector", out rightFootSandSelector);
				rightFootSand.getParameter("InsideOutside", out rightFootSandInsideOutside);
				rightFootSandSelector.setValue(Random.Range (1f, 9f));
				rightFootSandInsideOutside.setValue(1.75f);
				rightFootSand.start ();
				rightFootSand.release ();
			}
		}
	}

	public void PlayMiningSound()
	{
		if (CentralControl.isInside == true)
		{
			mining = FMOD_StudioSystem.instance.GetEvent("event:/Mining");
			mining.getParameter("MiningSelector", out miningSelector);
			mining.getParameter("InsideOutside", out miningInsideOutside);
			miningSelector.setValue(Random.Range (1f, 13f));
			miningInsideOutside.setValue(.5f);
			mining.start();
			mining.release ();
		}
		else
		{
			mining = FMOD_StudioSystem.instance.GetEvent("event:/Mining");
			mining.getParameter("MiningSelector", out miningSelector);
			mining.getParameter("InsideOutside", out miningInsideOutside);
			miningSelector.setValue(Random.Range (1f, 13f));
			miningInsideOutside.setValue(1.75f);
			mining.start();
			mining.release ();
		}
	}
	
	public void RefineryMachineControl(int controlNumber)
	{
		if (controlNumber == 0)
		{
			if (refineryPlaybackState == FMOD.Studio.PLAYBACK_STATE.STOPPED && refineryStarted == false)
			{
				refineryMachine.start();
				refineryStarted = true;
			}
		}
		else if (controlNumber == 1)
		{
			if (refineryPlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING && refineryStarted == true)
			{
				refineryStartingStopping = 2.5f;
				startingStopping.setValue(refineryStartingStopping);
				refineryStarted = false;
			}
		}

		if (refineryPlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING && refineryStarted == true)
		{
			refineryStartingStopping = 1.5f;
			startingStopping.setValue(refineryStartingStopping);
		}
	}
}