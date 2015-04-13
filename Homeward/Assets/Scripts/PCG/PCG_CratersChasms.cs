// ==================================================================================
// <file="PCG_CratersChasms.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PCG_CratersChasms : MonoBehaviour 
{
    PCG_Rand rand = new PCG_Rand();

    public GameObject _crater;
    public GameObject _craterA, _craterB, _craterC,
        _craterD, _craterE, _craterF, _craterG,
        _craterH, _craterI, _craterJ, _craterK,
        _craterL, _craterM, _craterN, _craterO,
        _craterP, _craterQ, _craterR, _craterS,
        _craterT;
    public GameObject _chasm;
    public GameObject _terrainRock;

    int[] _x = new int[1640];
    int[] _y = new int[1640];

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

    GameObject[] craters = new GameObject[1640];

    void PartiallyRemovingCollidedCraters(int pos)
    {
        if (craters[pos].name == "Craters 873" || craters[pos].name == "Craters 875" || craters[pos].name == "Craters 878" || craters[pos].name == "Craters 877" || craters[pos].name == "Craters 933")
        {
            craters[pos].GetComponentInChildren<SpriteRenderer>().enabled = false;
            Destroy(craters[pos].GetComponentInChildren<CircleCollider2D>());
            Destroy(craters[pos].GetComponentInChildren<EdgeCollider2D>());
            Destroy(craters[pos].GetComponentInChildren<BoxCollider2D>());
        }
    }

    public void RndNosGeneration()
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

                GameObject[] gameObjectTile = GameObject.FindGameObjectsWithTag("CratersOnScreen");
                List<GameObject> gameObjectTileList = gameObjectTile.ToList();
                List<GameObject> finalList = gameObjectTileList.Distinct().ToList();

                for (int i = 0; i < 1640; i++)
                {
                    var distance = Vector2.Distance(new Vector2(_x[i], _y[i]), mainPlayerPos);
                    if (distance < 20.0F)
                    {
                        craters[i] = Instantiate(_crater, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                        craters[i].tag = "CratersOnScreen";
                        craters[i].name = "Craters " + i;

                        PartiallyRemovingCollidedCraters(i);

                        switch (RandomNosRangeToExactValues(i))
                        {
                            case -1: Debug.LogError("RndNosOutOfBounds"); break;
                            case 0: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterA.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 1: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterB.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 2: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterC.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 3: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterD.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 4: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterE.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 5: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterF.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 6: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterG.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 7: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterH.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 8: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterI.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 9: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterJ.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 10: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterK.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 11: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterL.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 12: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterM.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 13: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterN.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 14: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterB.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 15: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterP.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 16: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterQ.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 17: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterR.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 18: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterS.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 19: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _craterT.GetComponentInChildren<SpriteRenderer>().sprite; break;
                            case 20: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _chasm.GetComponentInChildren<SpriteRenderer>().sprite;
                                craters[i].GetComponentInChildren<CircleCollider2D>().enabled = false;
                                craters[i].GetComponentInChildren<BoxCollider2D>().enabled = false;
                                craters[i].GetComponentInChildren<EdgeCollider2D>().enabled = true;
                                #region AccessAllChildrenOfCraters&DeleteCircleCollider
                                //Destroy(craters[i].GetComponentInChildren<CircleCollider2D>());
                                //var transform_of_craters = craters[i].transform;
                                //foreach (Transform child in transform_of_craters)
                                //{
                                //    child.gameObject.AddComponent<PolygonCollider2D>();
                                //}
                                #endregion
                                break;
                            case 21: craters[i].GetComponentInChildren<SpriteRenderer>().sprite = _terrainRock.GetComponentInChildren<SpriteRenderer>().sprite;
                                craters[i].GetComponentInChildren<CircleCollider2D>().enabled = false;
                                craters[i].GetComponentInChildren<EdgeCollider2D>().enabled = false;
                                craters[i].GetComponentInChildren<BoxCollider2D>().enabled = true;
                                break;
                        }

                        foreach (GameObject item in finalList)
                        {
                            if (item.name == craters[i].name) { Destroy(craters[i]); }
                        }
                    }
                }

                GameObject[] allTexturesMyFriend = GameObject.FindGameObjectsWithTag("CratersOnScreen");
                foreach (GameObject texture in allTexturesMyFriend)
                {
                    var distance2 = Vector2.Distance(texture.transform.position, mainPlayerPos);
                    if (distance2 > 20.0F) { GameObject.Destroy(texture); }
                }
            }
        }
    }

    int RandomNosRangeToExactValues(int pos)
    {
        if ((rand.tempRndNosRock1[pos] <= 0.03F) && (rand.tempRndNosRock1[pos] >= 0.0F)) return 0;
        if ((rand.tempRndNosRock1[pos] <= 0.06f) && (rand.tempRndNosRock1[pos] >= 0.03f)) return 1;
        if ((rand.tempRndNosRock1[pos] <= 0.09f) && (rand.tempRndNosRock1[pos] >= 0.06f)) return 2;
        if ((rand.tempRndNosRock1[pos] <= 0.12f) && (rand.tempRndNosRock1[pos] >= 0.09f)) return 3;
        if ((rand.tempRndNosRock1[pos] <= 0.15f) && (rand.tempRndNosRock1[pos] >= 0.12f)) return 4;
        if ((rand.tempRndNosRock1[pos] <= 0.18f) && (rand.tempRndNosRock1[pos] >= 0.15f)) return 5;
        if ((rand.tempRndNosRock1[pos] <= 0.21f) && (rand.tempRndNosRock1[pos] >= 0.18f)) return 6;
        if ((rand.tempRndNosRock1[pos] <= 0.24f) && (rand.tempRndNosRock1[pos] >= 0.21f)) return 7;
        if ((rand.tempRndNosRock1[pos] <= 0.27f) && (rand.tempRndNosRock1[pos] >= 0.24f)) return 8;
        if ((rand.tempRndNosRock1[pos] <= 0.30f) && (rand.tempRndNosRock1[pos] >= 0.27f)) return 9;
        if ((rand.tempRndNosRock1[pos] <= 0.33f) && (rand.tempRndNosRock1[pos] >= 0.30f)) return 10;
        if ((rand.tempRndNosRock1[pos] <= 0.36f) && (rand.tempRndNosRock1[pos] >= 0.33f)) return 11;
        if ((rand.tempRndNosRock1[pos] <= 0.39f) && (rand.tempRndNosRock1[pos] >= 0.36f)) return 12;
        if ((rand.tempRndNosRock1[pos] <= 0.42f) && (rand.tempRndNosRock1[pos] >= 0.39f)) return 13;
        if ((rand.tempRndNosRock1[pos] <= 0.45f) && (rand.tempRndNosRock1[pos] >= 0.42f)) return 14;
        if ((rand.tempRndNosRock1[pos] <= 0.48f) && (rand.tempRndNosRock1[pos] >= 0.45f)) return 15;
        if ((rand.tempRndNosRock1[pos] <= 0.51f) && (rand.tempRndNosRock1[pos] >= 0.48f)) return 16;
        if ((rand.tempRndNosRock1[pos] <= 0.54f) && (rand.tempRndNosRock1[pos] >= 0.51f)) return 17;
        if ((rand.tempRndNosRock1[pos] <= 0.57f) && (rand.tempRndNosRock1[pos] >= 0.54f)) return 18;
        if ((rand.tempRndNosRock1[pos] <= 0.60f) && (rand.tempRndNosRock1[pos] >= 0.57f)) return 19;
        if ((rand.tempRndNosRock1[pos] <= 0.80f) && (rand.tempRndNosRock1[pos] >= 0.60f)) return 20;
        if ((rand.tempRndNosRock1[pos] <= 1.00f) && (rand.tempRndNosRock1[pos] >= 0.80f)) return 21;
        else return -1;
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
