using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    public int health = 100;
    public GameObject ammoDrop;
    public int experienceDrop;
    public int damage;
    public WeaponController.EnergyTypes weakness;
    public int weaknessMultiplier;
    public float distanceFromPlayer;
    public Rigidbody2D rb;
    public GameObject player;
    public bool stopMovement;
    public EnemyConstants.EnemyTypes baddieType;
    public EnemyState state;
    public NavMeshAgent2D navi;
    public float debuffTime = 5;
	public List<Vector2> patrolPositions;
    public float healthDropChance = .7f;

    protected Vector2 stopPosition;
    protected float freezeMultiplier = 0.75f;
    protected float fireMultiplier = 0.1f;
    protected int firePerTick;
    protected float kineticMultiplier = 1.5f;
    protected float baseSpeed;

    private EnemyActions actions;

    protected virtual void Awake()
    {
        navi = GetComponent<NavMeshAgent2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        stopMovement = false;
		patrolPositions.Add(transform.position);

        actions = new EnemyActions(player, gameObject);

        baseSpeed = GetComponent<NavMeshAgent2D>().speed;
        weaknessMultiplier = 2;


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
        MoveAndAttack();
    }

    protected virtual void MoveAndAttack()
    {
        switch (baddieType)
        {
            case EnemyConstants.EnemyTypes.Baddie:
                GetComponent<NavMeshAgent2D>().destination = actions.DetectAndChase(transform.position, player.transform.position, 1);
                actions.MeleeAttack(damage);
                break;
            case EnemyConstants.EnemyTypes.DistanceBaddie:
                GetComponent<NavMeshAgent2D>().destination = actions.KeepDistance(transform.position, player.transform.position, Vector2.Distance(transform.position, player.transform.position));
                break;
            case EnemyConstants.EnemyTypes.Boss:
                GetComponent<NavMeshAgent2D>().destination = actions.Shuffle();
                break;
            default:
                break;
        }
    }

    public void TakeDamage(int initalDamage, WeaponController.EnergyTypes type)
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
            case WeaponController.EnergyTypes.Electric:
                DamangeElectric();
                break;
            case WeaponController.EnergyTypes.Fire:
                firePerTick = (int)(initalDamage * fireMultiplier);
                InvokeRepeating("FireTick", 1f, 1f);
                StartCoroutine(DamangeFire());
                break;
            case WeaponController.EnergyTypes.Freeze:
                StartCoroutine(DamangeFreeze());
                break;
            case WeaponController.EnergyTypes.Kinetic:
                DamangeKinetic(initalDamage);
                break;
            case WeaponController.EnergyTypes.Explosion:
                // Explosion effect?
                break;
        }


    }

    void Die()
    {
        if(Random.value >= healthDropChance)
            Instantiate((GameObject)Resources.Load("HealthGem", typeof(GameObject)), transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void DamangeElectric()
    {
        // Add electic damage filter
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
        if (GetComponent<NavMeshAgent2D>().speed == baseSpeed)
        {
            GetComponent<NavMeshAgent2D>().speed = (int)(GetComponent<NavMeshAgent2D>().speed * freezeMultiplier);
        }
        yield return new WaitForSeconds(debuffTime);
        GetComponent<NavMeshAgent2D>().speed = baseSpeed;
    }

    IEnumerator DamangeFire()
    {
        yield return new WaitForSeconds(debuffTime);
        CancelInvoke("FireTick");
    }

    public void setBaseSpeed()
    {
        GetComponent<NavMeshAgent2D>().speed = baseSpeed;
    }

    public void setPatrolSpeed()
    {
        GetComponent<NavMeshAgent2D>().speed = baseSpeed - 1.5f;
    }

    public void setToPlayerSpeed()
    {
        GetComponent<NavMeshAgent2D>().speed = player.GetComponent<PlayerMovement>().moveSpeed;
    }

    protected virtual Vector2 bossMoves()
    {
        return new Vector2();
    }

}
