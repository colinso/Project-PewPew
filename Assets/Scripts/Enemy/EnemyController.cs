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
    public WeaponController.energyTypes weakness;
    public int weaknessMultiplier;
    public float distanceFromPlayer;
    public Rigidbody2D rb;
    public GameObject player;
    public bool stopMovement;
    public EnemyTypes type;
    public EnemyState state;
    public float debuffTime;

    protected Vector2 stopPosition;
    protected float freezeMultiplier = 0.75f;
    protected float fireMultiplier = 0.1f;
    protected int firePerTick;
    protected float kineticMultiplier = 1.5f;
    protected int baseSpeed;
    protected CircleCollider2D circleCollider;

    private EnemyActions actions;
    public NavMeshAgent2D navi;

    protected virtual void Awake()
    {
        navi = GetComponent<NavMeshAgent2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        stopMovement = false;

        actions = new EnemyActions(player);

        baseSpeed = speed;
        weaknessMultiplier = 2;

        circleCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
        circleCollider.radius = 1.5f;
        circleCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }

    }

    private void FixedUpdate()
    {
        Move();
    }

    public void TakeDamage(int initalDamage, WeaponController.energyTypes type)
    {
        if (weakness == type)
        {
            health -= initalDamage * weaknessMultiplier;
        }
        else
        {
            health -= initalDamage;
        }

        switch (type)
        {
            case WeaponController.energyTypes.Electric:
                DamangeElectric();
                break;
            case WeaponController.energyTypes.Fire:
                firePerTick = (int)(initalDamage * fireMultiplier);
                InvokeRepeating("FireTick", 1f, 1f);
                StartCoroutine(DamangeFire());
                break;
            case WeaponController.energyTypes.Freeze:
                StartCoroutine(DamangeFreeze());
                break;
            case WeaponController.energyTypes.Kinetic:
                DamangeKinetic(initalDamage);
                break;
        }


    }

    public void InflictDamage()
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

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == player)
        {

            stopMovement = true;
            stopPosition = transform.position;

            InflictDamage();
            Debug.Log(player.GetComponent<PlayerController>().health);
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D other)
    {
        stopMovement = false;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void DamangeElectric()
    {
        int numColliders = 5;
        Collider2D[] colliders = new Collider2D[numColliders];
        ContactFilter2D contactFilter = new ContactFilter2D();

        Debug.Log(circleCollider.OverlapCollider(contactFilter, colliders));
        //OverlapCollider
    }

    private void FireTick()
    {
        health -= firePerTick;
    }

    private void DamangeKinetic(int initalDamage)
    {
        health -= (int)(initalDamage * kineticMultiplier - initalDamage);
    }

    IEnumerator DamangeFreeze()
    {
        if (speed == baseSpeed)
        {
            speed = (int)(speed * freezeMultiplier);
        }
        yield return new WaitForSeconds(debuffTime);
        speed = baseSpeed;
    }
    IEnumerator DamangeFire()
    {
        yield return new WaitForSeconds(debuffTime);
        CancelInvoke("FireTick");
    }


}
