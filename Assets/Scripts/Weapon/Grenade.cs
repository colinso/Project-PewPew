using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public Transform firePoint;
    public GameObject player;
    public GameObject explosion;
    public float speed = 10f;
    public float fuse = 1.5f;
    public int damage = 100;

    protected CircleCollider2D circleCollider;

    private HashSet<EnemyController> hitList;
    private Vector3 target;
    private bool hit;

	// Start is called before the first frame update
	void Start()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        target = Camera.main.ScreenToWorldPoint(mousePos);
        target.z = transform.position.z;

        circleCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
		circleCollider.enabled = false;
		circleCollider.radius = 2.5f;
        circleCollider.isTrigger = true;

        hitList = new HashSet<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position == target)
        {
            fuse -= Time.deltaTime;
            print(fuse);
            if (fuse <= 0)
            {
                circleCollider.enabled = true;

                if (hit)
                {
                    foreach (var item in hitList)
                    {
                        item.TakeDamage(damage, WeaponController.EnergyTypes.Explosion);
                    }
                }
                if (fuse <= -0.05)
                {
                    Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(gameObject);
                }

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();

        if (enemy != null)
        {
            hitList.Add(enemy);
            hit = true;
        }
    }


}
