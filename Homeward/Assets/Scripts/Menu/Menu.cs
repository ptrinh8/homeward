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
    public bool isQuit = false;

    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

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
        if (isQuit == true)
        {
            Application.Quit();
        }
        else
        {
            LevelLoader.Instance.LoadLevel("Planet", 3.0F);
        }
    }
}
