using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health;
    public GameObject ammoDrop;
    public int experienceDrop;
    public int damage;
    public int speed;
    public string weakness;
    public int weaknessMultiplier;
    public int distanceFromPlayer;
    public Rigidbody2D rb;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPosition = player.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);

    }

    public void takeDamage(int damageTaken)
    {
        health -= damageTaken;
    }

    public void inflictDamage(int damageInflicted)
    {
        player.GetComponent<PlayerController>().takeDamage(damageInflicted);
    }
}
