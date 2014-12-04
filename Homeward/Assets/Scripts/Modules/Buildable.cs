using UnityEngine;
using System.Collections;

public class Buildable : MonoBehaviour {

	public GameObject habitatModule;
	public int materialsRequired;
	private int buildingProgress;
	private SpriteRenderer spriteRenderer;
	private KeyCode buildKey = KeyCode.F;
	private KeyCode cancelKey = KeyCode.E;
	private Color color;
	private Detector detector;

	// Use this for initialization
	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

		color = spriteRenderer.color;
		color.a = 0.5f;
		spriteRenderer.color = color;
		buildingProgress = 0;

		detector = gameObject.GetComponentInChildren<Detector>();
	}
	
	// Update is called once per frame
	void Update () {
		if (buildingProgress == materialsRequired) {
			Instantiate(habitatModule, gameObject.transform.position, gameObject.transform.rotation);
			Destroy(gameObject);
			SpriteController.isEnter = true;
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (Input.GetKeyDown(cancelKey))
			Destroy(gameObject);
		if (Input.GetKeyDown(buildKey)) {
			buildingProgress++;
			color.a += 0.4f/materialsRequired;
			spriteRenderer.color = color;
		}
	}
}
