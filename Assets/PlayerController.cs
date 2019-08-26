using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
        health -= damageTaken;
    }
    public void inflictDamage(int damageInflicted)
    {
        
    }
}
