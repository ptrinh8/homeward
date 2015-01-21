// ==================================================================================
// <file="ProceduralScript.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Contains a base, abstract class for ProceduralContentGeneration</summary>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#endregion

public class ProceduralScript : MonoBehaviour
{
    public Texture2D mouseTexture;

    private PlayerController playerController = new PlayerController();
    private TriggerA triggerA = new TriggerA();
    private Inventory inventory = new Inventory();

    // Stores Player.Position X and Y values
    private float playerLocationX;
    private float playerLocationY;

    public GameObject planetTextureA; // The gameObject that needs to be spawned
    public int maxNumberPlanetTextureA; // Maximum number of sprites to be spawned

    public GameObject planetTextureB;
    public GameObject planetTextureC;

    // GameObject array variable to store all instances created, which is destoryed/created again as required
    private GameObject[] spritesInstances = new GameObject[1000];
    private int numberOfSpriteInstances = 1;

    // As name specifies, used to store data for each newly generated sprite and previously generated sprite
    private GameObject spriteGenerated;
    private GameObject previousSprite;

    private int spritesCount = 0;
    private int currentNumberOfSprites = 0;
    private const int moveRowUpOn = 11;  // Instances change row veritcally after these many created each row

    // To store/retrieve real-time information about the first sprite generated on the screen
    private GameObject firstSpriteGenerated;
    private float firstSpriteLocalPositionX;
    private float firstSpriteLocalPositionY;

    private GameObject[] allRocks1Generated;
    private GameObject[] allRocks2Generated;
    private bool stillGenerating = false;
    private float[] tempRndNosRock1 = new float[60];
    private float[] tempRndNosRock2 = new float[60];
    private List<int> listOfRndNos = new List<int>();    

    // To store/retrieve real-time information about all the sprite generated on the screen
    private GameObject allSpritesGenerated;
    private float allSpritesGeneratedLocalPosX;
    private float allSpritesGeneratedLocalPosY;

    // To store/retrieve real-time information about the last sprite generated on the screen
    private GameObject lastSpriteGenerated;
    private float lastSpriteGeneratedLocalPosX;
    private float lastSpriteGeneratedLocalPosY;

    // Makes the if-statment call only once in Update() function
    private bool hasThePlayerCrossedTile = false;
    // Used in calculating the difference b/w 2 different values of Player.Position to trigger hasThePlayerCrossedTile = true;
    private float playerPosY;
    private float playerPosX;

    private bool playerInsideTriggerA = true; 

    private void RndNosGenerationRock1()
    {
        float[] randomNos = new float[60];
        Random.seed = System.Environment.TickCount;

        for (int i = 0; i < randomNos.Length; i++)
        {
            randomNos[i] = Random.value;
            tempRndNosRock1[i] = randomNos[i];
        }
    }

    private void RndNosGenerationRock2()
    {
        float[] randomNumbers2 = new float[60];
        Random.seed = System.Environment.TickCount;

        for (int i = 0; i < randomNumbers2.Length; i++)
        {
            randomNumbers2[i] = Random.value;
            tempRndNosRock2[i] = randomNumbers2[i];
        }
    }

    void Start()
    {
        triggerA = FindObjectOfType(typeof(TriggerA)) as TriggerA;
        inventory = FindObjectOfType(typeof(Inventory)) as Inventory;

        RndNosGenerationRock1();
        RndNosGenerationRock2();

        Screen.showCursor = false;

        // To find Player.Position value.
        GameObject mainPlayerGameObject = GameObject.Find("MainPlayer");
        var mainPlayerPos = mainPlayerGameObject.transform.position;
        playerPosY = mainPlayerPos.y;
        playerPosX = mainPlayerPos.x;
    }

    void OnGUI()
    {
        var mousePos = Input.mousePosition;
        Rect pos = new Rect(mousePos.x - 13, Screen.height - mousePos.y - 11, mouseTexture.width, mouseTexture.height);
        GUI.Label(pos, mouseTexture);
    }

