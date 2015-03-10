using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AirControl : MonoBehaviour {

    private bool air;
    private float timer;
    private int flag;
    public float duration;
    public Scrollbar airPressureBar;
    public int manuallyOperateDifficulty;

    private LocalControl moduleControl;

    public bool Air
    {
        get
        {
            return air;
        }
        set
        {
            this.air = value;
        }
    }

    public float Timer
    {
        get
        {
            return timer;
        }
        set
        {
            this.timer = value;
        }
    }

    public int Flag
    {
        get
        {
            return flag;
        }
        set
        {
            this.flag = value;
        }
    }

    public void ResetTimer()
    {
        if (air)
        {
            flag = 1;
        }
        else
        {
            flag = -1;
        }
    }

	// Use this for initialization
	void Start () {
        air = true;
        timer = duration;
        flag = 0;

        moduleControl = gameObject.transform.root.gameObject.GetComponent<LocalControl>();
	}
	
	// Update is called once per frame
	void Update () {
        if (flag == 0) { 
            // do nothing
        }
        else if (flag == 1)
        {
            if (airPressureBar.size < 1)
            {
                timer += Time.deltaTime;
                airPressureBar.size = timer / duration;
            }
            else
            {
                airPressureBar.size = 1;
                flag = 0;
                air = true;
            }
        }
        else if (flag == -1)
        {
            if (airPressureBar.size > 0) {
                timer -= Time.deltaTime;
                airPressureBar.size = timer / duration;
            }
            else
            {
                airPressureBar.size = 0;
                flag = 0;
                air = false;
            }
        }
	}
    void AirModuleActivite()
    {
        if (moduleControl.isOn && moduleControl.IsPowered && !moduleControl.IsBroken)
        {
            if (air)
                flag = -1;
            else
                flag = 1;
        }
    }

}
