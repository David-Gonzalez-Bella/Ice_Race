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
        //if (Input.touchCount > 0) //If a touch in the screen occurs
        //{
        //Touch touch = Input.GetTouch(0); //We get that touch (we'll only have one touch in out game, the one used for jumping)
        //Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

        //basicJump = touch.phase == TouchPhase.Began ? true : false; //If it was just one touch (pulse and release immediately)

        //jumpingIntensity = touch.phase == TouchPhase.Stationary ? true : false; //If the touch has holded 

        //stopJumping = touch.phase == TouchPhase.Ended ? true : false; //If the touch was released

        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
        {
            basicJump = Input.GetMouseButtonDown(0);
            jumpingIntensity = Input.GetMouseButton(0);
            stopJumping = Input.GetMouseButtonUp(0);
        }
    }
}
