using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    GameObject player;
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        moveDirection = player.transform.position - transform.position;
        moveDirection.z = 0;

        circleCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
        circleCollider.radius = 5f;
        circleCollider.isTrigger = true;
        circleCollider.enabled = false;

        isPlayer = false;
    }
}
