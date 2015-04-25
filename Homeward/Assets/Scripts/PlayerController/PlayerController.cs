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
    private SpriteRenderer spriteRenderer;
	private bool leftRightFootstep = false;
	private float animationTime;
	private int animationFacing;

	private Animator anim;
	private bool playerMoving = false;

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

	public Object leftFootprint;
	public Object rightFootprint;

	public Color tileColor;


    public bool canSleep;
    [HideInInspector]
    public bool isKeyEnabled = true;
    private KeyCode consumeFoodKey = KeyCode.K;

    public Canvas canvas;
    private float currentHealth, currentStamina;

	/** mining bar **/
	public GameObject miningBarBackground;
	private RectTransform miningBarBackgroundRect;
	private GameObject miningBar;
	public GameObject miningBarAimingSpot;
	public GameObject miningCircle;
	private float timer;
	private Image miningBarImage;
	private bool goingUp;
	private float addition;
	private float accel;
	private float miningCircleInitialPosition;
	public bool miningBarActive;

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

	private ParticleSystem playerParticleSystem;

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
    private GameObject enhancedRadarCamera;
    private GameObject ToolBoxObject;
    
    private Camera mainCamera;

	private float miningBarFlashTimer;
	private float miningBarFlashTime;
	private int miningBarFlashNumber;
	private int miningBarFlashCount;
	private Color miningBarDefaultColor;
	private Color miningBarSucceedColor;
	private Color miningBarFailColor;
	private bool miningBarAlternate;
	private bool miningBarSuccess;
	private bool miningBarFlashing;

    public static bool pauseFlag;

    //Taylor
    private bool environmentAir;
	public static bool showRepairArrows;

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

    private GameObject pauseMenu;

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
		if (CentralControl.isInside) { 
		
			if (oxygen <= 100)
			{
				oxygen += .01f;
			}
		
		}
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

    GameObject miningProbe;
	void SetMiningBar()
	{
		miningBarBackground = Instantiate(Resources.Load("Mining/Mining Bar Background")) as GameObject;
		miningBarBackground.transform.parent = GameObject.Find("Canvas").transform;
		miningBarBackgroundRect = miningBarBackground.GetComponent<RectTransform>();
		miningBarBackgroundRect.localScale = new Vector3(1, 1, 1);
		miningBarBackgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150.0f);
		miningBarBackgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20.0f);
		miningBarBackgroundRect.position = GameObject.Find ("Canvas").transform.position + new Vector3(0f, 60.0f);
		
		miningCircle = Instantiate(Resources.Load("Mining/MiningCircle")) as GameObject;
		miningCircle.transform.parent = GameObject.Find("Canvas").transform;
		RectTransform miningCircleRect = miningCircle.GetComponent<RectTransform>();
		miningCircleRect.localScale = new Vector3(1, 1, 1);
		miningCircleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 15.0f);
		miningCircleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 15.0f);
		miningCircleRect.position = miningBarBackgroundRect.position;
		miningCircleInitialPosition = GameObject.Find ("Canvas").transform.position.x + 10.0f;
		miningCircle.SetActive(false);
		
		miningBar = GameObject.Find("Mining Bar");
		RectTransform miningBarRect = miningBar.GetComponent<RectTransform>();
		miningBarRect.localScale = new Vector3(1, 1, 1);
		miningBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, miningBarBackgroundRect.sizeDelta.x - 5.0f);
		miningBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, miningBarBackgroundRect.sizeDelta.y - 5.0f);
		
		miningBarAimingSpot = GameObject.Find("Aiming Spot");
		RectTransform miningBarAimingSpotRect = miningBarAimingSpot.GetComponent<RectTransform>();
		miningBarAimingSpotRect.localScale = new Vector3(1, 1, 1);
		miningBarAimingSpotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, miningBarBackgroundRect.sizeDelta.x * 0.3f);
		miningBarAimingSpotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, miningBarBackgroundRect.sizeDelta.y);
		miningBarAimingSpotRect.position = miningBarBackgroundRect.position;
		miningBarAimingSpot.SetActive(false);
		
		miningBarBackground.SetActive(false);
		
		miningBarImage = miningBar.GetComponent<Image>();
		addition = 4.0f;
		accel = 0.2f;
	}

	float GetNormalizedPosition()
	{
		return (miningCircle.transform.position.x - (miningBarBackgroundRect.transform.position.x - miningBarBackgroundRect.sizeDelta.x / 2)) / miningBarBackgroundRect.sizeDelta.x; 
	}
	
	public void AnimateMiningBar()
	{
		//Debug.Log (GetNormalizedPosition());
		if (miningBarFlashing == false)
		{
			if (goingUp)
			{
				if (GetNormalizedPosition() <= 0.5f)
				{
					accel += 0.01f;
					addition += accel;
				}
				else if (0.5f < GetNormalizedPosition())
				{
					if (addition > accel) 
					{ 
						accel -= 0.01f;
						addition -= accel; 
					}
				}
				
				miningCircle.transform.localPosition += new Vector3(addition % 100.0f, 0.0f);
			}
			else
			{
				if (GetNormalizedPosition() <= 0.5f)
				{
					if (addition > accel) 
					{ 
						accel -= 0.01f;
						addition -= accel; 
					}
				}
				else if (0.5f < GetNormalizedPosition())
				{
					accel += 0.01f;
					addition += accel;
				}
				
				miningCircle.transform.localPosition -= new Vector3(addition % 100.0f, 0.0f);
			}
			
			if (GetNormalizedPosition() < 0)
			{
				goingUp = true;
				addition = 1.0f;
				accel = 0.2f;
			}
			else if (GetNormalizedPosition() > 1)
			{
				goingUp = false;
				addition = 1.0f;
				accel = 0.2f;
			}
		}
	}

    void Start()
    {
        miningProbe = Instantiate(Resources.Load("Mining/MiningProbe")) as GameObject;
		showRepairArrows = false;// Taylor
        pauseFlag = false;

        minerals = FindObjectOfType(typeof(Mining)) as Mining;
        spaceship = FindObjectOfType(typeof(Spaceship)) as Spaceship;
        endgame = FindObjectOfType(typeof(EndGame)) as EndGame;
        dayNightController = GameObject.Find("DayNightController").GetComponent<DayNightController>();

        speed = 1.0f;

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

        pauseMenu = Instantiate(Resources.Load("Menu/PauseMenu")) as GameObject;
        pauseMenu.transform.SetParent(GameObject.Find("Canvas").transform);
        pauseMenu.AddComponent<CanvasGroup>();
        pauseMenu.GetComponent<CanvasGroup>().alpha = 0.7f;
        RectTransform pauseMenuRect = pauseMenu.GetComponent<RectTransform>();
        pauseMenuRect.transform.position = new Vector3(Screen.width / 2, Screen.height / 2);
        pauseMenuRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400.0f);
        pauseMenuRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200.0f);
        pauseMenu.SetActive(false);

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

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        enhancedRadarCamera = GameObject.Find("EnhancedRadarCamera");

        /*** PlayerInventory ***/
        playerInventory = Instantiate(playerInventory) as GameObject;
        playerInventory.transform.SetParent(GameObject.Find("Canvas").transform);
        playerInventory.transform.position = new Vector3(7.0f, Screen.height - 7.0f, 0.0f);
        showPlayerInventory = false;
        keyCode_I_Works = true;
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

		SetMiningBar();
		miningBarActive = false;
		miningBarFlashTime = .1f;
		miningBarFlashNumber = 3;
		miningBarDefaultColor = new Color(1f, 1f, 1f, 1f);
		miningBarSucceedColor = new Color(0f, 1f, 0f, 1f);
		miningBarFailColor = new Color(1f, 0f, 0f, 1f);
		miningBarAlternate = false;
		miningBarFlashCount = 0;


		playerParticleSystem = GameObject.Find ("Particle System").GetComponent<ParticleSystem>();
		playerParticleSystem.renderer.sortingLayerName = "GameplayLayer";
		playerParticleSystem.loop = false;

		anim = GetComponent<Animator>();
    }

    void Update()
    {
		if (miningBarFlashing == true)
		{
			miningBarFlashTimer += Time.deltaTime;
			StartCoroutine(FlashMiningBar());
		}
		if (miningBarActive == true)
		{
			AnimateMiningBar();
		}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseFlag = !pauseFlag;
        }

        if (pauseFlag)
        {
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
			if ((tileColor.r > tileColor.g) && (tileColor.r > tileColor.b))
			{
				playerParticleSystem.startColor = new Color(tileColor.r, tileColor.g + .3f, tileColor.b + .3f);
			}
			else if ((tileColor.g > tileColor.r) && (tileColor.g > tileColor.b))
			{
				playerParticleSystem.startColor = new Color(tileColor.r + .3f, tileColor.g, tileColor.b + .3f);
			}
			else if ((tileColor.b > tileColor.r) && (tileColor.b > tileColor.g))
			{
				playerParticleSystem.startColor = new Color(tileColor.r + .3f, tileColor.g + .3f, tileColor.b);
			}
            

            if (mainCamera.enabled)
            {
                zoomInWhenIndoor();
            }
            EndDemo();
            ManageOxygenLevels();

            if (CentralControl.healthStaminaModuleExists)
            {
                UIHealthBar.GetComponent<CanvasGroup>().alpha = 1;
                UIStaminaBar.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                UIHealthBar.GetComponent<CanvasGroup>().alpha = 0;
                UIStaminaBar.GetComponent<CanvasGroup>().alpha = 0;
            }

			if (CentralControl.oxygenModuleExists)
			{
				UIOxygenBar.GetComponent<CanvasGroup>().alpha = 1;
			}
			else
			{
				UIOxygenBar.GetComponent<CanvasGroup>().alpha = 0;
			}

			if (CentralControl.radarModuleExists)
			{
				RadarCamera.camera.cullingMask |= (1 << LayerMask.NameToLayer("Radar"));
			}
			else
			{
				RadarCamera.camera.cullingMask &= ~(1 << LayerMask.NameToLayer("Radar"));
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
                    }


                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (holdingMiningTool == true)
                        {
							if (miningBarActive == true)
							{
								if (0.45f < GetNormalizedPosition() && GetNormalizedPosition() < 0.55f)
								{
									miningBarFlashNumber = 5;
									miningBarSuccess = true;
									miningBarFlashing = true;
									Debug.Log("Great!");
									nearestMineral.Mine (2);
								}
								else if ((0.4f < GetNormalizedPosition() && GetNormalizedPosition() < 0.45f) || (0.55f < GetNormalizedPosition() && GetNormalizedPosition() < 0.6f))
								{
									miningBarFlashNumber = 3;
									miningBarSuccess = true;
									miningBarFlashing = true;
									Debug.Log("Nice!");
									nearestMineral.Mine (1);
								}
								else
								{
									miningBarFlashNumber = 3;
									miningBarSuccess = false;
									miningBarFlashing = true;
									nearestMineral.Mine (0);
									Debug.Log("No good");
								}
							}
							else if (nearestMineral != null && miningBarActive == false)
							{
								Debug.Log ("Animating");
								nearestMineral.SetMiningBarVisible();
								//AnimateMiningBar();
								miningBarActive = true;
							}
                        }
                    }
                    

                    /****************************************************************
                     * Inventory: This is how you add items to playerInventory
                     * **************************************************************/
                    if (showPlayerInventory) // player is able to control inventory only when inventory is visible
                    {
                        keyCode_B_Works = false;
                        playerInventory.GetComponent<CanvasGroup>().alpha = 1;
                        playerInventory.GetComponent<Inventory>().SetSlotsActive(true);
                        ToolBoxObject.GetComponent<CanvasGroup>().alpha = 1;

                        if (Input.GetKeyDown(KeyCode.P)) // p is temporary. Delete this once you find how to add item.
                        {
                            Item item = GameObject.Find("Mineral").GetComponent<Item>();
                            playerInventory.GetComponent<Inventory>().AddItem(item);
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

						if (Input.GetKeyDown(KeyCode.L))
						{
							Item item = GameObject.Find("Material").GetComponent<Item>();
							playerInventory.GetComponent<Inventory>().AddItem(item);
						}
						if (Input.GetKeyDown(KeyCode.K))
						{
							Item item = GameObject.Find("Food1").GetComponent<Item>();
							playerInventory.GetComponent<Inventory>().AddItem(item);
						}
						if (Input.GetKeyDown(KeyCode.J))
						{
							Item item = GameObject.Find("Oxygen").GetComponent<Item>();
							playerInventory.GetComponent<Inventory>().AddItem(item);
						}
						if (Input.GetKeyDown(KeyCode.N))
						{
							Item item = GameObject.Find("MiningProbe").GetComponent<Item>();
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
                        playerInventory.GetComponent<CanvasGroup>().alpha = 0;
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


					if (showPlayerInventory || showModuleSelection || showRepairArrows)
                    {
						if (showRepairArrows)
						{
							if (Input.GetKey(KeyCode.A))
							{ x = 0; y = 0;}
							else if (Input.GetKey(KeyCode.D))
							{ x = 0; y = 0;}
							if (Input.GetKey(KeyCode.W))
							{ x = 0; y = 0;}
							else if (Input.GetKey(KeyCode.S))
							{ x = 0; y = 0;}
						}
                    }
                    else
                    {
						CheckPlayerFacing();
                        x = Input.GetAxisRaw("Horizontal");   // Input.GetAxisRaw is independent of framerate, and also gives us raw input which is better for 2D
                        y = Input.GetAxisRaw("Vertical");
                    }

                    if (ModuleControl.ShowModuleControl)
                    {
                        x = y = 0.0f;
                    }

                    if (EnhancedRadar.showEnhancedRadar)
                    {
                        x = y = 0.0f;
                        enhancedRadarCamera.GetComponent<Camera>().fieldOfView = Mathf.Abs(Input.GetAxis("Horizontal")) * 28 + 148.0f; // 179 = max value of field view
                    }

					direction = new Vector2(x, y);      // storing the x and y Inputs from GetAxisRaw in a Vector2
					rigidbody2D.AddForce(direction * 5f);

					/*
					if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.W))
					{
						playerMoving = true;
					}
					else
					{
						playerMoving = false;
					}*/

					if (rigidbody2D.velocity.magnitude > .01f)
					{
						playerMoving = true;
					}
					else
					{
						playerMoving = false;
					}
					anim.SetBool("PlayerMoving", playerMoving);
					anim.SetFloat("VelocityX", rigidbody2D.velocity.x);
					anim.SetFloat("VelocityY", rigidbody2D.velocity.y);
					
					if (rigidbody2D.velocity.magnitude < .01f)
					{
						rigidbody2D.velocity = Vector2.zero;
					}
					//Debug.Log (rigidbody2D.velocity);
					//Debug.Log (rigidbody2D.velocity.magnitude);
					
					

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

	public void OxygenTaken()
	{
		oxygen += 10f;
	}

	public bool MiningToolCheck()
	{
		if (holdingMiningTool == true)
		{
			return true;
		}
		return false;
	}

	public bool BuildingToolCheck()
	{
		if (holdingBuildingTool == true)
		{
			return true;
		}
		return false;
	}

	public bool RepairingToolCheck()
	{
		if (holdingRepairTool == true)
		{
			return true;
		}
		return false;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "FinalTextures")
		{
			SpriteRenderer tile = other.GetComponentInChildren<SpriteRenderer>();
			tileColor = tile.color;
		}
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

	IEnumerator FlashMiningBar()
	{
		if (miningBarFlashCount < miningBarFlashNumber)
		{
			if (miningBarFlashTimer > miningBarFlashTime)
			{
				if (miningBarSuccess == true)
				{
					if (miningBarAlternate == false)
					{
						miningBar.GetComponent<Image>().color = miningBarSucceedColor;
						miningBarAlternate = true;
					}
					else if (miningBarAlternate == true)
					{
						miningBar.GetComponent<Image>().color = miningBarDefaultColor;
						miningBarAlternate = false;
					}
				}
				else
				{
					if (miningBarAlternate == false)
					{
						miningBar.GetComponent<Image>().color = miningBarFailColor;
						miningBarAlternate = true;
					}
					else if (miningBarAlternate == true)
					{
						miningBar.GetComponent<Image>().color = miningBarDefaultColor;
						miningBarAlternate = false;
					}
				}
				miningBarFlashCount++;
				miningBarFlashTimer = 0f;
			}
		}
		else
		{
			miningBar.GetComponent<Image>().color = miningBarDefaultColor;
			miningBarFlashCount = 0;
			miningBarFlashTimer = 0f;
			miningBarFlashing = false;
			if (nearestMineral != null)
			{
				nearestMineral.SetMiningBarInvisible();
			}
		}
		yield return null;
	}

	private void CheckPlayerFacing()
	{
		if (Input.GetKey (KeyCode.W) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) && !Input.GetKey (KeyCode.A))
		{
			anim.SetBool("WasMovingNorth", true);
			anim.SetBool("WasMovingNorthEast", false);
			anim.SetBool("WasMovingEast", false);
			anim.SetBool("WasMovingSouthEast", false);
			anim.SetBool("WasMovingSouth", false);
			anim.SetBool("WasMovingSouthWest", false);
			anim.SetBool("WasMovingWest", false);
			anim.SetBool("WasMovingNorthWest", false);
			animationFacing = 0;
		}
		else if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.D) && !Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.A))
		{
			anim.SetBool("WasMovingNorth", false);
			anim.SetBool("WasMovingNorthEast", true);
			anim.SetBool("WasMovingEast", false);
			anim.SetBool("WasMovingSouthEast", false);
			anim.SetBool("WasMovingSouth", false);
			anim.SetBool("WasMovingSouthWest", false);
			anim.SetBool("WasMovingWest", false);
			anim.SetBool("WasMovingNorthWest", false);
			animationFacing = 1;
		}
		else if (Input.GetKey (KeyCode.D) && !Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.W))
		{
			anim.SetBool("WasMovingNorth", false);
			anim.SetBool("WasMovingNorthEast", false);
			anim.SetBool("WasMovingEast", true);
			anim.SetBool("WasMovingSouthEast", false);
			anim.SetBool("WasMovingSouth", false);
			anim.SetBool("WasMovingSouthWest", false);
			anim.SetBool("WasMovingWest", false);
			anim.SetBool("WasMovingNorthWest", false);
			animationFacing = 2;
		}
		else if (Input.GetKey (KeyCode.D) && Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.A))
		{
			anim.SetBool("WasMovingNorth", false);
			anim.SetBool("WasMovingNorthEast", false);
			anim.SetBool("WasMovingEast", false);
			anim.SetBool("WasMovingSouthEast", true);
			anim.SetBool("WasMovingSouth", false);
			anim.SetBool("WasMovingSouthWest", false);
			anim.SetBool("WasMovingWest", false);
			anim.SetBool("WasMovingNorthWest", false);
			animationFacing = 3;
		}
		else if (Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.D) && !Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.W))
		{
			anim.SetBool("WasMovingNorth", false);
			anim.SetBool("WasMovingNorthEast", false);
			anim.SetBool("WasMovingEast", false);
			anim.SetBool("WasMovingSouthEast", false);
			anim.SetBool("WasMovingSouth", true);
			anim.SetBool("WasMovingSouthWest", false);
			anim.SetBool("WasMovingWest", false);
			anim.SetBool("WasMovingNorthWest", false);
			animationFacing = 4;
		}
		else if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.D))
		{
			anim.SetBool("WasMovingNorth", false);
			anim.SetBool("WasMovingNorthEast", false);
			anim.SetBool("WasMovingEast", false);
			anim.SetBool("WasMovingSouthEast", false);
			anim.SetBool("WasMovingSouth", false);
			anim.SetBool("WasMovingSouthWest", true);
			anim.SetBool("WasMovingWest", false);
			anim.SetBool("WasMovingNorthWest", false);
			animationFacing = 5;
		}
		else if (Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.D))
		{
			anim.SetBool("WasMovingNorth", false);
			anim.SetBool("WasMovingNorthEast", false);
			anim.SetBool("WasMovingEast", false);
			anim.SetBool("WasMovingSouthEast", false);
			anim.SetBool("WasMovingSouth", false);
			anim.SetBool("WasMovingSouthWest", false);
			anim.SetBool("WasMovingWest", true);
			anim.SetBool("WasMovingNorthWest", false);
			animationFacing = 6;
		}
		else if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
		{
			anim.SetBool("WasMovingNorth", false);
			anim.SetBool("WasMovingNorthEast", false);
			anim.SetBool("WasMovingEast", false);
			anim.SetBool("WasMovingSouthEast", false);
			anim.SetBool("WasMovingSouth", false);
			anim.SetBool("WasMovingSouthWest", false);
			anim.SetBool("WasMovingWest", false);
			anim.SetBool("WasMovingNorthWest", true);
			animationFacing = 7;
		}
	}

	private void AnimationEffectsLeft()
	{
		if (CentralControl.isInside == false)
		{
			switch (animationFacing)
			{
			case 0: //north
				Instantiate(leftFootprint, new Vector3(this.transform.position.x - .05f, this.transform.position.y - .4f), Quaternion.Euler(0, 0, 0));
				playerParticleSystem.Emit (5);
				break;
			case 1:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x, this.transform.position.y - .35f), Quaternion.Euler(0, 0, -45));
				playerParticleSystem.Emit (5);
				break;
			case 2:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x + .15f, this.transform.position.y - .4f), Quaternion.Euler(0, 0, -90));
				playerParticleSystem.Emit (5);
				break;
			case 3:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x + .15f, this.transform.position.y - .4f), Quaternion.Euler(0, 0, -135));
				playerParticleSystem.Emit (5);
				break;
			case 4:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x + .05f, this.transform.position.y - .4f), Quaternion.Euler(0, 0, -180));
				playerParticleSystem.Emit (5);
				break;
			case 5:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x + .02f, this.transform.position.y - .5f), Quaternion.Euler(0, 0, -225));
				playerParticleSystem.Emit (5);
				break;
			case 6:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x - .15f, this.transform.position.y - .5f), Quaternion.Euler(0, 0, -270));
				playerParticleSystem.Emit (5);
				break;
			case 7:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x - .1f, this.transform.position.y - .45f), Quaternion.Euler(0, 0, -315));
				playerParticleSystem.Emit (5);
				break;
			}
		}
	}

	private void AnimationEffectsRight()
	{
		if (CentralControl.isInside == false)
		{
			switch (animationFacing)
			{
			case 0: //north
				Instantiate(leftFootprint, new Vector3(this.transform.position.x + .05f, this.transform.position.y - .4f), Quaternion.Euler(0, 0, 0));
				playerParticleSystem.Emit (5);
				break;
			case 1:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x + .1f, this.transform.position.y - .45f), Quaternion.Euler(0, 0, -45));
				playerParticleSystem.Emit (5);
				break;
			case 2:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x + .2f, this.transform.position.y - .5f), Quaternion.Euler(0, 0, -90));
				playerParticleSystem.Emit (5);
				break;
			case 3:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x, this.transform.position.y - .5f), Quaternion.Euler(0, 0, -135));
				playerParticleSystem.Emit (5);
				break;
			case 4:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x - .05f, this.transform.position.y - .4f), Quaternion.Euler(0, 0, -180));
				playerParticleSystem.Emit (5);
				break;
			case 5:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x - .02f, this.transform.position.y - .35f), Quaternion.Euler(0, 0, -225));
				playerParticleSystem.Emit (5);
				break;
			case 6:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x - .075f, this.transform.position.y - .4f), Quaternion.Euler(0, 0, -270));
				playerParticleSystem.Emit (5);
				break;
			case 7:
				Instantiate(leftFootprint, new Vector3(this.transform.position.x, this.transform.position.y - .4f), Quaternion.Euler(0, 0, -315));
				playerParticleSystem.Emit (5);
				break;
			}
		}
	}

    void Quit()
    {
        Application.Quit();
    }

    void Continue()
    {
        pauseFlag = false;
    }
}