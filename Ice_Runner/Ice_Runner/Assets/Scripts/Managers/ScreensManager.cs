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
    //public GameObject mainMenu;
    //public GameObject chooseSkinScreen;
    //public GameObject chooseLevelScreen;
    //public GameObject winScreen;
    public Text levelTime;
    public Text levelScore;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    public void EnableScreen(string screenName) //This method enables the screen we want, and disables all other screens
    {
        bool screenActive;
        for (int i = 0; i < screens.Length; i++)
        {
            screenActive = screens[i].name.CompareTo(screenName) == 0 ? true : false;
            screens[i].SetActive(screenActive);
        }
    }

    public void DisableScreen(string screenName) //This method enables the screen we want, and disables all other screens
    {

    }
}
