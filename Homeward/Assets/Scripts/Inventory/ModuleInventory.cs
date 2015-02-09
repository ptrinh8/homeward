using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModuleInventory : MonoBehaviour {
    
    private static int emptySlots;
    
    public int slots; // set in inspector

    private static Slot slot;

    private List<GameObject> allSlots;

	// Use this for initialization
	void Start () {
        emptySlots = slots;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool AddItem(Item item)
    {
        if (item.maxSize == 1)
        {
            PlaceEmpty(item);
            return true;
        }
        else
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (!tmp.IsEmpty)
                {
                    if (tmp.CurrentItem.itemName == item.itemName && tmp.IsAvailable)
                    {
                        tmp.AddItem(item);
                        return true;
                    }
                    else if (tmp.CurrentItem.itemName == item.itemName && !tmp.IsAvailable)
                    {
                        return false;
                    }
                }
            }

            if (emptySlots > 0)
            {
                PlaceEmpty(item);
            }
        }


        return false;
    }

    private bool PlaceEmpty(Item item)
    {
        if (emptySlots > 0)
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (tmp.IsEmpty)
                {
                    tmp.AddItem(item);
                    emptySlots--;
                    return true;
                }
            }
        }

        return false;
    }


}
