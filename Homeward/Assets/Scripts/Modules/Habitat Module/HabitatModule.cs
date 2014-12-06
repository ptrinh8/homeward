using UnityEngine;
using System.Collections;

public class HabitatModule : MonoBehaviour {

	public GameObject bed;
	private GameObject connectPoint;
	private GameObject doorway;

	// Use this for initialization
	void Start () {

		bed = transform.Find("Bed").gameObject;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
