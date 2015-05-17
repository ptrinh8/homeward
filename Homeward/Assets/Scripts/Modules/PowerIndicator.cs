using UnityEngine;
using System.Collections;

public class PowerIndicator : MonoBehaviour {

	public Sprite[] indicatorSprites;
	private int powerIndicator;

	// Use this for initialization
	void Start () {
		powerIndicator = transform.root.gameObject.GetComponent<LocalControl>().PowerIndicator;
	}
	
	// Update is called once per frame
	void Update () {
		powerIndicator = transform.root.gameObject.GetComponent<LocalControl>().PowerIndicator;
		switch (powerIndicator) 
		{
			case 0:
				gameObject.GetComponent<SpriteRenderer>().sprite = indicatorSprites[0];
				break;
			case 1:
				gameObject.GetComponent<SpriteRenderer>().sprite = indicatorSprites[1];
				break;
			case 2:
				gameObject.GetComponent<SpriteRenderer>().sprite = indicatorSprites[2];
				break;
			case 3:
				gameObject.GetComponent<SpriteRenderer>().sprite = indicatorSprites[3];
				break;
			case 4:
				gameObject.GetComponent<SpriteRenderer>().sprite = indicatorSprites[4];
				break;
			case 5:
				gameObject.GetComponent<SpriteRenderer>().sprite = indicatorSprites[5];
				break;
			default:
				gameObject.GetComponent<SpriteRenderer>().sprite = indicatorSprites[5];
				gameObject.GetComponent<SpriteRenderer>().color = Color.green;
				break;
		}
	}
}
