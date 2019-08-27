using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions
{
    GameObject player;
    private float attackTimer;
    private bool firstAttack;

    private float originOffset = 0.5f;
    public float cooldown = 2f;


    public EnemyActions(GameObject player)
    {
        this.player = player;
        attackTimer = 0;
        firstAttack = true;
    }

    public Vector2 Follow(Vector2 position, int speed)
    {
        Vector2 playerPosition = player.transform.position;
        return Vector2.MoveTowards(position, playerPosition, speed * Time.deltaTime);
    }

    public Vector2 DetectAndChase(Vector2 position, Vector2 playerPosition)
    {
        Vector2 direction = playerPosition - position;
        RaycastHit2D raycastHit = Physics2D.Raycast(position, direction, 5);

        if (raycastHit.collider != null && raycastHit.collider.tag == "Player" && raycastHit.distance > 1)
        {
            return player.transform.position;
        }
        return position;
    }

    public void MeleeAttack(int damage)
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= cooldown || firstAttack)
        {
            inflictDamage(damage);
            Debug.Log(player.GetComponent<PlayerController>().health);
            attackTimer = 0f;
            firstAttack = false;
        }
    }

    private void inflictDamage(int damage)
    {
        player.GetComponent<PlayerController>().takeDamage(damage);
        if (player.GetComponent<PlayerController>().isDead())
        {
            Debug.Log("Player is Dead :(");
        }
    }

}
