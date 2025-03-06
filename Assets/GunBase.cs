using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunBase : MonoBehaviour
{
    [Header("Gun Settings")]
    public int magazineSize = 30;
    public int maxReserveAmmo = 120;
    public float fireRate = 0.1f; // Time between shots (higher = slower fire rate)
    public float reloadTime = 1.5f;

    [Header("References")]
    public Transform gunPoint; // The point where bullets spawn
    public GameObject bulletPrefab; // Assign bullet prefab in Inspector

    private int currentAmmoInMag;
    private int currentReserveAmmo;
    private bool isShooting = false;
    private bool isReloading = false;

    void Start()
    {
        currentAmmoInMag = magazineSize;
        currentReserveAmmo = maxReserveAmmo;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !isShooting && !isReloading)
        {
            if (currentAmmoInMag > 0)
            {
                StartCoroutine(Shoot());
            }
            else
            {
                Reload();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            Reload();
        }
    }

    private IEnumerator Shoot()
    {
        isShooting = true;

        // Fire bullet
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        // You can add bullet force logic here

        currentAmmoInMag--;
        yield return new WaitForSeconds(fireRate);

        isShooting = false;
    }

    private void Reload()
    {
        if (currentAmmoInMag == magazineSize || currentReserveAmmo <= 0) return;

        isReloading = true;
        Debug.Log("Reloading...");

        int bulletsNeeded = magazineSize - currentAmmoInMag;
        int bulletsToReload = Mathf.Min(bulletsNeeded, currentReserveAmmo);

        currentAmmoInMag += bulletsToReload;
        currentReserveAmmo -= bulletsToReload;

        Invoke(nameof(FinishReload), reloadTime);
    }

    private void FinishReload()
    {
        isReloading = false;
        Debug.Log("Reload Complete");
    }

    public void AddAmmo(int amount)
    {
        currentReserveAmmo = Mathf.Min(currentReserveAmmo + amount, maxReserveAmmo);
    }
}
