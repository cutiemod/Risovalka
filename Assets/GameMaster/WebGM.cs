//using System.Collections;
//using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.SceneManagement;

public class WebGM : MonoBehaviour
{
    #region Variables declaration

    public string arduinoPortFileName = "/port.txt";

    public float arduinoReadTimeout = 22;

    public string recievedWrongCommand = "#9999#";

    public bool enableCursor = true;

    //public bool EnableConsole = false;

    public const float defaultTextFadeTime = 1f;

    public static WebGM GM;

    //public GameObject consoleObject;
    public Canvas guiCanvas;

    public GameObject winScreen;
    public float winScreenTime;
    public string sceneAfterWin;

    internal int currentGameLine = 0;

    private bool connectedToArduino;

    //private GUIScript ConsoleComponent;

    public bool deleteAudioSource = false;

    #endregion

    void Awake()
    { 
        if (deleteAudioSource)
        {
            AudioSource audioSource = FindObjectOfType<AudioSource>() ?? null;
            if (audioSource != null)
            {
                Destroy(FindObjectOfType<AudioSource>().gameObject);
            }
        }

        //DontDestroyOnLoad(this);

        Cursor.visible = enableCursor;

        //EnablePanels(0, false);
    }

    #region Panels
    internal void EnablePanels(int count, bool enableFlag = true)
    {
        GetComponent<GMPanels>().EnablePanels(count, enableFlag);
    }

    internal void EnablePanels(bool enableFlag = true)
    {
        GetComponent<GMPanels>().EnablePanels(0, enableFlag);
    }

    internal void SetText(string text, int size = 0, float time = defaultTextFadeTime)
    {
        GetComponent<GMPanels>().SetText(text, size, time);
    }

    internal void FadeOutPanels(int count = 0)
    {
        GetComponent<GMPanels>().FadeOutPanels(count);
    }

    internal void MoveTopText(float x, float y)
    {
        GetComponent<GMPanels>().MoveTopText(x, y);
    }
    #endregion
}