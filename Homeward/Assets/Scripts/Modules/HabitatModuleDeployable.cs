﻿using UnityEngine;
using System.Collections;

public class HabitatModuleDeployable : MonoBehaviour {
	
	private SpriteRenderer spriteRenderer;
	private KeyCode deployKey = KeyCode.F;
	private KeyCode rotateKey = KeyCode.R;
	private Building building;
	private bool deployable, isInTrigger;
	private Detector[] detector;
	[HideInInspector]
	public bool isDeploying;
	public float maxLength;
	public GameObject habitatModuleUnfinished;
	private Buildable buildable;
	private int matchedPoint;
	
	private Color color;
	// Use this for initialization
	void Start () {
		transform.parent = GameObject.Find("MainPlayer").transform;
		transform.localPosition = new Vector3 (0, 0, 0);
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		
		color = spriteRenderer.color;
		color.a = 0.5f;
		spriteRenderer.color = color;
		detector = gameObject.GetComponentsInChildren<Detector>();

		deployable = true;
		isDeploying = true;

	}
	
	// Update is called once per frame
	void Update () {
		gameObject.SetActive(!SpriteController.isEnter);
		if (Input.GetKeyDown(deployKey) && deployable) {
			Instantiate(habitatModuleUnfinished, gameObject.transform.position, gameObject.transform.rotation);
			isDeploying = false;
			Reset();
		}
		if (Input.GetKeyDown(rotateKey)) {
			gameObject.transform.Rotate(new Vector3(0, 0, 90));
		}

		for (int i = 0; i < detector.Length; i++) {
			if (detector[i].matched) {
				matchedPoint = i;
				break;
			} else matchedPoint = -1;
		}
		
		if (matchedPoint != -1) {
			gameObject.transform.position += detector[matchedPoint].relation;
			if (deployable && spriteRenderer.color != new Color(0, 0.5f, 0, 0.7f)) {
				spriteRenderer.color = new Color (0, 0.5f, 0, 0.7f);
			}
		} else if ((spriteRenderer.color != color) && (deployable)) {
			spriteRenderer.color = color;
		}
		
		if (Mathf.Abs(gameObject.transform.localPosition.x) > maxLength || Mathf.Abs(gameObject.transform.localPosition.y) > maxLength) {
			gameObject.transform.localPosition = new Vector3(0, 0, 0);
		}
	}
	
	void OnTriggerStay2D (Collider2D other) {
		deployable = false;
		spriteRenderer.color = new Color(0.5f, 0, 0, 0.7f);
	}
	
	void OnTriggerExit2D (Collider2D other) {
		deployable = true;
		spriteRenderer.color = color;
	}

	void Reset () {
		spriteRenderer.color = color;
		gameObject.transform.localPosition = new Vector3(0, 0, 0);
		gameObject.SetActive(false);
		for (int i = 0; i < detector.Length; i++) detector[i].matched = false;
	}
}