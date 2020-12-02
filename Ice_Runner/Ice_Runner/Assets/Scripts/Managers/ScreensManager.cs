using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreensManager : MonoBehaviour
{
    //Variables
    public static ScreensManager sharedInstance { get; private set; }

    //References
    public GameObject winScreen;
    public Text levelTime;
    public Text levelScore;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    void Start()
    {
       // winScreen.SetActive(false);
    }

    void Update()
    {
        
    }
}
