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
//		Debug.Log(spriteRenderer.sortingLayerID);
		if (isEnter) {
			spriteRenderer.sprite = indoorSprite;
			spriteRenderer.sortingOrder = -2;
		} else {
			spriteRenderer.sprite = outdoorSprite;
			spriteRenderer.sortingOrder = -1;
		}
	}
}
