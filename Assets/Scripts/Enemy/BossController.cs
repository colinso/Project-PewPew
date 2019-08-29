using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    public float cooldown = 2f;
    private EnemyActions actions;
    enum BossStates { Shuffle, Chase, Shoot, ChangeType, Smash, Explosions, Spawn}
    BossStates state;
    float chaseTimerMax = 2f;
    float chaseTimer;
    float shuffleTimerMax = 1f;
    float shuffleTimer;
    float smashTimerMax = 0.5f;
    float smashTimer;
    float shootTimerMax = 3f;
    float shootTimer;

    protected override void Awake()
    {
        base.Awake();
        baddieType = EnemyConstants.EnemyTypes.Boss;
        damage = 20;
        health = 5000;
        maxHealth = 5000;
        chaseTimer = 0;
        shuffleTimer = 0;
        state = BossStates.Chase;

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
                    state = BossStates.Smash;
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
                        GetComponent<SpriteRenderer>().color = Color.yellow;
                        break;
                    case WeaponController.EnergyTypes.Fire:
                        GetComponent<SpriteRenderer>().color = Color.red;
                        break;
                    case WeaponController.EnergyTypes.Freeze:
                        GetComponent<SpriteRenderer>().color = Color.blue;
                        break;
                    case WeaponController.EnergyTypes.Kinetic:
                        GetComponent<SpriteRenderer>().color = Color.green;
                        break;
                    case WeaponController.EnergyTypes.Explosion:
                        GetComponent<SpriteRenderer>().color = Color.grey;
                        break;
                }
                state = BossStates.Shuffle;
                break;
            default:
                break;
        }
        return transform.position;
    }

    private int getRandomAttack()
    {
        System.Random rand = new System.Random();
        return rand.Next(1, 4);
    }

    private int getRandomType()
    {
        System.Random rand = new System.Random();
        return rand.Next(0, 5);
    }
}
