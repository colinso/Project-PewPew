using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Projectile
{
    public int pelletSize = 4;

    public override void Start()
    {
        base.Start();

        var mousePos = Input.mousePosition;
        mousePos.x += Random.Range(-15, 15);
        mousePos.y += Random.Range(-15, 15);

        moveDirection = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position);
        moveDirection.z = 0;

        damage = 6;
        speed = 25;
    }

    //public void Reset()
    //{
    //    damage = 6;
    //    speed = 5;
    //}
}
