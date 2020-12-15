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

    [HideInInspector] public Transform currentRespawnPoint;
    public Transform[] respawnPoints;


    void Start()
    {
        currentRespawnPoint = respawnPoints[0];
        levelSize = endPoint.position.x - startPoint.position.x;
    }
}
