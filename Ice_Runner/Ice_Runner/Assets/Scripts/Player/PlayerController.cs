using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputPlayer))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    //Variables
    //Movement
    public float xSpeed = 5.5f;
    public float jumpForceBasic = 115.0f;
    public float jumpForceIncrement = 400.0f;
    private bool isJumping = false;
    private float jumpTime = 0.15f;
    private float jumpTimeCounter;
    private bool wallJumping = false;
    public float wallJumpForce = 112.0f;
    private Vector2 wallJumpDirection = new Vector2(-1.0f, 1.2f);
    public LayerMask floorLayer;
    public LayerMask wallsLayer;

    //Score
    private int score = 0;

    //References
    private SpriteRenderer spr;
    private Animator anim;
    private Rigidbody2D rb;
    private InputPlayer input;

    private void Awake()
    {
        spr = GetComponentInChildren<SpriteRenderer>();
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
            Destroy(collision.gameObject);
        }
        else if (collision.tag.CompareTo("Castle") == 0)
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        if (collider.tag.CompareTo("Enemy") == 0)
        {
            GameObject enemy = collision.gameObject;
            float otherSpriteOffset = enemy.GetComponent<Enemy_Basic>().sr.size.y / 4;
            Vector3 contactPoint = collision.contacts[0].point; //Get the point of contact
            Vector3 center = collider.bounds.center; //The center of the 2D sphere collider
            if (contactPoint.y - spr.size.y / 2 > center.y + otherSpriteOffset)
            {
                Destroy(enemy);
                rb.AddForce(Vector2.up * 100.0f, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("Moririas");
            }
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
        return Physics2D.Raycast(this.transform.position, Vector2.down, 0.5f, floorLayer) || Physics2D.Raycast(this.transform.position, Vector2.down, 0.5f, wallsLayer); //The secod conditions is for the case where we step on a wall
    }

    private bool isTouchingWall() //We cast a ray from the player's position to check if it touches the floor layer
    {
        return Physics2D.Raycast(this.transform.position, Vector2.left, 0.8f, wallsLayer) || Physics2D.Raycast(this.transform.position, Vector2.right, 0.8f, wallsLayer);
    }
}
