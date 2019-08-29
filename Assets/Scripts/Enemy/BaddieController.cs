using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieController: EnemyController
{
    public float cooldown = 2f;
    private EnemyActions actions;

    protected override void Awake()
    {
        base.Awake();
        baddieType = EnemyConstants.EnemyTypes.Baddie;
        damage = 20;
    }

}
