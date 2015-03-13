// ==================================================================================
// <file="PCG_sprites.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PCG_Tiles : MonoBehaviour 
{
    private PCG_TriggerA trigger = new PCG_TriggerA();
    private PCG_TriggerB trigger1 = new PCG_TriggerB();
    private PCG_TriggerC trigger2 = new PCG_TriggerC();
    private PCG_TriggerD trigger3 = new PCG_TriggerD();
    private PCG_rocks pcg_rocks = new PCG_rocks();

    public GameObject tile;
    public GameObject planetTextureA, planetTextureB, planetTextureC, planetTextureD;
    private int[] _x = new int[11071];
    private int[] _y = new int[11071];
    private bool addRemoveTiles = false;
    private bool initialFunctionCall = false;
    private bool moveTrigger = false;
    private bool onceOnceOnce = false;

    private PCG_Rand rand = new PCG_Rand();

    int lowerLmt_Y = 0;
    int lowerLmt_X = 1;
    int vertiLmtINC = 0;
    int vertiLmtINC_again = 0;
    int vertiLmt = 90;
    int _Yincrement = 0;
    int _Xincrement = 1;

    private bool triggerEntered = false;

    private GameObject[] tiles = new GameObject[11071];

    public void RndNosGeneration()
    {
        for (int i = 0; i < 11071; i++)
        {
            _y[i] = 5 + _Yincrement;
            _Yincrement = _Yincrement + 5;

            if (i == vertiLmt || i == vertiLmt+vertiLmtINC)
            {
                vertiLmtINC = vertiLmtINC + vertiLmt;
                _Yincrement = 0;
            }
        }

        for (int j = 0; j < 11071; j++)
        {
            _x[j] = 5 + _Xincrement;

            if (j == vertiLmt || j == vertiLmt + vertiLmtINC_again)
            {
                vertiLmtINC_again = vertiLmtINC_again + vertiLmt;
                _Xincrement = _Xincrement + 5;
            }
        }
    }

    private void AddRemoveTiles()
    {
        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;

        if (triggerEntered == true || initialFunctionCall == false)
        {
            initialFunctionCall = true;
            if (!addRemoveTiles)
            {
                addRemoveTiles = true;
                triggerEntered = false;
                moveTrigger = false;

                GameObject[] gameObjectTile = GameObject.FindGameObjectsWithTag("FinalTextures");
                List<GameObject> gameObjectTileList = gameObjectTile.ToList();
                List<GameObject> finalList = gameObjectTileList.Distinct().ToList();


                for (int i = 0; i < 11071; i++)
                {
                        var distance = Vector2.Distance(new Vector2(_x[i], _y[i]), mainPlayerPos);
                        if (distance < 20.0F)
                        {
                            tiles[i] = Instantiate(tile, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                            tiles[i].tag = "FinalTextures";
                            tiles[i].name = "FinalTextures " + i;

                            if (tiles[i].name == "FinalTextures 90")
                            {
                                tiles[i].GetComponentInChildren<SpriteRenderer>().enabled = false;
                            }

                            if ((rand.tempRndNosRock1[i] < 0.33F) && (rand.tempRndNosRock1[i] > 0.00F))
                            {
                                tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureB.GetComponentInChildren<SpriteRenderer>().sprite;
                            }
                            else if (rand.tempRndNosRock1[i] < 0.66F && (rand.tempRndNosRock1[i] > 0.33F))
                            {
                                tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureA.GetComponentInChildren<SpriteRenderer>().sprite;
                            }
                            else if (rand.tempRndNosRock1[i] > 0.66F)
                            {
                                tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureD.GetComponentInChildren<SpriteRenderer>().sprite;
                            }

                            foreach (GameObject item in finalList)
                            {
                                if (item.name == tiles[i].name)
                                {
                                    Destroy(tiles[i]);
                                }
                            }
                        }

                }

                GameObject[] allTexturesMyFriend = GameObject.FindGameObjectsWithTag("FinalTextures");
                foreach (GameObject texture in allTexturesMyFriend)
                {
                    var distance2 = Vector2.Distance(texture.transform.position, mainPlayerPos);
                    if (distance2 > 20.0F)
                    {
                        GameObject.Destroy(texture);
                    }
                }
            }
        }
    }

    private void RepositionTriggers()
    {
        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;

        if (trigger.playerInsideCircleTriggerA == true)
        {
            if (!moveTrigger)
            {
                moveTrigger = true;
                addRemoveTiles = false;
                triggerEntered = true;
                trigger.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 5.0F);
                trigger1.transform.position = new Vector2(mainPlayerPos.x - 5.0F, mainPlayerPos.y);
                trigger2.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 5.0F);
                trigger3.transform.position = new Vector2(mainPlayerPos.x + 5.0F, mainPlayerPos.y);
                pcg_rocks.moveTrigger = true;
                pcg_rocks.addRemoveTiles = false;
                pcg_rocks.triggerEntered = true;
            }
        }
        if (trigger1.playerInsideCircleTriggerA1 == true)
        {
            if (!moveTrigger)
            {
                moveTrigger = true;
                addRemoveTiles = false;
                triggerEntered = true;
                onceOnceOnce = true;
                trigger.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 5.0F);
                trigger1.transform.position = new Vector2(mainPlayerPos.x - 5.0F, mainPlayerPos.y);
                trigger2.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 5.0F);
                trigger3.transform.position = new Vector2(mainPlayerPos.x + 5.0F, mainPlayerPos.y);
                pcg_rocks.moveTrigger = true;
                pcg_rocks.addRemoveTiles = false;
                pcg_rocks.triggerEntered = true;
            }
        }
        if (trigger2.playerInsideCircleTriggerA2 == true)
        {
            if (!moveTrigger)
            {
                moveTrigger = true;
                addRemoveTiles = false;
                triggerEntered = true;
                trigger.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 5.0F);
                trigger1.transform.position = new Vector2(mainPlayerPos.x - 5.0F, mainPlayerPos.y);
                trigger2.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 5.0F);
                trigger3.transform.position = new Vector2(mainPlayerPos.x + 5.0F, mainPlayerPos.y);
                pcg_rocks.moveTrigger = true;
                pcg_rocks.addRemoveTiles = false;
                pcg_rocks.triggerEntered = true;
            }
        }
        if (trigger3.playerInsideCircleTriggerA3 == true)
        {
            if (!moveTrigger)
            {
                moveTrigger = true;
                addRemoveTiles = false;
                triggerEntered = true;
                trigger.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 5.0F);
                trigger1.transform.position = new Vector2(mainPlayerPos.x - 5.0F, mainPlayerPos.y);
                trigger2.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 5.0F);
                trigger3.transform.position = new Vector2(mainPlayerPos.x + 5.0F, mainPlayerPos.y);
                pcg_rocks.moveTrigger = true;
                pcg_rocks.addRemoveTiles = false;
                pcg_rocks.triggerEntered = true;
            }
        }
    }

    void Start() 
    {
        rand = FindObjectOfType(typeof(PCG_Rand)) as PCG_Rand;
        trigger = FindObjectOfType(typeof(PCG_TriggerA)) as PCG_TriggerA;
        trigger1 = FindObjectOfType(typeof(PCG_TriggerB)) as PCG_TriggerB;
        trigger2 = FindObjectOfType(typeof(PCG_TriggerC)) as PCG_TriggerC;
        trigger3 = FindObjectOfType(typeof(PCG_TriggerD)) as PCG_TriggerD;
        pcg_rocks = FindObjectOfType(typeof(PCG_rocks)) as PCG_rocks;
        RndNosGeneration();	

        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;

        trigger.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 5.0F);
        trigger1.transform.position = new Vector2(mainPlayerPos.x - 5.0F, mainPlayerPos.y);
        trigger2.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 5.0F);
        trigger3.transform.position = new Vector2(mainPlayerPos.x + 5.0F, mainPlayerPos.y);

	}
	
	void Update () 
    {
        AddRemoveTiles();
        RepositionTriggers();
	}
}
