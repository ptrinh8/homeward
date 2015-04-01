using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    private Stack<Item> items;

    public Stack<Item> Items
    {
        get { return items; }
        set { items = value; }
    }

    public Text stackText;

    public Sprite slotEmpty;
    public Sprite slotHighLighted;

    public bool isEmpty;

    public bool IsEmpty
    {
        get { return isEmpty; }
    }

    public Item CurrentItem
    {
        get { return items.Peek(); }
    }

    public bool IsAvailable
    {
        get { return CurrentItem.maxSize > items.Count; }
    }

	void Start () 
    {
        items = new Stack<Item>();
        isEmpty = true;

        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform textRect = stackText.GetComponent<RectTransform>();

        int textScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);

        stackText.resizeTextMaxSize = textScaleFactor;
        stackText.resizeTextMinSize = textScaleFactor;

        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.x);
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.y);
	}
	
    public void AddItem(Item item)
    {
        items.Push(item);
        isEmpty = false;

        if (items.Count > 1)
        {
            stackText.text = items.Count.ToString();
        }

        ChangeSprite(item.spriteNeutral, item.spriteHighLighted);
    }

    public Item GetCurrentItem()
    {
        if (items == null)
        {
            Item item = new Item();
            return item;
        }

        return items.Peek();
    }

    public void AddItems(Stack<Item> items)
    {
        this.items = new Stack<Item>(items);
        isEmpty = false;

        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighLighted);
    }

    private void ChangeSprite(Sprite neutral, Sprite highlight)
    {
        GetComponent<Image>().sprite = neutral;

        SpriteState st = new SpriteState();

        st.highlightedSprite = highlight;
        st.pressedSprite = neutral;

        //GetComponent<Button>().spriteState = st;
    }

    public Item GetItem()
    {
        if (!IsEmpty)
        {
            isEmpty = false;
            Item item = items.Pop(); //items.Pop().Use(); you can use this instead if you want to use the Use() function in Item.cs

            stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

            if (items.Count == 0/*IsEmpty*/)
            {
                ClearSlot();
            }

            return item;
        }

        Debug.LogError("Slot.cs Error: Item is null");
        return null;
    }

	public Item CheckItem()
	{
		if (!IsEmpty)
		{
			isEmpty = false;
			Item item = items.Peek();

			return item;
		}
		Debug.LogError("Slot.cs Error: Item is null");
		return null;
	}

    public void ClearSlot()
    {
        items.Clear();
        isEmpty = true;

        ChangeSprite(slotEmpty, slotHighLighted);

        stackText.text = string.Empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Right)
        //{
        //    //UseItem();
        //}
    }
}
