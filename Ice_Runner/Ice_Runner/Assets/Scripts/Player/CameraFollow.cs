using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Variables
    public static CameraFollow sharedInstance { get; private set; } //Singleton

    //References
    public Transform player;

    //Camera follow
    public float xOffset;
    public float yOffset;
    private Vector3 camHeight;
    private Vector3 cameraPosition;
    private Vector3 lastCameraPosition;
    private Vector3 tracking;
    private Vector3 camVel;
    private float dampTime = 0.1f;
    [HideInInspector] public float followOffset;
    [HideInInspector] public bool followPlayerY = false;
    public Transform limitStopFollow;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;

        this.transform.position = Vector3.zero;
    }

    void Start()
    {
        camHeight = this.transform.position += Vector3.up * yOffset;
    }

    void Update()
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame && !player.GetComponent<PlayerController>().isDead)
        {
            if (limitStopFollow.position.x - player.position.x > 14)
            {
                if (!followPlayerY)
                {
                    cameraPosition = new Vector3(player.position.x + xOffset, camHeight.y, -10);
                }
                else
                {
                    cameraPosition = new Vector3(player.position.x + xOffset, player.position.y - followOffset + yOffset, -10);
                }
                lastCameraPosition = cameraPosition;
                tracking = Vector3.SmoothDamp(this.transform.position, cameraPosition, ref camVel, dampTime);
            }
        }
        else
        {
            tracking = Vector3.SmoothDamp(this.transform.position, lastCameraPosition, ref camVel, dampTime);
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
            this.transform.position = tracking;
    }
}
