using UnityEngine;
using System.Collections;

// This class is for handling any modules "on-player-blueprint" 
public class Deploy : MonoBehaviour {
	private SpriteRenderer spriteRenderer;	// Change the color of sprite when needed
	private KeyCode deployKey = KeyCode.F;
	private bool deployable;	// Whether the blueprint can be deployed 
	private Detector[] detector;	// Scripts in children gameobject that handle the "snap" or trigger
	private float maxLength;		// Max stretch length 
	public GameObject moduleUnfinished;	// Detached blueprint(prefabs)
	private int matchedPoint;	// record which detector is "matched"
	private Color color;

	private bool canDeploy;
	private bool sameModuleFlag;

	private AudioController audioController;

	private GameObject[] temp;

	void ChangeSameModuleFlag(bool changeTo)
	{
		sameModuleFlag = changeTo;
	}

	// Use this for initialization
	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

		color = new Color (0.5f, 0, 0, 0.7f);	// record the original sprite color
		spriteRenderer.color = color;
		detector = gameObject.GetComponentsInChildren<Detector>();
		temp = new GameObject[detector.Length];

		maxLength = 3f;

		deployable = true;
		transform.parent.gameObject.GetComponent<Deployable>().isDeploying = true;

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
			transform.parent.gameObject.GetComponent<Deployable>().isDeploying = false;
			Reset();
            Building.isDeploying = false;
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

		if (matchedPoint != -1 && !sameModuleFlag && deployable) 
		{
			transform.parent.localPosition += detector[matchedPoint].relation;	// Snap!
			Invoke ("setCanDeploy", 0.2f);
			if (Mathf.Abs(transform.parent.localPosition.x) > maxLength && Mathf.Abs(transform.parent.localPosition.y) > maxLength)
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
		else if (spriteRenderer.color != color || !deployable)
		{
			transform.parent.localPosition = new Vector3(0, 0, 0);
			spriteRenderer.color = color;
			canDeploy = false;
		}

		// Unsnap if on-player-blueprint get too far away from origin(player)
		if (Mathf.Abs(transform.parent.localPosition.x) > maxLength || Mathf.Abs(transform.parent.localPosition.y) > maxLength) {
			transform.parent.localPosition = new Vector3(0, 0, 0);
		}

	}

	// Cannot deploy when blocked
	void OnTriggerStay2D (Collider2D other) {
		if (!(other.gameObject.tag == "FinalTextures" || other.gameObject.tag == "Footprint" || other.gameObject.name.Contains("PCG") || 
		    other.gameObject.tag == "InitialTerrainTrigger" || other.gameObject.name.Contains("Point")))
		{
			if (other.gameObject.tag == "Player")
			{
				deployable = true;
				spriteRenderer.color = new Color (0, 0.5f, 0, 0.7f);

			}
			else
			{
				deployable = false;
				spriteRenderer.color = color;
			}
		}
	}

	void setCanDeploy()
	{
		canDeploy = true;
	}

	void Reset () {
		spriteRenderer.color = color;
		transform.parent.localPosition = new Vector3(0, 0, 0);
		transform.parent.gameObject.SetActive(false);
		for (int i = 0; i < detector.Length; i++) 
			detector[i].matched = false;
        Building.isDeploying = false;

		canDeploy = false;
	}
}
