using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreensManager : MonoBehaviour
{
    //Variables
    public static ScreensManager sharedInstance { get; private set; }

    //References
    public GameObject[] screens;
    public GameObject darkBackground;
    public TransitionFadeIn transitionAnim;
    public Text levelTime;
    public Text levelScore;

    public Text fpsText;
    private float deltaTime;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }

    public void ShowSettings(bool settingsEnabled)
    {
        screens[7].SetActive(settingsEnabled);
    }

    public void ShowWannaLeaveMM(bool settingsEnabled)
    {
        screens[8].SetActive(settingsEnabled);
    }

    public void ShowWannaLeaveIG(bool settingsEnabled)
    {
        screens[9].SetActive(settingsEnabled);
    }

    public void StartTransitionAnim(string screenName)
    {
        transitionAnim.nextScreen = screenName;
        transitionAnim.gameObject.SetActive(true);
    }

    public void HideAllScreens()
    {
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].SetActive(false);
        }
    }

    public void EnableScreen(string screenName) //This method enables the screen we want, and disables all other screens
    {
        //bool screenActive;
        for (int i = 0; i < screens.Length; i++)
        {
            if (screens[i].name.CompareTo(screenName) == 0)
                screens[i].SetActive(true);
            else if (screens[i].activeSelf)
                screens[i].SetActive(false);
        }
    }
}
