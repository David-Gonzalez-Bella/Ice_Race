using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Variables
    public static GameManager sharedInstance { get; private set; } //Singleton

    void Start()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    void Update()
    {
        
    }
}
