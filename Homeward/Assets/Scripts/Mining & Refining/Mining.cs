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

    private GameObject miningBarBackground;
    private GameObject miningBar;
    private float timer;
    private Image miningBarImage;

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
        mainPlayer = GameObject.Find("MainPlayer");
        playerController = mainPlayer.GetComponent<PlayerController>();
        inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
        randomMineralsQuantity = Random.Range(minimumMineralsThatCanBeExtracted, maximumMineralsThatCanBeExtracted);
        audioController = GameObject.Find("AudioObject").GetComponent<AudioController>();
        time = 2500.0F * Time.deltaTime;

        SetMiningBar();
    }

    void SetMiningBar()
    {
        miningBarBackground = Instantiate(Resources.Load("Mining/Mining Bar Background")) as GameObject;
        miningBarBackground.transform.parent = GameObject.Find("Canvas").transform;
        RectTransform miningBarBackgroundRect = miningBarBackground.GetComponent<RectTransform>();
        miningBarBackgroundRect.localScale = new Vector3(1, 1, 1);
        miningBarBackgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 70.0f);
        miningBarBackgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20.0f);
        miningBarBackgroundRect.position = mainPlayer.transform.position + new Vector3(80.0f, 90.0f);

        miningBar = GameObject.Find("Mining Bar");
        RectTransform miningBarRect = miningBar.GetComponent<RectTransform>();
        miningBarRect.localScale = new Vector3(1, 1, 1);
        miningBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 65.0f);
        miningBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 15.0f);
        miningBarBackground.SetActive(false);

        miningBarImage = miningBar.GetComponent<Image>();
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

        for (int i = 0; i < mineralsCount; i++)
        {
            mainPlayer.GetComponent<PlayerController>().playerInventory.GetComponent<Inventory>().AddItem(item);
        }
    }

    public void Mine()
	{
		audioController.PlayMiningSound();
		
        if (miningBarImage.fillAmount < 0.01f)
        {
            Debug.Log("Great!");
            MineSupportFunction(randomMineralsQuantity);
            randomMineralsQuantity = 0;
        }
        else if (0.01f < miningBarImage.fillAmount && miningBarImage.fillAmount < 0.25f)
        {
            Debug.Log("Nice!");
            MineSupportFunction(1);
            randomMineralsQuantity--;
        }
        else if (0.25f < miningBarImage.fillAmount)
        {
            Debug.Log("No good");
        }
	}

    IEnumerator DestroyMine()
    {
        animation.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        Destroy(miningBar);
        Destroy(miningBarBackground);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            miningBarBackground.SetActive(true);
        }

        timer = 0.0f;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        timer = Time.time;

        if (other.gameObject.tag == "Player") 
		{
			playerController.nearestMineral = this;
			playerInMiningPosition = true;
            AnimateMiningBar();
		}
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") 
		{ 
			playerController.nearestMineral = null;
			playerInMiningPosition = false;
            miningBarBackground.SetActive(false);
		}
    }

    void AnimateMiningBar()
    {
        miningBarImage.fillAmount = (float)Mathf.Abs(Mathf.Sin(timer*2));
    }
}
