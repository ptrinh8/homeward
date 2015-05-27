// ========================================================================
// <file="Mining.cs" product="Homeward">
// <date>2014-11-11</date>
// ========================================================================


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Mining : MonoBehaviour
{
    private Inventory inventory = new Inventory();
    private PlayerController playerController = new PlayerController();

    public int minimumMineralsThatCanBeExtracted;
    public int maximumMineralsThatCanBeExtracted;
    public int randomMineralsQuantity;

    public GameObject moduleInventory;
    private GameObject mainPlayer;
    private GameObject numberOfMineralQuantityText;

    [HideInInspector]
    public bool playerInMiningPosition = false;
    private bool isPlayerMining = false;
    private KeyCode miningKey = KeyCode.F;
    private AudioController audioController;
    private bool miningSoundPlayed;

    private float time = 0.0F;
    private bool timerReached = false;
    public float loadingUpdateTime;
    private float loadingStartTime;
    private float loadingPercent;

    /*** mining bar ***/
    private GameObject miningBarBackground;
    private RectTransform miningBarBackgroundRect;
    private GameObject miningBar;
    private GameObject miningBarAimingSpot;
    private GameObject miningCircle;
    private float timer;
    private Image miningBarImage;
    private bool goingUp;
    private float addition;
    private float accel;
    private float miningCircleInitialPosition;
	public bool miningBarVisible = false;

    private ParticleSystem rockParticleSystem;

	private int mineralType;
	private float mineralTypeSelector;
	private int mineralSpriteSelector;

	private Sprite mineralSprite1;
	private Sprite mineralSprite2;
	private Sprite mineralSprite3;
	private Sprite mineralSprite4;
	private Sprite mineralSprite5;
	private Sprite mineralSprite6;
	private Sprite mineralSprite7;
	private Sprite mineralSprite8;



    void StartTimer()
    {
        if (!timerReached) { loadingUpdateTime = loadingStartTime++; }
        if (loadingUpdateTime == time) { timerReached = true; }
    }

    void StopTimer()
    {
        if (timerReached == true) { loadingUpdateTime = 0.0f; loadingStartTime = 0.0f; }
        if (miningSoundPlayed == true)
        {
            miningSoundPlayed = false;
        }
    }

    private void PlayerMiningState()
    {
        if ((Input.GetKeyDown(miningKey)) && (playerController.miningTimer == 0) && playerInMiningPosition == true)
        {
            //audioController.PlayMiningSound();
            isPlayerMining = !isPlayerMining;
        }
    }

    private void StartDestoryMineCoroutine()
    {
        if ((randomMineralsQuantity == 0) && !animation.isPlaying && gameObject) { StartCoroutine(DestroyMine()); }
    }

    private void MineralsValidations()
    {
        if (randomMineralsQuantity < 0) { randomMineralsQuantity = 0; }
    }

    void Start()
    {
        addition = 4.0f;
        accel = 0.2f;
        goingUp = true;
        mainPlayer = GameObject.Find("MainPlayer");
        playerController = mainPlayer.GetComponent<PlayerController>();
        inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
        randomMineralsQuantity = Random.Range(minimumMineralsThatCanBeExtracted, maximumMineralsThatCanBeExtracted);
        audioController = GameObject.Find("AudioObject").GetComponent<AudioController>();
        time = 2500.0F * Time.deltaTime;

        rockParticleSystem = this.gameObject.GetComponentInChildren<ParticleSystem>();
        rockParticleSystem.renderer.sortingLayerName = "GameplayLayer";
        rockParticleSystem.renderer.sortingOrder = 1;
        rockParticleSystem.loop = false;

		mineralSprite1 = Resources.Load<Sprite>("Sprites/Rocks/1");
		mineralSprite2 = Resources.Load<Sprite>("Sprites/Rocks/2");
		mineralSprite3 = Resources.Load<Sprite>("Sprites/Rocks/3");
		mineralSprite4 = Resources.Load<Sprite>("Sprites/Rocks/4");
		mineralSprite5 = Resources.Load<Sprite>("Sprites/Rocks/5");
		mineralSprite6 = Resources.Load<Sprite>("Sprites/Rocks/6");
		mineralSprite7 = Resources.Load<Sprite>("Sprites/Rocks/7");
		mineralSprite8 = Resources.Load<Sprite>("Sprites/Rocks/8");

		mineralTypeSelector = Random.Range (1.0f, 10.0f);
		if (mineralTypeSelector < 5.0f)
		{
			mineralType = 1;
			mineralSpriteSelector = Random.Range(1, 3);
			switch (mineralSpriteSelector)
			{
			case 1:
				gameObject.GetComponent<SpriteRenderer>().sprite = mineralSprite7;
				break;
			case 2:
				gameObject.GetComponent<SpriteRenderer>().sprite = mineralSprite2;
				break;
			case 3:
				gameObject.GetComponent<SpriteRenderer>().sprite = mineralSprite4;
				break;
			}
		}
		else if (mineralTypeSelector >= 5.0f && mineralTypeSelector < 8.5f)
		{
			mineralType = 2;
			mineralSpriteSelector = Random.Range(1, 3);
			switch (mineralSpriteSelector)
			{
			case 1:
				gameObject.GetComponent<SpriteRenderer>().sprite = mineralSprite1;
				break;
			case 2:
				gameObject.GetComponent<SpriteRenderer>().sprite = mineralSprite3;
				break;
			case 3:
				gameObject.GetComponent<SpriteRenderer>().sprite = mineralSprite5;
				break;
			}
		}
		else if (mineralTypeSelector >= 8.5f)
		{
			mineralType = 3;
			mineralSpriteSelector = Random.Range(1, 2);
			switch (mineralSpriteSelector)
			{
			case 1:
				gameObject.GetComponent<SpriteRenderer>().sprite = mineralSprite6;
				break;
			case 2:
				gameObject.GetComponent<SpriteRenderer>().sprite = mineralSprite8;
				break;
			}
		}

    }

    void Update()
    {
        PlayerMiningState();
        StartDestoryMineCoroutine();
        MineralsValidations();
    }

    void MineSupportFunction(int mineralsCount)
    {
        randomMineralsQuantity -= mineralsCount;

        for (int i = 0; i < mineralsCount; i++)
        {
			if (mineralType == 1)
			{
				Item item = GameObject.Find("Mineral1").GetComponent<Item>();
				mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
			}
			else if (mineralType == 2)
			{
				Item item = GameObject.Find("Mineral2").GetComponent<Item>();
				mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
			}
			else if (mineralType == 3)
			{
				Item item = GameObject.Find("Mineral3").GetComponent<Item>();
				mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
			}
        }
    }

    public void Mine(int numberOfMinerals)
	{
		Debug.Log ("mining");
		MineSupportFunction(numberOfMinerals);
		playerController.PlayMiningAnimation();
	}

    IEnumerator DestroyMine()
    {
        animation.Play();
        SetMiningBarInvisible();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void SetMiningBarVisible()
    {
        playerController.miningBarBackground.SetActive(true);
        playerController.miningCircle.SetActive(true);
        playerController.miningBarAimingSpot.SetActive(true);
        playerController.AnimateMiningBar();
    }

    public void SetMiningBarInvisible()
    {
        playerController.miningBarActive = false;
        playerController.miningBarBackground.SetActive(false);
        playerController.miningCircle.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerController.nearestMineral = this;
            playerInMiningPosition = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerController.nearestMineral = null;
            playerInMiningPosition = false;
            SetMiningBarInvisible();
        }
    }

	public void PlayParticles()
	{
		Debug.Log ("playing");
		rockParticleSystem.Emit (5);
	}

}
