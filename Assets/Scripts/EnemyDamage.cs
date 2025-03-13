using UnityEngine;
public class EnemyDamage : MonoBehaviour
{
    public float damageAmount = 10f; // The amount of damage dealt to the player
    public float damageCooldown = 1f; // Time between each damage instance (in seconds)

    private float damageTimer = 0f; // Timer to control damage cooldown
    private bool playerInRange = false; // If the player is inside the collider

    private void Update()
    {
        if (playerInRange && damageTimer <= 0f)
        {
            // Deal damage if player is in range and cooldown has passed
            DealDamage();
            damageTimer = damageCooldown; // Reset the cooldown timer
        }

        // Countdown the damage timer
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the trigger collider
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // Player is in range to take damage
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If the player exits the trigger collider, stop dealing damage
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void DealDamage()
    {
        // Find the Player and apply damage
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>(); // Assumes the PlayerHealth script is attached to the player
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount); // Apply the damage to the player
        }
    }
}
