using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions
{
    GameObject player;
    GameObject enemy;
    private float attackTimer;
    private float detectionTimer;
    private bool firstAttack;
    private bool toPatrolPosition;

    private float originOffset = 0.5f;
    public float cooldown = 0.5f;
    public float detectionMax = 1.5f;
    public int raycastDistance = 5;


    public EnemyActions(GameObject player, GameObject enemy)
    {
        this.player = player;
        this.enemy = enemy;
        attackTimer = 0;
        detectionTimer = 0;
        firstAttack = true;
        toPatrolPosition = true;
    }

    public Vector2 Follow(Vector2 position, int speed)
    {
        Vector2 playerPosition = player.transform.position;
        return Vector2.MoveTowards(position, playerPosition, speed * Time.deltaTime);
    }

    public Vector2 DetectAndChase(Vector2 position, Vector2 playerPosition)
    {
        enemy.GetComponent<NavMeshAgent2D>().speed = getEnemy().getBaseSpeed();
        Vector2 direction = playerPosition - position;
        RaycastHit2D raycastHit = Physics2D.Raycast(position, direction, raycastDistance);
        
        if (raycastHit.collider != null && raycastHit.collider.tag == "Player" && raycastHit.distance > 1) // Player is detected between raycast distance and 1
        {
            detectionTimer = 0;
            return playerPosition; // Chase player
        }
        else if( (raycastHit.collider == null || raycastHit.collider.tag != "Player") && detectionTimer < detectionMax) // Player is not detected within range, but enemy can still chase
        {
            detectionTimer += Time.deltaTime; // Keep chasing player, but increase timer
            return playerPosition;
        }
        else if ( (raycastHit.collider == null || raycastHit.collider.tag != "Player") && detectionTimer >= detectionMax) // Player is not detected and timer is run out
        {
            enemy.GetComponent<NavMeshAgent2D>().speed = getEnemy().getPatrolSpeed();
            return Patrol(); // Go back to original position
        }
        else
        {
            return position; // When in doubt, stay put
        }
    }

    public Vector2 KeepDistance(Vector2 position, Vector2 playerPosition, float currentDistance)
    {
        Debug.Log("Keeping Distance " + currentDistance);
        Vector2 direction = playerPosition - position;
        RaycastHit2D raycastHit = Physics2D.Raycast(position, direction, raycastDistance);

        Debug.Log(raycastHit.distance);
        if (raycastHit.collider != null && raycastHit.collider.tag == "Player" && raycastHit.distance < raycastDistance)
        {
            Debug.Log("Get that distance");
            return getDistantLocation(currentDistance, 5, position, playerPosition); 
        }
        Debug.Log("Staying Put");
        return position;
    }

    public Vector2 Patrol()
    {
        enemy.GetComponent<NavMeshAgent2D>().speed = getEnemy().getPatrolSpeed();
        if (toPatrolPosition && (Vector2) getEnemy().transform.position != getEnemy().patrolPosition)
        {
            return getEnemy().patrolPosition;
        }
        else if (toPatrolPosition && (Vector2) getEnemy().transform.position == getEnemy().patrolPosition)
        {

            return getEnemy().startPosition;
        }
        else if (!toPatrolPosition && (Vector2)getEnemy().transform.position != getEnemy().startPosition)
        {
            return getEnemy().startPosition;
        }
        else
        {
            return getEnemy().patrolPosition;
        }
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
        getPlayer().takeDamage(damage);
        if (getPlayer().isDead())
        {
            Debug.Log("Player is Dead :(");
        }
    }

    private Vector2 getDistantLocation(float distance, float distanceWanted, Vector2 p1, Vector2 p2) // enemy, player position
    {
        float x = (float) ( 0.5 * ( (2 * 5) - distance + p2.x + p2.y + p1.x + p1.y ) );
        float y = (float) ( 0.5 * ( 5 + p2.x + p2.y - p1.x - p1.y) );
        Debug.Log("x " + x);
        Debug.Log("y " + y);
        return new Vector2(x, y);
    }

    private PlayerController getPlayer()
    {
        return player.GetComponent<PlayerController>();
    }

    private EnemyController getEnemy()
    {
        return enemy.GetComponent<EnemyController>();
    }
}
