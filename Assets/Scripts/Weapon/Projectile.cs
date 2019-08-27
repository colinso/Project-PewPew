using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 moveDirection;
    public Rigidbody2D rb;
    public int damange = 20;
    public float speed = 20f;
    public WeaponController.energyTypes energyType;

    // Start is called before the first frame update
    void Start()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 10;
        moveDirection = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position);
        moveDirection.z = 0;
    }

    private void Update()
    {
        transform.position = transform.position + moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.TakeDamage(damange, energyType);
            Destroy(gameObject);
        }
    }

    public void changeType(WeaponController.energyTypes newType) {
        energyType = newType;
    }
}
