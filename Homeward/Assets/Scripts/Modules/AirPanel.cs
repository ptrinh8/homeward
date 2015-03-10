using UnityEngine;
using System.Collections;

public class AirPanel : MonoBehaviour {

    private KeyCode airLockInteractKey = KeyCode.F;

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(airLockInteractKey))
        {
            gameObject.SendMessageUpwards("AirModuleActivite", SendMessageOptions.RequireReceiver);
        }
    }
}
