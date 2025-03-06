using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Dormant, Chasing }
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
        if (isWaveEnemy)
        {
            currentState = EnemyState.Chasing; // Wave enemies start chasing immediately
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

    void CheckForPlayer()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance < detectionRadius && Input.GetKey(KeyCode.LeftShift))  // Only wakes up if player is running
            {
                currentState = EnemyState.Chasing;
            }
        }
    }

    void ChasePlayer()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        }
    }
}
