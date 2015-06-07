
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class Computer : MonoBehaviour {

    public int MAX_CHARS_WIDTH; // set in inspector

    public int MAX_CHARS_HEIGHT; // set in inspector

    public static bool journalFlag;

    private RectTransform journalRect;

    private GameObject journalTextGameObject;

    private string prompt;

    private string indent;

    private string redirect;

	void Start () 
    {
        Initialize();
        CreateLayout();
	}

    void Initialize()
    {
        prompt = "\n>> ";
        indent = "\n   ";
        redirect = " >> ";
    }

    void CreateLayout()
    {
        journalRect = gameObject.GetComponent<RectTransform>();
        journalRect.localScale = new Vector3(1, 1, 1);
        //journalRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 5 * journalRect.sizeDelta.x);
        //journalRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 5 * journalRect.sizeDelta.y);

        journalTextGameObject = Instantiate(Resources.Load("Computer/ComputerText")) as GameObject;
        journalTextGameObject.transform.SetParent(gameObject.transform);
        journalTextGameObject.name = "JournalText";
        RectTransform journalTextRect = journalTextGameObject.GetComponent<RectTransform>();
        journalTextRect.localPosition = journalRect.localPosition;
        journalTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 0.6f);
        journalTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * 0.6f);
        journalTextGameObject.GetComponent<Text>().text = prompt; // journalTextGameObject is Text with rect transform
    }

    void Save(string content, string fileName)
    {
        StreamWriter sw;
        FileInfo fi;
        fi = new FileInfo(Application.dataPath + "/Resources/Text/Computer" + fileName); // fileName includes '.txt'
        sw = fi.AppendText();
        sw.WriteLine(content);
        sw.Flush();
        sw.Close();

        /*** this is another way ***/
        //FileStream f = new FileStream(/*** add path here ***/fileName, FileMode.Create, FileAccess.Write);
        //BinaryWriter writer = new BinaryWriter(f);
        //writer.Write(content);
        //writer.Close();
    }

    string Read(string fileName)
    {
        StreamReader sr;
        FileInfo fi;
        fi = new FileInfo(Application.dataPath + "/Resources/Text/Computer" + fileName); //TODO try catch : if there is no such file
        sr = fi.OpenText();

        string text = "";
        string output = "\n";

        do
        {
            text = sr.ReadLine();
            output += text;
        } while (text != null);

        output += "\n";

        return output;
    }

	void Update () 
    {
        if (journalFlag)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1;

            HandleKeyInput();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                HandleReturnKey();
            }
        }
        else
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }

        
	}


    void HandleKeyInput()
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
            if (journalTextGameObject.GetComponent<Text>().text.Length > 0)
            {
                if (!journalTextGameObject.GetComponent<Text>().text.Substring(journalTextGameObject.GetComponent<Text>().text.Length - prompt.Length, prompt.Length).Equals(prompt))
                {
                    if (journalTextGameObject.GetComponent<Text>().text.Substring(journalTextGameObject.GetComponent<Text>().text.Length - indent.Length, indent.Length).Equals(indent))
                    {
                        journalTextGameObject.GetComponent<Text>().text = journalTextGameObject.GetComponent<Text>().text.Substring(0, journalTextGameObject.GetComponent<Text>().text.Length - indent.Length - 1);
                    }
                    else
                    {
                        journalTextGameObject.GetComponent<Text>().text = journalTextGameObject.GetComponent<Text>().text.Substring(0, journalTextGameObject.GetComponent<Text>().text.Length - 1);
                    }
                }
            }
        }

        journalTextGameObject.GetComponent<Text>().text += str;
        journalTextGameObject.GetComponent<Text>().text = Indent(journalTextGameObject.GetComponent<Text>().text);
    }

    void HandleReturnKey()
    {
        string journalString = journalTextGameObject.GetComponent<Text>().text;
        string line = journalString.Substring(journalString.LastIndexOf(prompt));
        line = line.Replace(prompt, "");
        line = line.Replace(indent, "");
        //Debug.Log("line:" + line);

        
        if (line.Equals("cls", System.StringComparison.OrdinalIgnoreCase))
        {
            journalString = prompt;
        }
        else if (line.Equals("exit", System.StringComparison.OrdinalIgnoreCase))
        {
            journalString = prompt;
            journalFlag = false;
        }
        else if (line.Equals("man", System.StringComparison.OrdinalIgnoreCase))
        {
            journalString +=
                prompt + "not done yet" +
                //"\n>> man: Manual" + 
                //"\n>> cls: Clear Screen" +
                //"\n>> exit: Exit" + 
                //"\n>> mkdir: Create a new directory" +
                //"\n>> copy 'a' 'b': Copy a file 'a' into 'b'" + 
                prompt;
        }
        else if (line.Equals("ls", System.StringComparison.OrdinalIgnoreCase))
        {

        }
        else if (line.Equals("cd", System.StringComparison.OrdinalIgnoreCase))
        {

        }
        else if (line.Equals("move", System.StringComparison.OrdinalIgnoreCase))
        {

        }
        else if (line.Equals("john romero", System.StringComparison.OrdinalIgnoreCase))
        {
            journalString +=
                prompt +
                "'Yo, what's up'" +
                prompt;
        }
        else if (line.Contains("load") && line.Substring(0, "load".Length).Equals("load", System.StringComparison.OrdinalIgnoreCase))
        {
            if (line.Contains(".txt"))
            {
                string fileName = line.Substring("load".Length, line.LastIndexOf(".txt")).Trim();
                //Debug.Log("file='" + fileName + "'");

                if (fileName.Length > ".txt".Length && fileName.Substring(fileName.Length - ".txt".Length, ".txt".Length).Equals(".txt"))
                {
                    journalString += Read(fileName);
                }
                else
                {
                    journalString = journalString + prompt + "Error: file extension is not correct" + prompt;
                }
            }

            journalString += prompt;
        }
        else if (line.Contains("echo") && line.Substring(0, "echo".Length).Equals("echo", System.StringComparison.OrdinalIgnoreCase))
        {
            if (line.Contains(redirect) && line.Substring(line.IndexOf(redirect)).Contains(".txt"))
            {
                string content = line.Substring("echo".Length, line.LastIndexOf(redirect)).Replace(redirect, "");
                string fileName = line.Substring(line.LastIndexOf(redirect)).Replace(redirect, "");

                content = content.Trim();
                fileName = fileName.Trim();
                //Debug.Log("content='" + content + "'");
                //Debug.Log("file='" + fileName + "'");

                if (fileName.Length > ".txt".Length && fileName.Substring(fileName.Length - ".txt".Length, ".txt".Length).Equals(".txt"))
                {
                    Save(content, fileName);
                }
                else
                {
                    journalString = journalString + prompt + "Error: file extension is not correct" + prompt;
                }
            }

            journalString += prompt;
        }
        else
        {
            journalString += prompt;
        }

        journalString = Scroll(journalString);
        journalTextGameObject.GetComponent<Text>().text = journalString;
    }

    string Indent(string str)
    {
        if (str.Length > MAX_CHARS_WIDTH)
        {
            if (!str.Substring(str.Length - (MAX_CHARS_WIDTH + 1), MAX_CHARS_WIDTH + 1).Contains("\n"))
            {
                str += "\n   ";
                str = Scroll(str);
            }
        }

        return str;
    }

    string Scroll(string str)
    {
        if (str.Length - str.Replace("\n".ToString(), "").Length > MAX_CHARS_HEIGHT)
        {
            str = str.Substring(prompt.Length);
            str = str.Substring(str.IndexOf("\n"));
        }

        return str;
    }
}
