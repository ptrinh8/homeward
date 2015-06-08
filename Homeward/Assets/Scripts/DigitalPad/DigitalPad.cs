using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;

public class DigitalPad : MonoBehaviour
{
    public static bool journalFlag;

    private GameObject item;

    private RectTransform itemRect;

    private GameObject cursor;

    private Vector3 itemStartingPosition;

    private Vector3 itemGap;

    private RectTransform cursorRect;

    private List<GameObject> itemList;

    private List<GameObject> editTextList;

    private bool inEditMode;

    private int cursorCurrentPositionVertical;

    private int cursorCurrentPositionHorizontal;

    private GameObject currentText;

    private GameObject newButton;

    private GameObject quitButton;

    private int maxNumCharsOnList; // maximum number of characters shown on the list

    private bool firstFlag;

    /*** edit screen ***/
    private GameObject editBackground;

    private GameObject editText;

    private GameObject saveButton;

    private GameObject deleteButton;

    private GameObject cursorInEdit;

    private RectTransform cursorInEditRect;

    private bool isOnSave;

    void Start()
    {
        Initialize();
        CreateLayout();
    }

    void Initialize()
    {
        firstFlag = true;
        journalFlag = false;
        cursorCurrentPositionVertical = 0;
        cursorCurrentPositionHorizontal = -1; // 0: center, -1: left, 1: right
        itemList = new List<GameObject>();
        editTextList = new List<GameObject>();
        itemStartingPosition = gameObject.transform.position + new Vector3(0, Screen.height / 4);
        itemGap = new Vector3(0, 70.0f);
        maxNumCharsOnList = 24;
        inEditMode = false;
        isOnSave = true;
    }

