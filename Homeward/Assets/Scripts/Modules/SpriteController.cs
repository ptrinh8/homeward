using UnityEngine;
using System.Collections;

public class SpriteController : MonoBehaviour {

	public static bool isEnter;

	public Sprite indoorSprite;
	public Sprite outdoorSprite;
	private SpriteRenderer spriteRenderer;
//	private bool taylor;

	// Use this for initialization
	void Start () {
//		isEnter = false;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
//		taylor = isEnter;

	}
	
	// Update is called once per frame
	void Update () {
		if (isEnter) spriteRenderer.sprite = indoorSprite;
		else spriteRenderer.sprite = outdoorSprite;
	}
}
