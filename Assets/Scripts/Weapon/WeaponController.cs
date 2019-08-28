using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WeaponController : MonoBehaviour
{
	public Transform firePoint;
	public GameObject player;
    public GameObject host;
	public GameObject projectilePrefab;
	public GameObject granadePrefab;
    public float timerMax = 0.1f;
    public float timer;
	public EnergyTypes selectedType;

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
			selectedType = EnergyTypes.Electric;
		}
		if (Input.GetKeyDown("2"))
		{
			selectedType = EnergyTypes.Fire;
		}
		if (Input.GetKeyDown("3"))
		{
			selectedType = EnergyTypes.Freeze;
		}
		if (Input.GetKeyDown("4"))
		{
			selectedType = EnergyTypes.Kinetic;
		}
	}
}
