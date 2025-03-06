using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Make the EnemyState enum public so it can be accessed from other scripts
    public enum EnemyState { Dormant, Chasing }

    // Make currentState public so it can be accessed from other scripts
    public EnemyState currentState = EnemyState.Dormant;

    public Transform player;
    public float chaseSpeed = 3f;
    public float detectionRadius = 3f;  // Distance to wake up if player runs
    public bool isWaveEnemy = false; // If true, enemy starts in Chasing mode

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Choose a color
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Draws the detection radius
    }

    private void Start()
    {
        // If it's a wave enemy, start chasing immediately
        if (isWaveEnemy)
        {
            currentState = EnemyState.Chasing;
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Dormant:
                CheckForPlayer();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
        }
    }

    // Check if the player is running close to the enemy
    void CheckForPlayer()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            // Wake up and chase if the player is within the detection radius and running (holding LeftShift)
            if (distance < detectionRadius && Input.GetKey(KeyCode.LeftShift))
            {
                currentState = EnemyState.Chasing;
            }
        }
    }

    // Chase the player
    void ChasePlayer()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        }
    }
}
