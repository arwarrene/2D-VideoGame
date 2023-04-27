using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCollector : MonoBehaviour
{

    [SerializeField] private int fruit = 0;
    [SerializeField] private TextMeshProUGUI fruitText;
    [SerializeField] private AudioSource collectionSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pineapple"))
        {
            collectionSound.Play();
            Destroy(collision.gameObject);
            fruit++;
            fruitText.text = "Fruit Collected: " + fruit;
        }
    }
}
