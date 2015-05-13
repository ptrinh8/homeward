using UnityEngine;
using System.Collections;

// class attaches to air panel gameobject
// handle air panel interaction
public class AirPanel : MonoBehaviour {

    private KeyCode airLockInteractKey = KeyCode.F;
    private bool nearAirPenalFlag = false;

    void Update()
    {
        if (nearAirPenalFlag)
        {
            if (Input.GetKeyDown(airLockInteractKey))
            {
                gameObject.SendMessageUpwards("AirModuleActivite", SendMessageOptions.RequireReceiver);
            }
        }
		gameObject.GetComponent<SpriteRenderer>().material = transform.root.gameObject.GetComponent<SpriteRenderer>().material;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
		Debug.Log ("sup");
        if (other.transform.root.gameObject.tag == "Player")
        {
            nearAirPenalFlag = true;
            PlayerController.toolUsingEnable = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
		if (other.transform.root.gameObject.tag == "Player")
        {
            nearAirPenalFlag = false;
            PlayerController.toolUsingEnable = true;
        }
    }
}
