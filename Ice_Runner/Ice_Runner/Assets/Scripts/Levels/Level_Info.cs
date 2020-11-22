using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Info : MonoBehaviour
{
    //Variables
    public float levelSize { get; private set; }

    //References
    public Transform startPoint;
    public Transform endPoint;

    void Start()
    {
        levelSize = endPoint.position.x - startPoint.position.x;
    }

    void Update()
    {
        
    }
}
