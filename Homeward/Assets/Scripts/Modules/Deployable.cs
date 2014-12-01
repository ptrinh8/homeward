using UnityEngine;
using System.Collections;

public class Deployable : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private KeyCode deployKey = KeyCode.F;
	private Building building;
	private bool deployable;
	private Detector[] detector;
	public float maxLength;
	public GameObject haitatModuleUnfinished;
	private int matchedPoint;

	private Color color;
	// Use this for initialization
	void Start () {
		transform.parent = GameObject.Find("MainPlayer").transform;
		transform.localPosition = new Vector3 (0, 0, 0);
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

		color = new Color (0.5f, 0, 0, 0.7f);
		spriteRenderer.color = color;
		building = GameObject.Find("Module Building").GetComponent<Building>();
		detector = gameObject.GetComponentsInChildren<Detector>();
	
		deployable = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(deployKey) && building.isDeploying && deployable && matchedPoint != -1) {
			Instantiate(haitatModuleUnfinished, gameObject.transform.position, gameObject.transform.rotation);
			building.isDeploying = false;
			Reset();
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
		} else if (spriteRenderer.color != color) {
			spriteRenderer.color = color;
		}
		/**
		for (int i = 0; i < detector.Length; i++){
			if (detector[i].matched) {
				gameObject.transform.position += detector[i].relation;
				detector[i].matched = false;
				if (deployable) {
					Debug.Log("green");
					spriteRenderer.color = new Color (0, 0.5f, 0, 0.7f);
					break;
				}
			} else if (deployable) {
				Debug.Log("gray");
				spriteRenderer.color = color;
			}
		}
		**/
		if (Mathf.Abs(gameObject.transform.localPosition.x) > maxLength || Mathf.Abs(gameObject.transform.localPosition.y) > maxLength) {
			gameObject.transform.localPosition = new Vector3(0, 0, 0);
		}

	}

	void OnTriggerStay2D (Collider2D other) {
			deployable = false;
			spriteRenderer.color = Color.red;
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
