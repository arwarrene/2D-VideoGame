using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength;
    [SerializeField] private float jumpHeight;

    [SerializeField] private LayerMask jumpableGround;

    private Collider2D col;
    private Rigidbody2D rb;
 
    private bool facingLeft = true;

    protected override void Start()
    {
        base.Start();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < .1f)
            {
                anim.SetBool("falling", true);
                anim.SetBool("jumping", false);
            }
        }

        if(IsGrounded() && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }

    private void Movement()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {

                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                if (IsGrounded())
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("jumping", true);

                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                if (IsGrounded())
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
