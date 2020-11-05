using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables
    private float xSpeed = 5.0f;
    private float jumpForceBasic = 2.0f;
    private float jumpForceIncrement = 9.0f;
    private bool isJumping = false;
    private float jumpTime = 0.35f;
    private float jumpTimeCounter;
    public LayerMask floorLayer;

    //Components
    private Animator anim;
    private Rigidbody2D rb;
    private InputPlayer input;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputPlayer>();
    }

    void Start()
    {

    }

    private void Update()
    {
        if (input.basicJump && isTouchingFloor())//Cuando se presione espacio se salta
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            Jump();
        }
        if (input.jumpingIntensity && isJumping)
        {
            JumpIntensity();
        }
        if (input.stopJumping)
        {
            isJumping = false;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForceBasic, ForceMode2D.Impulse);
    }
    private void JumpIntensity()
    {
        if (jumpTimeCounter > 0)
        {
            rb.AddForce(Vector2.up * jumpForceIncrement, ForceMode2D.Force);
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }
    }

    private bool isTouchingFloor() //We cast a ray from the player's position to check if it touches the floor layer
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, 0.5f, floorLayer);
    }
}
