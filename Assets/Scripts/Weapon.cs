using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    //shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    //Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //spread
    public float spreadIntensity;

    //Reloading
    public float reloadingTime;
    public int bulletsLeft, magazineSize;
    public bool isReloading;

    //Bullet Properties
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform spawnBullet;
    float bulletLifeTime = 3f;
    public float bulletSpeed = 100f;

    public GameObject muzzleEffect;

    internal Animator animator;
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }
    public ShootingMode currentShootingMode;

    public enum WeaponModel
    {
        Pistol,
        M16
    }
    public WeaponModel thisWeaponModel;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;
            if (currentShootingMode == ShootingMode.Auto) // Holding down X key continously
            {
                isShooting = Input.GetKey(KeyCode.X);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst) // CLicking X key once
            {
                isShooting = Input.GetKeyDown(KeyCode.X);
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }
            if (Input.GetKeyDown(KeyCode.R) && !isReloading && bulletsLeft < magazineSize && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }
            //To automatically reload
            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0 && magazineSize != 0)
            {
                Reload();
            }
            if (isShooting && bulletsLeft == 0)
            {
                SoundManager.Instance.PlayEmptySound(thisWeaponModel);
            } 
        }
    }
    private void FireWeapon()
    {
        bulletsLeft--;

        // muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");

        readyToShoot = false; // so it doesnt shoot twice at once
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, spawnBullet.position, spawnBullet.rotation);
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }
        //Burst
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) // we already shoot once before this point thats why its >1
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    void Reload()
    {
        isReloading = true;
        Invoke("ReloadCompleted", reloadingTime);
        animator.SetTrigger("RELOAD");
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);   
    }
    void ReloadCompleted()
    {
        if(WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }        
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        isReloading = false;
    }
    void ResetShot()
    {
        allowReset = true;
        readyToShoot = true;
    }
    public Vector3 CalculateDirectionAndSpread()
    {
        //Shooting from the middle of the screen to check where we are pointing to
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - spawnBullet.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //Returning the shooting direction and spread
        return direction + new Vector3(x, y, 0);
    }
    IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
