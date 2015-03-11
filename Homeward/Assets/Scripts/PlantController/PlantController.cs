using UnityEngine;
using System.Collections;

public class PlantController : MonoBehaviour {
    private int maxPlantHealth; // plant's lifetime
    private int currentPlantHealth;
    private int plantLifeStages; // used to change the sprite of the plant (= number of png files)
    private DayNightController dayNightController;
    public Sprite[] sprites;
    public bool playerInWaterPosition = false;

	// Use this for initialization
	void Start () {
        maxPlantHealth = 2;
        currentPlantHealth = maxPlantHealth;
        plantLifeStages = 8;
        GetComponent<SpriteRenderer>().sprite = sprites[6];
        dayNightController = GameObject.Find("DayNightController").GetComponent<DayNightController>();
	}
	
	// Update is called once per frame
	void Update () {
         ///Debug.Log(dayNightController.dayCount); // for debug
        
        if (dayNightController.dayCount >= maxPlantHealth)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[7];
        }


        /***
         * Player
         * ***/
        if (playerInWaterPosition)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Giving Water to Plant");

                if (currentPlantHealth < maxPlantHealth)
                {
                    currentPlantHealth += 2;
                }
            }
        }

	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInWaterPosition = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInWaterPosition = false;
        }
    }
}
