using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour
{
    private float fadeSpeed = 1.5f;     
    private bool sceneStarting = true;  

    void Update()
    {
        if (sceneStarting) { StartScene(); }
    }

    public void FadeToClear()
    {
        guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    void FadeToWhite()
    {
        guiTexture.color = Color.Lerp(guiTexture.color, Color.white, fadeSpeed * Time.deltaTime);
    }

    void StartScene()
    {
        guiTexture.enabled = true;
        FadeToClear();
        guiTexture.enabled = false;

        if (guiTexture.color.a <= 0.05f)
        {
            guiTexture.color = Color.clear;
            guiTexture.enabled = false;

            sceneStarting = false;
        }
    }

    public void EndScene()
    {
        guiTexture.enabled = true;
        FadeToWhite();
        if (guiTexture.color.a >= 0.95f) { Application.LoadLevel(0); }
    }
}