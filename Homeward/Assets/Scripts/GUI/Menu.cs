// ==================================================================================
// <file="Menu.cs" product="Homeward">
// <date>2015-01-11</date>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

public class Menu : MonoBehaviour
{
    public bool isQuit;

    public bool isStartGame;

    public bool isContinue;

    void OnMouseEnter()
    {
        guiText.material.color = Color.blue;
    }

    void OnMouseExit()
    {
        guiText.material.color = Color.white;
    }

    void OnMouseUp()
    {
        if (isQuit)
        {
            Application.Quit();
        }
        else if (isStartGame)
        {
            LevelLoader.Instance.LoadLevel("Planet", 3.0F);
        }
    }
}
