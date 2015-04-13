using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Contintue()
    {
        PlayerController.pauseFlag = false;
    }

    public void StartGame()
    {

    }
}
