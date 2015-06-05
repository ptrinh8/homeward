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

	private bool canDeploy;
	private bool sameModuleFlag;

	private AudioController audioController;

	private GameObject[] temp;

	public GameObject flippedModuleDeploying;

	void ChangeSameModuleFlag(bool changeTo)
	{
		sameModuleFlag = changeTo;
	}

	// Use this for initialization
	void Start () {
		transform.parent = GameObject.Find("MainPlayer").transform;
		transform.localPosition = new Vector3 (0, 0, 0);
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

		color = new Color (0.5f, 0, 0, 0.7f);	// record the original sprite color
		spriteRenderer.color = color;
		detector = gameObject.GetComponentsInChildren<Detector>();
		temp = new GameObject[detector.Length];

		maxLength = 1.2f;

		deployable = true;
		isDeploying = true;

		matchedPoint = -1;	// -1 means no match

		audioController = GameObject.Find ("AudioObject").GetComponent<AudioController>();

		canDeploy = false;
		sameModuleFlag = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Player cannot deploy blueprint when indoor
		if (CentralControl.isInside) Reset();
		else {
			gameObject.SetActive(!CentralControl.isInside);
		}

		// Condition to detach the blueprint
		if (Input.GetKeyDown(deployKey) && deployable && canDeploy) {
			audioController.PlayBuildingSound(1);
			Instantiate(moduleUnfinished, gameObject.transform.position, gameObject.transform.rotation);
			isDeploying = false;
			Reset();
            Building.isDeploying = false;
		}

		// Rotation
		if (Input.GetKeyDown(rotateKey)) {
			//gameObject = flippedModuleDeploying;
			//gameObject = Instantiate(flippedModuleDeploying, gameObject.transform.position, Quaternion.identity);
			//GameObject.Destroy(gameObject);
		}

		for (int i = 0; i < detector.Length; i++)
		{ temp[i] = detector[i].connectedTo;}
		// See if any snap trigger is matched
		for (int i = 0; i < detector.Length; i++) 
		{
			if (detector[i].matched) 
			{
				matchedPoint = i;
				break;
			} 
			else 
			{
				matchedPoint = -1;
			}
		}

		if (!sameModuleFlag) 
		{
			if (matchedPoint != -1)
			{
				//Debug.Log("0");
				for (int m = 0; m < detector.Length; m++) 
				{
					if (detector[m].connectedTo != null) 
					{
						//Debug.Log("1");
						for (int n = m + 1; n < detector.Length; n++)
						{
							//Debug.Log("2");
							if (temp[n] != null)
							{
								//Debug.Log("3 " + "m:" + detector[m].connectedTo + " n:" + temp[n]);
								if (detector[m].connectedTo == temp[n])
								{ 
									//Debug.Log("4");
									matchedPoint = -1;
									sameModuleFlag = true;
									break;
								}
							}
						}
						if (matchedPoint == -1)
						{
							//Debug.Log("5");
							break;
						}
					}
				}
			}
		}



		if (matchedPoint != -1 && !sameModuleFlag) 
		{
			gameObject.transform.position += detector[matchedPoint].relation;	// Snap!
			if (Mathf.Abs(gameObject.transform.localPosition.x) < maxLength && Mathf.Abs(gameObject.transform.localPosition.y) < maxLength)
			{ canDeploy = true;}
			else
			{ 
				canDeploy = false;
				spriteRenderer.color = color;
			}

			// Change the sprite color to green
			if (deployable && canDeploy && spriteRenderer.color != new Color(0, 0.5f, 0, 0.7f)) 
			{
				spriteRenderer.color = new Color (0, 0.5f, 0, 0.7f);
			}
		} 
		else if (spriteRenderer.color != color && deployable)
		{
			spriteRenderer.color = color;
			canDeploy = false;
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

		canDeploy = false;
	}
}
