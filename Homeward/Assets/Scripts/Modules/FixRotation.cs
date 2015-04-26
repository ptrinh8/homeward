using UnityEngine;
using System.Collections;

public class FixRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.transform.rotation = Quaternion.EulerRotation(0, 0, 0);
	}
}
