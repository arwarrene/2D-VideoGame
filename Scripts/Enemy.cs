using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void JumpedOn()
    {
        anim.SetTrigger("enemydeath");
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
