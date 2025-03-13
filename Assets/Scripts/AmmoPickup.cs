using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 30; // Amount of ammo to give
    public KeyCode interactKey = KeyCode.E; // Interaction key

    private bool playerInRange = false;
    private GunBase gunScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // Find GunBase in player's child objects
            gunScript = other.GetComponentInChildren<GunBase>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            gunScript = null;
        }
    }

    private void Update()
    {
        if (playerInRange && gunScript != null && Input.GetKeyDown(interactKey))
        {
            gunScript.AddAmmo(ammoAmount); // Add ammo to the gun
            Destroy(gameObject); // Remove the pickup after use
        }
    }
}
