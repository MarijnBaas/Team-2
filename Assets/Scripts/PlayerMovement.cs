using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    private float dirY = 0f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveSpeedCap = 10f;
    [SerializeField] private float jumpForce = 12f;
    private float initialGravScale = 3.0f;
    [SerializeField] private float gravScaleFactor = 2f;

    private enum MovementState { idle, running, jumping, falling }

    [SerializeField] private AudioSource jumpSoundEffect;

    //Abilities
    public bool doubleJumpEnabled = true; // Editable using unlock
    private bool doubleJumpAvailable = false;
    private float doubleJumpTimer = 0.0f;
    [SerializeField] private float doubleJumpCooldown = 0.2f; // Prevents Double jumping too early, has to be >0 as otherwise the player will jump and double jump on the same frame.
    [SerializeField] private float doubleJumpMultiplier = 0.8f;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        initialGravScale = rb.gravityScale;
    }

    // Update is called once per frame
    private void Update()
    {
        // Player Movement
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");
        float acceleration = 0.0f;
        if (rb.velocity.x * dirX < 0){ // Cancelling velocity is quicker than gaining it
            acceleration = moveSpeed*2;
        } else {
            acceleration = moveSpeed;
        }
        rb.AddForce(new Vector2(dirX * acceleration * Time.deltaTime*1000, 0));

        // Player will slide to a halt on the ground
        if (dirX == 0.0f && dirY == 0.0f && IsGrounded()) {
            rb.drag = 3f;
        } else {
            rb.drag = 0.02f;
        }

        // Speed Capping
        if(rb.velocity.x > moveSpeedCap) {
            rb.velocity = new Vector2(moveSpeedCap, rb.velocity.y);
        } else if (-rb.velocity.x > moveSpeedCap){
            rb.velocity = new Vector2(-moveSpeedCap, rb.velocity.y);
        }

        //Jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, 0); // Allow jumping when still falling down, makes jumping easier.
            rb.AddForce(new Vector2(0, jumpForce*100));
            if (doubleJumpEnabled) {
                doubleJumpAvailable = true;
                doubleJumpTimer = Time.fixedTime;
            }
        }
        //Double Jumping
        if (Input.GetButtonDown("Jump") && doubleJumpAvailable && (Time.fixedTime - doubleJumpTimer > doubleJumpCooldown)) {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce*100*doubleJumpMultiplier));
            doubleJumpAvailable = false;
        }

        // Fall faster than you go up, more responsive
        if (rb.velocity.y < -.2f){
            rb.gravityScale = initialGravScale*gravScaleFactor;
        } else {
            rb.gravityScale = initialGravScale;
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
