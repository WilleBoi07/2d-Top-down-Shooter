using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healthAmount = 25f; // Amount of health restored
    public KeyCode interactKey = KeyCode.E; // Interaction key

    private bool playerInRange = false; // Track if the player is inside the trigger
    private PlayerHealth playerHealth; // Store the player's health script

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerHealth = other.GetComponent<PlayerHealth>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player left the trigger
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerHealth = null;
        }
    }

    private void Update()
    {
        // If player is in range and presses "E"
        if (playerInRange && playerHealth != null && Input.GetKeyDown(interactKey))
        {
            playerHealth.Heal(healthAmount); // Call the healing function
            Destroy(gameObject); // Destroy the pickup after use
        }
    }
}
