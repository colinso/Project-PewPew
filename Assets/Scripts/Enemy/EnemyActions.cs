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
    private int patrolIndex;

    private float originOffset = 0.5f;
    public float cooldown = 0.5f;
    public float detectionMax = 1.5f;
    public int raycastDistance = 10;


    public EnemyActions(GameObject player, GameObject enemy)
    {
        this.player = player;
        this.enemy = enemy;
        attackTimer = 0;
        detectionTimer = 0;
        firstAttack = true;
        patrolIndex = 0;
    }

    public Vector2 Follow(Vector2 position, int speed)
    {
        Vector2 playerPosition = player.transform.position;
        return Vector2.MoveTowards(position, playerPosition, speed * Time.deltaTime);
    }

    public Vector2 DetectAndChase(Vector2 position, Vector2 playerPosition)
    {
        getEnemy().setBaseSpeed();
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
            getEnemy().setPatrolSpeed();
            return Patrol(); // Go back to patrolling
        }
        else
        {
            return position; // When in doubt, stay put
        }
    }

    public Vector2 KeepDistance(Vector2 position, Vector2 playerPosition, float currentDistance)
    {
        getEnemy().setToPlayerSpeed();
        Vector2 direction = playerPosition - position;
        RaycastHit2D raycastHit = Physics2D.Raycast(position, direction, 10);

        if (raycastHit.collider != null && raycastHit.collider.tag == "Player" && raycastHit.distance < 5)
        {
            MeleeAttack(1);
            return getDistantLocation(raycastHit.distance, 5, position, playerPosition); 
        }
        else if (raycastHit.collider != null && raycastHit.collider.tag == "Player" && raycastHit.distance > 5)
        {
            if(raycastHit.distance < 6)
            {
                MeleeAttack(1);
            }
            return getPlayer().transform.position;
        }
        return position;
    }

    public Vector2 Patrol()
    {
        getEnemy().setPatrolSpeed();

        if((Vector2) getEnemy().transform.position != getEnemy().patrolPositions[patrolIndex])
        {
            return getEnemy().patrolPositions[patrolIndex];
        }
        else
        {
            setNextPatrolPosition();
            return getEnemy().patrolPositions[patrolIndex];
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

    private Vector2 getDistantLocation(float distance, float distanceWanted, Vector2 p, Vector2 p1) // p: enemy position; p1: player position
    {
        float xNorm = 1;
        float yNorm = 1;
        if (p.x < p1.x)
        {
            xNorm = -1;
        }
        if (p.y < p1.y)
        {
            yNorm= -1;
        }
        float beta = distance - distanceWanted;
        float x = p.x + ( xNorm * Mathf.Sqrt(Mathf.Pow(beta, 2) / ((Mathf.Pow(p.y - p1.y, 2) / Mathf.Pow(p.x - p1.x, 2)) + 1)) ); // Just trust me, it works
        float y = p.y + ( yNorm * Mathf.Sqrt(Mathf.Pow(beta, 2) / ((Mathf.Pow(p.x - p1.x, 2) / Mathf.Pow(p.y - p1.y, 2)) + 1)) );

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

    private void setNextPatrolPosition()
    {
        if(patrolIndex == getEnemy().patrolPositions.Count -1)
        {
            patrolIndex = 0;
        }
        else
        {
            patrolIndex++;
        }
    }
}
