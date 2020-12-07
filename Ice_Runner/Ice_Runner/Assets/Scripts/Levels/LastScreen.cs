using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastScreen : MonoBehaviour
{
    public void DestroyCurrentLevel()
    {
        GameManager.sharedInstance.DestroyCurrentLevel();
    }
}
