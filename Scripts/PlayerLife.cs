using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerLife : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private AudioSource deathSound;

    [SerializeField] private AudioSource gemCollectionSound;


    [SerializeField] private TextMeshProUGUI healthAmount;
    [SerializeField] private int health = 3;



    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        healthAmount.text = "Lives: " + health; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();

        if (collision.gameObject.CompareTag("Trap"))
        {
            health = 0;
            healthAmount.text = "Lives: " + health;
            Die();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            //check if state is MovementState.hurt
            //if it is, deduct health like above

            if (playerMovement.IsHurt())
            {
                health -= 1;
                healthAmount.text = "Lives: " + health;

                if (health <= 0)
                {
                    Die();
                } 
            }
        }
        else if (collision.gameObject.CompareTag("Gem"))
        {
            gemCollectionSound.Play();
            Gem gem = collision.gameObject.GetComponent<Gem>();
            gem.Collected();
            health++;
            healthAmount.text = "Lives: " + health;
        }
}

    private void Die()
    {
        deathSound.Play();
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");

        //Add in new scene to choose if you want to quit or restart level

    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
