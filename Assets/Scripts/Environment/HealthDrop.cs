using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public AudioClip healthClip;
    public int health = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(healthClip, transform.position);
            PlayerController player = collision.GetComponent<PlayerController>();
            player.health += health;
            Destroy(gameObject);
        }
    }
}
