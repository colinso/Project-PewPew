﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class EnemyController : MonoBehaviour
{
    public int health = 100;
    public GameObject ammoDrop;
    public int experienceDrop;
    public int damage;
    public WeaponController.energyTypes weakness;
    public int weaknessMultiplier;
    public float distanceFromPlayer;
    public Rigidbody2D rb;
    public GameObject player;
    public bool stopMovement;
    public EnemyTypes type;
    public EnemyState state;
    public float debuffTime = 5;

    protected Vector2 stopPosition;
    protected float freezeMultiplier = 0.75f;
    protected float fireMultiplier = 0.1f;
    protected int firePerTick;
    protected float kineticMultiplier = 1.5f;
    protected float baseSpeed;

    private EnemyActions actions;
    public NavMeshAgent2D navi;

    protected virtual void Awake()
    {
        navi = GetComponent<NavMeshAgent2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        stopMovement = false;

        actions = new EnemyActions(player);

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
        Move();
    }

    protected virtual void Move()
    {
        GetComponent<NavMeshAgent2D>().destination = actions.DetectAndChase(transform.position, player.transform.position);
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

    void Die()
    {
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


}
