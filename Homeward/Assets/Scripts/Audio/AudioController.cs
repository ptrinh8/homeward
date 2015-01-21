using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

	private FMOD.Studio.EventInstance music;
	private FMOD.Studio.ParameterInstance stemTrigger;
	public float stemTriggerValue;

	// Use this for initialization
	void Start () {

		music = FMOD_StudioSystem.instance.GetEvent("event:/MusicTrigger1");
		music.start();
		music.getParameter("StemSelector", out stemTrigger);
		stemTriggerValue = 25f;
	
	}
	
	// Update is called once per frame
	void Update () {

		//stemTrigger.setValue(25f);
	
	}

	void OnDisable()
	{
		music.release();
	}
}
