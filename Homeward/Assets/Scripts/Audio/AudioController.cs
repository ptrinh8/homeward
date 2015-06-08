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
	private FMOD.Studio.EventInstance repair;
	private FMOD.Studio.EventInstance repairStart;
	private FMOD.Studio.EventInstance repairEnd;
	private FMOD.Studio.EventInstance buildingStart;
	private FMOD.Studio.EventInstance building;
	private FMOD.Studio.EventInstance buttonPress;
	private FMOD.Studio.EventInstance depositBoxClose;
	private FMOD.Studio.EventInstance depositBoxOpen;
	private FMOD.Studio.EventInstance inventoryOpen;
	private FMOD.Studio.EventInstance inventoryClose;
	private FMOD.Studio.EventInstance toolEquip;
	private FMOD.Studio.EventInstance healthAlarm;
	private FMOD.Studio.EventInstance oxygenAlarm;
	private FMOD.Studio.EventInstance dawnDayTransition;
	private FMOD.Studio.EventInstance dayDuskTransition;
	private FMOD.Studio.EventInstance duskNightTransition;
	private FMOD.Studio.EventInstance nightDawnTransition;
	private FMOD.Studio.EventInstance rockPickupSound;
	private FMOD.Studio.EventInstance breath;
	private FMOD.Studio.EventInstance airlockDoorOpen;
	private FMOD.Studio.EventInstance airlockDoorClose;


	public FMOD.Studio.ParameterInstance stemTrigger1;
	public FMOD.Studio.ParameterInstance stemTrigger2;
	public FMOD.Studio.ParameterInstance stemTrigger3;
	public FMOD.Studio.ParameterInstance stemTrigger4;

	private FMOD.Studio.ParameterInstance leftFootMetalSelector;
	private FMOD.Studio.ParameterInstance rightFootMetalSelector;
	private FMOD.Studio.ParameterInstance leftFootSandSelector;
	private FMOD.Studio.ParameterInstance rightFootSandSelector;
	private FMOD.Studio.ParameterInstance repairSelector;
	private FMOD.Studio.ParameterInstance repairStartSelector;
	private FMOD.Studio.ParameterInstance repairEndSelector;
	private FMOD.Studio.ParameterInstance buildingStartSelector;
	private FMOD.Studio.ParameterInstance buildingSelector;
	private FMOD.Studio.ParameterInstance buttonPressSelector;
	private FMOD.Studio.ParameterInstance depositBoxCloseSelector;
	private FMOD.Studio.ParameterInstance depositBoxOpenSelector;
	private FMOD.Studio.ParameterInstance inventoryOpenSelector;
	private FMOD.Studio.ParameterInstance inventoryCloseSelector;
	private FMOD.Studio.ParameterInstance toolEquipSelector;
	private FMOD.Studio.ParameterInstance rockPickupSoundSelector;
	private FMOD.Studio.ParameterInstance breathSelector;
	private FMOD.Studio.ParameterInstance airlockDoorOpenSelector;
	private FMOD.Studio.ParameterInstance airlockDoorCloseSelector;

	private FMOD.Studio.ParameterInstance leftFootMetalInsideOutside;
	private FMOD.Studio.ParameterInstance rightFootMetalInsideOutside;
	private FMOD.Studio.ParameterInstance leftFootSandInsideOutside;
	private FMOD.Studio.ParameterInstance rightFootSandInsideOutside;
	private FMOD.Studio.ParameterInstance repairInsideOutside;
	private FMOD.Studio.ParameterInstance repairStartInsideOutside;
	private FMOD.Studio.ParameterInstance repairEndInsideOutside;
	private FMOD.Studio.ParameterInstance buildingStartInsideOutside;
	private FMOD.Studio.ParameterInstance buildingInsideOutside;
	private FMOD.Studio.ParameterInstance buttonPressInsideOutside;
	private FMOD.Studio.ParameterInstance depositBoxCloseInsideOutside;
	private FMOD.Studio.ParameterInstance depositBoxOpenInsideOutside;
	private FMOD.Studio.ParameterInstance inventoryOpenInsideOutside;
	private FMOD.Studio.ParameterInstance inventoryCloseInsideOutside;
	private FMOD.Studio.ParameterInstance toolEquipInsideOutside;
	private FMOD.Studio.ParameterInstance rockPickupSoundInsideOutside;
	private FMOD.Studio.ParameterInstance breathInsideOutside;
	private FMOD.Studio.ParameterInstance airlockDoorOpenInsideOutside;
	private FMOD.Studio.ParameterInstance airlockDoorCloseInsideOutside;


	private FMOD.Studio.ParameterInstance leftFootMetalAirlockPressure;
	private FMOD.Studio.ParameterInstance rightFootMetalAirlockPressure;
	private FMOD.Studio.ParameterInstance refineryAirlockPressure;
	private FMOD.Studio.ParameterInstance repairAirlockPressure;
	private FMOD.Studio.ParameterInstance repairStartAirlockPressure;
	private FMOD.Studio.ParameterInstance repairEndAirlockPressure;
	private FMOD.Studio.ParameterInstance buildingStartAirlockPressure;
	private FMOD.Studio.ParameterInstance buildingAirlockPressure;
	private FMOD.Studio.ParameterInstance buttonPressAirlockPressure;
	private FMOD.Studio.ParameterInstance depositBoxCloseAirlockPressure;
	private FMOD.Studio.ParameterInstance depositBoxOpenAirlockPressure;
	private FMOD.Studio.ParameterInstance inventoryOpenAirlockPressure;
	private FMOD.Studio.ParameterInstance inventoryCloseAirlockPressure;
	private FMOD.Studio.ParameterInstance toolEquipAirlockPressure;
	private FMOD.Studio.ParameterInstance airlockDoorOpenAirlockPressure;
	private FMOD.Studio.ParameterInstance airlockDoorCloseAirlockPressure;

	private FMOD.Studio.ParameterInstance miningSelector;
	private FMOD.Studio.ParameterInstance miningInsideOutside;
	private FMOD.Studio.ParameterInstance droneVolume;
	private FMOD.Studio.ParameterInstance startingStopping;

	private FMOD.Studio.ParameterInstance breathOxygenVolume;


	public FMOD.Studio.PLAYBACK_STATE dronePlaybackState;
	private FMOD.Studio.PLAYBACK_STATE refineryPlaybackState;
	public float stemTriggerValue1;
	public float stemTriggerValue2;
	public float stemTriggerValue3;
	public float stemTriggerValue4;
	public float droneVolumeValue;
    private float refineryStartingStopping;

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

	private float oxygenVolume;
	private PlayerController playerController;
	private bool breathStarted = false;

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
		songPlaying = false;
		controllerSoundPressure = 0f;

		breath = FMOD_StudioSystem.instance.GetEvent("event:/Breath");


		playerController = GameObject.Find ("MainPlayer").GetComponent<PlayerController>();

		airlockSound = FMOD_StudioSystem.instance.GetEvent("event:/Airlock");
		airlockSound.getParameter("AirlockPressure", out airlockPressure);
		airlockSound.getParameter("AirlockTransition", out airlockTransition);
		playerLeft = false;
		airlockSoundStarted = false;
		audioPressureTimer = 0f;

		healthAlarm = FMOD_StudioSystem.instance.GetEvent("event:/HealthAlarm");
		oxygenAlarm = FMOD_StudioSystem.instance.GetEvent("event:/OxygenAlarm");
		breath.getParameter("OxygenVolume", out breathOxygenVolume);
		breath.getParameter("InsideOutside", out breathInsideOutside);
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

		if (CentralControl.isInside == false && breathStarted == false)
		{
			breath.start();
			breathInsideOutside.setValue(1.75f);
			breathStarted = true;
		}
		else if (CentralControl.isInside == true)
		{
			breath.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
			breathStarted = false;
		}

		if (playerController.oxygen < 100)
		{
			if (playerController.oxygen >= 50)
			{
				breathOxygenVolume.setValue(.25f);
			}
			else if (playerController.oxygen >= 25 && playerController.oxygen < 50)
			{
				breathOxygenVolume.setValue(1.5f);
			}
			else if (playerController.oxygen < 25)
			{
				breathOxygenVolume.setValue(2.5f);
			}
		}

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

	public void HealthAlarmControl(int controlNumber)
	{
		if (controlNumber == 1)
		{
			//Debug.Log ("starting");
			healthAlarm.start ();
		}
		else if (controlNumber == 2)
		{
			//Debug.Log ("stop");
			healthAlarm.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}

	public void OxygenAlarmControl(int controlNumber)
	{
		if (controlNumber == 1)
		{
			//Debug.Log ("starting");
			oxygenAlarm.start ();
		}
		else if (controlNumber == 2)
		{
			//Debug.Log ("stop");
			oxygenAlarm.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}

	public void MusicControl(int controlNumber, int songNumber)
	{
		songSelectNumber = songNumber;
		// 1 = play, 2 = stop
		if (controlNumber == 1)
		{
			//Debug.Log("song playing");
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

	public void PlayButtonPress()
	{
		if (CentralControl.isInside == true)
		{
			buttonPress = FMOD_StudioSystem.instance.GetEvent("event:/ButtonPress");
			buttonPress.getParameter("Selector", out buttonPressSelector);
			buttonPress.getParameter("InsideOutside", out buttonPressInsideOutside);
			buttonPress.getParameter("AirlockPressure", out buttonPressAirlockPressure);
			buttonPressSelector.setValue(Random.Range (1f, 7f));
			buttonPressInsideOutside.setValue(.5f);
			buttonPressAirlockPressure.setValue(controllerSoundPressure);
			buttonPress.start();
			buttonPress.release ();
		}
		else
		{
			buttonPress = FMOD_StudioSystem.instance.GetEvent("event:/ButtonPress");
			buttonPress.getParameter("Selector", out buttonPressSelector);
			buttonPress.getParameter("InsideOutside", out buttonPressInsideOutside);
			buttonPressSelector.setValue(Random.Range (1f, 7f));
			buttonPressInsideOutside.setValue(1.75f);
			buttonPress.start();
			buttonPress.release();
		}
	}

	public void PlayRockPickupSound()
	{
		if (CentralControl.isInside == true)
		{
			rockPickupSound = FMOD_StudioSystem.instance.GetEvent("event:/RockPickupSound");
			rockPickupSound.getParameter("Selector", out rockPickupSoundSelector);
			rockPickupSound.getParameter("InsideOutside", out rockPickupSoundInsideOutside);
			rockPickupSoundSelector.setValue(Random.Range (1f, 8f));
			rockPickupSoundInsideOutside.setValue(.5f);
			rockPickupSound.start();
			rockPickupSound.release ();
		}
		else
		{
			rockPickupSound = FMOD_StudioSystem.instance.GetEvent("event:/RockPickupSound");
			rockPickupSound.getParameter("Selector", out rockPickupSoundSelector);
			rockPickupSound.getParameter("InsideOutside", out rockPickupSoundInsideOutside);
			rockPickupSoundSelector.setValue(Random.Range (1f, 8f));
			rockPickupSoundInsideOutside.setValue(1.75f);
			rockPickupSound.start();
			rockPickupSound.release();
		}
	}

	public void PlayDepositBox(int openClose)
	{
		if (openClose == 0)
		{
			if (CentralControl.isInside == true)
			{
				depositBoxOpen = FMOD_StudioSystem.instance.GetEvent("event:/DepositBoxOpen");
				depositBoxOpen.getParameter("Selector", out depositBoxOpenSelector);
				depositBoxOpen.getParameter("InsideOutside", out depositBoxOpenInsideOutside);
				depositBoxOpen.getParameter("AirlockPressure", out depositBoxOpenAirlockPressure);
				depositBoxOpenSelector.setValue(Random.Range (1f, 6f));
				depositBoxOpenInsideOutside.setValue(.5f);
				depositBoxOpenAirlockPressure.setValue(controllerSoundPressure);
				depositBoxOpen.start();
				depositBoxOpen.release ();
			}
			else
			{
				depositBoxOpen = FMOD_StudioSystem.instance.GetEvent("event:/DepositBoxOpen");
				depositBoxOpen.getParameter("Selector", out depositBoxOpenSelector);
				depositBoxOpen.getParameter("InsideOutside", out depositBoxOpenInsideOutside);
				depositBoxOpenSelector.setValue(Random.Range (1f, 6f));
				depositBoxOpenInsideOutside.setValue(1.75f);
				depositBoxOpen.start();
				depositBoxOpen.release();
			}
		}
		else if (openClose == 1)
		{
			if (CentralControl.isInside == true)
			{
				depositBoxClose = FMOD_StudioSystem.instance.GetEvent("event:/DepositBoxClose");
				depositBoxClose.getParameter("Selector", out depositBoxCloseSelector);
				depositBoxClose.getParameter("InsideOutside", out depositBoxCloseInsideOutside);
				depositBoxClose.getParameter("AirlockPressure", out depositBoxCloseAirlockPressure);
				depositBoxCloseSelector.setValue(Random.Range (1f, 6f));
				depositBoxCloseInsideOutside.setValue(.5f);
				depositBoxCloseAirlockPressure.setValue(controllerSoundPressure);
				depositBoxClose.start();
				depositBoxClose.release ();
			}
			else
			{
				depositBoxClose = FMOD_StudioSystem.instance.GetEvent("event:/DepositBoxClose");
				depositBoxClose.getParameter("Selector", out depositBoxCloseSelector);
				depositBoxClose.getParameter("InsideOutside", out depositBoxCloseInsideOutside);
				depositBoxCloseSelector.setValue(Random.Range (1f, 6f));
				depositBoxCloseInsideOutside.setValue(1.75f);
				depositBoxClose.start();
				depositBoxClose.release();
			}
		}
	}

	public void PlayInventorySound(int inventorySound)
	{
		if (inventorySound == 0)
		{
			if (CentralControl.isInside == true)
			{
				inventoryOpen = FMOD_StudioSystem.instance.GetEvent("event:/InventoryOpen");
				inventoryOpen.getParameter("Selector", out inventoryOpenSelector);
				inventoryOpen.getParameter("InsideOutside", out inventoryOpenInsideOutside);
				inventoryOpen.getParameter("AirlockPressure", out inventoryOpenAirlockPressure);
				inventoryOpenSelector.setValue(Random.Range (1f, 6f));
				inventoryOpenInsideOutside.setValue(.5f);
				inventoryOpenAirlockPressure.setValue(controllerSoundPressure);
				inventoryOpen.start();
				inventoryOpen.release ();
			}
			else
			{
				inventoryOpen = FMOD_StudioSystem.instance.GetEvent("event:/InventoryOpen");
				inventoryOpen.getParameter("Selector", out inventoryOpenSelector);
				inventoryOpen.getParameter("InsideOutside", out inventoryOpenInsideOutside);
				inventoryOpenSelector.setValue(Random.Range (1f, 6f));
				inventoryOpenInsideOutside.setValue(1.75f);
				inventoryOpen.start();
				inventoryOpen.release();
			}
		}
		else if (inventorySound == 1)
		{
			if (CentralControl.isInside == true)
			{
				inventoryClose = FMOD_StudioSystem.instance.GetEvent("event:/InventoryClose");
				inventoryClose.getParameter("Selector", out inventoryCloseSelector);
				inventoryClose.getParameter("InsideOutside", out inventoryCloseInsideOutside);
				inventoryClose.getParameter("AirlockPressure", out inventoryCloseAirlockPressure);
				inventoryCloseSelector.setValue(Random.Range (1f, 6f));
				inventoryCloseInsideOutside.setValue(.5f);
				inventoryCloseAirlockPressure.setValue(controllerSoundPressure);
				inventoryClose.start();
				inventoryClose.release ();
			}
			else
			{
				inventoryClose = FMOD_StudioSystem.instance.GetEvent("event:/InventoryClose");
				inventoryClose.getParameter("Selector", out inventoryCloseSelector);
				inventoryClose.getParameter("InsideOutside", out inventoryCloseInsideOutside);
				inventoryCloseSelector.setValue(Random.Range (1f, 6f));
				inventoryCloseInsideOutside.setValue(1.75f);
				inventoryClose.start();
				inventoryClose.release();
			}
		}
	}

	public void PlayMiningSound()
	{
//		Debug.Log ("mining sound");
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

	public void PlayRepairSound(int repairSound)
	{
		if (repairSound == 1) //repair action sound
		{
			if (CentralControl.isInside == true)
			{
				repair = FMOD_StudioSystem.instance.GetEvent("event:/Repair");
				repair.getParameter("Selector", out repairSelector);
				repair.getParameter("InsideOutside", out repairInsideOutside);
				repair.getParameter("AirlockPressure", out repairAirlockPressure);
				repairSelector.setValue(Random.Range (1f, 9f));
				repairInsideOutside.setValue(.5f);
				repairAirlockPressure.setValue(controllerSoundPressure);
				repair.start();
				repair.release();
			}
			else
			{
				repair = FMOD_StudioSystem.instance.GetEvent("event:/Repair");
				repair.getParameter("Selector", out repairSelector);
				repair.getParameter("InsideOutside", out repairInsideOutside);
				repair.getParameter("AirlockPressure", out repairAirlockPressure);
				repairSelector.setValue(Random.Range (1f, 9f));
				repairInsideOutside.setValue(1.75f);
				repairAirlockPressure.setValue(controllerSoundPressure);
				repair.start();
				repair.release();
			}
		}
		else if (repairSound == 2) //repair start sound
		{
			if (CentralControl.isInside == true)
			{
				repairStart = FMOD_StudioSystem.instance.GetEvent("event:/RepairStart");
				repairStart.getParameter("Selector", out repairStartSelector);
				repairStart.getParameter("InsideOutside", out repairStartInsideOutside);
				repairStart.getParameter("AirlockPressure", out repairStartAirlockPressure);
				repairStartSelector.setValue(Random.Range (1f, 5f));
				repairStartInsideOutside.setValue(.5f);
				repairStartAirlockPressure.setValue(controllerSoundPressure);
				repairStart.start();
				repairStart.release();
			}
			else
			{
				repairStart = FMOD_StudioSystem.instance.GetEvent("event:/RepairStart");
				repairStart.getParameter("Selector", out repairStartSelector);
				repairStart.getParameter("InsideOutside", out repairStartInsideOutside);
				repairStart.getParameter("AirlockPressure", out repairStartAirlockPressure);
				repairStartSelector.setValue(Random.Range (1f, 5f));
				repairStartInsideOutside.setValue(1.75f);
				repairStartAirlockPressure.setValue(controllerSoundPressure);
				repairStart.start();
				repairStart.release();
			}
		}
		else if (repairSound == 3) //repair end sound
		{
			if (CentralControl.isInside == true)
			{
				repairEnd = FMOD_StudioSystem.instance.GetEvent("event:/RepairEnd");
				repairEnd.getParameter("Selector", out repairEndSelector);
				repairEnd.getParameter("InsideOutside", out repairEndInsideOutside);
				repairEnd.getParameter("AirlockPressure", out repairEndAirlockPressure);
				repairEndSelector.setValue(Random.Range (1f, 9f));
				repairEndInsideOutside.setValue(.5f);
				repairEndAirlockPressure.setValue(controllerSoundPressure);
				repairEnd.start();
				repairEnd.release();
			}
			else
			{
				repairEnd = FMOD_StudioSystem.instance.GetEvent("event:/RepairEnd");
				repairEnd.getParameter("Selector", out repairEndSelector);
				repairEnd.getParameter("InsideOutside", out repairEndInsideOutside);
				repairEnd.getParameter("AirlockPressure", out repairEndAirlockPressure);
				repairEndSelector.setValue(Random.Range (1f, 9f));
				repairEndInsideOutside.setValue(1.75f);
				repairEndAirlockPressure.setValue(controllerSoundPressure);
				repairEnd.start();
				repairEnd.release();
			}
		}
	}

	public void PlayAirlockDoorSound(int airlockSound)
	{
		if (airlockSound == 1)
		{
			if (CentralControl.isInside == true)
			{
				airlockDoorOpen = FMOD_StudioSystem.instance.GetEvent("event:/AirlockDoorOpen");
				airlockDoorOpen.getParameter("Selector", out airlockDoorOpenSelector);
				airlockDoorOpen.getParameter("InsideOutside", out airlockDoorOpenInsideOutside);
				airlockDoorOpen.getParameter("AirlockPressure", out airlockDoorOpenAirlockPressure);
				airlockDoorOpenSelector.setValue(Random.Range (1f, 3f));
				airlockDoorOpenAirlockPressure.setValue(controllerSoundPressure);
				airlockDoorOpenInsideOutside.setValue(.5f);
				airlockDoorOpen.start();
				airlockDoorOpen.release();
			}
			else
			{
				airlockDoorOpen = FMOD_StudioSystem.instance.GetEvent("event:/AirlockDoorOpen");
				airlockDoorOpen.getParameter("Selector", out airlockDoorOpenSelector);
				airlockDoorOpen.getParameter("InsideOutside", out airlockDoorOpenInsideOutside);
				airlockDoorOpen.getParameter("AirlockPressure", out airlockDoorOpenAirlockPressure);
				airlockDoorOpenSelector.setValue(Random.Range (1f, 3f));
				airlockDoorOpenAirlockPressure.setValue(controllerSoundPressure);
				airlockDoorOpenInsideOutside.setValue(1.75f);
				airlockDoorOpen.start();
				airlockDoorOpen.release();
			}
		}
		else if (airlockSound == 2)
		{
			if (CentralControl.isInside == true)
			{
				airlockDoorClose = FMOD_StudioSystem.instance.GetEvent("event:/AirlockDoorClose");
				airlockDoorClose.getParameter("Selector", out airlockDoorCloseSelector);
				airlockDoorClose.getParameter("InsideOutside", out airlockDoorCloseInsideOutside);
				airlockDoorClose.getParameter("AirlockPressure", out airlockDoorCloseAirlockPressure);
				airlockDoorCloseSelector.setValue(Random.Range (1f, 3f));
				airlockDoorCloseAirlockPressure.setValue(controllerSoundPressure);
				airlockDoorCloseInsideOutside.setValue(.5f);
				airlockDoorClose.start();
				airlockDoorClose.release();
			}
			else
			{
				airlockDoorClose = FMOD_StudioSystem.instance.GetEvent("event:/AirlockDoorClose");
				airlockDoorClose.getParameter("Selector", out airlockDoorCloseSelector);
				airlockDoorClose.getParameter("InsideOutside", out airlockDoorCloseInsideOutside);
				airlockDoorClose.getParameter("AirlockPressure", out airlockDoorCloseAirlockPressure);
				airlockDoorCloseSelector.setValue(Random.Range (1f, 3f));
				airlockDoorCloseAirlockPressure.setValue(controllerSoundPressure);
				airlockDoorCloseInsideOutside.setValue(1.75f);
				airlockDoorClose.start();
				airlockDoorClose.release();
			}
		}
	}

	public void PlayBuildingSound(int buildingSound)
	{
		if (buildingSound == 1)
		{
			buildingStart = FMOD_StudioSystem.instance.GetEvent("event:/BuildingStart");
			buildingStart.getParameter("Selector", out buildingStartSelector);
			buildingStart.getParameter("InsideOutside", out buildingStartInsideOutside);
			buildingStart.getParameter("AirlockPressure", out buildingStartAirlockPressure);
			buildingStartSelector.setValue(Random.Range (1f, 9f));
			buildingStartAirlockPressure.setValue(controllerSoundPressure);
			buildingStart.start();
			buildingStart.release();
		}
		else if (buildingSound == 2)
		{
			building = FMOD_StudioSystem.instance.GetEvent("event:/Building");
			building.getParameter("Selector", out buildingSelector);
			building.getParameter("InsideOutside", out buildingInsideOutside);
			building.getParameter("AirlockPressure", out buildingAirlockPressure);
			buildingSelector.setValue(Random.Range(1f, 6f));
			buildingAirlockPressure.setValue(controllerSoundPressure);
			building.start();
			building.release();
		}
	}

	public void PlayToolEquipSound()
	{
		toolEquip = FMOD_StudioSystem.instance.GetEvent("event:/ToolEquip");
		toolEquip.getParameter("Selector", out toolEquipSelector);
		toolEquip.getParameter("InsideOutside", out toolEquipInsideOutside);
		toolEquip.getParameter("AirlockPressure", out toolEquipAirlockPressure);
		toolEquipSelector.setValue(Random.Range (1f, 6f));
		toolEquipAirlockPressure.setValue(controllerSoundPressure);
		toolEquip.start();
		toolEquip.release();
	}

	public void PlayDawnDayTransition()
	{
		dawnDayTransition = FMOD_StudioSystem.instance.GetEvent("event:/DawnDayTransition");
		dawnDayTransition.start();
		dawnDayTransition.release();
	}

	public void PlayDayDuskTransition()
	{
		dayDuskTransition = FMOD_StudioSystem.instance.GetEvent("event:/DayDuskTransition");
		dayDuskTransition.start();
		dayDuskTransition.release();
	}

	public void PlayDuskNightTransition()
	{
		duskNightTransition = FMOD_StudioSystem.instance.GetEvent("event:/DuskNightTransition");
		duskNightTransition.start();
		duskNightTransition.release();
	}

	public void PlayNightDawnTransition()
	{
		nightDawnTransition = FMOD_StudioSystem.instance.GetEvent("event:/NightDawnTransition");
		nightDawnTransition.start();
		nightDawnTransition.release();
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