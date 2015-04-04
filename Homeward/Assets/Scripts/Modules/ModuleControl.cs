using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ModuleControl : MonoBehaviour {

    private int outpostNumber;

    private List<GameObject> habitatModules;

    private Camera mainCamera;

    private static bool showModuleControl;

    public static bool ShowModuleControl
    {
        get { return showModuleControl; }
    }

    void OutpostGenerated(GameObject habitatModule)
    {
        outpostNumber++;

        GameObject controlObject = Instantiate(Resources.Load("ModuleControl/ControlObject")) as GameObject;
        controlObject.transform.parent = habitatModule.transform;
        controlObject.GetComponent<Camera>().camera.enabled = false;
        controlObject.transform.position = controlObject.transform.parent.position;
        controlObject.transform.localPosition = new Vector3(0.0f, 0.0f, -10.0f);
        
        habitatModules.Add(habitatModule);
    }

    void OutpostUpdated(GameObject habitatModule)
    {

    }

	void Awake () 
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamera.enabled = true;

        outpostNumber = -1;

        habitatModules = new List<GameObject>();
        showModuleControl = false;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showModuleControl = !showModuleControl;

            Debug.Log(habitatModules.Count);

            //outpostNumber++;
            //if (outpostNumber == habitatModules.Count)
            //{
            //    outpostNumber = 0;
            //}

            if (showModuleControl)
            {
                mainCamera.enabled = false;
                habitatModules[outpostNumber].GetComponentInChildren<Camera>().camera.enabled = true;
            }
            else
            {
                mainCamera.enabled = true;
                habitatModules[outpostNumber].GetComponentInChildren<Camera>().camera.enabled = false;
            }
        }
    }
}
