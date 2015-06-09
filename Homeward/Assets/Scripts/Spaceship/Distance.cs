using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Distance : MonoBehaviour {

	// Update is called once per frame
	void Update () {

		gameObject.GetComponent<Text>().text = Mathf.Round(Vector3.Distance(GameObject.FindWithTag("Player").transform.position, GameObject.Find("Spaceship").transform.position)).ToString();
	}
}
