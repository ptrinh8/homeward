// ==================================================================================
// <file="PCG_Rocks.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PCG_Rocks : MonoBehaviour
{
    PCG_Rand rand = new PCG_Rand();

    public GameObject rock;
    int[] _x = new int[2040];
    int[] _y = new int[2040];

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

    void PartiallyRemovingCollidedRocksInRegularInstensity(int pos)
    {
        if (rocks[pos].name == "Rocks 500")
        {
            #region GroupedRocksRemoval
            //var transform_of_rocks = rocks[pos].transform;
            //foreach (Transform child in transform_of_rocks)
            //{
            //    child.gameObject.transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
            //    child.gameObject.transform.GetComponentInChildren<PolygonCollider2D>().enabled = false;
            //    child.gameObject.transform.GetComponentInChildren<CircleCollider2D>().enabled = false;
            //}
            #endregion
            rocks[pos].transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
            rocks[pos].gameObject.transform.GetComponentInChildren<PolygonCollider2D>().enabled = false;
            rocks[pos].gameObject.transform.GetComponentInChildren<CircleCollider2D>().enabled = false;
        }
    }

    void PartiallyRemovingCollidedRocksInHighInstensity(int pos)
    {
        if (rocks[pos].name == "Rocks 873" || rocks[pos].name == "Rocks 875") 
        {
            rocks[pos].transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
            rocks[pos].gameObject.transform.GetComponentInChildren<PolygonCollider2D>().enabled = false;
            rocks[pos].gameObject.transform.GetComponentInChildren<CircleCollider2D>().enabled = false;
        }
    }

    void PartiallyRemovingCollidedRocksInExtremeInstensity(int pos)
    {
        if (rocks[pos].name == "Rocks 1057" || rocks[pos].name == "Rocks 1116" || rocks[pos].name == "Rocks 1173")
        {
            rocks[pos].transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
            rocks[pos].gameObject.transform.GetComponentInChildren<PolygonCollider2D>().enabled = false;
            rocks[pos].gameObject.transform.GetComponentInChildren<CircleCollider2D>().enabled = false;
        }
    }

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

                if (RocksIntensityRegular == true)
                {

                    for (int i = 0; i < 820; i++)
                    {
                        var distance = Vector2.Distance(new Vector2(_x[i], _y[i]), mainPlayerPos);
                        if (distance < 20.0F)
                        {
                        rocks[i] = Instantiate(rock, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                        rocks[i].tag = "RocksOnScreen";
                        rocks[i].name = "Rocks " + i;

                        PartiallyRemovingCollidedRocksInRegularInstensity(i);

                        if (rand.tempRndNosRock1[i] < 0.25F) { }
                        else if ((rand.tempRndNosRock1[i] < 0.50F) && (rand.tempRndNosRock1[i] > 0.25F)) { }
                        else if (rand.tempRndNosRock1[i] < 0.75F && (rand.tempRndNosRock1[i] > 0.50F)) { }
                        else if (rand.tempRndNosRock1[i] > 0.75F) { }

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
                        if (distance < 20.0F)
                        {
                        rocks[i] = Instantiate(rock, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                        rocks[i].tag = "RocksOnScreen";
                        rocks[i].name = "Rocks " + i;

                        PartiallyRemovingCollidedRocksInHighInstensity(i);

                        if (rand.tempRndNosRock1[i] < 0.25F) { }
                        else if ((rand.tempRndNosRock1[i] < 0.50F) && (rand.tempRndNosRock1[i] > 0.25F)) { }
                        else if (rand.tempRndNosRock1[i] < 0.75F && (rand.tempRndNosRock1[i] > 0.50F)) { }
                        else if (rand.tempRndNosRock1[i] > 0.75F) { }

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
                        if (distance < 20.0F)
                        {
                        rocks[i] = Instantiate(rock, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                        rocks[i].tag = "RocksOnScreen";
                        rocks[i].name = "Rocks " + i;

                        PartiallyRemovingCollidedRocksInExtremeInstensity(i);

                        if (rand.tempRndNosRock1[i] < 0.25F) { }
                        else if ((rand.tempRndNosRock1[i] < 0.50F) && (rand.tempRndNosRock1[i] > 0.25F)) { }
                        else if (rand.tempRndNosRock1[i] < 0.75F && (rand.tempRndNosRock1[i] > 0.50F)) { }
                        else if (rand.tempRndNosRock1[i] > 0.75F) { }

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
                    if (distance2 > 20.0F) { GameObject.Destroy(texture); }
                }
            }
        }
    }
    

    void Start()
    {
        rand = FindObjectOfType(typeof(PCG_Rand)) as PCG_Rand;
        RndNosGeneration();
    }

    void Update()
    {
        AddRemoveTiles();
    }
}
