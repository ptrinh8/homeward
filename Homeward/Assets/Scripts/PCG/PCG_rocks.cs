// ==================================================================================
// <file="PCG_Rocks.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PCG_Rocks : MonoBehaviour
{
    PCG_Rand rand = new PCG_Rand();
    Mining mining = new Mining();

    public GameObject rock;
    public Sprite _rockA, _rockB, _rockC,
    _rockD, _rockE, _rockF, _rockG, _rockH;
    int[] _x = new int[2040];
    int[] _y = new int[2040];

    float areaSpan = 10.0F;
    public bool RocksIntensityRegular = false, RocksIntensityHigh = false, RocksIntensityExtreme = false;

    [HideInInspector]
    public bool triggerEntered = false;
    [HideInInspector]
    public bool addRemoveTiles = false;
    [HideInInspector]
    public bool initialFunctionCall = false;
    [HideInInspector]
    public bool moveTrigger = false;

    public bool TriggerEntered_Property { get { return triggerEntered; } set { triggerEntered = value; } }
    public bool AddRemoveTiles_Property { get { return addRemoveTiles; } set { addRemoveTiles = value; } }
    public bool InitialFunctionCall_Property { get { return initialFunctionCall; } set { initialFunctionCall = value; } }
    public bool MoveTrigger_Property { get { return moveTrigger; } set { moveTrigger = value; } }

    int jj = 0;
    int ii = 0;
    int xx = 0;
    int hh = 0;

    GameObject[] rocks = new GameObject[2040];

    public void RndNosGeneration()
    {

        #region OldTestament
        //for (int i = 0; i < 820; i++)
        //{
        //    _y[i] = Random.Range(jj, jj + 10);
        //    jj = jj + 15;
        //    if (i == 20 || i == 30 + xx) { xx = xx + 30; jj = 0; }
        //}
        //for (int j = 0; j < 820; j++)
        //{
        //    _x[j] = Random.Range(ii, ii + 9);
        //    if (j == 20 || j == 20 + hh) { hh = hh + 20; ii = ii + 15; }
        //}
        #endregion

        if (RocksIntensityRegular == true)
        {

            for (int i = 0; i < 820; i++)
            {
                _y[i] = Random.Range(jj, jj + 40);                              //RND value y-grid
                jj = jj + 8;
                if (i == 60 || i == 60 + xx) { xx = xx + 60; jj = 0; }
            }

            for (int j = 0; j < 820; j++)
            {
                _x[j] = Random.Range(ii, ii + 40);                              //RND value x-grid
                if (j == 10 || j == 10 + hh) { hh = hh + 10; ii = ii + 8; }     //less j = wider x-axis
            }
        }

        else if (RocksIntensityHigh == true)
        {
            for (int i = 0; i < 1640; i++)
            {
                _y[i] = Random.Range(jj, jj + 40);
                jj = jj + 8;
                if (i == 60 || i == 60 + xx) { xx = xx + 60; jj = 0; }
            }

            for (int j = 0; j < 1640; j++)
            {
                _x[j] = Random.Range(ii, ii + 40);
                if (j == 20 || j == 20 + hh) { hh = hh + 20; ii = ii + 8; }
            }
        }

        else if (RocksIntensityExtreme == true)
        {
            for (int i = 0; i < 2040; i++)
            {
                _y[i] = Random.Range(jj, jj + 40);
                jj = jj + 8;
                if (i == 60 || i == 60 + xx) { xx = xx + 60; jj = 0; }
            }

            for (int j = 0; j < 2040; j++)
            {
                _x[j] = Random.Range(ii, ii + 40);
                if (j == 25 || j == 25 + hh) { hh = hh + 25; ii = ii + 8; }
            }
        }
    }

    void AddRemoveTiles()
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

                GameObject[] gameObjectTile = GameObject.FindGameObjectsWithTag("RocksOnScreen");
                List<GameObject> gameObjectTileList = gameObjectTile.ToList();
                List<GameObject> finalList = gameObjectTileList.Distinct().ToList();

                #region GenerateAllTilesTogether
                //if (RocksIntensityRegular == true)
                //{

                //    for (int i = 0; i < 820; i++)
                //    {
                //        var distance = Vector2.Distance(new Vector2(_x[i], _y[i]), mainPlayerPos);
                //        rocks[i] = Instantiate(rock, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                //        rocks[i].tag = "RocksOnScreen";
                //        rocks[i].name = "Rocks " + i;

                //        ChangeSprites(i);

                //        if (rand.seedRndNos_spawning[i] < 0.25F) { }
                //        else if ((rand.seedRndNos_spawning[i] < 0.50F) && (rand.seedRndNos_spawning[i] > 0.25F)) { }
                //        else if (rand.seedRndNos_spawning[i] < 0.75F && (rand.seedRndNos_spawning[i] > 0.50F)) { }
                //        else if (rand.seedRndNos_spawning[i] > 0.75F) { }

                //    }
                //}
                //else if (RocksIntensityHigh == true)
                //{
                //    for (int i = 0; i < 1640; i++)
                //    {
                //        var distance = Vector2.Distance(new Vector2(_x[i], _y[i]), mainPlayerPos);
                //        rocks[i] = Instantiate(rock, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                //        rocks[i].tag = "RocksOnScreen";
                //        rocks[i].name = "Rocks " + i;

                //        ChangeSprites(i);

                //        if (rand.seedRndNos_spawning[i] < 0.25F) { }
                //        else if ((rand.seedRndNos_spawning[i] < 0.50F) && (rand.seedRndNos_spawning[i] > 0.25F)) { }
                //        else if (rand.seedRndNos_spawning[i] < 0.75F && (rand.seedRndNos_spawning[i] > 0.50F)) { }
                //        else if (rand.seedRndNos_spawning[i] > 0.75F) { }
                //    }
                //}
                //else if (RocksIntensityExtreme == true)
                //{
                //    for (int i = 0; i < 2040; i++)
                //    {
                //        var distance = Vector2.Distance(new Vector2(_x[i], _y[i]), mainPlayerPos);
                //        rocks[i] = Instantiate(rock, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                //        rocks[i].tag = "RocksOnScreen";
                //        rocks[i].name = "Rocks " + i;

                //        ChangeSprites(i);

                //        if (rand.seedRndNos_spawning[i] < 0.25F) { }
                //        else if ((rand.seedRndNos_spawning[i] < 0.50F) && (rand.seedRndNos_spawning[i] > 0.25F)) { }
                //        else if (rand.seedRndNos_spawning[i] < 0.75F && (rand.seedRndNos_spawning[i] > 0.50F)) { }
                //        else if (rand.seedRndNos_spawning[i] > 0.75F) { }
                //    }
                //}
                #endregion

                #region GenerateTilesBasedOnPlayerPosition

                if (RocksIntensityRegular == true)
                {

                    for (int i = 0; i < 820; i++)
                    {
                        var distance = Vector2.Distance(new Vector2(_x[i], _y[i]), mainPlayerPos);
                        if (distance < areaSpan)
                        {
                            rocks[i] = Instantiate(rock, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                            rocks[i].tag = "RocksOnScreen";
                            rocks[i].name = "Rocks " + i;

                            ChangeSprites(i);

                            if (rand.seedRndNos_spawning[i] < 0.25F) { }
                            else if ((rand.seedRndNos_spawning[i] < 0.50F) && (rand.seedRndNos_spawning[i] > 0.25F)) { }
                            else if (rand.seedRndNos_spawning[i] < 0.75F && (rand.seedRndNos_spawning[i] > 0.50F)) { }
                            else if (rand.seedRndNos_spawning[i] > 0.75F) { }

                            foreach (GameObject item in finalList)
                            {
                                if (item.name == rocks[i].name) { Destroy(rocks[i]); }
                            }
                        }
                    }
                }
                else if (RocksIntensityHigh == true)
                {
                    for (int i = 0; i < 1640; i++)
                    {
                        var distance = Vector2.Distance(new Vector2(_x[i], _y[i]), mainPlayerPos);
                        if (distance < areaSpan)
                        {
                            rocks[i] = Instantiate(rock, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                            rocks[i].tag = "RocksOnScreen";
                            rocks[i].name = "Rocks " + i;

                            ChangeSprites(i);

                            if (rand.seedRndNos_spawning[i] < 0.25F) { }
                            else if ((rand.seedRndNos_spawning[i] < 0.50F) && (rand.seedRndNos_spawning[i] > 0.25F)) { }
                            else if (rand.seedRndNos_spawning[i] < 0.75F && (rand.seedRndNos_spawning[i] > 0.50F)) { }
                            else if (rand.seedRndNos_spawning[i] > 0.75F) { }

                            foreach (GameObject item in finalList)
                            {
                                if (item.name == rocks[i].name) { Destroy(rocks[i]); }
                            }
                        }
                    }
                }
                else if (RocksIntensityExtreme == true)
                {
                    for (int i = 0; i < 2040; i++)
                    {
                        var distance = Vector2.Distance(new Vector2(_x[i], _y[i]), mainPlayerPos);
                        if (distance < areaSpan)
                        {
                            rocks[i] = Instantiate(rock, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                            rocks[i].tag = "RocksOnScreen";
                            rocks[i].name = "Rocks " + i;

                            ChangeSprites(i);

                            if (rand.seedRndNos_spawning[i] < 0.25F) { }
                            else if ((rand.seedRndNos_spawning[i] < 0.50F) && (rand.seedRndNos_spawning[i] > 0.25F)) { }
                            else if (rand.seedRndNos_spawning[i] < 0.75F && (rand.seedRndNos_spawning[i] > 0.50F)) { }
                            else if (rand.seedRndNos_spawning[i] > 0.75F) { }

                            foreach (GameObject item in finalList)
                            {
                                if (item.name == rocks[i].name) { Destroy(rocks[i]); }
                            }
                        }
                    }
                }

                GameObject[] allTexturesMyFriend = GameObject.FindGameObjectsWithTag("RocksOnScreen");
                foreach (GameObject texture in allTexturesMyFriend)
                {
                    var distance2 = Vector2.Distance(texture.transform.position, mainPlayerPos);
                    if (distance2 > areaSpan) { GameObject.Destroy(texture); }
                }

                #endregion
            }
        }
    }

    int RandomNosRangeToExactValues(int pos)
    {
        if ((rand.seedRndNos_spawning[pos] <= 0.12F) && (rand.seedRndNos_spawning[pos] >= 0.0F)) return 0;
        if ((rand.seedRndNos_spawning[pos] <= 0.24f) && (rand.seedRndNos_spawning[pos] >= 0.12f)) return 1;
        if ((rand.seedRndNos_spawning[pos] <= 0.36f) && (rand.seedRndNos_spawning[pos] >= 0.24f)) return 2;
        if ((rand.seedRndNos_spawning[pos] <= 0.48f) && (rand.seedRndNos_spawning[pos] >= 0.36f)) return 3;
        if ((rand.seedRndNos_spawning[pos] <= 0.60f) && (rand.seedRndNos_spawning[pos] >= 0.48f)) return 4;
        if ((rand.seedRndNos_spawning[pos] <= 0.72f) && (rand.seedRndNos_spawning[pos] >= 0.60f)) return 5;
        if ((rand.seedRndNos_spawning[pos] <= 0.84f) && (rand.seedRndNos_spawning[pos] >= 0.72f)) return 6;
        if ((rand.seedRndNos_spawning[pos] <= 1.00f) && (rand.seedRndNos_spawning[pos] >= 0.84f)) return 7;
        else return -1;
    }

    void ChangeSprites(int pos)
    {
        switch (RandomNosRangeToExactValues(pos))
        {
            case -1: Debug.LogError("RndNosOutOfBounds"); break;
            case 0: rocks[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rockA; ChangeColliderDimensions(pos, 0); break;
            case 1: rocks[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rockB; ChangeColliderDimensions(pos, 1); break;
            case 2: rocks[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rockC; ChangeColliderDimensions(pos, 2); break;
            case 3: rocks[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rockD; ChangeColliderDimensions(pos, 3); break;
            case 4: rocks[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rockE; ChangeColliderDimensions(pos, 4); break;
            case 5: rocks[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rockF; ChangeColliderDimensions(pos, 5); break;
            case 6: rocks[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rockG; ChangeColliderDimensions(pos, 6); break;
            case 7: rocks[pos].GetComponentInChildren<SpriteRenderer>().sprite = _rockH; ChangeColliderDimensions(pos, 7); break;
            default: break;
        }
    }

    void ChangeColliderDimensions(int pos, int index)
    {
        switch (index)
        {
            case 0: rocks[pos].GetComponentInChildren<CircleCollider2D>().radius = 0.54F; rocks[pos].GetComponentInChildren<CircleCollider2D>().center = new Vector2(-0.03F, 0.00F);
                RemoveAdd_PolygonCollider2D(pos); break;
            case 1: rocks[pos].GetComponentInChildren<CircleCollider2D>().radius = 0.54F; rocks[pos].GetComponentInChildren<CircleCollider2D>().center = new Vector2(-0.03F, 0.00F);
                RemoveAdd_PolygonCollider2D(pos); break;
            case 2: rocks[pos].GetComponentInChildren<CircleCollider2D>().radius = 0.59F; rocks[pos].GetComponentInChildren<CircleCollider2D>().center = new Vector2(0.01F, 0.00F);
                RemoveAdd_PolygonCollider2D(pos); break;
            case 3: rocks[pos].GetComponentInChildren<CircleCollider2D>().radius = 0.59F; rocks[pos].GetComponentInChildren<CircleCollider2D>().center = new Vector2(0.01F, 0.00F);
                RemoveAdd_PolygonCollider2D(pos); break;
            case 4: rocks[pos].GetComponentInChildren<CircleCollider2D>().radius = 0.59F; rocks[pos].GetComponentInChildren<CircleCollider2D>().center = new Vector2(0.01F, 0.00F);
                RemoveAdd_PolygonCollider2D(pos); break;
            case 5: rocks[pos].GetComponentInChildren<CircleCollider2D>().radius = 0.58F; rocks[pos].GetComponentInChildren<CircleCollider2D>().center = new Vector2(-0.06F, 0.00F);
                RemoveAdd_PolygonCollider2D(pos); break;
            case 6: rocks[pos].GetComponentInChildren<CircleCollider2D>().radius = 0.54F; rocks[pos].GetComponentInChildren<CircleCollider2D>().center = new Vector2(-0.03F, 0.00F);
                RemoveAdd_PolygonCollider2D(pos); break;
            case 7: rocks[pos].GetComponentInChildren<CircleCollider2D>().radius = 0.51F; rocks[pos].GetComponentInChildren<CircleCollider2D>().center = new Vector2(0.00F, 0.00F);
                RemoveAdd_PolygonCollider2D(pos); break;
            default: break;
        }
    }

    void RemoveAdd_PolygonCollider2D(int pos)
    {
        foreach (Transform child in rocks[pos].transform)
        {
            Destroy(child.GetComponent<PolygonCollider2D>());
            child.gameObject.AddComponent<PolygonCollider2D>();
        }
    }

    void Start()
    {
        rand = FindObjectOfType(typeof(PCG_Rand)) as PCG_Rand;
        mining = FindObjectOfType(typeof(Mining)) as Mining;
        RndNosGeneration();
    }

    void Update()
    {
        AddRemoveTiles();
    }
}
