using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// class that handle airpresure in module
public class AirControl : MonoBehaviour {

    private bool air;
    private float timer;
    public int flag;
    public float duration;
	[HideInInspector]
    public float airPressureBar;
    public int manuallyOperateDifficulty;

    private LocalControl moduleControl;

	//audio stuff for airlock
	public float doorTimer;
	public float doorLength;
	public bool airlockActivated;
	private FMOD.Studio.EventInstance airlockSound;
	public FMOD.Studio.ParameterInstance airlockPressure;
	public FMOD.Studio.ParameterInstance airlockTransition;
	public FMOD.Studio.PLAYBACK_STATE airlockPlaybackState;
	public float soundPressure;
	public float soundTransition;
	public float pressureTimer;

	private AudioController audioController;

    public bool Air
    {
        get { return air;}
        set { this.air = value;}
    }

    public float Timer
    {
        get { return timer;}
        set { this.timer = value;}
    }

    public int Flag
    {
        get { return flag;}
        set { this.flag = value;}
    }

    public void ResetTimer()
    {
        if (air) { flag = 1;}
        else { flag = -1;}
    }

	// Use this for initialization
	void Start () {
        air = true;
		duration = 4f;
        timer = duration;
        flag = 0;
		airlockSound = FMOD_StudioSystem.instance.GetEvent("event:/Airlock");
		airlockSound.getParameter("AirlockPressure", out airlockPressure);
		airlockSound.getParameter("AirlockTransition", out airlockTransition);
		doorTimer = 0f;
		doorLength = 1.25f;
		airlockActivated = false;
        moduleControl = gameObject.transform.root.gameObject.GetComponent<LocalControl>();
		audioController = GameObject.Find ("AudioObject").GetComponent<AudioController>();
		soundPressure = 0f;
		audioController.airlockDuration = duration;

		airPressureBar = 1;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(flag);
		airlockSound.getPlaybackState(out airlockPlaybackState);
        if (flag == 0) { 
            // do nothing
        }
        else if (flag == 1)
        {
            if (airPressureBar < 1)
            {
                timer += Time.deltaTime;
				airPressureBar = timer / duration;
				audioController.pressureRisingFalling = true;
            }
            else
            {
				audioController.pressureRisingFalling = false;
                airPressureBar = 1;
                flag = 0;
                air = true;
            }
        }
        else if (flag == -1)
        {
            if (airPressureBar > 0) 
			{
                timer -= Time.deltaTime;
                airPressureBar = timer / duration;
				audioController.pressureRisingFalling = true;
            }
            else
            {
				audioController.pressureRisingFalling = false;
                airPressureBar = 0;
                flag = 0;
                air = false;
            }
        }

		if (airlockActivated == true)
		{
			doorTimer += Time.deltaTime;
			if (audioController.playerLeft == false)
			{
				if (doorTimer >= doorLength)
				{
					flag = -1;
					doorTimer = 0;
					airlockActivated = false;
					audioController.airlockActivated = false;
					audioController.airlockSoundStarted = false;
				}
			}
			else if (audioController.playerLeft == true)
			{
				if (doorTimer >= doorLength)
				{
					flag = 1;
					doorTimer = 0;
					airlockActivated = false;
					audioController.airlockActivated = false;
					audioController.airlockSoundStarted = false;
				}
			}
		}
	}

	// method to activite airlock module
    void AirModuleActivite()
    {
        if (moduleControl.isOn && moduleControl.IsPowered && !moduleControl.IsBroken)
        {
			if (airlockActivated == false)
			{
				airlockActivated = true;
				audioController.airlockActivated = true;
			}
        }
    }

}
