// ==================================================================================
// <file="PCG_sprites.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PCG_sprites : MonoBehaviour 
{
    public GameObject rock;
    public GameObject planetTextureA, planetTextureB, planetTextureC, planetTextureD;
    public GameObject real_Rock;
    private int[] _x = new int[11071];
    private int[] _y = new int[11071];
    private bool doOnce = false;

    private PCG_Rand rand = new PCG_Rand();

    int lowerLmt_Y = 0;
    int lowerLmt_X = 1;
    int vertiLmtINC = 0;
    int vertiLmtINC_again = 0;
    int vertiLmt = 90;
    int iii = 0;
    int zzz = 1;

    private GameObject[] rocks = new GameObject[11071];

    public void RndNosGeneration()
    {
        for (int i = 0; i < 11071; i++)
        {

            _y[i] = 5 + iii;
            iii = iii + 5;

            // move down to the right side after touching vertiLmt
            if (i == vertiLmt || i == vertiLmt+vertiLmtINC)
            {
                vertiLmtINC = vertiLmtINC + vertiLmt;
                iii = 0;
            }
        }

        for (int j = 0; j < 11071; j++)
        {

            _x[j] = 5 + zzz;

            // increment x part of grid -> after touching vertiLimit 
            if (j == vertiLmt || j == vertiLmt + vertiLmtINC_again)
            {
                vertiLmtINC_again = vertiLmtINC_again + vertiLmt;
                zzz = zzz + 5;
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
        if (!doOnce)
        {
            doOnce = true;
            for (int i = 0; i < 11071; i++)
            {
                rocks[i] = Instantiate(rock, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
                
                if (rand.tempRndNosRock1[i] < 0.25F)
                {
                    rocks[i].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureC.GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else if ((rand.tempRndNosRock1[i] < 0.50F) && (rand.tempRndNosRock1[i] > 0.25F))
                {
                    rocks[i].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureB.GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else if (rand.tempRndNosRock1[i] < 0.75F && (rand.tempRndNosRock1[i] > 0.50F))
                {
                    rocks[i].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureA.GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else if (rand.tempRndNosRock1[i] > 0.75F)
                {
                    rocks[i].GetComponentInChildren<SpriteRenderer>().sprite = planetTextureD.GetComponentInChildren<SpriteRenderer>().sprite;
                }
            }
        }
	}
}
