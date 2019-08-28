using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBaddieController: EnemyController
{
    public float cooldown = 2f;

    private EnemyActions actions;

    protected override void Awake()
    {
        base.Awake();
        baddieType = EnemyConstants.EnemyTypes.DistanceBaddie;
        damage = 10;
        health = 80;

        actions = new EnemyActions(player, gameObject);

        CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
        circleCollider.radius = 2f;
        circleCollider.isTrigger = true;
    }

    protected virtual void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            print("Within Range, captain");
        }
    }

}
