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
    public AudioClip shotClip;

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
    public float GrenadeFireRate = 0.5f;
    public float MinigunFireRate = 10f;
    public float ShotgunFireRate = 3.5f;
    public float SniperFireRate = 1.5f;


    // Start is called before the first frame update
    void Start()
	{
        timer = 0;
        SetPrimaryEneryTypeUI(energyType);
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
            ChangeWeaponType();

            // Shoot
            Shoot();

            // Throw Nades
            Throw();
        }
        ChangeWeaponText();
    }

    public void ShootEnemyWeapon(bool isBoss)
    {
        GameObject projectile = Instantiate(pistolPrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Projectile>().setSpeed(10);
        projectile.GetComponent<Projectile>().setDamage(5);
        if (isBoss)
        {
            projectile.GetComponent<Projectile>().setSpeed(29);
            projectile.GetComponent<Projectile>().setDamage(2);
        }
    }

    public virtual void Shoot()
	{
        if(Input.GetMouseButtonDown(0))
        {
            AudioSource.PlayClipAtPoint(shotClip, transform.position);
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
                        AudioSource.PlayClipAtPoint(shotClip, transform.position);
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
            if (Time.time - lastfired > 1 / GrenadeFireRate)
            {
                lastfired = Time.time;
                Instantiate(granadePrefab, firePoint.position, firePoint.rotation);
            }
        }
    }

  void ChangeEnergyType()
	{

        if (Input.GetKeyDown("q"))
        {
            energyType = EnergyNext(energyType);
        }
        SetPrimaryEneryTypeUI(energyType);
	}

  void SetPrimaryEneryTypeUI(EnergyTypes primary)
    {
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        if (cam.transform.Find("Electric") != null)
        {
            SpriteRenderer electricProj = cam.transform.Find("Electric").gameObject.GetComponent<SpriteRenderer>();
            SpriteRenderer fireProj = cam.transform.Find("Fire").gameObject.GetComponent<SpriteRenderer>();
            SpriteRenderer freezeProj = cam.transform.Find("Freeze").gameObject.GetComponent<SpriteRenderer>();
            SpriteRenderer KineticProj = cam.transform.Find("Kinetic").gameObject.GetComponent<SpriteRenderer>();
            electricProj.color = new Color(electricProj.color.r, electricProj.color.g, electricProj.color.b, .3f);
            fireProj.color = new Color(fireProj.color.r, fireProj.color.g, fireProj.color.b, .3f);
            freezeProj.color = new Color(freezeProj.color.r, freezeProj.color.g, freezeProj.color.b, .3f);
            KineticProj.color = new Color(KineticProj.color.r, KineticProj.color.g, KineticProj.color.b, .3f);

            if (primary == EnergyTypes.Electric)
                electricProj.color = new Color(electricProj.color.r, electricProj.color.g, electricProj.color.b, 1f);
            else if (primary == EnergyTypes.Fire)
                fireProj.color = new Color(fireProj.color.r, fireProj.color.g, fireProj.color.b, 1f);
            else if (primary == EnergyTypes.Freeze)
                freezeProj.color = new Color(freezeProj.color.r, freezeProj.color.g, freezeProj.color.b, 1f);
            else if (primary == EnergyTypes.Kinetic)
                KineticProj.color = new Color(KineticProj.color.r, KineticProj.color.g, KineticProj.color.b, 1f);
        }
	}

	EnergyTypes EnergyNext (EnergyTypes myEnum)
    {
        switch (myEnum)
        {
            case EnergyTypes.Electric:
                return EnergyTypes.Fire;
            case EnergyTypes.Fire:
                return EnergyTypes.Freeze;
            case EnergyTypes.Freeze:
                return EnergyTypes.Kinetic;
            case EnergyTypes.Kinetic:
                return EnergyTypes.Electric;
            default:
                return EnergyTypes.Electric;
        }
    }

	void ChangeWeaponType()
	{
        if (Input.GetKeyDown("e"))
        {
            weaponType = WeaponNext(weaponType);
        }
	}

    WeaponTypes WeaponNext(WeaponTypes myEnum)
	{
		switch (myEnum)
		{
			case WeaponTypes.Pistol:
				return WeaponTypes.Shotgun;
			case WeaponTypes.Shotgun:
				return WeaponTypes.Minigun;
			case WeaponTypes.Minigun:
				return WeaponTypes.Sniper;
			case WeaponTypes.Sniper:
				return WeaponTypes.Pistol;
			default:
				return WeaponTypes.Pistol;
		}
	}

    void ChangeWeaponText()
    {
        GameObject WeaponUI = GameObject.Find("WeaponText");
        if (WeaponUI != null)
        {
            if (weaponType == WeaponTypes.Minigun)
            {
                WeaponUI.GetComponent<TMPro.TextMeshProUGUI>().text = "Minigun";
            }
            else if (weaponType == WeaponTypes.Pistol)
            {
                WeaponUI.GetComponent<TMPro.TextMeshProUGUI>().text = "Pistol";
            }
            else if (weaponType == WeaponTypes.Shotgun)
            {
                WeaponUI.GetComponent<TMPro.TextMeshProUGUI>().text = "Shotgun";
            }
            else if (weaponType == WeaponTypes.Sniper)
            {
                WeaponUI.GetComponent<TMPro.TextMeshProUGUI>().text = "Sniper";
            }
        }
    }
}
