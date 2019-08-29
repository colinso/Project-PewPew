using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Projectile
{

    public override void Start()
    {
        base.Start();

        damage = 50;
        speed = 40;
    }
}
