using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Projectile
{
    public int pelletSize = 4;
    public float pelletSpread = 35f;

    public override void Start()
    {
        base.Start();


        var mousePos = Input.mousePosition;

        //pelletSpread = Mathf.Tan(pelletSpread) * Vector3.Distance(mousePos, GameObject.FindGameObjectWithTag("Player").transform.position);

        mousePos.x += Random.Range(-pelletSpread, pelletSpread);
        mousePos.y += Random.Range(-pelletSpread, pelletSpread);
        mousePos.z = 10;
        moveDirection = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position);
        moveDirection.z = 0;

        damage = 6;
        speed = 25;
    }
}
