using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 moveDirection;
    public Rigidbody2D rb;
    public int damage = 20;
    public float speed = 20f;
    public bool isPlayer;
    public WeaponController.EnergyTypes energyType;
    protected CircleCollider2D circleCollider;
    private bool hit;
    private HashSet<EnemyController> hitList;
    private float killDelay = 0.05f;

    // Start is called before the first frame update
    public virtual void Start()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 10;
        moveDirection = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position);
        moveDirection.z = 0;

        circleCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
        circleCollider.radius = 5f;
        circleCollider.isTrigger = true;
        circleCollider.enabled = false;
        isPlayer = true;

        hitList = new HashSet<EnemyController>();
    }

    private void Update()
    {
        if (hit && !isPlayer)
        {
            Destroy(gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().takeDamage(5);
        }
        else if (!hit || !isPlayer)
        {
            transform.position = transform.position + moveDirection * speed * Time.deltaTime;
        } else if( hit && isPlayer)
        {
            killDelay -= Time.deltaTime;
            if (killDelay <= 0)
            {
                
                foreach (var item in hitList)
                {
                    print(item);
                    item.TakeDamage(damange, energyType);
                }

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer)
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();

            if (enemy != null)
            {
                hitList.Add(enemy);
                circleCollider.enabled = true;
                hit = true;
                transform.position = enemy.transform.position;
            }
        }
        else
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if(player != null && energyType == WeaponController.EnergyTypes.Electric)
            {
                circleCollider.enabled = true;
                hit = true;
                transform.position = player.transform.position;
            }
        }
    }

    public void changeType(WeaponController.EnergyTypes newType) {
        energyType = newType;
    }
}
