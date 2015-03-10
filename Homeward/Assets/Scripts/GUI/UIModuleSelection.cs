using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIModuleSelection : MonoBehaviour
{

    private bool firstTimeFlag = true;

    private static GameObject newBox;

    private float selectionBoxSize;

    private List<GameObject> allModuleSlots;

    private float moduleSelectionBoxWidth, moduleSelectionBoxHeight;

    private RectTransform moduleSelectionBoxRect;

    private int nthBox;

    private GameObject moduleSelection;

    void Start()
    {
        GameObject moduleSelectionGameObject = GameObject.Find("MainPlayer").GetComponent<PlayerController>().moduleSelection;
        //selectionBoxRows = GameObject.Find("MainPlayer").GetComponent<PlayerController>();
        //selectionBoxSlots = GameObject.Find("MainPlayer").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.showModuleSelection)
        {
            if (firstTimeFlag)
            {
                drawModuleSelection();
                drawSelectionBox(0);
            }

            firstTimeFlag = false;
        }
        else
        {
            firstTimeFlag = true;
        }
    }

    void drawModuleSelection()
    {
    }

    void drawSelectionBox(int nth)
    {
        newBox = Instantiate(Resources.Load("Inventory/SelectionBox")) as GameObject;
        RectTransform selectionBoxRect = newBox.GetComponent<RectTransform>();

        newBox.name = "SelectionBox" + nth;
        newBox.transform.SetParent(this.transform.parent);
        newBox.transform.localScale = new Vector3(1, 1, 1);

        //selectionBoxRect.localPosition = moduleSelection.GetComponent<ModuleSelection>().moduleSelectionRect.localPosition +
        //    new Vector3(nth % moduleSelection.GetComponent<ModuleSelection>().rows * moduleSelection.GetComponent<ModuleSelection>().moduleSlotSize + moduleSelection.GetComponent<ModuleSelection>().moduleSlotPaddingLeft,
        //        (-1) * nth / moduleSelection.GetComponent<ModuleSelection>().rows * moduleSelection.GetComponent<ModuleSelection>().moduleSlotSize + moduleSelection.GetComponent<ModuleSelection>().moduleSlotPaddingTop);
        selectionBoxRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, selectionBoxSize);
        selectionBoxRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, selectionBoxSize);
    }
}
