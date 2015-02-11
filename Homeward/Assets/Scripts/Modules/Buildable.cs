﻿using UnityEngine;
using System.Collections;

// This class is for handling detached blueprint
public class Buildable : MonoBehaviour 
{
    //private ItemDatabase itemDatabase;
    //private Inventory inventory;
	public GameObject module;	// Completed module(prefabs)
	public int materialsRequired;	// Materials required to complete the module
	private int buildingProgress;	// Number of materials spend to build this module
	private SpriteRenderer spriteRenderer;	// Change of sprite color when needed
	private KeyCode buildKey = KeyCode.F;
	private KeyCode cancelKey = KeyCode.E;
	private Color color;

	// Use this for initialization
	void Start () 
    {
        //itemDatabase = FindObjectOfType(typeof(ItemDatabase)) as ItemDatabase;
        //inventory  = FindObjectOfType(typeof(Inventory)) as Inventory;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		color = spriteRenderer.color;
		color.a = 0.5f;
		spriteRenderer.color = color;
		buildingProgress = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (buildingProgress >= materialsRequired) {
			Instantiate(module, gameObject.transform.position, gameObject.transform.rotation);
			Destroy(gameObject);
		}
	}
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(cancelKey))
            Destroy(gameObject);

        if (Input.GetKeyDown(buildKey))
        {
			// Inventory code here
			if(other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().CountItems(ItemName.Material) > 0)
			{
				buildingProgress++;
				color.a += 0.4f / materialsRequired;
				spriteRenderer.color = color;
				other.gameObject.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().GetItem(ItemName.Material);
			}

        }
    }
}
