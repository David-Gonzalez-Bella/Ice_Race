using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    //Variables
    public bool basicJump;
    public bool jumpingIntensity;
    public bool stopJumping;

    void Start()
    {
 
    }

    void Update()
    {
        basicJump = Input.GetButtonDown("Jump");
        jumpingIntensity = Input.GetButton("Jump");
        stopJumping = Input.GetButtonUp("Jump");
    }
}
