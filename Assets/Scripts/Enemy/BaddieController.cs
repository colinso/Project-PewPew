using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieController: EnemyController
{
    private float attackTimer;
    public float cooldown = 2f;
    private bool firstAttack;
    protected override void Awake()
    {
        base.Awake();
        type = EnemyTypes.Baddie;
        damage = 20;
        health = 40;
        attackTimer = 0;
        firstAttack = true;

        CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
        circleCollider.radius = 0.1f;
        circleCollider.isTrigger = true;
    }

    protected virtual void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject == player)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        attackTimer += Time.deltaTime;
        if(attackTimer >= cooldown || firstAttack)
        {
            InflictDamage();
            Debug.Log(player.GetComponent<PlayerController>().health);
            attackTimer = 0f;
            firstAttack = false;
        }
    }
}
