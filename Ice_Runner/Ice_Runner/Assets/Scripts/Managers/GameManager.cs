using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Variables
    public static GameManager sharedInstance { get; private set; } //Singleton

    //References
    public GameObject player;
    public Camera mainCamera;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    void Start()
    {
        ScaleCamera();
    }

    void Update()
    {
        
    }

    private void ScaleCamera()
    {
        Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
        float srcWidth = Screen.width;
        float srcHeight = Screen.height;

        float DEVICE_SCREEN_ASPECT = srcWidth / srcHeight;
        mainCamera.aspect = DEVICE_SCREEN_ASPECT;
    }
}
