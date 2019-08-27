using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions
{
    GameObject player;
    private float attackTimer;
    public float cooldown = 2f;
    private bool firstAttack;

    public EnemyActions(GameObject player)
    {
        this.player = player;
        attackTimer = 0;

    }

    public Vector2 Follow(Vector2 position, int speed)
    {
        Vector2 playerPosition = player.transform.position;
        return Vector2.MoveTowards(position, playerPosition, speed * Time.deltaTime);
    }

    //public void MeleeAttack()
    //{
    //    attackTimer += Time.deltaTime;
    //    if (attackTimer >= cooldown || firstAttack)
    //    {
    //        inflictDamage();
    //        Debug.Log(player.GetComponent<PlayerController>().health);
    //        attackTimer = 0f;
    //        firstAttack = false;
    //    }
    //}
}
