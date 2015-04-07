using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Color deployingColor, originalColor;

	// Use this for initialization
	void Start () {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        deployingColor = new Color(.35f, 1, .24f, .5f);
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<SpriteRenderer>().material = transform.root.gameObject.GetComponent<SpriteRenderer>().material;

        if (Building.isDeploying) 
        {
            if (spriteRenderer.color != deployingColor)
                spriteRenderer.color = deployingColor;
        } 
        else if (spriteRenderer.color != originalColor)
            spriteRenderer.color = originalColor;
	}
}
