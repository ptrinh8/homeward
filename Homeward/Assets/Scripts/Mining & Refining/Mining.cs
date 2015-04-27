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

    void StartTimer()
    {
        if (!timerReached) { loadingUpdateTime = loadingStartTime++; }
        if (loadingUpdateTime == time) { timerReached = true; }
    }

    void StopTimer()
    {
        if (timerReached == true) { loadingUpdateTime = 0.0f; loadingStartTime = 0.0f; }
        if (miningSoundPlayed = true)
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
    }

    void Update()
    {
        PlayerMiningState();
        StartDestoryMineCoroutine();
        MineralsValidations();
    }

    void MineSupportFunction(int mineralsCount)
    {
        Item item = GameObject.Find("Mineral").GetComponent<Item>();
        randomMineralsQuantity -= mineralsCount;

        for (int i = 0; i < mineralsCount; i++)
        {
            mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
        }
    }

    public void Mine(int numberOfMinerals)
	{
		if (numberOfMinerals == 0)
		{
			rockParticleSystem.Emit (3);
		}
		else if (numberOfMinerals == 1)
		{
			rockParticleSystem.Emit (6);
		}
		else if (numberOfMinerals == 2)
		{
			rockParticleSystem.Emit (10);
		}
		audioController.PlayMiningSound();
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

}
