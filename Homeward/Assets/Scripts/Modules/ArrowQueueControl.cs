using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ArrowQueueControl : MonoBehaviour {

	public GameObject arrow;
	private List<GameObject> arrows;
	private bool correctInput;
	private Color alphaPointFive;
	private int arrowsCount, currentPosition;
	private int currentInput;// 1 stands for UP; 2 stands for LEFT; 3 stands for DOWN; 4 stands for RIGHT

	public bool CorrectInput
	{
		get {return this.correctInput;}
		set {this.correctInput = value;}
	}

	void Awake () {
		alphaPointFive = Color.white;
		alphaPointFive.a = 0.5f;
	}
	// Use this for initialization
	void Start () {
		Reset();
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = GameObject.Find("MainPlayer").transform.position + new Vector3(10, 0, 0);
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			currentInput = 1;
		} 
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			currentInput = 2;
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			currentInput = 3;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			currentInput = 4;
		}
		else
		{
			currentInput = -1;
		}

		if (currentInput > 0) 
		{
			if (currentPosition < arrows.Count)
			{
				if (currentInput == arrows[currentPosition].GetComponent<Arrow>().Direction)
				{
					// current input correct
					arrows[currentPosition].GetComponent<Image>().color = Color.white;
					currentPosition ++;
					if (currentPosition >= arrows.Count)
					{
						// all input correct
						foreach (GameObject arr in arrows)
						{
							arr.GetComponent<Image>().color = Color.green;
						}
						correctInput = true;
					}
				}
				else 
				{
					// current input not correct
					arrows[currentPosition].GetComponent<Image>().color = Color.red;
					// reset arrows
					Invoke("Reset", 0.5f);
				}
			}
		}
	}

	void Reset () {
		GameObject tempArrowGameobject = arrow;
		foreach(Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		arrows = new List<GameObject>();
		arrowsCount = Random.Range(4,7);
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(arrowsCount * 50, 1 * 50);
		//gameObject.GetComponent<RectTransform>().rect.width = arrowsCount;
		currentPosition = 0;
		currentInput = 0;
		correctInput = false;
		for (int i = 0; i < arrowsCount; i++)
		{
			tempArrowGameobject = arrow;
			tempArrowGameobject = Instantiate(tempArrowGameobject) as GameObject;
			tempArrowGameobject.transform.parent = gameObject.transform;
			tempArrowGameobject.GetComponent<RectTransform>().localPosition = new Vector2((float)i * 50f - (arrowsCount * 50) / 2 + 25, 0);
			tempArrowGameobject.GetComponent<Image>().color = alphaPointFive;
			arrows.Add(tempArrowGameobject);
		}
	}
}
