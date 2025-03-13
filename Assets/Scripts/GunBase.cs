using UnityEngine;
using TMPro;  // Add the TMPro namespace
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

    [Header("UI References")]
    public TextMeshProUGUI ammoText; // Changed to TextMeshProUGUI for TMP support

    private int currentAmmoInMag;
    private int currentReserveAmmo;
    private bool isShooting = false;
    private bool isReloading = false;

    void Start()
    {
        currentAmmoInMag = magazineSize;
        currentReserveAmmo = maxReserveAmmo;

        UpdateAmmoUI(); // Update ammo UI on start
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
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private IEnumerator Shoot()
    {
        isShooting = true;

        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, transform.parent.rotation);
       // bullet.transform.up = 
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDamage(dmg); // Example damage value
        }

        currentAmmoInMag--;
        UpdateAmmoUI();  // Update the ammo display after shooting

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

        UpdateAmmoUI();  // Update the ammo display after reload

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
        UpdateAmmoUI();  // Update the ammo display after adding ammo
    }

    // Function to update the ammo display
    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"Ammo: {currentAmmoInMag}/{magazineSize} | Reserve: {currentReserveAmmo}/{maxReserveAmmo}";
        }
    }
}
