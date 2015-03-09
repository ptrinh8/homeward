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


	private FMOD.Studio.ParameterInstance stemTrigger1;
	private FMOD.Studio.ParameterInstance stemTrigger2;
	private FMOD.Studio.ParameterInstance stemTrigger3;
	private FMOD.Studio.ParameterInstance stemTrigger4;
	private FMOD.Studio.ParameterInstance leftFootMetalSelector;
	private FMOD.Studio.ParameterInstance rightFootMetalSelector;
	private FMOD.Studio.ParameterInstance leftFootSandSelector;
	private FMOD.Studio.ParameterInstance rightFootSandSelector;
	private FMOD.Studio.ParameterInstance leftFootMetalInsideOutside;
	private FMOD.Studio.ParameterInstance rightFootMetalInsideOutside;
	private FMOD.Studio.ParameterInstance leftFootSandInsideOutside;
	private FMOD.Studio.ParameterInstance rightFootSandInsideOutside;
	private FMOD.Studio.ParameterInstance miningSelector;
	private FMOD.Studio.ParameterInstance miningInsideOutside;
	private FMOD.Studio.ParameterInstance droneVolume;
	private FMOD.Studio.ParameterInstance startingStopping;

	private FMOD.Studio.PLAYBACK_STATE dronePlaybackState;
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

	private int songSelectNumber;
	private float songTriggerValue;
	private FMOD.Studio.EventInstance currentSong;
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
	}
	// Update is called once per frame
	void Update () {
		//stemTrigger.setValue(stemTriggerValue);

		drone.getPlaybackState(out dronePlaybackState);
		refineryMachine.getPlaybackState(out refineryPlaybackState);

	}
	void OnDisable()
	{
		music1.release();
	}

	public void MusicControl(int controlNumber, int songNumber)
	{
		switch(songNumber)
		{
		case 1:
			currentSong = music1;
			break;
		case 2:
			currentSong = music2;
			break;
		case 3:
			currentSong = music3;
			break;
		case 4:
			currentSong = music4;
			break;
		}
		// 1 = play, 2 = stop
		if (controlNumber == 1)
		{
			currentSong.start();
			if (songTriggerValue < .51f)
			{
				songTriggerValue += .01f;
				switch(songNumber)
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
		Debug.Log ("playing footstep");
		if (footstep == 0)
		{
			if (CentralControl.isInside == true)
			{
				leftFootMetal = FMOD_StudioSystem.instance.GetEvent("event:/LeftFootMetal");
				leftFootMetal.getParameter("Selector", out leftFootMetalSelector);
				leftFootMetal.getParameter("InsideOutside", out leftFootMetalInsideOutside);
				leftFootMetalSelector.setValue(Random.Range (1f, 11f));
				leftFootMetalInsideOutside.setValue(.5f);
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
				rightFootMetalSelector.setValue(Random.Range(1f, 11f));
				rightFootMetalInsideOutside.setValue(.5f);
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