using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CentralControl : MonoBehaviour {

	public Sprite indoorSprite;
	public Sprite outdoorSprite;
	public static bool isInside; // This static variable is the general location of player(whether inside)
	private SpriteRenderer spriteRenderer;
	private bool isEnter;
	private int moduleID;

	private List <GameObject> locals; // List of all the modules within the outpost
//	private List <GameObject> connections;
	// Use this for initialization
	void Start () {
		locals = new List<GameObject>(); 

		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		isEnter = true;
		isInside = isEnter;
	}
	
	// Update is called once per frame
	void Update () {
		if (isEnter) {
			ShowInside();
		}
		else ShowOutside();
//		Debug.Log(powerInGeneral);
		if (Input.GetKeyDown(KeyCode.B))
			BFS(0);
	}

	void ShowInside () {
		// Show inside sprite of habitat module
		spriteRenderer.sprite = indoorSprite;
		spriteRenderer.sortingOrder = -3;
		// Show inside sprite of all other modules within the outpost
		foreach (GameObject local in locals)
			local.SendMessage("ShowInside");
	}

	void ShowOutside () {
		spriteRenderer.sprite = outdoorSprite;
		spriteRenderer.sortingOrder = -1;
		foreach (GameObject local in locals)
			local.SendMessage("ShowOutside");
	}

	void DoorWayTriggered () {
		isEnter = !isEnter;
		isInside = isEnter;
	}

	void ChangeLocation (bool isEnter) {
		this.isEnter = isEnter;
	}

	void AddLocal (GameObject local) {
		local.GetComponent<LocalControl>().moduleID = locals.Count;
		locals.Add(local);
	}

	void BFS(int startModuleId) {
		bool[] visited = new bool[locals.Count];
		Queue Q = new Queue();
		float powerInGeneral = 0;
		float powerConsumptionInGeneral = locals[startModuleId].GetComponent<LocalControl>().powerConsumption;
		for (int i = 0; i < locals.Count; i++)
			visited[i] = false;
		visited[startModuleId] = true;
		Q.Enqueue(startModuleId);
		Debug.Log(locals[startModuleId]);
		while (Q.Count != 0) {
			int front = (int)Q.Peek();
			Debug.Log(front);
			Q.Dequeue();
			foreach (GameObject connection in locals[front].GetComponent<LocalControl>().connections) {
				Debug.Log(connection);
				if (connection.tag == "HabitatModule") {
					powerInGeneral += 2;
				} else if (!visited[connection.GetComponent<LocalControl>().moduleID] && connection.GetComponent<LocalControl>().isOn) {
					Debug.Log(connection);
					Q.Enqueue(connection.GetComponent<LocalControl>().moduleID);
					visited[connection.GetComponent<LocalControl>().moduleID] = true;
					if (connection.GetComponent<LocalControl>().powerConsumption > 0)
						powerConsumptionInGeneral += connection.GetComponent<LocalControl>().powerConsumption;
					else 
						powerInGeneral -= connection.GetComponent<LocalControl>().powerConsumption;
				}
			}
		}
//		Debug.Log(powerConsumptionInGeneral);
//		Fix needed here 1/19
		for (int i = 0; i < locals.Count; i++)
			locals[i].GetComponent<LocalControl>().powerLevel = powerInGeneral / powerConsumptionInGeneral;

	}

	void CheckPowerSupply() {
		BFS (0);
		foreach (GameObject local in locals)
			local.SendMessage("CheckPowerSupply");
	}
}
