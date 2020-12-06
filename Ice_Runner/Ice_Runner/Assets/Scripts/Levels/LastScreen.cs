using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastScreen : MonoBehaviour
{
    public void DestroyCurrentLevel()
    {
        UI_Manager.sharedInstance.inGameUI.SetActive(false);
        GameManager.sharedInstance.DestroyCurrentLevel();
    }
}
