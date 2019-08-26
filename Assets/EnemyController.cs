using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class EnemyController : MonoBehaviour
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
    public bool stopMovement;
    public EnemyTypes type;
    protected Vector2 stopPosition;

    protected virtual void Awake()
    {
        stopMovement = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void takeDamage(int damageTaken)
    {
        health -= damageTaken;

        if (health <= 0)
        {
            die();
        }
    }

    public void inflictDamage()
    {   
        player.GetComponent<PlayerController>().takeDamage(damage);
        if (player.GetComponent<PlayerController>().isDead())
        {
            Debug.Log("Player is Dead :(");
        }
        
    }

    protected virtual void Move()
    {
        if (stopMovement == true)
        {
            transform.position = stopPosition;
        }
        else
        {
            Vector2 playerPosition = player.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        stopMovement = true;
        stopPosition = transform.position;
        if (other.gameObject == player)
        {
            inflictDamage();
            Debug.Log(player.GetComponent<PlayerController>().health);
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D other)
    {
        stopMovement = false;
    }

    void die()
    {
        Destroy(gameObject);
    }
}
