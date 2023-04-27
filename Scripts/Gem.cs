using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    protected Animator anim;
    private Collider2D col;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    public void Collected()
    {
        anim.SetTrigger("gemCollected");
        col.enabled = false;
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
