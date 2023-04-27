using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float hurtForce = 10f;

    private enum MovementState { idle, running, jumping, falling, crouch, hurt }
    private MovementState state = MovementState.idle;

    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource crouchSound;
    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private AudioSource deathSound;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (state != MovementState.hurt)
        {
            Movement();
        }

        UpdateAnimation();
        anim.SetInteger("state", (int)state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (state == MovementState.falling)
            {
                Jump();
                enemy.JumpedOn();
                deathSound.Play();
            }
            else
            {
                state = MovementState.hurt;
                hurtSound.Play();


                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.position.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.position.y);
                }

            }

        }
    }

    private void Movement()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }
    }

    public void Jump()
    {
        state = MovementState.jumping;
        jumpSound.Play();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }


    }

    private void CheckDirection()
    {
        if (dirX > 0f)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }

    private void UpdateAnimation()
    {
        if (state == MovementState.jumping)
        {
            CheckDirection();

            if (rb.velocity.y < -.1f)
            {
                state = MovementState.falling;
            }
        }
        else if (state == MovementState.falling)
        {
            CheckDirection();

            if (IsGrounded())
            {
                state = MovementState.idle;
            }
        }
        else if (Input.GetButton("Crouch") && IsGrounded())
        {
            state = MovementState.crouch;
            crouchSound.Play();
            CheckDirection();
        }
        else if (state == MovementState.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = MovementState.idle;
            }
        }
        else if (dirX > 0f && !(Input.GetButton("Crouch")))
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f && !(Input.GetButton("Crouch")))
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }
    }

    public bool IsHurt()
    {
        if (state == MovementState.hurt)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
