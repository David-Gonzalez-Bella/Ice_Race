using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputPlayer))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    //Variables
    //Movement
    private float xSpeed = 5.0f;
    private float jumpForceBasic = 2.0f;
    private float jumpForceIncrement = 9.0f;
    private bool isJumping = false;
    private float jumpTime = 0.35f;
    private float jumpTimeCounter;
    private bool wallJumping = false;
    private float wallJumpForce = 4.0f;
    private Vector2 wallJumpDirection = new Vector2(-1.0f, 1.5f);
    public LayerMask floorLayer;
    public LayerMask wallsLayer;

    //Score
    private int score = 0;

    //References
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
        //Jumping
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

        //Wall-jumping
        if (!isTouchingFloor() && isTouchingWall())
        {
            if (input.basicJump)
            {
                WallJump();
            }
        }
        if (isTouchingFloor())
        {
            wallJumping = false;
            wallJumpDirection.x = -1;
        }
    }

    void FixedUpdate()
    {
        if (!wallJumping)
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.CompareTo("Coin") == 0) //If the player grabs a coin
        {
            UI_Manager.sharedInstance.UpdateScoreText(++score);
        }
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

    private void WallJump()
    {
        wallJumping = true;
        rb.AddForce(wallJumpDirection * wallJumpForce, ForceMode2D.Impulse);
        wallJumpDirection.x *= -1;
    }

    private bool isTouchingFloor() //We cast a ray from the player's position to check if it touches the floor layer
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, 0.5f, floorLayer);
    }

    private bool isTouchingWall() //We cast a ray from the player's position to check if it touches the floor layer
    {
        return Physics2D.Raycast(this.transform.position, Vector2.left, 0.5f, wallsLayer) || Physics2D.Raycast(this.transform.position, Vector2.right, 0.5f, wallsLayer);
    }
}
