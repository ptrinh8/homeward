using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// random direction arrow generation
public class Arrow : MonoBehaviour {

	public Sprite upArrowSprite;
	public Sprite downArrowSprite;
	public Sprite leftArrowSprite;
	public Sprite rightArrowSprite;

	private int direction;
	public int Direction
	{
		get {return this.direction;}
	}

	// Use this for initialization
	void Start () {
		// 1 stands for UP; 2 stands for LEFT; 3 stands for DOWN; 4 stands for RIGHT
		direction = Random.Range(1,5);
		switch (direction)
		{
		case 1:
			gameObject.GetComponent<Image>().sprite = upArrowSprite;
			break;
		case 2:
			gameObject.GetComponent<Image>().sprite = leftArrowSprite;
			break;
		case 3:
			gameObject.GetComponent<Image>().sprite = downArrowSprite;
			break;
		case 4:
			gameObject.GetComponent<Image>().sprite = rightArrowSprite;
			break;
		}
	}
}