    void Update()
    {
        // Has the maxNumberPlanetTextureA been reached?
        if (currentNumberOfSprites < maxNumberPlanetTextureA)
        {
            DrawSprite();
        }

        // To find Player.Position value.
        GameObject mainPlayerGameObject = GameObject.Find("MainPlayer");
        var mainPlayerPos = mainPlayerGameObject.transform.position;
        playerLocationX = mainPlayerPos.x;
        playerLocationY = mainPlayerPos.y;

        if (triggerA.playerInsideTriggerA == true)
        {
          //  LoadNewSprites();
            if (playerInsideTriggerA)
            {
                // inventory.RemoveItem(0);
                playerInsideTriggerA = false;
            }
        }

        if (currentNumberOfSprites == maxNumberPlanetTextureA)
        {
            if (!stillGenerating)
            {
                for (int i = 0; i < maxNumberPlanetTextureA + 1; i++)
                {
                    allRocks1Generated = GameObject.FindGameObjectsWithTag("Rock1");
                    allRocks2Generated = GameObject.FindGameObjectsWithTag("Rock2");
                    
                    int randomIntNumberWithinRange = (int)Random.Range(0.0F, maxNumberPlanetTextureA);
                    listOfRndNos.Add(randomIntNumberWithinRange);
                    
                    // 90% chance of not rock1 not showing up
                    if (tempRndNosRock1[i] < 0.90F)
                    {
                        DestroyMine(i);
                        // 70% chance of destroying extra rocks2
                        if (tempRndNosRock2[i] < 0.70F)
                        {
                            DestroyMine2(i);
                        }
                        // 30% chance of destorying more extra rocks2
                        else if (tempRndNosRock2[i] > 0.70F)
                        {
                            DestroyMine2(randomIntNumberWithinRange);                            
                        }
                    }
                        //10% chance of rock1 showing up
                        //destroying rock2 where rock1 is not getting destoryed
                    else if (tempRndNosRock1[i] > 0.90F)
                    {
                        DestroyMine2(i);
                    }

                }
                
                stillGenerating = true;
            }
        }

        #region playerMovement
        /*  // Debug
          Debug.Log("playerLocation.X " + playerLocationX + " | playerLocation.Y " + playerLocationY 
                          + " bool value= " + hasThePlayerCrossedTile + "playerPosY: " + playerPosY);     */

        // Has the player travelled a distance of more than 3.0f in Y+ direction.
        /*     if ((playerLocationY - playerPosY) > 3.0f)
             {
                 playerPosY = playerLocationY;
                 hasThePlayerCrossedTile = false;

                 // is hasThePlayerCrossedTile value = false?
                 if (!hasThePlayerCrossedTile)
                 {
                     // is Player going in Y+ direction?
                     if (mainPlayerPos.y == mainPlayerPos.y++)
                     {
                         // Destroy instances created in DrawSprite() call
                         for (int j = 0; j < 32; j++)
                         { Destroy(spritesInstances[j]); }

                         // Reset values
                         numberOfSpriteInstances = 1;      // Set array to 1
                         currentNumberOfSprites = 0;     // triggers DrawSprite(), since it makes currentNumberOfSprites < maxNumberPlanetTextureA
                         spritesCount = 0;   // keeps instances create new rows vertically

                         // Tiles (gameObjects)
                         GameObject plainSurfaceTile = GameObject.Find("PlanetTextureA");
                         GameObject elementsSurfaceTile = GameObject.Find("PlanetTextureB");
                         // Moves the tiles (gameObjects) to new position, i.e. around the Player.Position
                         plainSurfaceTile.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(mainPlayerPos.x - 7.28f, mainPlayerPos.y - 2.879985f, 0);
                         elementsSurfaceTile.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(mainPlayerPos.x - 7.28f, mainPlayerPos.y - 2.879985f, 0);

                         hasThePlayerCrossedTile = true;
                     }
                 }
             }

             else if ((playerLocationY - playerPosY) < -3f)
             {
                 playerPosY = playerLocationY;
                 hasThePlayerCrossedTile = false;

                 if (!hasThePlayerCrossedTile)
                 {

                     if (mainPlayerPos.y == mainPlayerPos.y--)
                     {
                         for (int j = 0; j < 32; j++)
                             Destroy(spritesInstances[j]);

                         numberOfSpriteInstances = 1;
                         currentNumberOfSprites = 0;
                         spritesCount = 0;

                         GameObject plainSurfaceTile = GameObject.Find("PlanetTextureA");
                         GameObject elementsSurfaceTile = GameObject.Find("PlanetTextureB");
                         plainSurfaceTile.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(mainPlayerPos.x - 7.28f, mainPlayerPos.y - 2.879985f, 0);
                         elementsSurfaceTile.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(mainPlayerPos.x - 7.28f, mainPlayerPos.y - 2.879985f, 0);

                         hasThePlayerCrossedTile = true;
                     }
                 }
             }

             else if ((playerLocationX - playerPosX) > 3f)
             {
                 playerPosX = playerLocationX;
                 hasThePlayerCrossedTile = false;

                 if (!hasThePlayerCrossedTile)
                 {

                     if (mainPlayerPos.x == mainPlayerPos.x++)
                     {
                         for (int j = 0; j < 32; j++)
                             Destroy(spritesInstances[j]);

                         numberOfSpriteInstances = 1;
                         currentNumberOfSprites = 0;
                         spritesCount = 0;

                         GameObject plainSurfaceTile = GameObject.Find("PlanetTextureA");
                         GameObject elementsSurfaceTile = GameObject.Find("PlanetTextureB");

                         plainSurfaceTile.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(mainPlayerPos.x - 7.28f, mainPlayerPos.y - 2.879985f, 0);
                         elementsSurfaceTile.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(mainPlayerPos.x - 7.28f, mainPlayerPos.y - 2.879985f, 0);
                         hasThePlayerCrossedTile = true;
                     }
                 }
             }

             else if ((playerLocationX - playerPosX) < -3f)
             {
                 playerPosX = playerLocationX;
                 hasThePlayerCrossedTile = false;

                 if (!hasThePlayerCrossedTile)
                 {
                     if (mainPlayerPos.x == mainPlayerPos.x--)
                     {
                         for (int j = 0; j < 32; j++)
                             Destroy(spritesInstances[j]);

                         numberOfSpriteInstances = 1;
                         currentNumberOfSprites = 0;
                         spritesCount = 0;

                         GameObject plainSurfaceTile = GameObject.Find("PlanetTextureA");
                         GameObject elementsSurfaceTile = GameObject.Find("PlanetTextureB");

                         plainSurfaceTile.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(mainPlayerPos.x - 7.28f, mainPlayerPos.y - 2.879985f, 0);
                         elementsSurfaceTile.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(mainPlayerPos.x - 7.28f, mainPlayerPos.y - 2.879985f, 0);
                         hasThePlayerCrossedTile = true;
                     }
                 }
             }       */
        #endregion
    }

