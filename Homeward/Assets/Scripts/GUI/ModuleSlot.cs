using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ModuleSlot : MonoBehaviour {

    private Stack<ModuleItem> items;

    public Stack<ModuleItem> Items
    {
        get { return items; }
        set { items = value; }
    }

    private bool isEmpty = true;

    public bool IsEmpty
    {
        get { return isEmpty; }
    }


	// Use this for initialization
	void Awake () {
        items = new Stack<ModuleItem>();
        isEmpty = true;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void AddItem(ModuleItem moduleItem)
    {
        items.Push(moduleItem);
        isEmpty = false;

        if (items.Count > 1)
        {
            //stackText.text = items.Count.ToString();
        }

        ChangeSprite(moduleItem.spriteNeutral, moduleItem.spriteHighLighted);
    }

    private void ChangeSprite(Sprite neutral, Sprite highlight)
    {
        GetComponent<Image>().sprite = neutral;

        SpriteState st = new SpriteState();

        st.highlightedSprite = highlight;
        st.pressedSprite = neutral;

        //GetComponent<Button>().spriteState = st;
    }

    public ModuleItem GetItem()
    {
        if (!IsEmpty)
        {
            isEmpty = false;
            ModuleItem item = items.Pop(); //items.Pop().Use(); you can use this instead if you want to use the Use() function in Item.cs

            //stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

            if (items.Count == 0/*IsEmpty*/)
            {
                isEmpty = true;
                //ChangeSprite(slotEmpty, slotHighLighted);
            }

            return item;
        }

        Debug.LogError("Slot.cs Error: Item is null");
        return null;
    }
}
