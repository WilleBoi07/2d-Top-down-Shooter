using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    private Rigidbody2D rb;
    private float damage;
    public float alertRadius = 7f; // Larger than the enemy's running detection

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.up * speed; // Move the bullet forward
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Health enemyHealth = collision.collider.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // Apply damage to enemy
            }
        }

        AlertNearbyEnemies(); // Notify enemies of the shot
        Destroy(gameObject);
    }

    private void AlertNearbyEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, alertRadius);
        foreach (Collider2D collider in hitEnemies)
        {
            EnemyAI enemy = collider.GetComponent<EnemyAI>();
            if (enemy != null && enemy.currentState == EnemyAI.EnemyState.Dormant)
            {
                enemy.StartWakingUp(); // Notify dormant enemies
            }
        }
    }
}
