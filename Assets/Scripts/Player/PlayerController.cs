using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int stamina;
    public int experience;
    public int level;
    public GameObject currentWeapon;
    public List<GameObject> weapons;
    public bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void takeDamage(int damageTaken)
    {
        if(health - damageTaken <= 0)
        {
            health = 0;
            Die();
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
    void Die()
    {
        if (SceneManager.GetSceneByName("MainScene").isLoaded && !reloading)
        {
            reloading = true;
            LoadSceneManager.Instance.Reload();
        }
    }
}
