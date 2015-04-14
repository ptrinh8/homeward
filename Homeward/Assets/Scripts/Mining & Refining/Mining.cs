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

        SetMiningBar();
    }

    void SetMiningBar()
    {
        miningBarBackground = Instantiate(Resources.Load("Mining/Mining Bar Background")) as GameObject;
        miningBarBackground.transform.parent = GameObject.Find("Canvas").transform;
        miningBarBackgroundRect = miningBarBackground.GetComponent<RectTransform>();
        miningBarBackgroundRect.localScale = new Vector3(1, 1, 1);
        miningBarBackgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150.0f);
        miningBarBackgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20.0f);
        miningBarBackgroundRect.position = mainPlayer.transform.position + new Vector3(80.0f, 90.0f);

        miningCircle = Instantiate(Resources.Load("Mining/MiningCircle")) as GameObject;
        miningCircle.transform.parent = GameObject.Find("Canvas").transform;
        RectTransform miningCircleRect = miningCircle.GetComponent<RectTransform>();
        miningCircleRect.localScale = new Vector3(1, 1, 1);
        miningCircleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 15.0f);
        miningCircleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 15.0f);
        miningCircleRect.position = miningBarBackgroundRect.position;
        miningCircleInitialPosition = mainPlayer.transform.position.x + 80.0f;
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

        if (0.499f < GetNormalizedPosition() && GetNormalizedPosition() < 0.501f)
        {
            Debug.Log("Great!");
            MineSupportFunction(2);
            randomMineralsQuantity -= 2;
        }
        else if ((0.3f < GetNormalizedPosition() && miningBarImage.fillAmount < 0.4999f) || (0.5001f < GetNormalizedPosition() && GetNormalizedPosition() < 0.7f))
        {
            Debug.Log("Nice!");
            MineSupportFunction(1);
            randomMineralsQuantity--;
        }
        else
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
        Destroy(miningCircle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            miningBarBackground.SetActive(true);
            miningCircle.SetActive(true);
            miningBarAimingSpot.SetActive(true);
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
            miningCircle.SetActive(false);
		}
    }

    float GetNormalizedPosition()
    {
        return (miningCircle.transform.position.x - (miningBarBackgroundRect.transform.position.x - miningBarBackgroundRect.sizeDelta.x / 2)) / miningBarBackgroundRect.sizeDelta.x; 
    }

    void AnimateMiningBar()
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
