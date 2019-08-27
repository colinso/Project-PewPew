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
    public float distanceFromPlayer;
    public Rigidbody2D rb;
    public GameObject player;
    public bool stopMovement;
    public EnemyTypes type;
    public EnemyState state;
    protected Vector2 stopPosition;
    private EnemyActions actions;
    public NavMeshAgent2D navi;

    protected virtual void Awake()
    {
        navi = GetComponent<NavMeshAgent2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        stopMovement = false;
        actions = new EnemyActions(player);
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
        GetComponent<NavMeshAgent2D>().destination = actions.DetectAndChase(transform.position, player.transform.position);
    } 

    void die()
    {
        Destroy(gameObject);
    }
}
