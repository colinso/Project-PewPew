using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions
{
    GameObject player;
    GameObject enemy;
    private Vector2 shufflePosition;
    private float attackTimer;
    private float detectionTimer;
    private bool shuffle;
    private bool firstAttack;
    private int patrolIndex;
    private float originOffset = 0.5f;

    public float cooldown = 0.5f;
    public float detectionMax = 1.5f;
    public float shotTimerMax = 1f;
    public float bossShotTimerMax = 0.2f;
    public float shotTimer;
    public int raycastDistance;


    public EnemyActions(GameObject player, GameObject enemy)
    {
        this.player = player;
        this.enemy = enemy;
        attackTimer = 0;
        detectionTimer = 0;
        shotTimer = 0;
        firstAttack = true;
        shuffle = false;
        patrolIndex = 0;
        raycastDistance = 10;
        shufflePosition = enemy.transform.position;
    }

    public Vector2 Follow(Vector2 position, int speed)
    {
        Vector2 playerPosition = player.transform.position;
        return Vector2.MoveTowards(position, playerPosition, speed * Time.deltaTime);
    }

    public void SetDetectionDistance(int distance)
    {
        raycastDistance = distance;
    }

    public Vector2 DetectAndChase(Vector2 position, Vector2 playerPosition, int distanceFromPlayer)
    {
        if(distanceFromPlayer <= 1)
        {
            getEnemy().setBaseSpeed();
        }
        else
        {
            getEnemy().setToPlayerSpeed();
        }
        Vector2 direction = playerPosition - position;
        RaycastHit2D raycastHit = Physics2D.Raycast(position, direction, raycastDistance);
        
        if (raycastHit.collider != null && raycastHit.collider.tag == "Player" && raycastHit.distance > distanceFromPlayer) // Player is detected between raycast distance and 1
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
            fireAtWill();
            return getDistantLocation(raycastHit.distance, 5, position, playerPosition); 
        }
        else if (raycastHit.collider != null && raycastHit.collider.tag == "Player" && raycastHit.distance > 5)
        {
            fireAtWill();
            return getPlayer().transform.position;
        }
        return Patrol();
    }

    public void fireAtWill()
    {
        shotTimer += Time.deltaTime;
        if(shotTimer >= shotTimerMax)
        {
            shotTimer = 0;
            WeaponController weapon = enemy.GetComponentInChildren<WeaponController>();
            weapon.ShootEnemyWeapon(false);
        }
    }

    public void bossFireGun()
    {
        shotTimer += Time.deltaTime;
        if (shotTimer >= bossShotTimerMax)
        {
            shotTimer = 0;
            WeaponController weapon = enemy.GetComponentInChildren<WeaponController>();
            weapon.ShootEnemyWeapon(true);
        }
    }

    public Vector2 Shuffle()
    {
        getEnemy().setToPlayerSpeed();
        Debug.Log(shufflePosition);
        if (shuffle && ( (Vector2) enemy.transform.position == shufflePosition ))
        {
            Debug.Log("Go right");
            shuffle = !shuffle;
            float x = getEnemy().transform.position.x + 1;
            shufflePosition = new Vector2(x, getEnemy().transform.position.y);
            return shufflePosition;
        }
        else if (!shuffle && (Vector2)enemy.transform.position == shufflePosition)
        {
            Debug.Log("Go left");
            shuffle = !shuffle;
            float x = getEnemy().transform.position.x - 1;
            shufflePosition = new Vector2(x, getEnemy().transform.position.y);
            return shufflePosition;
        }
        return shufflePosition;
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

    public void MeleeAttack(int damage, float distanceWanted)
    {
        attackTimer += Time.deltaTime;
        if (Vector2.Distance(getEnemy().transform.position, getPlayer().transform.position) <= distanceWanted && (attackTimer >= cooldown || firstAttack))
        {
            inflictDamage(damage);
            //Debug.Log(player.GetComponent<PlayerController>().health);
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
