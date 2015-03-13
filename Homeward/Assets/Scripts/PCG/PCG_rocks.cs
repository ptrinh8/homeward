// ==================================================================================
// <file="PCG_rocks.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PCG_rocks : MonoBehaviour 
{
    private PCG_Rand rand = new PCG_Rand();

    public GameObject rock;
    private int[] _x = new int[620];
    private int[] _y = new int[620];
    private bool doOnce = false;

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
    int ii = 1;
    int xx = 0;
    int hh = 0;

    private GameObject[] rocks = new GameObject[620];

    public void RndNosGeneration()
    {
        for (int i = 0; i < 620; i++)
        {
            _y[i] = Random.Range(jj, jj + 21);
            jj = jj + 21;
            if (i == 21 || i == 21+xx)
            {
                xx = xx + 21;
                jj = 0;
            }
        }

        for (int j = 0; j < 620; j++)
        {

            _x[j] = Random.Range(ii, ii + 19);
            if (j == 21 || j == 21 + hh)
            {
                hh = hh + 21;
                ii = ii + 21;
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


                for (int i = 0; i < 620; i++)
                {
                    var distance = Vector2.Distance(new Vector2(_x[i], _y[i]), mainPlayerPos);
                    if (distance < 20.0F)
                    {

                        rocks[i] = Instantiate(rock, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                        rocks[i].tag = "RocksOnScreen";
                        rocks[i].name = "Rocks " + i;

                        if (rand.tempRndNosRock1[i] < 0.25F)
                        {
                        }
                        else if ((rand.tempRndNosRock1[i] < 0.50F) && (rand.tempRndNosRock1[i] > 0.25F))
                        {
                        }
                        else if (rand.tempRndNosRock1[i] < 0.75F && (rand.tempRndNosRock1[i] > 0.50F))
                        {
                        }
                        else if (rand.tempRndNosRock1[i] > 0.75F)
                        {
                        }

                        foreach (GameObject item in finalList)
                        {
                            if (item.name == rocks[i].name)
                            {
                                Destroy(rocks[i]);
                            }
                        }

                    }
                }

                GameObject[] allTexturesMyFriend = GameObject.FindGameObjectsWithTag("RocksOnScreen");
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

    void Start() 
    {
        rand = FindObjectOfType(typeof(PCG_Rand)) as PCG_Rand;
        RndNosGeneration();
	
	}
	
	void Update () 
    {
        AddRemoveTiles();
	}
}
