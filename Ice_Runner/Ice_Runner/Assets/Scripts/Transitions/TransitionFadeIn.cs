using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionFadeIn : MonoBehaviour
{
    public string nextScreen;

    public void FinishTransition()
    {
        this.gameObject.SetActive(false);
    }

    public void Transition()
    {
        if (nextScreen.CompareTo("InGame") == 0)
            ScreensManager.sharedInstance.HideAllScreens();
        else
            ScreensManager.sharedInstance.EnableScreen(nextScreen);
    }
}
