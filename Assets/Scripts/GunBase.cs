using UnityEngine;
using System.Collections;

public class GunBase : MonoBehaviour
{
    [Header("Gun Settings")]
    public int magazineSize = 30;
    public int maxReserveAmmo = 120;
    public float fireRate = 0.1f;
    public float reloadTime = 1.5f;
    [SerializeField] int dmg = 10;

    [Header("References")]
    public Transform gunPoint;
    public GameObject bulletPrefab;

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
        RotateGunTowardsMouse();

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

    private void RotateGunTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }

    private IEnumerator Shoot()
    {
        isShooting = true;

        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDamage(dmg); // Example damage value
        }

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
