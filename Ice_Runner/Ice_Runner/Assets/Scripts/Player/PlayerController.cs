using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputPlayer))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    //Variables
    //Movement
    public float xSpeed;// = 5.5f;
    public float jumpForceBasic;// = 115.0f;
    public float jumpForceIncrement;// = 400.0f;
    private bool isJumping = false;
    private float jumpTime = 0.15f;
    private float jumpTimeCounter;
    private bool wallJumping = false;
    public float wallJumpForce;// = 110.0f;
    private float finalWallJumpForce;
    private Vector2 wallJumpDirection = new Vector2(-1.2f, 1.2f);
    public LayerMask floorLayer;
    public LayerMask wallsLayer;

    //State
    public bool isDead = false;

    //Animations
    [HideInInspector] public int StopHashCode;
    [HideInInspector] public int DieHashCode;

    //Score
    [HideInInspector] public int score = 0;

    //References
    [HideInInspector] public Animator anim;
    private SpriteRenderer spr;
    private Rigidbody2D rb;
    private InputPlayer input;
    private Collider2D col;
    private Health health;
    [HideInInspector] public Level_Info currentLevel;

    private void Awake()
    {
        spr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputPlayer>();
        col = GetComponent<Collider2D>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        StopHashCode = Animator.StringToHash("Stop");
        DieHashCode = Animator.StringToHash("Die");
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
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
        {
            if (collision.tag.CompareTo("Coin") == 0) //If the player grabs a coin
            {
                UI_Manager.sharedInstance.UpdateScoreText(++score);
                Destroy(collision.gameObject);
            }
            else if (collision.tag.CompareTo("Castle") == 0)
            {
                GameManager.sharedInstance.GoToWinScreen(score, UI_Manager.sharedInstance.countDownTime);
            }
            else if (collision.tag.CompareTo("Spikes") == 0)
            {
                health.CurrentLifes--;
                UI_Manager.sharedInstance.pauseButton.interactable = false;
                DieAnimation();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
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
                    health.CurrentLifes--;
                    DieAnimation();
                }
            }
        }
    }

    public void CheckDie()
    {
        ScreensManager.sharedInstance.transitionAnim.gameObject.SetActive(true);
        StartCoroutine(RespawnTransitionCoroutine());
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
        finalWallJumpForce = rb.velocity.y > 0 ? wallJumpForce : wallJumpForce + (rb.velocity.y * (Physics2D.gravity.y * rb.gravityScale));
        rb.AddForce(wallJumpDirection * new Vector2(wallJumpForce, finalWallJumpForce), ForceMode2D.Impulse);
        wallJumpDirection.x *= -1;
    }

    private void DieAnimation()
    {
        isDead = true;
        StartCoroutine(PauseButtonInteractable());
        GameManager.sharedInstance.FreezePlayer();
        col.enabled = false;
        anim.SetBool(DieHashCode, true);
    }

    public void SendToRespawnPoint()
    {
        isDead = false;
        GameManager.sharedInstance.UnfreezePlayer();
        transform.position = currentLevel.currentRespawnPoint.position;
    }

    private bool isTouchingFloor() //We cast a ray from the player's position to check if it touches the floor layer
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, 0.5f, floorLayer) || Physics2D.Raycast(this.transform.position, Vector2.down, 0.5f, wallsLayer); //The secod conditions is for the case where we step on a wall
    }

    private bool isTouchingWall() //We cast a ray from the player's position to check if it touches the floor layer
    {
        return Physics2D.Raycast(this.transform.position, Vector2.left, 0.8f, wallsLayer) || Physics2D.Raycast(this.transform.position, Vector2.right, 0.8f, wallsLayer);
    }

    IEnumerator RespawnTransitionCoroutine()
    {
        yield return new WaitForSeconds(0.3f);

        col.enabled = true;
        anim.SetBool(DieHashCode, false);

        if (health.CurrentLifes > 0)
        {
            SendToRespawnPoint();
        }
        else
        {
            GameManager.sharedInstance.GoToDeadScreen();
        }
    }

    IEnumerator PauseButtonInteractable()
    {
        UI_Manager.sharedInstance.pauseButton.interactable = false;
        yield return new WaitForSeconds(3.0f);
        UI_Manager.sharedInstance.pauseButton.interactable = true;
    }
 }
