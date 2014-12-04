using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
    public bool isQuit = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
        guiText.material.color = Color.red;
    }

    void OnMouseExit()
    {
        guiText.material.color = Color.white;
    }

    void OnMouseUp()
    {
        if (isQuit == true)
        {
            Application.Quit();
        }
        else
        {
            Application.LoadLevel("Planet");
        }
    }
}
