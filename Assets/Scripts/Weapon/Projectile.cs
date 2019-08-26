using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 moveDirection;
    public Rigidbody2D rb;
    public int damange = 25;
    public float speed = 20f;
    public enum type { Electric, Fire, Freeze, Kinetic};

    // Start is called before the first frame update
    void Start()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 10;
        moveDirection = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position);
        moveDirection.z = 0;
        moveDirection.Normalize();
        //rb.velocity = transform.right * speed;
    }

    private void Update()
    {
        transform.position = transform.position + moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();

        if(enemy != null)
        {
            enemy.takeDamage(damange);
            Destroy(gameObject);
        }
    }
}
