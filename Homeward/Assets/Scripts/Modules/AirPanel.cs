using UnityEngine;
using System.Collections;

public class AirPanel : MonoBehaviour {

    private KeyCode airLockInteractKey = KeyCode.F;
    private bool nearAirPenalFlag = false;

    void Update()
    {
        gameObject.GetComponent<SpriteRenderer>().material = gameObject.transform.root.gameObject.GetComponent<SpriteRenderer>().material;
        if (nearAirPenalFlag)
        {
            if (Input.GetKeyDown(airLockInteractKey))
            {
                gameObject.SendMessageUpwards("AirModuleActivite", SendMessageOptions.RequireReceiver);
            }
        }
    }

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        if (Input.GetKeyDown(airLockInteractKey))
    //        {
    //            gameObject.SendMessageUpwards("AirModuleActivite", SendMessageOptions.RequireReceiver);
    //        }
    //    }
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            nearAirPenalFlag = true;
            PlayerController.toolUsingEnable = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            nearAirPenalFlag = false;
            PlayerController.toolUsingEnable = true;
        }
    }
}
