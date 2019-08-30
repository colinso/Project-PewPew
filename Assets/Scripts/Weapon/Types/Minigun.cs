using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Projectile
{

    public override void Start()
    {
        base.Start();

        var mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos.x += Random.Range(-25, 25);
        mousePos.y += Random.Range(-25, 25);

        moveDirection = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position);
        moveDirection.z = 0;

        damage = 10;
        speed = 25;
    }

    //public void Reset()
    //{
    //    damage = 6;
    //    speed = 5;
    //}
}
