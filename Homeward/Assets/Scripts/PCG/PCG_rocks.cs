// ==================================================================================
// <file="PCG_rocks.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PCG_rocks : MonoBehaviour 
{
    public GameObject rock;
    private int[] _x = new int[620];
    private int[] _y = new int[620];
    private bool doOnce = false;

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

    void Start() 
    {
        RndNosGeneration();
	
	}
	
	void Update () 
    {
        if (!doOnce)
        {
            doOnce = true;
            for (int i = 0; i < 620; i++)
            {
                rocks[i] = Instantiate(rock, new Vector3(_x[i], _y[i], 0), transform.rotation) as GameObject;
            }
        }
	}
}
