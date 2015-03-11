using UnityEngine;
using System.Collections;

public class EndText : MonoBehaviour 
{
    Spaceship spaceship = new Spaceship();

	void Start () 
    {
        GameObject endScreenText = GameObject.Find("EndScreenText");
        endScreenText.GetComponentInChildren<GUIText>().enabled = false;
        spaceship = FindObjectOfType(typeof(Spaceship)) as Spaceship;
	}
	
	void Update () 
    {
        if (spaceship.endDemo == true)
        {
            GameObject endScreenText = GameObject.Find("EndScreenText");
            endScreenText.GetComponentInChildren<GUIText>().enabled = true;
        }
	}
}
