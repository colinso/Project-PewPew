using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int stamina;
    public int experience;
    public int level;
    public GameObject currentWeapon;
    public List<GameObject> weapons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int damageTaken)
    {
        if(health - damageTaken <= 0)
        {
            health = 0;
        }
        else
        {
            health -= damageTaken;
        }
    }

    public void inflictDamage(int damageInflicted)
    {
        
    }

    public bool isDead()
    {
        return health == 0;
    }
    public void disableColliders()
    {
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        foreach(Collider2D c in colliders)
        {
            c.enabled = false;
        }
    }
    public void enableColliders()
    {
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        foreach (Collider2D c in colliders)
        {
            c.enabled = true;
        }
    }
}
