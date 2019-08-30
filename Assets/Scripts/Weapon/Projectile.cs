using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Vector3 moveDirection;
    public Rigidbody2D rb;
    public bool isPlayer;
    protected WeaponController.EnergyTypes energyType;
    protected WeaponController.WeaponTypes weaponType;
    protected int damage = 20;
    protected float speed = 20f;
    protected CircleCollider2D circleCollider;
    private bool hit;
    private float timer;
    private HashSet<EnemyController> hitList;
    private float killDelay = 0f;

    // Start is called before the first frame update
    public virtual void Start()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 10;

        moveDirection = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position);
        moveDirection.z = 0;
        timer = 0;

        circleCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
        circleCollider.radius = 1.5f;
        circleCollider.isTrigger = true;
        circleCollider.enabled = false;
        isPlayer = true;

        hitList = new HashSet<EnemyController>();
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            timer = 0;
            Destroy(gameObject);
        }
        if (hit && !isPlayer)
        {
            Destroy(gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().takeDamage(15);
            print("Player health: " + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().health);
        }
        else if (!hit || !isPlayer)
        {
            transform.position = transform.position + moveDirection.normalized * speed * Time.deltaTime;
        } else
        {
            if (energyType != WeaponController.EnergyTypes.Electric)
                Destroy(gameObject);
            killDelay -= Time.deltaTime;
            if (killDelay <= 0)
            {

                foreach (var item in hitList)
                {
                    Debug.Log(energyType);
                    if(item != null)
					{
                        item.TakeDamage(damage, energyType);
					}
                }
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();
        PlayerController player = collision.GetComponent<PlayerController>();
        Projectile projectile = collision.GetComponent<Projectile>();

        if (isPlayer)
        {
            print("isplayer");
            if (enemy != null)
            {
                if (energyType == WeaponController.EnergyTypes.Electric)
                {
                    print("Yeeeeet!");
                    circleCollider.enabled = true;

                }
                hitList.Add(enemy);
                hit = true;
                transform.position = enemy.transform.position;
            } else if (!player && !enemy && !hit && !projectile)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            print("not");
            if (player != null)
            {

                circleCollider.enabled = true;
                hit = true;
                transform.position = player.transform.position;
            } else if (!player && !enemy && !hit && !projectile)
            {
                Destroy(gameObject);
            }
        }
    }

    public void changeType(WeaponController.EnergyTypes newEng) {
        energyType = newEng;

        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        switch (energyType)
        {
            case WeaponController.EnergyTypes.Electric:
                trailRenderer.material.color = new Color(.85f, .185f, .194f);
                spriteRenderer.color = new Color(.85f, .185f, .194f);
                break;
            case WeaponController.EnergyTypes.Fire:
                trailRenderer.material.color = new Color(1f, 0.84f, 0.07f);
                spriteRenderer.color = new Color(1f, 0.84f, 0.07f);
                break;
            case WeaponController.EnergyTypes.Freeze:
                trailRenderer.material.color = new Color(0f, 0.8f, 1f);
                spriteRenderer.color = new Color(0f, 0.8f, 1f);
                break;
            case WeaponController.EnergyTypes.Kinetic:
                trailRenderer.material.color = new Color(0.39f, 1f, 0.51f);
                spriteRenderer.color = new Color(0.39f, 1f, 0.51f);
                break;
            default:
                break;
        }

    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }
}
