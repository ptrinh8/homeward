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
    private Spaceship spaceship;
    private EndGame endgame;

    public float speed;
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private float animateSpeed;         //Time between frames of animation
    private float animateTime;          //Variable storing the timer for the animation
    public int animateIterator;         //Variable storing the frame of the spritesheet to use
    private int animateZone;            //Specifies the direction of the astronaut's animation (up, down, left, right)
    private bool frameAscending;        //boolean to tell AnimateFrames if spritesheet animation frame is increasing
    private bool frameDescending;       //boolean to tell AnimateFrames if spritesheet animation frame is decreasing
    private int leftRightFootstep = 0;

    public bool isDead = false;
    private float deadTimer;
    private float deadLength;

    public float miningCooldown;	        // mining speed per sec
    public float miningTimer;	        // record mining time
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
    public float healthLostPerSecondNight;
	private bool hasEaten;
	private float eatingTimer;
	private float eatingTime;

	private float oxygen;
	private float oxygenDEC = 1F;
	private float oxygenTimer;
	private float oxygenLossTime = 1f;
	private float oxygenLossAmount;
	private float oxygenHealthTimer;
	private float oxygenHealthLossTime = 1f;
	private float oxygenHealthLossAmount = .5f;


    public bool canSleep;
    [HideInInspector]
    public bool isKeyEnabled = true;
    private KeyCode consumeFoodKey = KeyCode.K;

    public Canvas canvas;
    private float currentHealth, currentStamina;

    // Taylor
    public float CurrentStamina
    {
        get
        {
            return currentStamina;
        }
        set
        {
            currentStamina = value;
        }
    }
    // Taylor End

    //AUDIO STUFF
    private AudioController audioController;
    private int songSelected;
    private float songLength;
    public float songTimer;
    private float songSilenceLength;
    public float songSilenceTimer;
    public bool isSongPlaying;
    public bool isSongQueued;

    //sleeping stuff
    private float sleepTimePassed;
    private LocalControl[] allLocalControl;
    private CentralControl[] allCentralControl;
    public int durabilityLossAmount;
    public GUITexture sleepTexture;
    private float sleepFadeOutLength;
    public bool isSleeping;
    private Color sleepColor;
    private Color originalColor;
    public Color tempColor;
    private float sleepTimer;
    private bool slept;

    public float currentXStamina;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            manageHealth();
            manageStamina();
        }
    }
    public float maxHealth, maxStamina;
    public Text healthText, staminaText, oxygenText;
    public Image healthImage, staminaImage;
    public float coolDown;
    private bool onCoolDown;

    [HideInInspector]
    public float x, y;

    public static bool holdingRepairTool;
    public static bool holdingMiningTool;
    public static bool holdingBuildingTool;

	public bool holdingRepairToolCheck;
	public bool holdingMiningToolCheck;
	public bool holdingBuildingToolCheck;

    public GameObject playerInventory;

    public GameObject moduleSelection;

    public GameObject moduleDescription;

    public static bool showModuleSelection;

    public static bool showPlayerInventory;

    private bool firstSongPlayed;
    // Taylor
    public static bool toolUsingEnable = true;

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

    private static bool keyCode_B_Works;

    /*** Tool Box UI ***/
    public Image toolBoxUIImage;
    public Sprite repairToolSprite;
    public Sprite miningToolSprite;
    public Sprite buildingToolSprite;
    public Sprite defaultSprite;

	public Mining nearestMineral;

    private GameObject UIHealthBar;
    private GameObject UIStaminaBar;
    private GameObject UIOxygenBar;
    private GameObject UIClock;
    private GameObject RadarCamera;
    private GameObject ToolBoxObject;

    //Taylor
    private bool environmentAir;

    public bool EnvironmentalAir
    {
        get
        {
            return environmentAir;
        }
        set
        {
            environmentAir = value;
        }
    }
    // Taylor End 

    private void EndDemo()
    {
        if (spaceship.playerInsideSpaceship == true)
        {
            spaceship.DemoEnds = true;
        }
        if (spaceship.endDemo == true)
        {
            rigidbody2D.velocity = new Vector2(0F, 0);
            endgame.EndScene();
        }
    }

	private void ManageOxygenLevels()
	{
		oxygenTimer += Time.deltaTime;
		if (CentralControl.isInside) { /* What happens to oxygen when the player is inside the base? */ }
		else if (oxygenTimer >= oxygenLossTime){ 

			oxygen -= oxygenLossAmount; 
			oxygenTimer = 0;
		
		}
		if (oxygen < 0.0F) { oxygen = 0.0F; }
		if (oxygen > 100.0F) { oxygen = 100.0F; }
		oxygenText.text = "Oxygen: " + (int)oxygen;
		GameObject oxygenBar = GameObject.Find("OxygenBar");
		Image oxygenBarImage = oxygenBar.GetComponent<Image>();
		oxygenBarImage.fillAmount = (float)oxygen / 100.0F;
	}


    void Start()
    {
        minerals = FindObjectOfType(typeof(Mining)) as Mining;
        spaceship = FindObjectOfType(typeof(Spaceship)) as Spaceship;
        endgame = FindObjectOfType(typeof(EndGame)) as EndGame;
        dayNightController = GameObject.Find("DayNightController").GetComponent<DayNightController>();

        speed = 1.0f;
        animateSpeed = .15f;
        animateTime = 0f;
        frameAscending = true;
        frameDescending = false;

        miningTimer = 0;
        isRepairing = false;
        holdingRepairTool = false;
        holdingMiningTool = false;
        holdingBuildingTool = false;

        health = 100;
        stamina = 100f;
		oxygen = 100.0f;

        UIHealthBar = GameObject.Find("Health Bar Background");
        UIHealthBar.AddComponent<CanvasGroup>();
        UIStaminaBar = GameObject.Find("Stamina Bar Background");
        UIStaminaBar.AddComponent<CanvasGroup>();
        UIOxygenBar = GameObject.Find("Oxygen Bar Background");
        UIOxygenBar.AddComponent<CanvasGroup>();
        UIClock = GameObject.Find("Clock");
        UIClock.AddComponent<CanvasGroup>();
        RadarCamera = GameObject.Find("RadarCamera");
        ToolBoxObject = GameObject.Find("ToolBoxImage");
        ToolBoxObject.AddComponent<CanvasGroup>();

        canSleep = false;

        Camera.main.orthographic = true;

        dayLength = dayNightController.dayCycleLength / 2;
        nightLength = dayNightController.dayCycleLength / 2;
        timeUntilSleepPenalty = (dayLength / 10) * 8;
        staminaLostPerSecond = stamina / (dayLength + nightLength);
		oxygenLossAmount = oxygen / (dayLength + nightLength) * 2;
        healthLostPerSecond = health / ((dayLength + nightLength) * 4 / 5);
        healthLostPerSecondNight = 5f;
		eatingTime = dayLength / 4f;

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
        playerInventory.AddComponent<UIInventory>();

        /*** Module Selection ***/
        keyCode_B_Works = true;
        moduleSelection = Instantiate(moduleSelection) as GameObject;
        moduleSelection.transform.SetParent(GameObject.Find("Canvas").transform);
        moduleSelection.transform.position = new Vector3(250.0f, Screen.height - 100.0f, 0.0f);
        showModuleSelection = false;
        moduleSelection.AddComponent<CanvasGroup>();
        moduleSelection.GetComponent<CanvasGroup>().alpha = 0;
        moduleSelection.GetComponent<ModuleSelection>().SetModuleSlotsActive(showModuleSelection);
        moduleSelection.AddComponent<UIModuleSelection>();

        moduleDescription = Instantiate(moduleDescription) as GameObject;
        moduleDescription.transform.SetParent(moduleSelection.transform);
        moduleDescription.transform.localPosition = - new Vector3(0.0f, 80.0f);
        RectTransform moduleDescriptionRect = moduleDescription.GetComponent<RectTransform>();
        moduleDescriptionRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, moduleSelection.GetComponent<ModuleSelection>().moduleSelectionWidth);
        moduleDescriptionRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, moduleSelection.GetComponent<ModuleSelection>().moduleSelectionHeight);
        moduleDescription.AddComponent<CanvasGroup>();
        moduleDescription.GetComponent<CanvasGroup>().alpha = 0;

        songLength = 120f;
        songSilenceLength = 120f;
        songTimer = 0f;
        songSilenceTimer = 0f;
        isSongQueued = false;
        isSongPlaying = false;
        audioController = GameObject.Find("AudioObject").GetComponent<AudioController>();
        firstSongPlayed = false;

        sleepFadeOutLength = 2f;
        isSleeping = false;
        sleepTexture = GameObject.Find("EndScreenFader").GetComponent<GUITexture>();
        originalColor = sleepTexture.color;
        sleepColor = new Color(0f, 0f, 0f, 1f);
        slept = false;
		
		deadTimer = 0f;
		deadLength = 3f;
		
		miningCooldown = 1f;

        // Taylor
        environmentAir = true;
    }

    void Update()
    {
        zoomInWhenIndoor();
        EndDemo();
        ManageOxygenLevels();

        if (CentralControl.healthStaminaModuleExists)
        {
            UIHealthBar.GetComponent<CanvasGroup>().alpha = 1;
            UIStaminaBar.GetComponent<CanvasGroup>().alpha = 1;
            UIOxygenBar.GetComponent<CanvasGroup>().alpha = 1;
            UIClock.GetComponent<CanvasGroup>().alpha = 1;
            RadarCamera.camera.cullingMask |= (1<<LayerMask.NameToLayer("Radar"));
        }
        else
        {
            UIHealthBar.GetComponent<CanvasGroup>().alpha = 0;
            UIStaminaBar.GetComponent<CanvasGroup>().alpha = 0;
            UIOxygenBar.GetComponent<CanvasGroup>().alpha = 0;
            UIClock.GetComponent<CanvasGroup>().alpha = 0;
            RadarCamera.camera.cullingMask &= ~(1<<LayerMask.NameToLayer("Radar"));
        }

		holdingBuildingToolCheck = holdingBuildingTool;
		holdingRepairToolCheck = holdingRepairTool;
		holdingMiningToolCheck = holdingMiningTool;

        if (holdingRepairTool)
        {
            toolBoxUIImage.sprite = repairToolSprite;
        }
        else if (holdingMiningTool)
        {
            toolBoxUIImage.sprite = miningToolSprite;
        }
        else if (holdingBuildingTool)
        {
            toolBoxUIImage.sprite = buildingToolSprite;
        }
        else
        {
            toolBoxUIImage.sprite = defaultSprite;
        }

        if (Input.GetKeyDown(consumeFoodKey) == true)
        {
            if (isKeyEnabled)
            {
            }
        }

        if (isSongQueued == true)
        {
            songSilenceTimer += Time.deltaTime;
            if (songSilenceTimer > songSilenceLength)
            {
                //Debug.Log("silence over");
                if (isSongPlaying == false)
                {
                    audioController.MusicControl(1, songSelected);
                    isSongPlaying = true;
                }
            }
        }
        else if (isSongQueued == false)
        {
            // Debug.Log ("starting coroutine");
            StartCoroutine(MusicTrigger());
        }

        if (isSongPlaying == true)
        {
            songTimer += Time.deltaTime;
            if (songTimer > songLength)
            {
                audioController.MusicControl(2, songSelected);
                songTimer = 0;
                songSilenceTimer = 0;
                isSongPlaying = false;
                isSongQueued = false;
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

        if (currentHealth <= 0)
        {
            isDead = true;
        }

        

        if (currentHealth > 0 && isDead == false)
        {
            if (isSleeping == false)
            {
                staminaTimer += Time.deltaTime;
				miningTimer += Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    //TODO
                }

                if (Input.GetKeyDown(KeyCode.B) && keyCode_B_Works)
                {
                    showModuleSelection = !showModuleSelection;
                    moduleSelection.GetComponent<ModuleSelection>().SetModuleSlotsActive(showModuleSelection);
                }

                if (Input.GetKeyDown(KeyCode.I) && keyCode_I_Works)
                {
                    showPlayerInventory = !showPlayerInventory;
                    playerInventory.GetComponent<Inventory>().SetSlotsActive(showPlayerInventory);
                }

				if (miningTimer >= miningCooldown)
				{
					if (Input.GetKeyDown(KeyCode.F))
					{
						if (holdingMiningTool == true)
						{
							if (nearestMineral != null)
							{
								nearestMineral.Mine();
								miningTimer = 0;
							}
						}
					}
				}

                /****************************************************************
                 * Inventory: This is how you add items to playerInventory
                 * **************************************************************/
                if (showPlayerInventory) // player is able to control inventory only when inventory is visible
                {
                    keyCode_B_Works = false;
                    playerInventory.SetActive(true);
                    playerInventory.GetComponent<Inventory>().SetSlotsActive(true);
                    playerInventory.GetComponent<Inventory>().GetComponent<CanvasGroup>().alpha = 1;
                    ToolBoxObject.GetComponent<CanvasGroup>().alpha = 1;

                    if (Input.GetKeyDown(KeyCode.P)) // p is temporary. Delete this once you find how to add item.
                    {
                        Item item = GameObject.Find("Mineral").GetComponent<Item>();
                        playerInventory.GetComponent<Inventory>().AddItem(item);
                        item = GameObject.Find("Material").GetComponent<Item>();
                        playerInventory.GetComponent<Inventory>().AddItem(item);
                        item = GameObject.Find("Food1").GetComponent<Item>();
                        playerInventory.GetComponent<Inventory>().AddItem(item);
                        playerInventory.GetComponent<Inventory>().DebugShowInventory();
                    }

                    if (Input.GetKeyDown(KeyCode.O)) // o is temporary. Delete this once you find how to add item.
                    {
                        Item item = GameObject.Find("BuildingTool").GetComponent<Item>();
                        playerInventory.GetComponent<Inventory>().AddItem(item);
                        item = GameObject.Find("MiningTool").GetComponent<Item>();
                        playerInventory.GetComponent<Inventory>().AddItem(item);
                        item = GameObject.Find("RepairingTool").GetComponent<Item>();
                        playerInventory.GetComponent<Inventory>().AddItem(item);
                    }

                    if (Input.GetKeyDown(KeyCode.Y))
                    {
                        if (holdingBuildingTool == true)
                        {
                            Item item = GameObject.Find("BuildingTool").GetComponent<Item>();
                            playerInventory.GetComponent<Inventory>().AddItem(item);
                        }
                        else if (holdingMiningTool == true)
                        {
                            Item item = GameObject.Find("MiningTool").GetComponent<Item>();
                            playerInventory.GetComponent<Inventory>().AddItem(item);
                        }
                        else if (holdingRepairTool == true)
                        {
                            Item item = GameObject.Find("RepairingTool").GetComponent<Item>();
                            playerInventory.GetComponent<Inventory>().AddItem(item);
                        }
                        holdingBuildingTool = false;
                        holdingMiningTool = false;
                        holdingRepairTool = false;
                    }
                }
                else
                {
                    keyCode_B_Works = true;
                    playerInventory.SetActive(true);
                    playerInventory.GetComponent<Inventory>().GetComponent<CanvasGroup>().alpha = 0;
                    playerInventory.GetComponent<Inventory>().SetSlotsActive(false);
                    ToolBoxObject.GetComponent<CanvasGroup>().alpha = 0;
                }
                /*******************************************************************
                 * Inventory END
                 * *****************************************************************/

                /****************************************************************
                * ModuleSelection
                * **************************************************************/
                if (showModuleSelection)
                {
                    keyCode_I_Works = false;
                    moduleSelection.GetComponent<CanvasGroup>().alpha = 1;
                    moduleDescription.GetComponent<CanvasGroup>().alpha = 1;
                }
                else
                {
                    keyCode_I_Works = true;
                    moduleSelection.GetComponent<CanvasGroup>().alpha = 0;
                    moduleDescription.GetComponent<CanvasGroup>().alpha = 0;
                }
                /****************************************************************
                 * ModuleSelection END
                 * **************************************************************/

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

                if (currentStamina <= 50)
                {
                    healthTimer += Time.deltaTime;
                    if (healthTimer > 1f)
                    {
                        if (currentHealth > 0)
                        {
                            StartCoroutine(CoolDownDamage());
                            currentHealth -= healthLostPerSecond;
                            manageHealth();
                        }
                        health -= healthLostPerSecond;
                        healthTimer = 0;
                    }
                }

				if (oxygen <= 50)
				{
					oxygenHealthTimer += Time.deltaTime;
					if (oxygenHealthTimer > 1f)
					{
						if (currentHealth > 0)
						{
							StartCoroutine(CoolDownDamage());
							currentHealth -= oxygenHealthLossAmount;
							manageHealth();
						} 
						oxygenHealthTimer = 0;
					}
				}

                if (dayNightController.currentPhase == DayNightController.DayPhase.Night)
                {
                    if (CentralControl.isInside == false)
                    {
                        healthTimer += Time.deltaTime;
                        if (healthTimer > 1f)
                        {
                            if (currentHealth > 0)
                            {
                                StartCoroutine(CoolDownDamage());
                                currentHealth -= healthLostPerSecondNight;
                                manageHealth();
                            }
                            healthTimer = 0;
                        }
                    }
                }

				if (hasEaten == true)
				{
					eatingTimer += Time.deltaTime;
					if (eatingTimer >= eatingTime)
					{
						hasEaten = false;
						eatingTimer = 0;
					}
				}

                if (staminaTimer > 1f && hasEaten == false)
                {
                    if (currentStamina > 0)
                    {
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
                x = y = 0.0f;
                Vector2 direction = new Vector2(x, y);      // storing the x and y Inputs from GetAxisRaw in a Vector2


                if (showPlayerInventory || showModuleSelection)
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        x = -1.0f;
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        x = 1.0f;
                    }
                    if (Input.GetKey(KeyCode.W))
                    {
                        y = 1.0f;
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        y = -1.0f;
                    }
                }
                else
                {
                    x = Input.GetAxisRaw("Horizontal");   // Input.GetAxisRaw is independent of framerate, and also gives us raw input which is better for 2D
                    y = Input.GetAxisRaw("Vertical");
                }

                direction = new Vector2(x, y);      // storing the x and y Inputs from GetAxisRaw in a Vector2
                rigidbody2D.velocity = direction * speed;   // speed is changable by us

                //using the velocity of the character to determine which direction it's facing and which frames from the spritesheet to use for animation
                if (rigidbody2D.velocity.y > 0 || (rigidbody2D.velocity.y > 0 && rigidbody2D.velocity.x != 0))		// y > 0
                {
                    AnimateFrames(1);
                    this.GetComponentInChildren<SpriteRenderer>().sprite = sprites[animateIterator]; //actually drawing the sprite
                }
                else if (rigidbody2D.velocity.y < 0 || (rigidbody2D.velocity.y < 0 && rigidbody2D.velocity.x != 0))		// y < 0
                {
                    AnimateFrames(0);
                    this.GetComponentInChildren<SpriteRenderer>().sprite = sprites[animateIterator];	// Turn Down
                }
                else if (rigidbody2D.velocity.x > 0 || (rigidbody2D.velocity.x > 0 && rigidbody2D.velocity.y != 0))	// x > 0
                {
                    AnimateFrames(3);
                    this.GetComponentInChildren<SpriteRenderer>().sprite = sprites[animateIterator];
                }
                else if (rigidbody2D.velocity.x < 0 || (rigidbody2D.velocity.x > 0 && rigidbody2D.velocity.y != 0))	// x < 0
                {
                    AnimateFrames(2);
                    this.GetComponentInChildren<SpriteRenderer>().sprite = sprites[animateIterator];
                }
            }


            if (canSleep == true)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    isSleeping = true;
                    sleepTexture.enabled = true;
                }
            }

            if (isSleeping == true && slept == false)
            {
                sleepTimer += Time.deltaTime;
                if (sleepTimer > sleepFadeOutLength)
                {
                    Sleep();
                    slept = true;
                }
                tempColor = Color.Lerp(Color.clear, Color.black, sleepTimer);
                sleepTexture.color = tempColor;
            }
            else if (isSleeping == true && slept == true)
            {
                sleepTimer -= Time.deltaTime;
                if (sleepTimer < 0f)
                {
                    isSleeping = false;
                    slept = false;
                }
                tempColor = Color.Lerp(Color.clear, Color.black, sleepTimer);
                sleepTexture.color = tempColor;
            }
            else if (isSleeping == false)
            {
                sleepTexture.enabled = false;
                sleepTexture.color = originalColor;
            }
        }

        if (isDead == true)
        {
            sleepTexture.enabled = true;
            deadTimer += Time.deltaTime;
            tempColor = Color.Lerp(Color.clear, Color.black, deadTimer);
            sleepTexture.color = tempColor;
            if (deadTimer > deadLength)
            {
                Application.LoadLevel(0);
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
                    //audioController.PlayFootstep(0);
                    frameAscending = true;
                    frameDescending = false;
                }
                else if (animateIterator == 1)
                {
                    if (leftRightFootstep == 0)
                    {
                        audioController.PlayFootstep(0);
                        leftRightFootstep = 1;
                    }
                    else
                    {
                        audioController.PlayFootstep(1);
                        leftRightFootstep = 0;
                    }
                }
                else if (animateIterator == 2) // top of iterator zone, should descend
                {
                    //audioController.PlayFootstep(1);
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
                    //audioController.PlayFootstep(0);
                    frameAscending = true;
                    frameDescending = false;
                }
                else if (animateIterator == 4)
                {
                    if (leftRightFootstep == 0)
                    {
                        audioController.PlayFootstep(0);
                        leftRightFootstep = 1;
                    }
                    else
                    {
                        audioController.PlayFootstep(1);
                        leftRightFootstep = 0;
                    }
                }
                else if (animateIterator == 5)
                {
                    //audioController.PlayFootstep(1);
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
                    //audioController.PlayFootstep(0);
                    frameAscending = true;
                    frameDescending = false;
                }
                else if (animateIterator == 7)
                {
                    if (leftRightFootstep == 0)
                    {
                        audioController.PlayFootstep(0);
                        leftRightFootstep = 1;
                    }
                    else
                    {
                        audioController.PlayFootstep(1);
                        leftRightFootstep = 0;
                    }
                }
                else if (animateIterator == 8)
                {
                    //audioController.PlayFootstep(1);
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
                    //audioController.PlayFootstep(0);
                    frameAscending = true;
                    frameDescending = false;
                }
                else if (animateIterator == 10)
                {
                    if (leftRightFootstep == 0)
                    {
                        audioController.PlayFootstep(0);
                        leftRightFootstep = 1;
                    }
                    else
                    {
                        audioController.PlayFootstep(1);
                        leftRightFootstep = 0;
                    }
                }
                else if (animateIterator == 11)
                {
                    //audioController.PlayFootstep(1);
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

    private void manageHealth()
    {
        healthText.text = "Health: " + (int)currentHealth;
        GameObject healthBar = GameObject.Find("HealthBar");
        Image healthBarImage = healthBar.GetComponent<Image>();
        healthBarImage.fillAmount = (float)CurrentHealth / 100.0F;
        //if (!CentralControl.healthStaminaModuleExists)
        //{
        //    Debug.LogError("12121212");
        //    healthBar.SetActive(false);
        //}
        //else
        //{
        //    Debug.LogError("iiiiiii");
        //    healthBar.SetActive(true);
        //}
    }

    private void manageStamina()
    {
        staminaText.text = "Stamina: " + (int)currentStamina;
        GameObject healthBar = GameObject.Find("StaminaBar");
        Image healthBarImage = healthBar.GetComponent<Image>();
        healthBarImage.fillAmount = (float)currentStamina / 100.0F;
        //if (!CentralControl.healthStaminaModuleExists)
        //{
        //    Debug.LogError("1111");
        //    //healthBar.SetActive(false);
        //}
        //else
        //{
        //    Debug.LogError("2222");
        //    //healthBar.SetActive(true);
        //}
    }

    private void Sleep()
    {
        currentHealth = 100;
        currentStamina = 100;
        manageHealth();
        manageStamina();
        sleepTimePassed = (dayNightController.dayCycleLength / dayNightController.hoursPerDay) * 4;
        dayNightController.currentCycleTime += sleepTimePassed;
        allLocalControl = FindObjectsOfType<LocalControl>();
        allCentralControl = FindObjectsOfType<CentralControl>();



        for (int i = 0; i < allLocalControl.Length; i++)
        {
            allLocalControl[i].durability -= (int)sleepTimePassed / (int)allLocalControl[i].durabilityLossTime;
        }

        for (int i = 0; i < allCentralControl.Length; i++)
        {
            allCentralControl[i].durability -= ((int)sleepTimePassed / (int)allCentralControl[i].durabilityLossTime) / 2;
        }
    }

	public void FoodEaten()
	{
		eatingTimer = 0;
		hasEaten = true;
	}

    IEnumerator CoolDownDamage()
    {
        onCoolDown = true;
        yield return new WaitForSeconds(coolDown);
        onCoolDown = false;
    }

    IEnumerator MusicTrigger()
    {
        // Debug.Log ("checking trigger");
        if (Random.Range(0, 4) > 2)
        {
            // Debug.Log ("song queued");
            if (isSongPlaying == false)
            {
                // Debug.Log ("selecting song");
                songSelected = Random.Range(1, 5);
                songLength = Random.Range(100f, 200f);
                songSilenceLength = Random.Range(100f, 200f);
                isSongQueued = true;
                firstSongPlayed = true;
            }
        }
        if (firstSongPlayed == false)
        {
            yield return new WaitForSeconds(3f);
        }
        yield return new WaitForSeconds(30f);
    }
}