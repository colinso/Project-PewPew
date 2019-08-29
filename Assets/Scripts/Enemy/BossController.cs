using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    public float cooldown = 2f;
    private EnemyActions actions;
    enum BossStates { Shuffle, Chase, Explosions, Spawn, ChangeType, Shoot }

    protected override void Awake()
    {
        base.Awake();
        baddieType = EnemyConstants.EnemyTypes.Baddie;
        damage = 20;
        health = 5000;

        actions = new EnemyActions(player, gameObject);
    }


}
