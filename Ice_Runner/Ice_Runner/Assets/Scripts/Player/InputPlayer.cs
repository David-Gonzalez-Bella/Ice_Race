using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    //Variables
    public bool basicJump;
    public bool jumpingIntensity;
    public bool stopJumping;

    void Update()
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
        {
            basicJump = Input.GetMouseButtonDown(0);
            jumpingIntensity = Input.GetMouseButton(0);
            stopJumping = Input.GetMouseButtonUp(0);
        }
    }
}
