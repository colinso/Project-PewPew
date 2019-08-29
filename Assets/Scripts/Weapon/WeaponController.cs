using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WeaponController : MonoBehaviour
{
    public Transform firePoint;
    public GameObject player;
    public GameObject host;
    public float timerMax = 0.1f;
    public float timer;
    public GameObject granadePrefab;

    // Weapon Types
    public GameObject minigunPrefab;
    public GameObject pistolPrefab;
    public GameObject sniperPrefab;
    public GameObject shotgunPrefab;

    public EnergyTypes energyType;
    public WeaponTypes weaponType;

    public int pelletSize = 4;

    private float lastfired;      // The value of Time.time at the last firing moment

    // The number of bullets fired per second
    public float MinigunFireRate = 10;  
    public float ShotgunFireRate = 3.5f; 
    public float SniperFireRate = 1.5f;


    // Start is called before the first frame update
    void Start()
	{
        timer = 0;
	}

	// Update is called once per frame
	void Update()
	{
        timer += Time.deltaTime;
        // set position of the gun
		transform.position = host.transform.position;

        if(host.tag == "Player")
        {
            // Change energy type
            ChangeEnergyType();

            // Shoot
            Shoot();

            // Throw Nades
            Throw();
        }
        else if(timer >= timerMax)
        {
            timer = 0;
            Instantiate(pistolPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public virtual void Shoot()
	{
        if(Input.GetMouseButtonDown(0))
        {
            GameObject projectile_1;
            switch (weaponType)
            {
                case WeaponTypes.Pistol:
                    projectile_1 = Instantiate(pistolPrefab, firePoint.position, firePoint.rotation);
                    projectile_1.GetComponent<Projectile>().changeType(energyType);
                    break;
                case WeaponTypes.Shotgun:
                    if (Time.time - lastfired > 1 / ShotgunFireRate)
                    {
                        lastfired = Time.time;

                        for (int i = 0; i < pelletSize; i++)
                        {
                            GameObject tmp = Instantiate(shotgunPrefab, firePoint.position, firePoint.rotation);
                            tmp.GetComponent<Projectile>().changeType(energyType);
                        }
                    }
                    break;
                case WeaponTypes.Sniper:
                    if (Time.time - lastfired > 1 / SniperFireRate)
                    {
                        lastfired = Time.time;

                        projectile_1 = Instantiate(sniperPrefab, firePoint.position, firePoint.rotation);
                        projectile_1.GetComponent<Projectile>().changeType(energyType);
                    }
                    break;
            }

        }
        if (Input.GetButton("Fire1"))
        {
            switch (weaponType)
            {
                case WeaponTypes.Minigun:
                    if (Time.time - lastfired > 1 / MinigunFireRate)
                    {
                        lastfired = Time.time;
                        GameObject tmp = Instantiate(minigunPrefab, firePoint.position, firePoint.rotation);
                        tmp.GetComponent<Projectile>().changeType(energyType);
                    }
                    break;
            }
        }
    }

    void Throw()
    {
        if (Input.GetKeyDown("g"))
        {
            GameObject granade = Instantiate(granadePrefab, firePoint.position, firePoint.rotation);
        }
    }

    void ChangeEnergyType()
	{
		if (Input.GetKeyDown("1"))
		{
            energyType = EnergyTypes.Electric;
		}
		if (Input.GetKeyDown("2"))
		{
			energyType = EnergyTypes.Fire;
		}
		if (Input.GetKeyDown("3"))
		{
            energyType = EnergyTypes.Freeze;
		}
		if (Input.GetKeyDown("4"))
		{
            energyType = EnergyTypes.Kinetic;
		}
	}
}
