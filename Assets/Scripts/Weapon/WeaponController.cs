using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	public Transform firePoint;
	public GameObject player;
	public GameObject projectilePrefab;
	public enum energyTypes { Electric, Fire, Freeze, Kinetic };
	public energyTypes selectedType;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
        // set position of the gun
		transform.position = player.transform.position;

        // Change energy type
        ChangeEnergyType();

        // Shoot
        Shoot();
	}

	void Shoot()
	{
        if(Input.GetMouseButtonDown(0))
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.GetComponent<Projectile>().changeType(selectedType);

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