    void CreateLayout()
    {
        /*** new button ***/
        newButton = Instantiate(Resources.Load("DigitalPad/NewButton")) as GameObject;
        newButton.transform.SetParent(gameObject.transform);
        newButton.name = "+NewButton";
        RectTransform newButtonRect = newButton.GetComponent<RectTransform>();
        newButtonRect.position = gameObject.transform.position + new Vector3(-Screen.width / 2 + newButtonRect.sizeDelta.x / 2, Screen.height / 2 - newButtonRect.sizeDelta.y / 2);
        newButtonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newButtonRect.sizeDelta.x / 2);
        newButtonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newButtonRect.sizeDelta.y / 2);

        /*** quit button ***/
        quitButton = Instantiate(Resources.Load("DigitalPad/QuitButton")) as GameObject;
        quitButton.transform.SetParent(gameObject.transform);
        quitButton.name = "QuitButton";
        RectTransform quitButtonRect = quitButton.GetComponent<RectTransform>();
        quitButtonRect.position = gameObject.transform.position + new Vector3(Screen.width / 2 - quitButtonRect.sizeDelta.x / 2, Screen.height / 2 - quitButtonRect.sizeDelta.y / 2);
        quitButtonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, quitButtonRect.sizeDelta.x / 2);
        quitButtonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, quitButtonRect.sizeDelta.y / 2);

        /*** cursor ***/
        cursor = Instantiate(Resources.Load("DigitalPad/DigitalPadCursor")) as GameObject;
        cursor.transform.SetParent(gameObject.transform);
        cursor.name = "DigitalPadCursor";
        cursorRect = cursor.GetComponent<RectTransform>();
        cursorRect.position = newButton.transform.position;
        cursorRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cursorRect.sizeDelta.x);
        cursorRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cursorRect.sizeDelta.y);
        cursor.GetComponent<CanvasGroup>().alpha = 1;

        /*** edit background ***/
        editBackground = Instantiate(Resources.Load("DigitalPad/EditBackground")) as GameObject;
        editBackground.transform.SetParent(gameObject.transform);
        editBackground.name = "EditBackground";
        RectTransform editBackgroundRect = editBackground.GetComponent<RectTransform>();
        editBackgroundRect.position = gameObject.transform.position;
        editBackgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 0.7f);
        editBackgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * 0.7f);

        /*** save button ***/
        saveButton = Instantiate(Resources.Load("DigitalPad/SaveButton")) as GameObject;
        saveButton.transform.SetParent(editBackground.transform);
        RectTransform saveButtonRect = saveButton.GetComponent<RectTransform>();
        saveButtonRect.position = gameObject.transform.position + new Vector3(-Screen.width / 2 + saveButtonRect.sizeDelta.x / 2, Screen.height / 2 - saveButtonRect.sizeDelta.y / 2);
        saveButtonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, saveButtonRect.sizeDelta.x / 2);
        saveButtonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, saveButtonRect.sizeDelta.y / 2);

        /*** delete button ***/
        deleteButton = Instantiate(Resources.Load("DigitalPad/DeleteButton")) as GameObject;
        deleteButton.transform.SetParent(editBackground.transform);
        RectTransform deleteButtonRect = deleteButton.GetComponent<RectTransform>();
        deleteButtonRect.position = gameObject.transform.position + new Vector3(Screen.width / 2 - deleteButtonRect.sizeDelta.x / 2, Screen.height / 2 - deleteButtonRect.sizeDelta.y / 2);
        deleteButtonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, deleteButtonRect.sizeDelta.x / 2);
        deleteButtonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, deleteButtonRect.sizeDelta.y / 2);

        /*** cursor in edit mode ***/
        cursorInEdit = Instantiate(Resources.Load("DigitalPad/DigitalPadCursor")) as GameObject;
        cursorInEdit.transform.SetParent(editBackground.transform);
        cursorInEdit.name = "EditModeCursor";
        cursorInEditRect = cursorInEdit.GetComponent<RectTransform>();
        cursorInEditRect.position = saveButton.transform.position;
        cursorInEditRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cursorInEditRect.sizeDelta.x);
        cursorInEditRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cursorInEditRect.sizeDelta.y);
        cursorInEdit.GetComponent<CanvasGroup>().alpha = 0;

        /*** current text initializing ***/
        currentText = Instantiate(Resources.Load("DigitalPad/EditText")) as GameObject;
        currentText.transform.SetParent(editBackground.transform);
        currentText.name = "CurrentText";
        currentText.GetComponent<Text>().text = "";
        RectTransform CurrentTextRect = currentText.GetComponent<RectTransform>();
        CurrentTextRect.position = editBackground.transform.position;
        CurrentTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 0.6f);
        CurrentTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * 0.6f);
    }

    
    void Update()
    {
        if (journalFlag)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1;

            if (firstFlag)
            {
                inEditMode = true;
                cursorCurrentPositionVertical = itemList.Count;
                CreateItem();
                SwitchModes();
                firstFlag = false;
            }
            HandleKeyInput();
        }
        else
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    void CreateItem()
    {
        item = Instantiate(Resources.Load("DigitalPad/Item")) as GameObject;
        item.transform.SetParent(gameObject.transform);
        item.name = "DigitalPadItem_" + itemList.Count;
        itemRect = item.GetComponent<RectTransform>();
        itemRect.position = itemStartingPosition - itemGap * itemList.Count;
        itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 0.6f);
        itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * 0.1f);

        editText = Instantiate(Resources.Load("DigitalPad/EditText")) as GameObject;
        editText.transform.SetParent(item.transform);
        editText.name = "EditText_" + itemList.Count;
        RectTransform editTextRect = editText.GetComponent<RectTransform>();
        editTextRect.position = gameObject.transform.position;
        editTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 0.6f);
        editTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * 0.6f);

        itemList.Add(item);
        editTextList.Add(editText);
    }

    void DeleteItem()
    {
        editTextList.RemoveAt(cursorCurrentPositionVertical);
        itemList.RemoveAt(cursorCurrentPositionVertical);
        Destroy(GameObject.Find("EditText_" + cursorCurrentPositionVertical));
        Destroy(GameObject.Find("DigitalPadItem_" + cursorCurrentPositionVertical));

        FileInfo fi = new FileInfo(Application.dataPath + "/Resources/Text/Journal/journal_" + cursorCurrentPositionVertical + ".txt");
        
        if (fi.Exists)
        {
            fi.Delete();
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].name = "DigitalPadItem_" + i;
            editTextList[i].name = "EditText_" + i;
            
            FileInfo fimeta = new FileInfo(Application.dataPath + "/Resources/Text/Journal/journal_" + i + ".txt.meta");

            if (fimeta.Exists)
            {
                fimeta.Delete();
            }
        }

        for (int i = cursorCurrentPositionVertical; i < itemList.Count; i++)
        {
            File.Move(Application.dataPath + "/Resources/Text/Journal/journal_" + (i + 1) + ".txt", 
            Application.dataPath + "/Resources/Text/Journal/journal_" + i + ".txt");
        }

        File.Delete(Application.dataPath + "/Resources/Text/Journal/journal_" + itemList.Count + ".txt");

        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].transform.position = itemStartingPosition - itemGap * i;
            editTextList[i].transform.position = gameObject.transform.position;
            
            try
            {
                File.Delete(Application.dataPath + "/Resources/Text/Journal/journal_" + i + ".txt.meta");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        cursorCurrentPositionVertical = 0;
        cursorCurrentPositionHorizontal = -1;
        inEditMode = false;
        cursorRect.position = saveButton.transform.position;
    }

    void SwitchModes()
    {
        if (inEditMode) // is in edit mode after switch
        {
            cursor.GetComponent<CanvasGroup>().alpha = 0;
            newButton.GetComponent<CanvasGroup>().alpha = 0;
            quitButton.GetComponent<CanvasGroup>().alpha = 0;
            cursorInEdit.GetComponent<CanvasGroup>().alpha = 1;
            editBackground.GetComponent<CanvasGroup>().alpha = 1;
            currentText.GetComponent<Text>().text = GameObject.Find("EditText_" + cursorCurrentPositionVertical).GetComponent<Text>().text;
            currentText.GetComponent<CanvasGroup>().alpha = 1;
            ReadText();
        }
        else
        {
            cursor.GetComponent<CanvasGroup>().alpha = 1;
            newButton.GetComponent<CanvasGroup>().alpha = 1;
            quitButton.GetComponent<CanvasGroup>().alpha = 1;
            cursorInEdit.GetComponent<CanvasGroup>().alpha = 0;
            editBackground.GetComponent<CanvasGroup>().alpha = 0;
            currentText.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    void MoveCursorVertical()
    {
        cursorRect.position = itemStartingPosition - new Vector3(0, cursorCurrentPositionVertical * itemGap.y) + new Vector3(-Screen.width * 0.6f / 2, 0); // screen.width*0.6 = itemRect.sizeDelta.x
    }

    void MoveCursorHorizontal()
    {
        if (cursorCurrentPositionHorizontal == 0)
        {
            cursorCurrentPositionVertical = 0;
            MoveCursorVertical();
        }
        else if (cursorCurrentPositionHorizontal == -1)
        {
            cursorRect.position = saveButton.transform.position;
        }
        else if (cursorCurrentPositionHorizontal == 1)
        {
            cursorRect.position = deleteButton.transform.position;
        }
    }

    void HandleKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (cursorCurrentPositionHorizontal == 0)
            {
                if (cursorCurrentPositionVertical < itemList.Count - 1)
                {
                    cursorCurrentPositionVertical++;
                    MoveCursorVertical();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (cursorCurrentPositionHorizontal == 0)
            {
                if (cursorCurrentPositionVertical > 0)
                {
                    cursorCurrentPositionVertical--;
                    MoveCursorVertical();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (inEditMode)
            {
                if (isOnSave) // on left side
                {
                    cursorInEditRect.position = deleteButton.transform.position;
                    isOnSave = false;
                }
            }
            else
            {
                if (cursorCurrentPositionHorizontal == 0)
                {
                    cursorCurrentPositionHorizontal = 1;
                    MoveCursorHorizontal();
                }
                else if (cursorCurrentPositionHorizontal == -1)
                {
                    if (itemList.Count == 0)
                    {
                        cursorCurrentPositionHorizontal = 1;
                    }
                    else
                    {
                        cursorCurrentPositionHorizontal = 0;
                    }

                    MoveCursorHorizontal();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (inEditMode)
            {
                if (!isOnSave) // on right side
                {
                    cursorInEditRect.position = saveButton.transform.position;
                    isOnSave = true;
                }
            }
            else
            {
                if (cursorCurrentPositionHorizontal == 0)
                {
                    cursorCurrentPositionHorizontal = -1;
                    MoveCursorHorizontal();
                }
                else if (cursorCurrentPositionHorizontal == 1)
                {
                    if (itemList.Count == 0)
                    {
                        cursorCurrentPositionHorizontal = -1;
                    }
                    else
                    {
                        cursorCurrentPositionHorizontal = 0;
                    }

                    MoveCursorHorizontal();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (inEditMode)
            {
                if (isOnSave) // isOnSave = cursor is on saveButton
                {
                    SaveText();
                    inEditMode = false;
                    SwitchModes();
                }
                else // !isOnSave = cursor is on deleteButton
                {
                    inEditMode = false;
                    SwitchModes();
                    DeleteItem();
                }
            }
            else
            {
                if (cursorCurrentPositionHorizontal == -1) // cursor is on +new
                {
                    inEditMode = true;
                    cursorCurrentPositionVertical = itemList.Count;
                    CreateItem();
                    SwitchModes();
                }
                else if (cursorCurrentPositionHorizontal == 1) // cursor is on quit
                {
                    journalFlag = false;
                    firstFlag = true;
                }
                else if (cursorCurrentPositionHorizontal == 0) // cursor is on journal item
                {
                    inEditMode = true;
                    SwitchModes();
                }
            }
        }


        if (inEditMode)
        {
            EditText();
        }
    }

    void ReadText()
    {
        StreamReader sr;
        FileInfo fi;
        fi = new FileInfo(Application.dataPath + "/Resources/Text/Journal/journal_" + cursorCurrentPositionVertical + ".txt");
        
        try
        {
            sr = fi.OpenText();
        }
        catch (Exception e)
        {
            currentText.GetComponent<Text>().text = "new journal";
            SaveText(); // cursorCurrentPositionHorizontal should be the last index of itemlist
            sr = fi.OpenText();
        }

        string text = "";
        string output = "\n";

        do
        {
            text = sr.ReadLine();
            output += text;
        } while (text != null);

        currentText.GetComponent<Text>().text = output;

        sr.Close();
    }

    void SaveText()
    {
        StreamWriter sw;
        FileInfo fi;
        fi = new FileInfo(Application.dataPath + "/Resources/Text/Journal/journal_" + cursorCurrentPositionVertical + ".txt");
        sw = fi.CreateText();
        sw.WriteLine(currentText.GetComponent<Text>().text);
        sw.Flush();
        sw.Close();

        if (currentText.GetComponent<Text>().text.Length > maxNumCharsOnList)
        {
            itemList[cursorCurrentPositionVertical].GetComponent<Text>().text = currentText.GetComponent<Text>().text.Substring(0, maxNumCharsOnList);
        }
        else
        {
            itemList[cursorCurrentPositionVertical].GetComponent<Text>().text = currentText.GetComponent<Text>().text;
        }

        GameObject.Find("EditText_" + cursorCurrentPositionVertical).GetComponent<Text>().text = currentText.GetComponent<Text>().text;
    }
    

    void EditText()
    {
        string str = "";

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "A";
            }
            else
            {
                str = "a";
            }
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "B";
            }
            else
            {
                str = "b";
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "C";
            }
            else
            {
                str = "c";
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "D";
            }
            else
            {
                str = "d";
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "E";
            }
            else
            {
                str = "e";
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "F";
            }
            else
            {
                str = "f";
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "G";
            }
            else
            {
                str = "g";
            }
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "H";
            }
            else
            {
                str = "h";
            }
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "I";
            }
            else
            {
                str = "i";
            }
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "J";
            }
            else
            {
                str = "j";
            }
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "K";
            }
            else
            {
                str = "k";
            }
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "L";
            }
            else
            {
                str = "l";
            }
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "M";
            }
            else
            {
                str = "m";
            }
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "N";
            }
            else
            {
                str = "n";
            }
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "O";
            }
            else
            {
                str = "o";
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "P";
            }
            else
            {
                str = "p";
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "Q";
            }
            else
            {
                str = "q";
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "R";
            }
            else
            {
                str = "r";
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "S";
            }
            else
            {
                str = "s";
            }
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "T";
            }
            else
            {
                str = "t";
            }
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "U";
            }
            else
            {
                str = "u";
            }
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "V";
            }
            else
            {
                str = "v";
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "W";
            }
            else
            {
                str = "w";
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "X";
            }
            else
            {
                str = "x";
            }
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "Y";
            }
            else
            {
                str = "y";
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "Z";
            }
            else
            {
                str = "z";
            }
        }
        else if (Input.GetKeyDown(KeyCode.Period))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = ">";
            }
            else
            {
                str = ".";
            }
        }
        else if (Input.GetKeyDown(KeyCode.Comma))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                str = "<";
            }
            else
            {
                str = ",";
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            str = "1";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            str = "2";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            str = "3";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            str = "4";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            str = "5";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            str = "6";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            str = "7";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            str = "8";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            str = "9";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            str = "0";
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            str = " ";
        }
        else if (Input.GetKeyDown(KeyCode.Exclaim))
        {
            str = "!";
        }
        else if (Input.GetKeyDown(KeyCode.Question))
        {
            str = "?";
        }
        else if (Input.GetKeyDown(KeyCode.LeftParen))
        {
            str = "(";
        }
        else if (Input.GetKeyDown(KeyCode.RightParen))
        {
            str = ")";
        }
        else if (Input.GetKeyDown(KeyCode.Colon))
        {
            str = ":";
        }
        else if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            str = ";";
        }
        else if (Input.GetKeyDown(KeyCode.DoubleQuote))
        {
            str = "\"";
        }
        else if (Input.GetKeyDown(KeyCode.Quote))
        {
            str = "'";
        }
        else if (Input.GetKeyDown(KeyCode.Slash))
        {
            str = "/";
        }
        else if (Input.GetKeyDown(KeyCode.Greater))
        {
            str = ">";
        }
        else if (Input.GetKeyDown(KeyCode.Less))
        {
            str = "<";
        }
        else if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
        {
            if (currentText.GetComponent<Text>().text.Length > 0)
            {
                currentText.GetComponent<Text>().text = currentText.GetComponent<Text>().text.Substring(0, currentText.GetComponent<Text>().text.Length - 1);
            }
        }

        currentText.GetComponent<Text>().text += str;
    }
}