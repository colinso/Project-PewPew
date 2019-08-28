using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public Transform parent;
    public float length = .5f;
    public float height = .7f;
    float lineSize;
    float newLineSize;
    LineRenderer sprite;
    Vector3 barOffsetLeft;
    Vector3 barOffsetRight;
    float originalDistance;

    void Start()
    {
        barOffsetLeft = new Vector3(-length, height, 0);
        barOffsetRight = new Vector3(length, height, 0);
        originalDistance = Vector3.Distance(barOffsetLeft, barOffsetRight);
        sprite = GetComponent<LineRenderer>();
        parent = transform.parent;
        newLineSize = lineSize;
        sprite.SetPosition(0, transform.parent.position + barOffsetLeft);
        sprite.SetPosition(1, transform.parent.position + barOffsetRight);
        lineSize = Vector3.Distance(sprite.GetPosition(0), sprite.GetPosition(1));
    }

    void Update()
    {
        sprite.SetPosition(0, transform.parent.position + barOffsetLeft);
        if(parent.tag == "Player")
        {
            PlayerController player = parent.GetComponent<PlayerController>();
            float healthPercentage = (float)player.health / (float)player.maxHealth;
            resizeHPBar(healthPercentage);

        }
        else if(parent.GetComponent<EnemyController>())
        {
            EnemyController enemy = parent.GetComponent<EnemyController>();
            float healthPercentage = (float)enemy.health / (float)enemy.maxHealth;
            float distanceAmount = originalDistance * healthPercentage;

            Vector3 newPos = sprite.GetPosition(0) + new Vector3(distanceAmount, 0, 0);

            sprite.SetPosition(1, newPos);
            resizeHPBar(healthPercentage);
        }
    }

    void resizeHPBar(float healthPercentage)
    {
        float distanceAmount = originalDistance * healthPercentage;

        Vector3 newPos = sprite.GetPosition(0) + new Vector3(distanceAmount, 0, 0);

        sprite.SetPosition(1, newPos);
    }
}
