using UnityEngine;
using System.Collections;

public class MaterialController : MonoBehaviour {

    private Material material;

	// Use this for initialization
	void Start () {
        this.material = gameObject.renderer.material;
	}
	
	// Update is called once per frame
	void Update () {
        float intensity = Mathf.Sin(Time.time);

        this.material.SetFloat("_RimPower", intensity);
	}
}
