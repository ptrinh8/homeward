// =======================================================================
// <file="PlayerController.cs" product="Homeward">
// <date>2014-11-12</date>
// <summary>Contains a base, abstract class for PlayerController</summary>
// =======================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

public class PlayerController : MonoBehaviour 
{
	public float speed;
	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	private float animateSpeed; //Time between frames of animation
	private float animateTime; //Variable storing the timer for the animation
	public int animateIterator; //Variable storing the frame of the spritesheet to use
	private int animateZone; //Specifies the direction of the astronaut's animation (up, down, left, right)
	private bool frameAscending; //boolean to tell AnimateFrames if spritesheet animation frame is increasing
	private bool frameDescending; //boolean to tell AnimateFrames if spritesheet animation frame is decreasing

	public float miningSpeed;	// mining speed per sec
	private float miningTimer;	// record mining time
	private bool miningNow, isMining;	// miningNow is the signal for mineral class
	public GameObject textFinder;
	private GUIText miningText;

    // Do not delete, for test/debug purpose only
    public int testVariable = 10;

	public bool MiningNow {
		set {
			miningNow = value;
		}
		get {
			return miningNow;
		}
	}

    // Do not delete, for test/debug purpose only
    public int TestPurposeProperty
    {
        get { return testVariable; }
        set { testVariable = value; }
    }

	void Start () 
	{
		//this.GetComponentInChildren<SpriteRenderer>().sprite = sprites[4];
		speed = 2.5f;
		animateSpeed = .1f;
		animateTime = 0f;
		frameAscending = true;
		frameDescending = false;

		miningTimer = 0;
		miningText = textFinder.GetComponent<GUIText> ();
		miningText.text = "";
		miningNow = false;
		isMining = false;
	}

	void Update () 
	{
        // Function deals with everything related to zooming-in when on base
        zoomInWhenOnBase();

		float x = Input.GetAxisRaw("Horizontal"); // Input.GetAxisRaw is independent of framerate, and also gives us raw input which is better for 2D
		float y = Input.GetAxisRaw("Vertical");
		Vector2 direction = new Vector2(x, y); // storing the x and y Inputs from GetAxisRaw in a Vector2
		rigidbody2D.velocity = direction * speed; // speed is changable by us

		// mining control
		if ((isMining) && (miningTimer < miningSpeed)) {
			miningText.text = "Mining...";
			miningTimer += Time.deltaTime;
		}
		else if (miningTimer > miningSpeed){
			miningNow = true;
			miningTimer = 0;
			miningText.text = "";
			isMining = false;
		}
		// set mining state
		if ((Input.GetKeyDown(KeyCode.F)) && (miningTimer == 0)) isMining = true;
		

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
        // Zoom-in when on the base

        GameObject shipGameObject = GameObject.Find("Base");
        int shipPosX = (int)shipGameObject.GetComponentInChildren<SpriteRenderer>().transform.position.x;
        int shipPosY = (int)shipGameObject.GetComponentInChildren<SpriteRenderer>().transform.position.y;

        GameObject mainPlayerGameObject = GameObject.Find("MainPlayer");
        var mainPlayerPos = mainPlayerGameObject.transform.position;
        int playerPosY = (int)mainPlayerPos.y;
        int playerPosX = (int)mainPlayerPos.x;

        if ((playerPosX > (shipPosX - 1.5)) && (playerPosX < (shipPosX + 1.5)))
        {
            if ((playerPosY > (shipPosY - 1.5)) && (playerPosY < (shipPosY + 1.5)))
            {
                Camera.main.orthographic = true;
                Camera.main.orthographicSize = 2;
            }
            else
            {
                Camera.main.orthographic = true;
                Camera.main.orthographicSize = 5;
            }
        }
        else
        {
            Camera.main.orthographic = true;
            Camera.main.orthographicSize = 5;
        }
    }
}