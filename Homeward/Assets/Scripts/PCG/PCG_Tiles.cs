// ==================================================================================
// <file="PCG_Tiles.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PCG_Tiles : MonoBehaviour
{
    PCG_SurfaceElements pcg_surfaceElements = new PCG_SurfaceElements();
    PCG_Rocks pcg_rocks = new PCG_Rocks();
    PCG_TriggerA trigger = new PCG_TriggerA();
    PCG_TriggerB trigger1 = new PCG_TriggerB();
    PCG_TriggerC trigger2 = new PCG_TriggerC();
    PCG_TriggerD trigger3 = new PCG_TriggerD();

    public GameObject _tile;
    public Sprite _tileA, _tileB, _tileC,
    _tileD, _tileE, _tileF, _tileG,
    _tileH, _tileI, _tileJ, _tileK,
    _tileL, _tileM, _tileN, _tileO,
    _tileP, _tileQ, _tileR, _tileS,
    _tileT;

    public float areaSpan = 50.0F;

    [HideInInspector]
    public int[] extractableValue = new int[11071];

    float[] _x = new float[11071];
    float[] _y = new float[11071];
    bool addRemoveTiles = false;
    bool initialFunctionCall = false;
    bool moveTrigger = false;

    PCG_Rand rand = new PCG_Rand();

    int vertiLmtINC = 0;
    int vertiLmtINC_again = 0;
    int vertiLmt = 90;
    float _Yincrement = 0;
    float _Xincrement = 1;

    bool triggerEntered = false;

    GameObject[] tiles = new GameObject[11071];
    float distBWplayerTiles;

    public int[] ExtractableValues
    {
        get { return extractableValue; }
        set { extractableValue = value; }
    }

    public void RndNosGeneration()
    {
        for (int i = 0; i < 11071; i++)
        {
            _y[i] = 5.12F + _Yincrement;
            _Yincrement = _Yincrement + 5.12F;

            if (i == vertiLmt || i == vertiLmt + vertiLmtINC)
            {
                vertiLmtINC = vertiLmtINC + vertiLmt;
                _Yincrement = 0;
            }
        }

        for (int j = 0; j < 11071; j++)
        {
            _x[j] = 5.12F + _Xincrement;

            if (j == vertiLmt || j == vertiLmt + vertiLmtINC_again)
            {
                vertiLmtINC_again = vertiLmtINC_again + vertiLmt;
                _Xincrement = _Xincrement + 5.12F;
            }
        }
    }

    void AddRemoveTiles()
    {
        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;

        Debug.Log("Outside Generate Content");

        if (triggerEntered == true || initialFunctionCall == false)
        {
            initialFunctionCall = true;
            if (!addRemoveTiles)
            {
                Debug.Log("Generating Content...");
                addRemoveTiles = true;
                triggerEntered = false;
                moveTrigger = false;

                GameObject[] gameObjectTile = GameObject.FindGameObjectsWithTag("FinalTextures");
                List<GameObject> gameObjectTileList = gameObjectTile.ToList();
                List<GameObject> finalList = gameObjectTileList.Distinct().ToList();

                for (int i = 0; i < 11071; i++)
                {
                    distBWplayerTiles = Vector2.Distance(new Vector2(_x[i], _y[i]), mainPlayerPos);
                    if (distBWplayerTiles < areaSpan)
                    {
                        tiles[i] = Instantiate(_tile, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                        tiles[i].tag = "FinalTextures";
                        tiles[i].name = "FinalTextures " + i;
                        extractableValue[i] = Random.Range(0, 10);

                        if (tiles[i].name == "FinalTextures 90") { tiles[i].GetComponentInChildren<SpriteRenderer>().enabled = false; }

                        switch (RandomNosRangeToExactValues(i))
                        {
                            case -1: Debug.LogError("RndNosOutOfBounds"); break;
                            case 0: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileA; break;
                            case 1: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileB; break;
                            case 2: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileC; break;
                            case 3: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileD; break;
                            case 4: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileE; break;
                            case 5: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileF; break;
                            case 6: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileG; break;
                            case 7: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileH; break;
                            case 8: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileI; break;
                            case 9: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileJ; break;
                            case 10: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileK; break;
                            case 11: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileL; break;
                            case 12: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileM; break;
                            case 13: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileN; break;
                            case 14: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileO; break;
                            case 15: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileP; break;
                            case 16: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileQ; break;
                            case 17: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileR; break;
                            case 18: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileS; break;
                            case 19: tiles[i].GetComponentInChildren<SpriteRenderer>().sprite = _tileT; break;
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
                    if (distance2 > areaSpan)
                    {
                        GameObject.Destroy(texture);
                    }
                }
            }
        }
    }

    int RandomNosRangeToExactValues(int pos)
    {
        if ((rand.seedRndNos_spawning[pos] <= 0.05F) && (rand.seedRndNos_spawning[pos] >= 0.0F)) return 0; if ((rand.seedRndNos_spawning[pos] <= 0.10f) && (rand.seedRndNos_spawning[pos] >= 0.05f)) return 1;
        if ((rand.seedRndNos_spawning[pos] <= 0.15f) && (rand.seedRndNos_spawning[pos] >= 0.10f)) return 2; if ((rand.seedRndNos_spawning[pos] <= 0.20f) && (rand.seedRndNos_spawning[pos] >= 0.15f)) return 3;
        if ((rand.seedRndNos_spawning[pos] <= 0.25f) && (rand.seedRndNos_spawning[pos] >= 0.20f)) return 4; if ((rand.seedRndNos_spawning[pos] <= 0.30f) && (rand.seedRndNos_spawning[pos] >= 0.25f)) return 5;
        if ((rand.seedRndNos_spawning[pos] <= 0.35f) && (rand.seedRndNos_spawning[pos] >= 0.30f)) return 6; if ((rand.seedRndNos_spawning[pos] <= 0.40f) && (rand.seedRndNos_spawning[pos] >= 0.35f)) return 7;
        if ((rand.seedRndNos_spawning[pos] <= 0.45f) && (rand.seedRndNos_spawning[pos] >= 0.40f)) return 8; if ((rand.seedRndNos_spawning[pos] <= 0.50f) && (rand.seedRndNos_spawning[pos] >= 0.45f)) return 9;
        if ((rand.seedRndNos_spawning[pos] <= 0.55f) && (rand.seedRndNos_spawning[pos] >= 0.50f)) return 10; if ((rand.seedRndNos_spawning[pos] <= 0.60f) && (rand.seedRndNos_spawning[pos] >= 0.55f)) return 11;
        if ((rand.seedRndNos_spawning[pos] <= 0.65f) && (rand.seedRndNos_spawning[pos] >= 0.60f)) return 12; if ((rand.seedRndNos_spawning[pos] <= 0.70f) && (rand.seedRndNos_spawning[pos] >= 0.65f)) return 13;
        if ((rand.seedRndNos_spawning[pos] <= 0.75f) && (rand.seedRndNos_spawning[pos] >= 0.70f)) return 14; if ((rand.seedRndNos_spawning[pos] <= 0.80f) && (rand.seedRndNos_spawning[pos] >= 0.75f)) return 15;
        if ((rand.seedRndNos_spawning[pos] <= 0.85f) && (rand.seedRndNos_spawning[pos] >= 0.80f)) return 16; if ((rand.seedRndNos_spawning[pos] <= 0.90f) && (rand.seedRndNos_spawning[pos] >= 0.85f)) return 17;
        if ((rand.seedRndNos_spawning[pos] <= 0.95f) && (rand.seedRndNos_spawning[pos] >= 0.90f)) return 18; if ((rand.seedRndNos_spawning[pos] <= 1.00f) && (rand.seedRndNos_spawning[pos] >= 0.95f)) return 19;
        else return -1;
    }

    void RepositionTriggers()
    {
        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;
        Debug.Log("Outside Reposition Triggers");
        if (trigger.playerInsideCircleTriggerA == true)
        {
            if (!moveTrigger)
            {
                Debug.Log("A");
                moveTrigger = true;
                addRemoveTiles = false;
                triggerEntered = true;
                trigger.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 2.5F);
                trigger1.transform.position = new Vector2(mainPlayerPos.x - 2.5F, mainPlayerPos.y);
                trigger2.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 2.5F);
                trigger3.transform.position = new Vector2(mainPlayerPos.x + 2.5F, mainPlayerPos.y);
                pcg_rocks.moveTrigger = true;
                pcg_rocks.addRemoveTiles = false;
                pcg_rocks.triggerEntered = true;
                pcg_surfaceElements.moveTrigger = true;
                pcg_surfaceElements.addRemoveTiles = false;
                pcg_surfaceElements.triggerEntered = true;
            }
        }
        if (trigger1.playerInsideCircleTriggerA1 == true)
        {
            if (!moveTrigger)
            {
                Debug.Log("B");
                moveTrigger = true;
                addRemoveTiles = false;
                triggerEntered = true;
                trigger.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 2.5F);
                trigger1.transform.position = new Vector2(mainPlayerPos.x - 2.5F, mainPlayerPos.y);
                trigger2.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 2.5F);
                trigger3.transform.position = new Vector2(mainPlayerPos.x + 2.5F, mainPlayerPos.y);
                pcg_rocks.moveTrigger = true;
                pcg_rocks.addRemoveTiles = false;
                pcg_rocks.triggerEntered = true;
                pcg_surfaceElements.moveTrigger = true;
                pcg_surfaceElements.addRemoveTiles = false;
                pcg_surfaceElements.triggerEntered = true;
            }
        }
        if (trigger2.playerInsideCircleTriggerA2 == true)
        {
            if (!moveTrigger)
            {
                Debug.Log("C");
                moveTrigger = true;
                addRemoveTiles = false;
                triggerEntered = true;
                trigger.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 2.5F);
                trigger1.transform.position = new Vector2(mainPlayerPos.x - 2.5F, mainPlayerPos.y);
                trigger2.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 2.5F);
                trigger3.transform.position = new Vector2(mainPlayerPos.x + 2.5F, mainPlayerPos.y);
                pcg_rocks.moveTrigger = true;
                pcg_rocks.addRemoveTiles = false;
                pcg_rocks.triggerEntered = true;
                pcg_surfaceElements.moveTrigger = true;
                pcg_surfaceElements.addRemoveTiles = false;
                pcg_surfaceElements.triggerEntered = true;
            }
        }
        if (trigger3.playerInsideCircleTriggerA3 == true)
        {
            if (!moveTrigger)
            {
                Debug.Log("D");
                moveTrigger = true;
                addRemoveTiles = false;
                triggerEntered = true;
                trigger.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 2.5F);
                trigger1.transform.position = new Vector2(mainPlayerPos.x - 2.5F, mainPlayerPos.y);
                trigger2.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 2.5F);
                trigger3.transform.position = new Vector2(mainPlayerPos.x + 2.5F, mainPlayerPos.y);
                pcg_rocks.moveTrigger = true;
                pcg_rocks.addRemoveTiles = false;
                pcg_rocks.triggerEntered = true;
                pcg_surfaceElements.moveTrigger = true;
                pcg_surfaceElements.addRemoveTiles = false;
                pcg_surfaceElements.triggerEntered = true;
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
        pcg_surfaceElements = FindObjectOfType(typeof(PCG_SurfaceElements)) as PCG_SurfaceElements;
        RndNosGeneration();

        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;

        trigger.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 2.5F);
        trigger1.transform.position = new Vector2(mainPlayerPos.x - 2.5F, mainPlayerPos.y);
        trigger2.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 2.5F);
        trigger3.transform.position = new Vector2(mainPlayerPos.x + 2.5F, mainPlayerPos.y);
    }

    void Update()
    {
        AddRemoveTiles();
        RepositionTriggers();
        Debug.Log("MoveTrigger: " + moveTrigger);
        Debug.Log("AddRemove: " + addRemoveTiles);
    }
}
