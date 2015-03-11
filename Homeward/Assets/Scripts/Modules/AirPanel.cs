using UnityEngine;
using System.Collections;

public class AirPanel : MonoBehaviour {

    private KeyCode airLockInteractKey = KeyCode.F;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(airLockInteractKey))
            {
                gameObject.SendMessageUpwards("AirModuleActivite", SendMessageOptions.RequireReceiver);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.toolUsingEnable = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.toolUsingEnable = true;
        }
    }
}
