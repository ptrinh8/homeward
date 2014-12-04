using UnityEngine;
using System.Collections;

public class Enterable : MonoBehaviour {
	public static bool isEnter;
	private GameObject mainPlayer;
	private PlayerController playerC;
//	private float x, y;

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponentInParent<Animator>();
		Enterable.isEnter = true;
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool("isEnter", Enterable.isEnter);
		Debug.Log(Enterable.isEnter);
	}

	void OnTriggerEnter2D (Collider2D other) {

	}

	void OnTriggerExit2D (Collider2D other) {

			Enterable.isEnter = !Enterable.isEnter;
	//		anim.SetBool("isEnter", isEnter);
		/** 
		 * Lagcy Code (Incase new code dosen't work)
		 * 
		int rotation = (int)transform.rotation.eulerAngles.z;
		if (rotation == 90 || rotation == 270)
			if (playerC.y == y) {
			isEnter = !isEnter;
			anim.SetBool("isEnter", isEnter);
			}
		if (rotation == 0 || rotation == 180)
			if (playerC.x == x) {
			isEnter = !isEnter;
			anim.SetBool("isEnter", isEnter);
		**/

	}	
}
