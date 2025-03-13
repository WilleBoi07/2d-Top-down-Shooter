using UnityEngine;
using System.Collections;
using static EnemyAI;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;  // Maximum health of the entity
    private float currentHealth;     // Current health of the entity
    public bool IsDead => currentHealth <= 0; // Checks if the entity is dead

    // References
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public Animator Anim;

    // Event that will be triggered when health reaches 0 (death)
    public delegate void OnDeath();
    public event OnDeath DeathEvent;

    void Start()
    {
        currentHealth = maxHealth;  // Set the current health to the max health at the start

        // Get the SpriteRenderer and Rigidbody2D (for death animation)
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.isKinematic = true;  // Disable physics initially to prevent falling until death
        }
    }

    // Method to take damage
    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} took {damage} damage! Current Health: {currentHealth}");

        if (IsDead)
        {
            Die();
        }
    }

    // Called when health reaches 0 (death)
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");

        // Trigger death event (useful for custom death logic)
        DeathEvent?.Invoke();

        // Get the EnemyAI component attached to this GameObject
        EnemyAI enemyAI = GetComponent<EnemyAI>();

        // Ensure that the EnemyAI component is present
        if (enemyAI != null)
        {
            // Set the enemy's state to Death
            enemyAI.currentState = EnemyAI.EnemyState.Death;
        }
        else
        {
            Debug.LogError("EnemyAI component is missing!");
        }

        // Change color to a death color (red or any color you like)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red; // Change color to red, you can change this to any color
        }

        // If the enemy has a Rigidbody2D, apply physics to make it fall over
        if (rb != null)
        {
            rb.isKinematic = false;  // Enable physics to allow falling
            rb.gravityScale = 0.3f;     // Allow gravity to pull the enemy down
            rb.AddTorque(100f);       // Add torque to make the enemy fall over
        }

        // Stop all movement and actions since the enemy is dead
        Anim.SetBool("run", false); // Stop running animation if applicable
        Anim.SetBool("death", true);  // Play death animation

        // Optionally, you could add a death animation or sound here before destroying the object
        // Example: play a death animation or sound effect

        // Destroy the enemy after some time (to allow the death effects to be visible)
        Destroy(gameObject, 1f);  // Delay the destruction for 1 second (adjust as needed)
    }



}
