// ==================================================================================
// <file="Instruction.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Contains a base, abstract class for Instruction Management</summary>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

#endregion

public class Instruction : MonoBehaviour 
{
    public static bool showInstruction;

    /*** Fixed Variables ***/

    private int numRowsMain;

    private int numRowsSub;

    private static GameObject selectionBox;

    private RectTransform selectionBoxRect;

    private float selectionBoxSize;

    private List<GameObject> mainMenu;

    private List<GameObject> subMenu1, subMenu2, subMenu3, subMenu;

    private GameObject slotPrefab;

    private float instructionWidth;

    private float instructionHeight;

    private RectTransform instructionRect;

    private float slotMainPaddingTop;

    private float slotMainPaddingLeft;

    private float slotSubPaddingTop;

    private float slotSubPaddingLeft;

    private float slotWidth;

    private float slotHeight;

    /*** Dynamically Changeable Variables ***/

    private int mainNthRow;

    private int subNthRow;

    private bool isOnMainMenu;

    private List<string> column1, column2, column3_1, column3_2;

    private int numTmp;

    private GameObject instructionDetail;

    private GameObject instructionDetailText;


    // Use this for initialization
    void Start()
    {
        Initialize();
        CreateLayout();

        SetActiveSubMenu(subMenu1, 1);
    }

    void Initialize()
    {
        numRowsMain = 3;
        selectionBoxSize = 3.0f;
        slotMainPaddingTop = 120.0f;
        slotMainPaddingLeft = -Screen.width / 2 + 75.0f;
        slotWidth = 0;
        slotHeight = 0;
        mainMenu = new List<GameObject>();
        subMenu = new List<GameObject>();
        isOnMainMenu = true;
        numTmp = 0;

        gameObject.AddComponent<CanvasGroup>();
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        slotPrefab = Instantiate(Resources.Load("Instruction/InstructionSlot")) as GameObject;
        selectionBox = Instantiate(Resources.Load("Inventory/SelectionBox")) as GameObject;
        
    }

    /**************************************************
     * need to change to reading these variables from the csv file
     * *****************************************/

    int numRows1 = 7;
    int numRows2 = 15;
    int numRows3 = 4;

