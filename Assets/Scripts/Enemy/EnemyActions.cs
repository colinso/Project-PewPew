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

    private float originOffset = 0.5f;
    public float cooldown = 0.5f;
    public float detectionMax = 1.5f;
    public int raycastDistance = 10;


    public EnemyActions(GameObject player)
    {
        this.player = player;
        attackTimer = 0;
        detectionTimer = 0;
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
            return position; // Stay put
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

    private Vector2 getDistantLocation(float distance, float distanceWanted, Vector2 p1, Vector2 p2) // enemy, player position
    {
        float x = (float) ( 0.5 * ( (2 * 5) - distance + p2.x + p2.y + p1.x + p1.y ) );
        float y = (float) ( 0.5 * ( 5 + p2.x + p2.y - p1.x - p1.y) );
        Debug.Log("x " + x);
        Debug.Log("y " + y);
        return new Vector2(x, y);
    }
}
