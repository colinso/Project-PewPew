using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieController: EnemyController
{
    protected override void Awake()
    {
        base.Awake();
        player = base.player;
        type = EnemyTypes.Baddie;
        damage = 20;
        health = 40;
    }
}