    void CreateLayout()
    {
        ReadCSV();

        instructionWidth = Screen.width;
        instructionHeight = Screen.height;

        instructionRect = GetComponent<RectTransform>();
        instructionRect.localScale = new Vector3(1, 1, 1);
        instructionRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, instructionWidth);
        instructionRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, instructionHeight);

        instructionDetail = Instantiate(Resources.Load("Instruction/InstructionDetail")) as GameObject;
        instructionDetail.name = "InstructionDetail";
        instructionDetail.transform.SetParent(this.transform);
        RectTransform instructionDetailRect = instructionDetail.GetComponent<RectTransform>();
        instructionDetailRect.localScale = new Vector3(1, 1, 1);
        instructionDetailRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300.0f);
        instructionDetailRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500.0f);//(Screen.height / 2 + -slotMainPaddingTop) * 2);
        instructionDetailRect.transform.position = instructionRect.position + new Vector3(130.0f, 0.0f);

        instructionDetailText = Instantiate(Resources.Load("Instruction/InstructionSlotText")) as GameObject;
        instructionDetailText.name = "InstructionDetailText";
        instructionDetailText.GetComponent<Text>().text = column3_1[0];
        instructionDetailText.transform.SetParent(instructionDetail.transform);
        RectTransform instructionDetailTextRect = instructionDetailText.GetComponent<RectTransform>();
        instructionDetailTextRect.localScale = new Vector3(1, 1, 1);
        instructionDetailTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, instructionDetailRect.sizeDelta.x);
        instructionDetailTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, instructionDetailRect.sizeDelta.y);
        instructionDetailTextRect.position = instructionDetailRect.transform.position;

        
        for (int i = 0; i < numRowsMain; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab) as GameObject;

            RectTransform slotRect = newSlot.GetComponent<RectTransform>();

            GameObject newText = Instantiate(Resources.Load("Instruction/InstructionSlotText")) as GameObject;
            newText.transform.SetParent(newSlot.transform);
            newText.name = "InstructionSlotText_" + i + "_" + 0;
            newText.GetComponent<Text>().text = column1[i];

            newSlot.name = "InstructionSlot_" + i + "_" + 0;
            newSlot.transform.SetParent(this.transform);
            newSlot.transform.localScale = new Vector3(1, 1, 1);

            slotWidth = 130.0f;//newSlot.GetComponent<RectTransform>().sizeDelta.x;
            slotHeight = 50.0f;//newSlot.GetComponent<RectTransform>().sizeDelta.y;

            slotRect.localPosition = instructionRect.localPosition + new Vector3(slotMainPaddingLeft, Screen.height/2 + -slotMainPaddingTop * (i + 1));
            mainMenu.Add(newSlot);

            /*** sub menu ***/

            if (i == 0)
            {
                numRowsSub = numRows1;
                subMenu1 = new List<GameObject>();
            }
            else if (i == 1)
            {
                numRowsSub = numRows2;
                subMenu2 = new List<GameObject>();
            }
            else if (i == 2)
            {
                numRowsSub = numRows3;
                subMenu3 = new List<GameObject>();
            }

            List<GameObject> tmpList = new List<GameObject>();

            for (int j = 0; j < numRowsSub; j++)
            {
                newSlot = Instantiate(slotPrefab) as GameObject;
                newSlot.AddComponent<CanvasGroup>();
                newSlot.GetComponent<CanvasGroup>().alpha = 0;

                slotRect = newSlot.GetComponent<RectTransform>();

                newText = Instantiate(Resources.Load("Instruction/InstructionSlotText")) as GameObject;
                newText.transform.SetParent(newSlot.transform);
                newText.name = "InstructionSlotText_" + j + "_" + 1;
                newText.GetComponent<Text>().text = column2[numTmp + j];

                newSlot.name = "InstructionSlot_" + j + "_" + 1;
                newSlot.transform.SetParent(this.transform);
                newSlot.transform.localScale = new Vector3(1, 1, 1);

                slotWidth = newSlot.GetComponent<RectTransform>().sizeDelta.x;
                slotHeight = newSlot.GetComponent<RectTransform>().sizeDelta.y;

                slotRect.localPosition = instructionRect.localPosition + new Vector3(slotMainPaddingLeft + slotWidth * 1.5f, Screen.height / 2 + -slotMainPaddingTop * (j + 1));
                tmpList.Add(newSlot);
            }

            numTmp += numRowsSub;

            if (i == 0)
            {
                subMenu1.AddRange(tmpList);
            }
            else if (i == 1)
            {
                subMenu2.AddRange(tmpList);
            }
            else if (i == 2)
            {
                subMenu3.AddRange(tmpList);
            }

            tmpList.Clear();
            numRowsSub = numRows1;
            DrawSelectionBox(0, 0);
        }
    }

    void Update()
    {
        if (showInstruction)
        {
            GetComponent<CanvasGroup>().alpha = 1;
            MoveSelectionBox();
            Time.timeScale = 0.0f;
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = 0;
            Time.timeScale = 1.0f;
        }
    }

    void ReadCSV()
    {
        TextAsset instructionRawText = Resources.Load<TextAsset>("Text/Instruction");
        SplitCsvGrid(instructionRawText.text);
        //DebugOutputGrid(grid);
    }

    static public void DebugOutputGrid(string[,] grid)
    {
        string textOutput = "";
        for (int y = 0; y < grid.GetUpperBound(1); y++)
        {
            for (int x = 0; x < grid.GetUpperBound(0); x++)
            {
                textOutput += grid[x, y];
                textOutput += "|";
            }
            textOutput += "\n";
        }
        Debug.Log(textOutput);
    }

    public void SplitCsvGrid(string csvText)
    {
        string[] lines = csvText.Split("\n"[0]);

        //Debug.Log("lines[0] +" + lines[0]);
        //Debug.Log("lines[1] +" + lines[1]);
        //Debug.Log("lines[2] +" + lines[2]);

        column1 = new List<string>();
        column2 = new List<string>();
        column3_1 = new List<string>();
        column3_2 = new List<string>();

        string[] splitted = new string[3];
        string tmpString1 = "";
        string tmpString2 = "";

        for (int y = 0; y < lines.Length - 1; y++)
        {
            splitted = lines[y].Split(","[0]);
            
            if (splitted[0].Substring(0, 3).Equals("1. ") ||
                splitted[0].Substring(0, 3).Equals("2. ") ||
                splitted[0].Substring(0, 3).Equals("3. "))
            {
                column1.Add(splitted[0]);
                column3_1.Add(splitted[1].Substring(1)); // removing "/"
            }
            else if (splitted[0].Substring(0, 2).Equals("1.") ||
                splitted[0].Substring(0, 2).Equals("2.") ||
                splitted[0].Substring(0, 2).Equals("3."))
            {
                bool flag = false;
                List<string> tmpList1 = new List<string>();
                List<string> tmpList2 = new List<string>();

                for (int i = 0; i < splitted.Length; i++)
                {
                    if (splitted[i].Contains("/"))
                    {
                        splitted[i] = splitted[i].Substring(1); // removing "/"
                        flag = true;
                    }

                    if (!flag) // store stuff before "/" into tmpArray1
                    {
                        tmpList1.Add(splitted[i]);
                    }
                    else // store stuff after "/" into tmpArray2
                    {
                        tmpList2.Add(splitted[i]);
                    }
                }
                tmpString1 = Concatenate(tmpList1);
                tmpString2 = Concatenate(tmpList2);
                column2.Add(tmpString1);
                column3_2.Add(tmpString2);
            }
        }

        /*** For Debug ***/
        //int m = 0;
        //foreach (string oh in column1)
        //{
        //    Debug.Log("column1[" + m + "] = " + oh);
        //    m++;
        //}
        //m = 0;
        //foreach (string oh in column2)
        //{
        //    Debug.Log("column2[" + m + "] = " + oh);
        //    m++;
        //}
        //m = 0;
        //foreach (string oh in column3_2)
        //{
        //    Debug.Log("column3_2[" + m + "] = " + oh);
        //    m++;
        //}
    }

    string Concatenate(List<string> splitted)
    {
        string outcome = "";

        if (splitted[0].Substring(splitted[0].Length - 1, 1).Equals("\""))
        {
            if (splitted.Count > 1)
            {
                outcome = splitted[0].Substring(0, splitted[0].Length - 1) + ", " + Concatenate(CutArrayByOne(splitted)).Substring(1); // splitted[1].Substring(1, splitted[1].Length - 1);
            }
        }
        else
        {
            outcome = outcome + splitted[0];
        }

        return outcome;
    }

    List<string> CutArrayByOne(List<string> tmp)
    {
        List<string> array = new List<string>();

        for (int i = 1; i < tmp.Count; i++)
        {
            array.Add(tmp[i]);
        }

        return array;
    }

    void SetActiveSubMenu(List<GameObject> menus, int alpha)
    {
        foreach(GameObject menu in menus)
        {
            menu.GetComponent<CanvasGroup>().alpha = alpha;
        }
    }
    
    void MoveSelectionBox()
    {
        /*** ***/
        if (mainNthRow == 0)
        {
            numRowsSub = numRows1;
            subMenu.Clear();
            subMenu.AddRange(subMenu1);
        }
        else if (mainNthRow == 1)
        {
            numRowsSub = numRows2;
            subMenu.Clear();
            subMenu.AddRange(subMenu2);
        }
        else if (mainNthRow == 2)
        {
            numRowsSub = numRows3;
            subMenu.Clear();
            subMenu.AddRange(subMenu3);
        }

        if (isOnMainMenu)
        {
            instructionDetailText.GetComponent<Text>().text = column3_1[mainNthRow];
        }
        else
        {
            if (mainNthRow == 0)
            {
                instructionDetailText.GetComponent<Text>().text = column3_2[subNthRow];
            }
            else if (mainNthRow == 1)
            {
                instructionDetailText.GetComponent<Text>().text = column3_2[subNthRow + numRows1];
            }
            else if (mainNthRow == 2)
            {
                instructionDetailText.GetComponent<Text>().text = column3_2[subNthRow + numRows1 + numRows2];
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isOnMainMenu)
            {
                if (mainNthRow > 0)
                {
                    mainNthRow--;
                    MoveMenuBoxes(mainMenu, -1);
                    CheckWhichSubMenu();
                }
            }
            else
            {
                if (subNthRow > 0)
                {
                    subNthRow--;
                    MoveMenuBoxes(subMenu, -1);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isOnMainMenu)
            {
                if (mainNthRow < numRowsMain - 1)
                {
                    mainNthRow++;
                    MoveMenuBoxes(mainMenu, 1);
                    CheckWhichSubMenu();
                }
            }
            else
            {
                if (subNthRow < numRowsSub - 1)
                {
                    subNthRow++;
                    MoveMenuBoxes(subMenu, 1);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (isOnMainMenu)
            {
                isOnMainMenu = false;
                selectionBoxRect.localPosition = instructionRect.localPosition + new Vector3(slotMainPaddingLeft + slotWidth * 1.5f - slotWidth / 2, Screen.height / 2 + -slotMainPaddingTop + slotHeight / 2);
                subNthRow = 0;
                MoveMenuBoxesDedfaultPosition();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!isOnMainMenu)
            {
                isOnMainMenu = true;
                selectionBoxRect.localPosition = instructionRect.localPosition + new Vector3(slotMainPaddingLeft - slotWidth / 2, Screen.height / 2 + -slotMainPaddingTop + slotHeight / 2);
                subNthRow = 0;
                MoveMenuBoxesDedfaultPosition();
            }
        }
    }

    void CheckWhichSubMenu()
    {
        if (mainNthRow == 0)
        {
            SetActiveSubMenu(subMenu1, 1);
            SetActiveSubMenu(subMenu2, 0);
            SetActiveSubMenu(subMenu3, 0);
        }
        else if (mainNthRow == 1)
        {
            SetActiveSubMenu(subMenu1, 0);
            SetActiveSubMenu(subMenu2, 1);
            SetActiveSubMenu(subMenu3, 0);
        }
        else if (mainNthRow == 2)
        {
            SetActiveSubMenu(subMenu1, 0);
            SetActiveSubMenu(subMenu2, 0);
            SetActiveSubMenu(subMenu3, 1);
        }
    }

    void DrawSelectionBox(int row, int column)
    {
        selectionBoxRect = selectionBox.GetComponent<RectTransform>();

        selectionBox.name = "SelectionBoxForInstruction";
        selectionBox.transform.SetParent(this.transform);
        selectionBox.transform.localScale = new Vector3(1, 1, 1);

        GameObject instructionSlot = GameObject.Find("InstructionSlot_" + row + "_" + column);

        selectionBoxRect.localPosition = new Vector3(
            instructionSlot.transform.localPosition.x - instructionSlot.GetComponent<RectTransform>().sizeDelta.x / 2,
            instructionSlot.transform.localPosition.y + instructionSlot.GetComponent<RectTransform>().sizeDelta.y / 2);
        selectionBoxRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotWidth);
        selectionBoxRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotHeight);
    }

    void MoveMenuBoxes(List<GameObject> menus, int direction)
    {
        foreach (GameObject menu in menus)
        {
            menu.transform.localPosition = menu.transform.localPosition + new Vector3(0, slotMainPaddingTop * direction);
        }
    }

    void MoveMenuBoxesDedfaultPosition()
    {
        int k = 0;

        foreach (GameObject menu in subMenu)
        {
            menu.transform.localPosition = instructionRect.localPosition + new Vector3(slotMainPaddingLeft + slotWidth * 1.5f, Screen.height / 2 + -slotMainPaddingTop * (k + 1));
            k++;
        }
    }
}
