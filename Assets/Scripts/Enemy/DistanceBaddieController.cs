using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBaddieController: EnemyController
{
    public float cooldown = 2f;

    private EnemyActions actions;

    protected override void Awake()
    {
        base.Awake();
        baddieType = EnemyConstants.EnemyTypes.DistanceBaddie;
        health = 80;
    }


}
