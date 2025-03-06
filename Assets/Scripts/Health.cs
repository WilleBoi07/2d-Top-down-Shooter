using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;  // Maximum health of the entity
    private float currentHealth;     // Current health of the entity
    public bool IsDead => currentHealth <= 0; // Checks if the entity is dead

    // Event that will be triggered when health reaches 0 (death)
    public delegate void OnDeath();
    public event OnDeath DeathEvent;

    void Start()
    {
        currentHealth = maxHealth;  // Set the current health to the max health at the start
    }

    // Method to take damage
    public void TakeDamage(float damage)
    {
        if (IsDead) return;  // If already dead, no damage is taken

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // Ensure health doesn't go below 0

        if (IsDead)
        {
            Die();
        }
    }

    // Method to heal the entity (optional)
    public void Heal(float healAmount)
    {
        if (IsDead) return;  // Can't heal if dead

        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // Ensure health doesn't exceed max health
    }

    // Called when health reaches 0 (death)
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");

        // Trigger death event (useful for custom death logic)
        DeathEvent?.Invoke();

        // Placeholder for any additional death logic you want to add here
        // For example: disabling movement, playing death sound, etc.
    }
}
