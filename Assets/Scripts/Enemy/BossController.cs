﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    public float cooldown = 2f;
    private EnemyActions actions;
    enum BossStates { Shuffle, Chase, Explosions, Spawn, ChangeType, Shoot }
    BossStates state;

    protected override void Awake()
    {
        base.Awake();
        baddieType = EnemyConstants.EnemyTypes.Boss;
        damage = 20;
        health = 5000;

        actions = new EnemyActions(player, gameObject);
        actions.SetDetectionDistance(50);
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case BossStates.Shuffle:
                break;
            case BossStates.Chase:
                actions.DetectAndChase(transform.position, player.transform.position, 50);
                break;
            default:
                break;
        }
    }

    //protected override Vector2 bossMoves(){

    //}
}
