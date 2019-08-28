﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Vector3 moveDirection;
    public Rigidbody2D rb;
    public bool isPlayer;
    protected WeaponController.EnergyTypes energyType;
    protected WeaponController.WeaponTypes weaponType;
    protected int damage = 20;
    protected float speed = 5f;
    protected CircleCollider2D circleCollider;
    private bool hit;
    private HashSet<EnemyController> hitList;
    private float killDelay = 0f;

    // Start is called before the first frame update
    public virtual void Start()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 10;

        moveDirection = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position);
        moveDirection.z = 0;

        circleCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
        circleCollider.radius = 1.5f;
        circleCollider.isTrigger = true;
        circleCollider.enabled = false;
        isPlayer = true;

        hitList = new HashSet<EnemyController>();
    }

    private void Update()
    {
        if (hit && !isPlayer)
        {
            Destroy(gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().takeDamage(5);
        }
        else if (!hit || !isPlayer)
        {
            transform.position = transform.position + moveDirection.normalized * speed * Time.deltaTime;
        } else
        {
            killDelay -= Time.deltaTime;
            if (killDelay <= 0)
            {

                foreach (var item in hitList)
                {
                    Debug.Log(item);
                    item.TakeDamage(damage, energyType);
                }

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collision2D collision)
    {
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        if (isPlayer)
        {

            if (enemy != null && energyType == WeaponController.EnergyTypes.Electric)
            {
                hitList.Add(enemy);
                circleCollider.enabled = true;
                hit = true;
                transform.position = enemy.transform.position;
            } else if (!player && !enemy && !hit && !projectile)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if(player != null)
            {

                circleCollider.enabled = true;
                hit = true;
                transform.position = player.transform.position;
            }
        }
    }

    public void changeType(WeaponController.EnergyTypes newEng) {
        energyType = newEng;
    }
}
