using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    private Rigidbody2D rb;
    private float damage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = -transform.up * speed; // Move the bullet forward based on its rotation
        }
        else
        {
            Debug.LogError("Rigidbody2D is missing on bullet!");
        }

        Destroy(gameObject, lifeTime); // Destroy the bullet after lifetime expires
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    // This function handles bullet collision with any object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an enemy
        if (collision.collider.CompareTag("Enemy"))
        {
            Health enemyHealth = collision.collider.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // Apply damage to the enemy
            }
        }

        // Destroy the bullet when it hits anything (including walls, non-trigger colliders)
        Destroy(gameObject);
    }
}
