using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DistantDisplay : MonoBehaviour {

    private Text infoText;
	// Use this for initialization
    void Start()
    {
        infoText = gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        infoText.text = "Distance to Habitat Module: " + Mathf.Round(DistantHabitatModule.min) + "\nDistance to Ship: " + Mathf.Round(Spaceship.distance);
	}
}
