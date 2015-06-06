using UnityEngine;
using System.Collections;

public class Deployable : MonoBehaviour {

	private KeyCode rotateKey = KeyCode.R;
	[HideInInspector]
	public bool isDeploying;	// Whether this module is deploying


	// Use this for initialization
	void Start () {
		isDeploying = false;

		transform.parent = GameObject.Find("MainPlayer").transform;
		transform.localPosition = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(rotateKey))
		{
			foreach(Transform child in transform)
			{
				child.gameObject.SetActive(!child.gameObject.activeSelf);
			}
		}
	}
}
