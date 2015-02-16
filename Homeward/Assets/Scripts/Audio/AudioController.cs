using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
	private FMOD.Studio.EventInstance music;
	private FMOD.Studio.EventInstance leftFootMetal;
	private FMOD.Studio.EventInstance rightFootMetal;
	private FMOD.Studio.EventInstance leftFootSand;
	private FMOD.Studio.EventInstance rightFootSand;
	private FMOD.Studio.EventInstance mining;
	private FMOD.Studio.ParameterInstance stemTrigger;
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
	public float stemTriggerValue;
	private PlayerController player;
	// Use this for initialization
	void Start () {
		music = FMOD_StudioSystem.instance.GetEvent("event:/MusicTrigger");
		//music.start();
		music.getParameter("StemSelector", out stemTrigger);
		stemTriggerValue = 0f;
		player = GameObject.Find ("MainPlayer").GetComponent<PlayerController>();
	}
	// Update is called once per frame
	void Update () {
		//stemTrigger.setValue(stemTriggerValue);
	}
	void OnDisable()
	{
		music.release();
	}


	public void PlayFootstep(int footstep)
	{
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
}