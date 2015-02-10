// =======================================================================
// <file="PlayerController.cs" product="Homeward">
// <date>2014-11-12</date>
// =======================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
    private Mining minerals;
	private DayNightController dayNightController;

	public float speed;
	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	private float animateSpeed;         //Time between frames of animation
	private float animateTime;          //Variable storing the timer for the animation
	public int animateIterator;         //Variable storing the frame of the spritesheet to use
	private int animateZone;            //Specifies the direction of the astronaut's animation (up, down, left, right)
	private bool frameAscending;        //boolean to tell AnimateFrames if spritesheet animation frame is increasing
	private bool frameDescending;       //boolean to tell AnimateFrames if spritesheet animation frame is decreasing

	public float miningSpeed;	        // mining speed per sec
	public float miningTimer;	        // record mining time
	public bool miningNow, isMining;    // miningNow is the signal for mineral class
    public static bool isRepairing;
	public GameObject textFinder;

    private float zoomDuration = 1.0f;
    private float zoomElapsed = 0.0f;
    private float zoomExitDuration = 1.0f;
    private float zoomExitElapsed = 0.0f;
    private bool zoomTransition = false;

	public float health;
	public float stamina;
	private float healthTimer;
	private float staminaTimer;
	private float dayLength;
	private float nightLength;
	private float timeUntilSleepPenalty;
	public float staminaLostPerSecond;
	public float healthLostPerSecond;

	public bool canSleep;
    [HideInInspector]
    public bool isKeyEnabled = true;
    private KeyCode consumeFoodKey = KeyCode.K;

    public Canvas canvas;
    public RectTransform healthTransform;
    public RectTransform staminaTransform;
    private float healthbarPositionY, staminabarPositionY;
    private float healthbarPositionMinX, staminabarPositionMinX;
    private float healthbarPositionMaxX, staminabarPositionMaxX;
    private float currentHealth, currentStamina;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { 
            currentHealth = value;
            manageHealth();
            manageStamina();
        }
    }
    public float maxHealth, maxStamina;
    public Text healthText, staminaText;
    public Image healthImage, staminaImage;
    public float coolDown;
    private bool onCoolDown;

	[HideInInspector]
	public float x, y;

    public bool PlayerIsMiningNow
    {
        set { miningNow = value; }
        get { return miningNow; }
    }

    public static bool holdingRepairTool;
    
    public GameObject playerInventory;
    
    private static bool showPlayerInventory;

    public static bool ShowPlayerInventory
    {
        get { return showPlayerInventory; }
        set { showPlayerInventory = value; }
    }

    private static bool keyCode_I_Works;

    public static bool KeyCode_I_Works
    {
        get { return keyCode_I_Works; }
        set { keyCode_I_Works = value; }
    }

	void Start () 
	{
        minerals = FindObjectOfType(typeof(Mining)) as Mining;
		dayNightController = GameObject.Find ("DayNightController").GetComponent<DayNightController>();

		speed = 2.5f;
		animateSpeed = .1f;
		animateTime = 0f;
		frameAscending = true;
		frameDescending = false;

		miningTimer = 0;
		miningNow = false;
		isMining = false;
        isRepairing = false;
        holdingRepairTool = false;

		health = 100;
		stamina = 100f;

		canSleep = false;

        Camera.main.orthographic = true;

		dayLength = dayNightController.dayCycleLength / 2;
		nightLength = dayNightController.dayCycleLength / 2;
		timeUntilSleepPenalty = (dayLength / 10) * 8;
		staminaLostPerSecond = stamina / (dayLength + nightLength);
		healthLostPerSecond = health / ((dayLength + nightLength) * 4 / 5);

        /*** Initializing GUI variables ***/
        healthbarPositionY = healthTransform.position.y;
        healthbarPositionMaxX = healthTransform.position.x;
        healthbarPositionMinX = healthTransform.position.x - healthTransform.rect.width;
        staminabarPositionY = staminaTransform.position.y;
        staminabarPositionMaxX = staminaTransform.position.x;
        staminabarPositionMinX = staminaTransform.position.x - staminaTransform.rect.width;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        onCoolDown = false;

        /*** PlayerInventory ***/
        playerInventory = Instantiate(playerInventory) as GameObject;
        playerInventory.transform.SetParent(GameObject.Find("Canvas").transform);
        playerInventory.transform.position = new Vector3(7.0f, Screen.height - 7.0f, 0.0f); 
        showPlayerInventory = false;
        keyCode_I_Works = true;
        playerInventory.SetActive(showPlayerInventory);
        playerInventory.AddComponent<CanvasGroup>();
	}

	void Update () 
	{
		zoomInWhenIndoor();

        if (Input.GetKeyDown(consumeFoodKey) == true)
        {
            if (isKeyEnabled)
            {
            }
        }

		/*** if inside in a module turn the flag on ***/
		if (CentralControl.isInside)
		{
//			if (hold repair tool)
			{
				if (Input.GetKeyDown(KeyCode.F))
				{
					isRepairing = true;
				}
			}
		}

		if (health > 0)
		{
			staminaTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                //TODO
            }

            if (Input.GetKeyDown(KeyCode.I) == true && keyCode_I_Works)
            {
                showPlayerInventory = !showPlayerInventory;
                playerInventory.transform.position = new Vector3(9999.0f, Screen.height - 7.0f, 0.0f);
                playerInventory.GetComponent<Inventory>().SetSlotsActive(showPlayerInventory);
                if(showPlayerInventory) playerInventory.GetComponent<Inventory>().DebugShowInventory();
            }

            /****************************************************************
             * Inventory: This is how you add items to playerInventory
             * **************************************************************/
            if (showPlayerInventory) // player is able to control inventory only when inventory is visible
            {
                playerInventory.SetActive(true);
                playerInventory.GetComponent<Inventory>().SetSlotsActive(true);
                playerInventory.transform.position = new Vector3(7.0f, Screen.height - 7.0f, 0.0f);
                playerInventory.GetComponent<Inventory>().GetComponent<CanvasGroup>().alpha = 1;

        
                if (Input.GetKeyDown(KeyCode.P)) // p is temporary. Delete this once you find how to add item.
                {
                    Item item = GameObject.Find("Mineral").GetComponent<Item>();
                    playerInventory.GetComponent<Inventory>().AddItem(item);
                }

                if (Input.GetKeyDown(KeyCode.O)) // o is temporary. Delete this once you find how to add item.
                {
                    Item item = GameObject.Find("RepairingTool").GetComponent<Item>();
                    playerInventory.GetComponent<Inventory>().AddItem(item);
                }

                // get item from player inventory and put it in the inventory he is at
                if (Input.GetKeyDown(KeyCode.J)) // J is temporary. Delete this once you find how to add item.
                {
                    Item item = playerInventory.GetComponent<Inventory>().GetItem(ItemName.Mineral);
                }
            }
            if (!showPlayerInventory)
            {
                playerInventory.SetActive(true);
                playerInventory.GetComponent<Inventory>().GetComponent<CanvasGroup>().alpha = 0;
                playerInventory.GetComponent<Inventory>().SetSlotsActive(false);
            }

            /*******************************************************************
             * Inventory END
             * *****************************************************************/

            if (CentralControl.isInside)
            {
                if (holdingRepairTool)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        isRepairing = true;
                    }
                }


            }

			if (stamina <= 50)
			{
				healthTimer += Time.deltaTime;
				if (healthTimer > 1f)
				{
					if(currentHealth > 0)
					{
						StartCoroutine(CoolDownDamage());
						currentHealth -= healthLostPerSecond;
						manageHealth();
					}
					health -= healthLostPerSecond;
					healthTimer = 0;
				}
			}

			if (dayNightController.currentPhase == DayNightController.DayPhase.Night || dayNightController.currentPhase == DayNightController.DayPhase.Dusk)
			{
				if (CentralControl.isInside == false)
				{
					healthTimer += Time.deltaTime;
					if (healthTimer > nightLength / 5)
                    {
                        if(currentHealth > 0)
                        {
                            StartCoroutine(CoolDownDamage());
                            currentHealth -= healthLostPerSecond;
                            manageHealth();
                        }
						health -= 26;
						healthTimer = 0;
					}
				}
			}

			if (staminaTimer > 1f)
            {
                if (currentStamina > 0){
                    StartCoroutine(CoolDownDamage());
                    currentStamina -= staminaLostPerSecond;
                    manageStamina();
                }

                stamina -= staminaLostPerSecond;
				staminaTimer = 0;
			}

	/*        // Mining control
	        if ((isMining) && (miningTimer < miningSpeed))
	        {
	            if (playerStatus.maxMineralsHaveReached == false)
	            miningTimer += Time.deltaTime;
	        }
	        else if (miningTimer > miningSpeed)
	        {
	            miningNow = true;
	            miningTimer = 0;
	            isMining = false;
	        }
            */
			x = Input.GetAxisRaw("Horizontal");   // Input.GetAxisRaw is independent of framerate, and also gives us raw input which is better for 2D
			y = Input.GetAxisRaw("Vertical");
			Vector2 direction = new Vector2(x, y);      // storing the x and y Inputs from GetAxisRaw in a Vector2
			rigidbody2D.velocity = direction * speed;   // speed is changable by us

			//using the velocity of the character to determine which direction it's facing and which frames from the spritesheet to use for animation
			if(rigidbody2D.velocity.y > 0 || (rigidbody2D.velocity.y > 0 && rigidbody2D.velocity.x != 0))		// y > 0
			{
				AnimateFrames(1);
				this.GetComponentInChildren<SpriteRenderer>().sprite = sprites[animateIterator]; //actually drawing the sprite
			}
			else if(rigidbody2D.velocity.y < 0 || (rigidbody2D.velocity.y < 0 && rigidbody2D.velocity.x != 0))		// y < 0
			{
				AnimateFrames(0);
				this.GetComponentInChildren<SpriteRenderer>().sprite = sprites[animateIterator];	// Turn Down
			}
			else if(rigidbody2D.velocity.x > 0 || (rigidbody2D.velocity.x > 0 && rigidbody2D.velocity.y != 0))	// x > 0
			{
				AnimateFrames(3);
				this.GetComponentInChildren<SpriteRenderer>().sprite = sprites[animateIterator];
			}
			else if(rigidbody2D.velocity.x < 0 || (rigidbody2D.velocity.x > 0 && rigidbody2D.velocity.y != 0))	// x < 0
			{
				AnimateFrames(2);
				this.GetComponentInChildren<SpriteRenderer>().sprite = sprites[animateIterator];
			}
		}
	}
    
	/*---------------------------------------------------------------------------------------------------------------------------------------------------
	 * AnimateFrames takes an int that tells which "zone" or "direction" the character is facing
	 * It then tells animateIterator to iterate back and forth across the spritesheet to create a walking animation in each direction
	 * ------------------------------------------------------------------------------------------------------------------------------------------------*/
	void AnimateFrames(int animateZoneNumber)
	{
		animateTime += Time.deltaTime; //add game clock time to our timer
		if (animateTime >= animateSpeed) //if our timer has passed the time to switch frames
		{
			if (animateZoneNumber == 0)
			{
				if (animateIterator == 0) //bottom of iterator zone, should ascend
				{
					frameAscending = true;
					frameDescending = false;
				}
				else if (animateIterator == 2) // top of iterator zone, should descend
				{
					frameAscending = false;
					frameDescending = true;
				}

				if (animateIterator > 2) // when switching directions, if our animation is facing one direction make sure it switches to the proper direction
				{
					animateIterator = 1;
				}
				else if (frameAscending == true && frameDescending == false) //ascend if frameascending is true
				{
					animateIterator++;
				}
				else if (frameAscending == false && frameDescending == true) //descend if framedescending is true
				{
					animateIterator--;
				}
			}
			else if (animateZoneNumber == 1)
			{
				if (animateIterator == 3)
				{
					frameAscending = true;
					frameDescending = false;
				}
				else if (animateIterator == 5)
				{
					frameAscending = false;
					frameDescending = true;
				}
				
				if (animateIterator < 3 || animateIterator > 5)
				{
					animateIterator = 4;
				}
				else if (frameAscending == true && frameDescending == false)
				{
					animateIterator++;
				}
				else if (frameAscending == false && frameDescending == true)
				{
					animateIterator--;
				}
			}
			else if (animateZoneNumber == 2)
			{
				if (animateIterator == 6)
				{
					frameAscending = true;
					frameDescending = false;
				}
				else if (animateIterator == 8)
				{
					frameAscending = false;
					frameDescending = true;
				}
				
				if (animateIterator < 6 || animateIterator > 8)
				{
					animateIterator = 7;
				}
				else if (frameAscending == true && frameDescending == false)
				{
					animateIterator++;
				}
				else if (frameAscending == false && frameDescending == true)
				{
					animateIterator--;
				}
			}
			else if (animateZoneNumber == 3)
			{
				if (animateIterator == 9)
				{
					frameAscending = true;
					frameDescending = false;
				}
				else if (animateIterator == 11)
				{
					frameAscending = false;
					frameDescending = true;
				}
				
				if (animateIterator < 9 || animateIterator > 11)
				{
					animateIterator = 10;
				}
				else if (frameAscending == true && frameDescending == false)
				{
					animateIterator++;
				}
				else if (frameAscending == false && frameDescending == true)
				{
					animateIterator--;
				}
			}
			animateTime = 0; //reset timer after animation time has passed
		}
	}

    void zoomInWhenOnBase()
    {
        GameObject shipGameObject = GameObject.Find("Base");
        int shipPosX = (int)shipGameObject.GetComponentInChildren<SpriteRenderer>().transform.position.x;
        int shipPosY = (int)shipGameObject.GetComponentInChildren<SpriteRenderer>().transform.position.y;

        GameObject mainPlayerGameObject = GameObject.Find("MainPlayer");
        var mainPlayerPos = mainPlayerGameObject.transform.position;
        int playerPosY = (int)mainPlayerPos.y;
        int playerPosX = (int)mainPlayerPos.x;

        // If inside the falcon's boundaries, set zoomTransition = true
        if ((playerPosX > (shipPosX - 1.5)) && (playerPosX < (shipPosX + 1.5)))
        {
            if ((playerPosY > (shipPosY - 1.5)) && (playerPosY < (shipPosY + 1.5)))
            {
                zoomTransition = true;
            }
        }

        if (zoomTransition)
        {
            // Variables used in 'else' condition below
            zoomExitDuration = 1.0f;
            zoomExitElapsed = 0.0f;

            zoomElapsed += Time.deltaTime / zoomDuration;
            Camera.main.orthographicSize = Mathf.Lerp(5, 2, zoomElapsed);
            zoomTransition = false;
        }
        else 
        {
            // Variables used in 'if' condition above
            zoomDuration = 1.0f;
            zoomElapsed = 0.0f;

            zoomExitElapsed += Time.deltaTime / zoomExitDuration;
            if (Time.timeSinceLevelLoad < 1f)
            {
                Camera.main.orthographicSize = Mathf.Lerp(5, 5, zoomExitElapsed);
            }
            else
            {
                Camera.main.orthographicSize = Mathf.Lerp(2, 5, zoomExitElapsed);
            }
        }
    }

	void zoomInWhenIndoor()
	{
		GameObject mainPlayerGameObject = GameObject.Find("MainPlayer");
		var mainPlayerPos = mainPlayerGameObject.transform.position;
		int playerPosY = (int)mainPlayerPos.y;
		int playerPosX = (int)mainPlayerPos.x;
		
		// If inside the falcon's boundaries, set zoomTransition = true
		zoomTransition = CentralControl.isInside;
		
		if (zoomTransition)
		{
			// Variables used in 'else' condition below
			zoomExitDuration = 1.0f;
			zoomExitElapsed = 0.0f;
			
			zoomElapsed += Time.deltaTime / zoomDuration;
			Camera.main.orthographicSize = Mathf.Lerp(5, 2, zoomElapsed);
			zoomTransition = false;
		}
		else 
		{
			// Variables used in 'if' condition above
			zoomDuration = 1.0f;
			zoomElapsed = 0.0f;
			
			zoomExitElapsed += Time.deltaTime / zoomExitDuration;
			if (Time.timeSinceLevelLoad < 1f)
			{
				Camera.main.orthographicSize = Mathf.Lerp(5, 5, zoomExitElapsed);
			}
			else
			{
				Camera.main.orthographicSize = Mathf.Lerp(2, 5, zoomExitElapsed);
			}
		}
	}

    private float mapValues(float currentX, float inMin, float inMax, float minX, float maxX)
    {
        return (currentX - inMin) * (maxX - minX) / (inMax - inMin) + minX;
    }

    private void manageHealth()
    {
        healthText.text = "Health:  " + currentHealth;
        float currentXHealth = mapValues(currentHealth, 0, maxHealth, healthbarPositionMinX, healthbarPositionMaxX);
        
        healthTransform.position = new Vector3(currentXHealth, healthbarPositionY);

        if (currentHealth > maxHealth / 2)
        {
            healthImage.color = new Color32((byte)mapValues(currentHealth, maxHealth / 2, maxHealth, 255, 0), 255, 0, 255);
        }
        else
        {
            healthImage.color = new Color32(255, (byte)mapValues(currentHealth, 0, maxHealth / 2, 0, 255), 0, 255);
        }
    }

    private void manageStamina()
    {
        staminaText.text = "Stamina:    " + currentStamina;

        //Debug.Log("staminabarPositionY: "+staminabarPositionY+", maxstamina: " + maxStamina + ", staminaMinX: " + staminabarPositionMinX + ", staminaMaxX: " + staminabarPositionMaxX);


        float currentXStamina = mapValues(currentStamina, 0, maxStamina, staminabarPositionMinX, staminabarPositionMaxX);
        staminaTransform.position = new Vector3(currentXStamina, staminabarPositionY);

        if (currentStamina > maxStamina / 2)
        {
            staminaImage.color = new Color32((byte)mapValues(currentStamina, maxStamina / 2, maxStamina, 255, 0), 255, 0, 255);
        }
        else
        {
            staminaImage.color = new Color32(255, (byte)mapValues(currentStamina, 0, maxStamina / 2, 0, 255), 0, 255);
        }
    }

    IEnumerator CoolDownDamage()
    {
        onCoolDown = true;
        yield return new WaitForSeconds(coolDown);
        onCoolDown = false;
    }
}