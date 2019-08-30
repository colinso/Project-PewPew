using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    public int xMin = -12;
    public int xMax = 12;
    public int yMin = -8;
    public int yMax = 8;

    public GameObject EnemyPrefab1;
    public GameObject EnemyPrefab2;
    public float cooldown = 2f;
    private EnemyActions actions;
    enum BossStates { Shuffle, Chase, Shoot, ChangeType, Spawn, Smash, Explosions}
    BossStates state;
    float chaseTimerMax = 3f;
    float chaseTimer;
    float shuffleTimerMax = 2f;
    float shuffleTimer;
    float smashTimerMax = 1.5f;
    float smashTimer;
    float shootTimerMax = 2f;
    float shootTimer;
    float spawnTimerMax = 2f;
    float spawnTimer;

    protected override void Awake()
    {
        base.Awake();
        baddieType = EnemyConstants.EnemyTypes.Boss;
        damage = 20;
        health = 5000;
        maxHealth = 5000;
        chaseTimer = 0;
        shuffleTimer = 0;
        spawnTimer = 0;
        state = BossStates.Chase;
        GetComponent<SpriteRenderer>().color = new Color(.85f, .185f, .194f);

        actions = new EnemyActions(player, gameObject);
        actions.SetDetectionDistance(50);
    }

    protected override Vector2 bossMoves()
    {
        switch (state)
        {
            case BossStates.Shuffle:
                if (shuffleTimer >= shuffleTimerMax)
                {
                    shuffleTimer = 0;
                    actions.MeleeAttack(12, 8);
                    print("Get Random Attack");
                    //state = BossStates.ChangeType;
                    state = (BossStates)getRandomAttack();
                    randomSpawn();
                    print("State: " + state);
                    break;
                }
                shuffleTimer += Time.deltaTime;
                return transform.position;
            case BossStates.Chase:
                actions.SetDetectionDistance(50);
                if(chaseTimer >= chaseTimerMax)
                {
                    chaseTimer = 0;
                    actions.MeleeAttack(12, 8);
                    print("attack!");
                    state = BossStates.Shuffle;
                    break;
                }
                chaseTimer += Time.deltaTime;
                return actions.DetectAndChase(transform.position, player.transform.position, 4);
            case BossStates.Shoot:
                if(shootTimer <= shootTimerMax)
                {
                    actions.bossFireGun();
                }
                else
                {
                    shootTimer = 0;
                    state = BossStates.Shuffle;
                    break;
                }
                shootTimer += Time.deltaTime;
                return actions.DetectAndChase(transform.position, player.transform.position, 4);
            case BossStates.Smash:
                if(smashTimer >= smashTimerMax)
                {
                    smashTimer = 0;
                    actions.MeleeAttack(12, 8);
                    state = BossStates.Shuffle;
                }
                smashTimer += Time.deltaTime;
                return transform.position;
            case BossStates.ChangeType:
                weakness = (WeaponController.EnergyTypes) getRandomType();
                switch (weakness)
                {
                    case WeaponController.EnergyTypes.Electric:
                        GetComponent<SpriteRenderer>().color = new Color(.85f, .185f, .194f);
                        break;
                    case WeaponController.EnergyTypes.Fire:
                        GetComponent<SpriteRenderer>().color = new Color(1f, 0.84f, 0.07f);
                        break;
                    case WeaponController.EnergyTypes.Freeze:
                        GetComponent<SpriteRenderer>().color = new Color(0f, 0.8f, 1f);
                        break;
                    case WeaponController.EnergyTypes.Kinetic:
                        GetComponent<SpriteRenderer>().color = new Color(0.39f, 1f, 0.51f);
                        break;
                    case WeaponController.EnergyTypes.Explosion:
                        GetComponent<SpriteRenderer>().color = Color.grey;
                        break;
                }
                state = BossStates.Shuffle;
                break;
            case BossStates.Spawn:
                if(spawnTimer >= spawnTimerMax)
                {
                    spawnTimer = 0;
                    randomSpawn();
                    state = BossStates.Shuffle;
                }
                spawnTimer += Time.deltaTime;
                break;
            default:
                break;
        }
        return transform.position;
    }

    private int getRandomAttack()
    {
        System.Random rand = new System.Random();
        return rand.Next(1, 5);
    }

    private int getRandomType()
    {
        System.Random rand = new System.Random();
        return rand.Next(0, 5);
    }

    private void randomSpawn()
    {
        Vector2 pos = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
        Instantiate(EnemyPrefab1, pos, transform.rotation);

        pos = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
        Instantiate(EnemyPrefab2, pos, transform.rotation);
    }
}
