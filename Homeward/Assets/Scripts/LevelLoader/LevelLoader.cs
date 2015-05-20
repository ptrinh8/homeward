// ==================================================================================
// <file="LevelLoader.cs" product="Homeward">
// <date>2015-01-11</date>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;

#endregion

public class LevelLoader : MonoBehaviour, ILevelLoader
{
    //Member variables	
    private bool loading = false;
    private List<LoadObject> preActions = new List<LoadObject>();
    private List<LoadObject> actions = new List<LoadObject>();
    private List<LoadObject> postActions = new List<LoadObject>();
    private List<Action> finishedLoadingCallbackActions = new List<Action>();

    private ILevelToLoad levelToLoad;
    private Texture2D textureToRender;
    private Texture2D bgTextureToRender;
    private string label;

    private Rect screenRect;
    private Rect labelRect = new Rect();

    private Stopwatch stopWatch = new Stopwatch();
    private float minimumDuration;

    //Singleton
    private static ILevelLoader instance;

    public Color BG_Color;
    public Texture2D BG_Texture;
    public string LevelLoadingText = "Please Wait! Generating World...";
    public GUIStyle LabelStyle;
    public bool HideCursor = true;
    public Texture2D loadingImage;

    //Properties
    public static ILevelLoader Instance
    {
        get
        {
            return instance ?? (instance = new LevelLoaderDummy());
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            LevelLoaderDummy temp = instance as LevelLoaderDummy;

            if (temp == null)
            {
                Destroy(this.gameObject);
                return;
            }
        }

        instance = this;
        bgTextureToRender = TextureGenerator.CreateTexture(BG_Color);
        textureToRender = BG_Texture;
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        DontDestroyOnLoad(this);
        UpdateLabel(LevelLoadingText);
    }

    void OnLevelWasLoaded()
    {
        StartCoroutine(LoadAllActions());
    }

    private IEnumerator LoadAllActions()
    {
        LoadObject objectToLoad;
        while (preActions.Count > 0)
        {
            objectToLoad = preActions[0];
            UpdateLabel(objectToLoad.Message);
            yield return null;
            objectToLoad.Command.Invoke();
            preActions.Remove(objectToLoad);
        }

        while (actions.Count > 0)
        {
            objectToLoad = actions[0];
            UpdateLabel(objectToLoad.Message);
            yield return null;
            objectToLoad.Command.Invoke();
            actions.Remove(objectToLoad);
        }

        while (postActions.Count > 0)
        {
            objectToLoad = postActions[0];
            UpdateLabel(objectToLoad.Message);
            yield return null;
            objectToLoad.Command.Invoke();
            postActions.Remove(objectToLoad);
        }

        StartCoroutine(FinishLoading());
    }

    void OnGUI()
    {
        GUI.depth = -2;
        if (loading)
        {
            GUI.DrawTexture(screenRect, bgTextureToRender);
            if (textureToRender != null)
            {
                GUI.DrawTexture(screenRect, textureToRender);
            }
            GUI.DrawTexture(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 110, 80, 80), loadingImage, ScaleMode.StretchToFill, true, 10.0F);
            GUI.Label(labelRect, label, LabelStyle);
        }

        if (levelToLoad != null)
        {
            levelToLoad.LoadLevel();
            levelToLoad = null;
        }
    }

    void Update()
    {
        // Debug.Log("Update Is Working");
        // Debug.Log("LevelName: + levelName);

    }

    public void LoadLevel(string levelName)
    {
        LoadLevel(levelName, 0);
    }

    public void LoadLevel(string levelName, float minimumDurationSeconds)
    {
        loading = true;
        levelToLoad = new LoadString(levelName);

        if (HideCursor)
        {
            Screen.showCursor = false;
        }

        UpdateLabel(LevelLoadingText);
        minimumDuration = minimumDurationSeconds;
        stopWatch.Start();
    }

    public void LoadLevel(int levelID)
    {
        LoadLevel(levelID, 0);
    }

    public void LoadLevel(int levelID, float minimumDurationSeconds)
    {
        loading = true;
        levelToLoad = new LoadInt(levelID);

        if (HideCursor)
        {
            Screen.showCursor = false;
        }

        UpdateLabel(LevelLoadingText);
        minimumDuration = minimumDurationSeconds;
        stopWatch.Start();
    }

    public void ReSize()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        UpdateLabel(label);
    }

    private IEnumerator FinishLoading()
    {
        if (stopWatch.ElapsedMilliseconds / 1000.0f < minimumDuration)
        {
            yield return new WaitForSeconds(minimumDuration - (stopWatch.ElapsedMilliseconds / 1000.0f));
        }

        loading = false;

        foreach (Action action in finishedLoadingCallbackActions)
        {
            action.Invoke();
        }

        if (HideCursor)
        {
            Screen.showCursor = true;
        }

        finishedLoadingCallbackActions.Clear();

        stopWatch.Reset();
    }

    private void UpdateLabel(string message)
    {
        label = message;

        Vector2 size = LabelStyle.CalcSize(new GUIContent(message));

        float x = 0, y = 0, width = size.x, height = size.y;

        // LabelLocation set to centre of screen
        x = (Screen.width / 2.0f) - (size.x / 2.0f);
        y = (Screen.height / 2.0f) - (size.y / 2.0f);

        labelRect = new Rect(x, y, width, height);

    }

    public void RegisterPreAction(Action action, string message)
    {
        preActions.Add(new LoadObject(action, message));
    }

    public void RegisterAction(Action action, string message)
    {
        actions.Add(new LoadObject(action, message));
    }

    public void RegisterPostAction(Action action, string message)
    {
        postActions.Add(new LoadObject(action, message));
    }

    public void AssignFinishedLoadingCallback(Action finishedLoadingAction)
    {
        finishedLoadingCallbackActions.Add(finishedLoadingAction);
    }

    private class LoadObject
    {
        public string Message;
        public Action Command;
        public float Weighting = 1.0f;

        public LoadObject(Action action, string message)
        {
            Message = message;
            Command = action;
        }

        public LoadObject(Action action, string message, float weighting)
        {
            Message = message;
            Command = action;
            Weighting = weighting;
        }
    }

    private class LevelLoaderDummy : ILevelLoader
    {
        public void RegisterPreAction(Action action, string message)
        {
            action.Invoke();
        }

        public void RegisterAction(Action action, string message)
        {
            action.Invoke();
        }

        public void RegisterPostAction(Action action, string message)
        {
            action.Invoke();
        }

        public void ReSize()
        {

        }

        public void AssignFinishedLoadingCallback(Action finishedLoadingAction)
        {
            finishedLoadingAction.Invoke();
        }

        public void LoadLevel(string level)
        {
            Application.LoadLevel(level);
        }

        public void LoadLevel(int levelID)
        {
            Application.LoadLevel(levelID);
        }

        public void LoadLevel(string level, float minDuration)
        {
            Application.LoadLevel(level);
        }

        public void LoadLevel(int levelID, float minDuration)
        {
            Application.LoadLevel(levelID);
        }
    }
}


