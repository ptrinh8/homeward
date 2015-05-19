using UnityEngine;
using System.Collections;

// This class is for handling any modules "on-player-blueprint" 
public class Deployable : MonoBehaviour {

	private SpriteRenderer spriteRenderer;	// Change the color of sprite when needed
	private KeyCode deployKey = KeyCode.F;
	private KeyCode rotateKey = KeyCode.R;
	private bool deployable;	// Whether the blueprint can be deployed 
	private Detector[] detector;	// Scripts in children gameobject that handle the "snap" or trigger
	[HideInInspector]
	public bool isDeploying;	// Whether this module is deploying
	private float maxLength;		// Max stretch length 
	public GameObject moduleUnfinished;	// Detached blueprint(prefabs)
	private int matchedPoint;	// record which detector is "matched"
	private Color color;

	private AudioController audioController;

	// Use this for initialization
	void Start () {
		transform.parent = GameObject.Find("MainPlayer").transform;
		transform.localPosition = new Vector3 (0, 0, 0);
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

		color = new Color (0.5f, 0, 0, 0.7f);	// record the original sprite color
		spriteRenderer.color = color;
		detector = gameObject.GetComponentsInChildren<Detector>();

		maxLength = 1.2f;

		deployable = true;
		isDeploying = true;

		matchedPoint = -1;	// -1 means no match

		audioController = GameObject.Find ("AudioObject").GetComponent<AudioController>();
	}
	
	// Update is called once per frame
	void Update () {
		// Player cannot deploy blueprint when indoor
		if (CentralControl.isInside) Reset();
		else {
			gameObject.SetActive(!CentralControl.isInside);
		}

		// Condition to detach the blueprint
		if (Input.GetKeyDown(deployKey) && deployable && matchedPoint != -1) {
			audioController.PlayBuildingSound(1);
			Instantiate(moduleUnfinished, gameObject.transform.position, gameObject.transform.rotation);
			isDeploying = false;
			Reset();
            Building.isDeploying = false;
		}

		// Rotation
		if (Input.GetKeyDown(rotateKey)) {
			gameObject.transform.Rotate(new Vector3(0, 0, 90));
		}

		// See if any snap trigger is matched
		for (int i = 0; i < detector.Length; i++) {
			if (detector[i].matched) {
				matchedPoint = i;
				break;
			} else matchedPoint = -1;
		}

		if (matchedPoint != -1) {
			gameObject.transform.position += detector[matchedPoint].relation;	// Snap!
			// Change the sprite color to green
			if (deployable && spriteRenderer.color != new Color(0, 0.5f, 0, 0.7f)) {
				spriteRenderer.color = new Color (0, 0.5f, 0, 0.7f);
			}
		} else if (spriteRenderer.color != color && deployable) {
			spriteRenderer.color = color;
		}

		// Unsnap if on-player-blueprint get too far away from origin(player)
		if (Mathf.Abs(gameObject.transform.localPosition.x) > maxLength || Mathf.Abs(gameObject.transform.localPosition.y) > maxLength) {
			gameObject.transform.localPosition = new Vector3(0, 0, 0);
		}
	}

	// Cannot deploy when blocked
	void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject.tag != "FinalTextures" && other.gameObject.tag != "Footprint" && !other.gameObject.name.Contains("PCG") && other.gameObject.tag != "Wall" && 
		    other.gameObject.tag != "InitialTerrainTrigger" && !other.gameObject.name.Contains("Point") && other.gameObject.name.Contains("Modules"))
		{
			//Debug.Log(other.gameObject);
			deployable = false;
			spriteRenderer.color = new Color (0.5f, 0, 0, 0.7f);;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
			deployable = true;
			spriteRenderer.color = color;
	}

	void Reset () {
		spriteRenderer.color = color;
		gameObject.transform.localPosition = new Vector3(0, 0, 0);
		gameObject.SetActive(false);
		for (int i = 0; i < detector.Length; i++) 
			detector[i].matched = false;
        Building.isDeploying = false;
	}
}
