using UnityEngine;
using System.Collections;

public class HabitatModuleDeploy : MonoBehaviour {

	private SpriteRenderer spriteRenderer;	// Change the color of sprite when needed
	private KeyCode deployKey = KeyCode.F;
	private bool deployable;	// Whether the blueprint can be deployed 
	private float maxLength;		// Max stretch length 
	public GameObject habitatModuleUnfinished;	// Detached blueprint(prefabs)
	private Color color; // record alpha color
	private AudioController audioController;
	
	// Use this for initialization
	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		audioController = GameObject.Find ("AudioObject").GetComponent<AudioController>();
		color = spriteRenderer.color;	// record the original sprite color
		color.a = 0.5f;
		spriteRenderer.color = color;
		deployable = true;
		transform.parent.gameObject.GetComponent<Deployable>().isDeploying = true;
		maxLength = 3f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.parent.localPosition = new Vector3(0, 0, 0);
		// Player cannot deploy blueprint when indoor
		if (CentralControl.isInside) Reset();
		else {
			gameObject.SetActive(!CentralControl.isInside);
		}
		
		// Condition to detach the blueprint
		if (Input.GetKeyDown(deployKey) && deployable) {
			audioController.PlayBuildingSound(1);
			Instantiate(habitatModuleUnfinished, gameObject.transform.position, gameObject.transform.rotation);	// Instantiate the detached blueprint
			transform.parent.gameObject.GetComponent<Deployable>().isDeploying = false;
			Reset();	// Reset the blueprint
			Building.isDeploying = false;
		}

		// Unsnap if on-player-blueprint get too far away from origin(player)
		if (Mathf.Abs(gameObject.transform.localPosition.x) > maxLength || Mathf.Abs(gameObject.transform.localPosition.y) > maxLength) {
			gameObject.transform.localPosition = new Vector3(0, 0, 0);
		}
		
	}
	
	// Cannot deploy when blocked
	void OnTriggerStay2D (Collider2D other) {
		if (!(other.gameObject.tag == "FinalTextures" || other.gameObject.tag == "Footprint" || other.gameObject.name.Contains("PCG") || 
		      other.gameObject.tag == "InitialTerrainTrigger" || other.gameObject.name.Contains("Point")))
		{
			if (other.gameObject.tag != "Player")
			{
				deployable = false;
				spriteRenderer.color = new Color (0.5f, 0, 0, 0.7f);
				return ;
			}
			else
			{
				deployable = true;
				spriteRenderer.color = color;
			}
		}
	}
	
	void Reset () {
		spriteRenderer.color = color;
		transform.parent.localPosition = new Vector3(0, 0, 0);
		transform.parent.gameObject.SetActive(false);
		Building.isDeploying = false;
	}
}
