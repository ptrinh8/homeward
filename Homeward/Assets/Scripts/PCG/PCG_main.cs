// ==================================================================================
// <file="PCG_main.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PCG_main : MonoBehaviour
{
    private PlayerController playerController = new PlayerController();
    private PCG_TriggerA triggerA = new PCG_TriggerA();
    private PCG_Rand rand = new PCG_Rand();

    public GameObject planetTextureA, planetTextureB, planetTextureC, planetTextureD;
    public int maxNumberPlanetTextureA;

    private float playerLocationX, playerLocationY;

    private GameObject[] spritesInstances = new GameObject[10000];
    private GameObject spriteGenerated, previousSprite;                                         

    private int spritesCount = 0;
    private const int moveRowUpOn = 70;                                         

    private GameObject firstSpriteGenerated;                                    
    private float firstSpriteLocalPositionX, firstSpriteLocalPositionY;

    private GameObject allSpritesGenerated;                                     
    private float allSpritesGeneratedLocalPosX, allSpritesGeneratedLocalPosY;

    private GameObject lastSpriteGenerated;
    private float lastSpriteGeneratedLocalPosX, lastSpriteGeneratedLocalPosY;

    private GameObject[] allRocks1Generated = new GameObject[10000];
    private GameObject[] allRocks2Generated = new GameObject[10000];
    private float playerPosX, playerPosY;

    private bool doOnce = false;
    private bool activateTriggerAgain = false;

    private GameObject[] rocks1;
    private GameObject[] rocks2;

    void TriggerA_Impact()
    {
        if (triggerA.playerInsideTriggerA == true)
        {
            if (!doOnce)
            {   
                int temp = maxNumberPlanetTextureA;
                maxNumberPlanetTextureA += 140;
                triggerA.transform.position += new Vector3(0, 6, 0);
                activateTriggerAgain = true;
                doOnce = true;     
            }
        }
        if (triggerA.playerExitedTriggerA == true)
        {
            if (activateTriggerAgain)
            {
                doOnce = false;
                activateTriggerAgain = false;
            }
        }
    }

    void changeTilesTextures()
    {
        if (rand.tempRndNosRock1[spritesCount] < 0.25F)
        {
            spritesInstances[spritesCount].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureC.GetComponentInChildren<SpriteRenderer>().sprite;
        }
        else if ((rand.tempRndNosRock1[spritesCount] < 0.50F) && (rand.tempRndNosRock1[spritesCount] > 0.25F))
        {
            spritesInstances[spritesCount].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureB.GetComponentInChildren<SpriteRenderer>().sprite;
        }
        else if (rand.tempRndNosRock1[spritesCount] < 0.75F && (rand.tempRndNosRock1[spritesCount] > 0.50F))
        {
            spritesInstances[spritesCount].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureA.GetComponentInChildren<SpriteRenderer>().sprite;
        }
        else if (rand.tempRndNosRock1[spritesCount] > 0.75F)
        {
            spritesInstances[spritesCount].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureD.GetComponentInChildren<SpriteRenderer>().sprite;
        }
    }

    void Start()
    {
        triggerA = FindObjectOfType(typeof(PCG_TriggerA)) as PCG_TriggerA;
        rand = FindObjectOfType(typeof(PCG_Rand)) as PCG_Rand;

        GameObject mainPlayerGameObject = GameObject.Find("MainPlayer");       
        var mainPlayerPos = mainPlayerGameObject.transform.position;
        playerPosY = mainPlayerPos.y;
        playerPosX = mainPlayerPos.x;
    }

    void Update()
    {
        allRocks1Generated = GameObject.FindGameObjectsWithTag("Rock1");
        allRocks2Generated = GameObject.FindGameObjectsWithTag("Rock2");

        TriggerA_Impact();

        GameObject mainPlayerGameObject = GameObject.Find("MainPlayer");       
        var mainPlayerPos = mainPlayerGameObject.transform.position;
        playerLocationX = mainPlayerPos.x;
        playerLocationY = mainPlayerPos.y;

        if (spritesCount < maxNumberPlanetTextureA)                   
        {
            DrawSprite();
        }
    }
    
    public void DrawSprite()
    {
        if (spritesCount == 0)                                       
        {
            spriteGenerated = Instantiate(planetTextureA, transform.position, transform.rotation) as GameObject;                       
            spritesInstances[0] = spriteGenerated;                                                                                     

            firstSpriteGenerated = spriteGenerated;
            firstSpriteLocalPositionX = spriteGenerated.transform.localPosition.x;
            firstSpriteLocalPositionY = spriteGenerated.transform.localPosition.y;

            spritesCount++;
            spriteGenerated.name = "Sprite " + spritesCount;
        }

        else
        {
            previousSprite = GameObject.Find("Sprite " + spritesCount);

            spriteGenerated = Instantiate(planetTextureA, transform.position, transform.rotation) as GameObject;
            spritesInstances[spritesCount] = spriteGenerated;

            changeTilesTextures();

            spriteGenerated.transform.localPosition = previousSprite.transform.localPosition;               

            var previousSpriteRenderer = previousSprite.GetComponentInChildren<SpriteRenderer>();          
            var previousSpriteExtentsX = previousSpriteRenderer.renderer.bounds.extents.x;
            var previousSpriteExtentsY = previousSpriteRenderer.renderer.bounds.extents.y;

            var spriteGeneratedRenderer = spriteGenerated.GetComponentInChildren<SpriteRenderer>();        
            var spriteGeneratedExtentsX = spriteGeneratedRenderer.renderer.bounds.extents.x;
            var spriteGeneratedExtentsY = spriteGeneratedRenderer.renderer.bounds.extents.y;

            allSpritesGenerated = spriteGenerated;
            allSpritesGeneratedLocalPosX = spriteGenerated.transform.localPosition.x;
            allSpritesGeneratedLocalPosY = spriteGenerated.transform.localPosition.y;

            lastSpriteGenerated = spriteGenerated;                                                          
            lastSpriteGeneratedLocalPosX = spriteGenerated.transform.localPosition.x + 2 * (spriteGeneratedExtentsX);
            lastSpriteGeneratedLocalPosY = spriteGenerated.transform.localPosition.y;

            // While using smaller sized tiles (192x192)
               spriteGenerated.transform.Translate(previousSpriteExtentsX + spriteGeneratedExtentsX, 0, 0);     // Translations to keep the sprites bounded together in consideration with specific parameters.

            #region Debug: To Check Position of Tiles Generated
            // Debug
            /*           Debug.Log("previousSprite.Extents.X " + previousSpriteExtentsX + " | previousSprite.Entents.Y " + spriteGeneratedExtentsX
                           + " | TotalSpritesCount = " + (spritesCount) + " | First Sprite X/Y: " + firstSpriteLocalPositionX + "  " + firstSpriteLocalPositionY
                           + " | All Sprites Generated Pos X/Y: " + allSpritesGeneratedLocalPosX + "  " + allSpritesGeneratedLocalPosY + " | Last Sprite X/Y (ONLY check last value): " 
                            + lastSpriteGeneratedLocalPosX + "  " + lastSpriteGeneratedLocalPosY); */
            #endregion

            for (int i = 0; i < 500; i++)
            {
                if (spritesCount == i * moveRowUpOn)
                {
                    var previousSpriteLocalX = previousSprite.transform.localPosition.x;
                    spriteGenerated.transform.Translate(-previousSpriteLocalX - 1.92f, (previousSpriteExtentsY + previousSpriteExtentsY), 0);
                }
            }

            spritesCount++;
            spriteGenerated.name = "Sprite " + spritesCount;
        }
    }
}