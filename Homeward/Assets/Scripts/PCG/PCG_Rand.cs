// ==================================================================================
// <file="PCG_Rand.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PCG_Rand : MonoBehaviour 
{
    // Make changes to these values in the editor.
    [HideInInspector]
    public List<float> tempRndNosRock1 = new List<float>(20000);
    [HideInInspector]
    public List<float> tempRndNosRock2 = new List<float>(20000);
    

    public List<float> RndNosRock1
    {
        get { return tempRndNosRock1; }
        set { tempRndNosRock1 = value; }
    }


    public List<float> RndNosRock2
    {
        get { return tempRndNosRock2; }
        set { tempRndNosRock2 = value; }
    }

	void Start () 
    {
        RndNosGenerationRock1();
        RndNosGenerationRock2();
	}
	
	void Update () 
    {


	}

    public void RndNosGenerationRock1()
    {
        Random.seed = System.Environment.TickCount;

        for (int i = 0; i < tempRndNosRock1.Count; i++)
        {
            tempRndNosRock1[i] = Random.value;
        }
    }

    public void RndNosGenerationRock2()
    {
        Random.seed = System.Environment.TickCount;

        for (int i = 0; i < tempRndNosRock2.Count; i++)
        {
            tempRndNosRock2[i] = Random.value;
        }
    }
}
