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
    private PCG_Rocks pcg_rocks = new PCG_Rocks();

    public GameObject tile;
    public GameObject _tileA, _tileB, _tileC, 
    _tileD, _tileE, _tileF, _tileG, 
    _tileH, _tileI, _tileJ, _tileK, 
    _tileL, _tileM, _tileN, _tileO, 
    _tileP, _tileQ, _tileR, _tileS, 
    _tileT;

    private int[] _x = new int[11071];
    private int[] _y = new int[11071];
    private bool addRemoveTiles = false;
    private bool initialFunctionCall = false;
    private bool moveTrigger = false;
    private bool onceOnceOnce = false;

    private PCG_Rand rand = new PCG_Rand();
    private float increm = 33.0F;

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

                            switch (RandomNosRangeToExactValues(i))
                            {
                                case -1: Debug.LogError("RndNosOutOfBounds"); break;;
                                case 0: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileA.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 1: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileB.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 2: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileC.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 3: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileD.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 4: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileE.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 5: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileF.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 6: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileG.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 7: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileH.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 8: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileI.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 9: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileJ.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 10: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileK.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 11: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileL.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 12: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileM.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 13: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileN.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 14: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileO.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 15: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileP.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 16: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileQ.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 17: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileR.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 18: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileS.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                case 19: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileT.GetComponentInChildren<SpriteRenderer>().sprite; break;;
                                default: break;
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

    int RandomNosRangeToExactValues(int pos)
    {
        if ((rand.tempRndNosRock1[pos] < 0.10f) && (rand.tempRndNosRock1[pos] > 0.05f)) return 1;
        if ((rand.tempRndNosRock1[pos] < 0.15f) && (rand.tempRndNosRock1[pos] > 0.10f)) return 2;
        if ((rand.tempRndNosRock1[pos] < 0.20f) && (rand.tempRndNosRock1[pos] > 0.15f)) return 3;
        if ((rand.tempRndNosRock1[pos] < 0.25f) && (rand.tempRndNosRock1[pos] > 0.20f)) return 4;
        if ((rand.tempRndNosRock1[pos] < 0.30f) && (rand.tempRndNosRock1[pos] > 0.25f)) return 5;
        if ((rand.tempRndNosRock1[pos] < 0.35f) && (rand.tempRndNosRock1[pos] > 0.30f)) return 6;
        if ((rand.tempRndNosRock1[pos] < 0.40f) && (rand.tempRndNosRock1[pos] > 0.35f)) return 7;
        if ((rand.tempRndNosRock1[pos] < 0.45f) && (rand.tempRndNosRock1[pos] > 0.40f)) return 8;
        if ((rand.tempRndNosRock1[pos] < 0.50f) && (rand.tempRndNosRock1[pos] > 0.45f)) return 9;
        if ((rand.tempRndNosRock1[pos] < 0.55f) && (rand.tempRndNosRock1[pos] > 0.50f)) return 10;
        if ((rand.tempRndNosRock1[pos] < 0.60f) && (rand.tempRndNosRock1[pos] > 0.55f)) return 11;
        if ((rand.tempRndNosRock1[pos] < 0.65f) && (rand.tempRndNosRock1[pos] > 0.60f)) return 12;
        if ((rand.tempRndNosRock1[pos] < 0.70f) && (rand.tempRndNosRock1[pos] > 0.65f)) return 13;
        if ((rand.tempRndNosRock1[pos] < 0.75f) && (rand.tempRndNosRock1[pos] > 0.70f)) return 14;
        if ((rand.tempRndNosRock1[pos] < 0.80f) && (rand.tempRndNosRock1[pos] > 0.75f)) return 15;
        if ((rand.tempRndNosRock1[pos] < 0.85f) && (rand.tempRndNosRock1[pos] > 0.80f)) return 16;
        if ((rand.tempRndNosRock1[pos] < 0.90f) && (rand.tempRndNosRock1[pos] > 0.85f)) return 17;
        if ((rand.tempRndNosRock1[pos] < 0.95f) && (rand.tempRndNosRock1[pos] > 0.90f)) return 18;
        if ((rand.tempRndNosRock1[pos] < 0.100f) && (rand.tempRndNosRock1[pos] > 0.95f)) return 19;
        return 0;
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
        pcg_rocks = FindObjectOfType(typeof(PCG_Rocks)) as PCG_Rocks;
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
