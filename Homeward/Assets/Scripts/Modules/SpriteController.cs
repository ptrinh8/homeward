using UnityEngine;
using System.Collections;

public class SpriteController : MonoBehaviour {

	public static bool isEnter;

	public Sprite indoorSprite;
	public Sprite outdoorSprite;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
		if (isEnter) {
			spriteRenderer.sprite = indoorSprite;
			spriteRenderer.sortingOrder = -3;
		} else {
			spriteRenderer.sprite = outdoorSprite;
			spriteRenderer.sortingOrder = -1;
		}
	}
}