    public void DestroyMine(int pos)
    {
        Destroy(allRocks1Generated[pos]);
    }
    public void DestroyMine2(int pos)
    {
        Destroy(allRocks2Generated[pos]);
    }

    public void DrawSprite()
    {
        // First instance created
        if (currentNumberOfSprites == 0)
        {
            // Clone tile (gameObject)
            spriteGenerated = Instantiate(planetTextureA, transform.position, transform.rotation) as GameObject;

            firstSpriteGenerated = spriteGenerated;
            firstSpriteLocalPositionX = spriteGenerated.transform.localPosition.x;
            firstSpriteLocalPositionY = spriteGenerated.transform.localPosition.y;

            // Add the instance in gameObject array
            spritesInstances[0] = spriteGenerated;

            spritesCount++;
            spriteGenerated.name = "Sprite " + spritesCount;
            currentNumberOfSprites++;
        }

        // Reset instances created
        else
        {
            previousSprite = GameObject.Find("Sprite " + spritesCount);

            // Clone tile (gameObject)
            spriteGenerated = Instantiate(planetTextureA, transform.position, transform.rotation) as GameObject;

            // Add the instances in gameObject array
            spritesInstances[numberOfSpriteInstances] = spriteGenerated;

            if (tempRndNosRock1[numberOfSpriteInstances] < 0.33F)
            {
                spritesInstances[numberOfSpriteInstances].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureC.GetComponentInChildren<SpriteRenderer>().sprite;
                //   spriteGenerated.GetComponentInChildren<SpriteRenderer>().sprite = planetTextureB.GetComponentInChildren<SpriteRenderer>().sprite;
            }
            else if ((tempRndNosRock1[numberOfSpriteInstances] < 0.66F) && (tempRndNosRock1[numberOfSpriteInstances] > 0.33F))
            {
                spritesInstances[numberOfSpriteInstances].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureB.GetComponentInChildren<SpriteRenderer>().sprite;
            }
            else if (tempRndNosRock1[numberOfSpriteInstances] > 0.66F)
            {
                spritesInstances[numberOfSpriteInstances].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureA.GetComponentInChildren<SpriteRenderer>().sprite;
            }

            // Position of newly generated sprite = position of previously generated sprite
            spriteGenerated.transform.localPosition = previousSprite.transform.localPosition;

            // Bounds.extents (half the actual size) in both x & y direction for previousSprite
            var previousSpriteRenderer = previousSprite.GetComponentInChildren<SpriteRenderer>();
            var previousSpriteExtentsX = previousSpriteRenderer.renderer.bounds.extents.x;
            var previousSpriteExtentsY = previousSpriteRenderer.renderer.bounds.extents.y;

            // Bounds.extents (half the actual size) in both x & y direction for spriteGenerated
            var spriteGeneratedRenderer = spriteGenerated.GetComponentInChildren<SpriteRenderer>();
            var spriteGeneratedExtentsX = spriteGeneratedRenderer.renderer.bounds.extents.x;
            var spriteGeneratedExtentsY = spriteGeneratedRenderer.renderer.bounds.extents.y;

            allSpritesGenerated = spriteGenerated;
            allSpritesGeneratedLocalPosX = spriteGenerated.transform.localPosition.x;
            allSpritesGeneratedLocalPosY = spriteGenerated.transform.localPosition.y;

            // Finding the position of lastSritesGenerated. Keep in mind, this data will give wrong information about all other
            // sprites that are being generated, except the last sprite. To find data on other sprites, use values as displayed
            // by allSpritesGenerated variable
            lastSpriteGenerated = spriteGenerated;
            lastSpriteGeneratedLocalPosX = spriteGenerated.transform.localPosition.x + 2 * (spriteGeneratedExtentsX);
            lastSpriteGeneratedLocalPosY = spriteGenerated.transform.localPosition.y;

            // Debug
            /*           Debug.Log("previousSprite.Extents.X " + previousSpriteExtentsX + " | previousSprite.Entents.Y " + spriteGeneratedExtentsX
                           + " | TotalSpritesCount = " + (spritesCount) + " | First Sprite X/Y: " + firstSpriteLocalPositionX + "  " + firstSpriteLocalPositionY
                           + " | All Sprites Generated Pos X/Y: " + allSpritesGeneratedLocalPosX + "  " + allSpritesGeneratedLocalPosY + " | Last Sprite X/Y (ONLY check last value): " 
                            + lastSpriteGeneratedLocalPosX + "  " + lastSpriteGeneratedLocalPosY); */

            // Translations to keep the sprites bounded together in consideration with specific parameters.
            spriteGenerated.transform.Translate(previousSpriteExtentsX + spriteGeneratedExtentsX, 0, 0);

            if (spritesCount == moveRowUpOn)
            {
                var previousSpriteLocalX = previousSprite.transform.localPosition.x;
                spriteGenerated.transform.Translate(-previousSpriteLocalX - 1.92f, (previousSpriteExtentsY + previousSpriteExtentsY), 0);
            }

            spriteGenerated.transform.Translate(0, 0, 0);

            if (spritesCount == 2 * moveRowUpOn)
            {
                var previousSpriteLocalX = previousSprite.transform.localPosition.x;
                spriteGenerated.transform.Translate(-previousSpriteLocalX - 1.92f, (previousSpriteExtentsY + previousSpriteExtentsY), 0);
            }

            if (spritesCount == 3 * moveRowUpOn)
            {
                var previousSpriteLocalX = previousSprite.transform.localPosition.x;
                spriteGenerated.transform.Translate(-previousSpriteLocalX - 1.92f, (previousSpriteExtentsY + previousSpriteExtentsY), 0);
            }

            if (spritesCount == 4 * moveRowUpOn)
            {
                var previousSpriteLocalX = previousSprite.transform.localPosition.x;
                spriteGenerated.transform.Translate(-previousSpriteLocalX - 1.92f, (previousSpriteExtentsY + previousSpriteExtentsY), 0);
            }

            spriteGenerated.transform.Translate(0, 0, 0);

            spritesCount++;
            numberOfSpriteInstances++;
            spriteGenerated.name = "Sprite " + spritesCount;
            currentNumberOfSprites++;
        }
    }

    void LoadNewSprites()
    {
        // is hasThePlayerCrossedTile value = false?
        if (!hasThePlayerCrossedTile)
        {
            // Destroy instances created in DrawSprite() call
            for (int j = 0; j < 32; j++)
            { Destroy(spritesInstances[j]); }

            // Reset values
            numberOfSpriteInstances = 1;        // Set array to 1
            currentNumberOfSprites = 0;         // triggers DrawSprite(), since it makes currentNumberOfSprites < maxNumberPlanetTextureA
            spritesCount = 0;                   // keeps instances create new rows vertically

            // Moves the tiles (gameObjects) to new position, i.e. around the Player.Position
            planetTextureA.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(0F, 4.0F, 0);
            planetTextureB.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(0F, 4.0F, 0);

            hasThePlayerCrossedTile = true;
        }

    }
}