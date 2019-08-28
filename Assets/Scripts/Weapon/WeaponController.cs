using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	public Transform firePoint;
	public GameObject player;
    public GameObject host;
	public GameObject projectilePrefab;
	public GameObject granadePrefab;
    public enum energyTypes { Electric, Fire, Freeze, Kinetic, Explosion };
	public energyTypes selectedType;
    public float timerMax = 0.1f;
    public float timer;

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
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }

	void Shoot()
	{
        if(Input.GetMouseButtonDown(0))
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.GetComponent<Projectile>().changeType(selectedType);

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
			selectedType = energyTypes.Electric;
		}
		if (Input.GetKeyDown("2"))
		{
			selectedType = energyTypes.Fire;
		}
		if (Input.GetKeyDown("3"))
		{
			selectedType = energyTypes.Freeze;
		}
		if (Input.GetKeyDown("4"))
		{
			selectedType = energyTypes.Kinetic;
		}
	}
}
