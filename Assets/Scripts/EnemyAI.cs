using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Dormant, WakingUp, Chasing, Death }

    public EnemyState currentState = EnemyState.Dormant;

    public Animator Anim;
    public Transform player;
    public float chaseSpeed = 3f;

    Rigidbody2D rb;

    [Header("Detection Settings")]
    public float detectionRadius = 3f;  // Distance to wake up if player runs near
    public float alertRadius = 6f;  // Larger area where gunshots wake enemy up

    [Header("Wake Up Settings")]
    public float wakeUpTime = 2f; // Time needed to fully wake up
    private float wakeUpTimer = 0f;
    private bool playerTriggeredWakeUp = false;

    [Header("Return to Dormant Settings")]
    public float returnToDormantTime = 3f; // Time before returning to dormant if player stops triggering wake-up
    private float dormantTimer = 0f;

    public bool isWaveEnemy = false; // If true, enemy starts in Chasing mode


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Running detection range

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRadius); // Shooting detection range
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError(gameObject.name + " is missing a Rigidbody2D!");
        }

        rb.bodyType = RigidbodyType2D.Dynamic;

        if (isWaveEnemy)
        {
            currentState = EnemyState.Chasing;
            Anim.SetBool("Waking up", true);
            Anim.SetBool("run", true);
            Anim.SetBool("Waking up", true);
        }
    }

    void Update()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;

        switch (currentState)
        {
            case EnemyState.Dormant:
                CheckForPlayer();
                break;
            case EnemyState.WakingUp:
                ProcessWakeUp();
                break;
            case EnemyState.Chasing:
                // Only chase if we are in Chasing state
                ChasePlayer();
                break;
        }
    }

    //  **CHECK IF PLAYER RUNS CLOSE TO ENEMY**
    void CheckForPlayer()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            // Wake up if player runs close
            if (distance < detectionRadius && Input.GetKey(KeyCode.LeftShift))
            {
                StartWakingUp();
            }
        }
    }

    //  **START WAKING UP PHASE**
    public void StartWakingUp()
    {
        if (currentState == EnemyState.Dormant)
        {
            currentState = EnemyState.WakingUp;
            wakeUpTimer = 0f;
            Anim.SetBool("Waking up", true); // Play wake-up animation
        }
    }

    //  **PROCESS WAKING UP**
    void ProcessWakeUp()
    {
        wakeUpTimer += Time.deltaTime;

        bool playerStillTriggering = (player != null && Vector2.Distance(transform.position, player.position) < detectionRadius && Input.GetKey(KeyCode.LeftShift));

        if (wakeUpTimer >= wakeUpTime || playerStillTriggering)
        {
            // Enemy fully wakes up and starts chasing
            currentState = EnemyState.Chasing;
            Anim.SetBool("run", true);
        }
        else
        {
            // If player stops running/shooting before full wake up, start dormant timer
            dormantTimer += Time.deltaTime;
            if (dormantTimer >= returnToDormantTime)
            {
                GoBackToDormant();
            }
        }
    }

    // **RETURN TO DORMANT STATE**
    void GoBackToDormant()
    {
        currentState = EnemyState.Dormant;
        dormantTimer = 0f;
        wakeUpTimer = 0f;
        Anim.SetBool("Waking up", false);
        Anim.SetBool("Sleeping", true); // Play animation if needed
    }

    // **CHASE THE PLAYER** - This will now happen inside the Update method directly.
    void ChasePlayer()
    {
        if (player != null)
        {
            // Move towards the player's current position (stored in playerPosition)
            rb.linearVelocity = (player.position - transform.position).normalized * chaseSpeed;

            // Rotate to face the player while chasing
            Vector2 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Get the angle to the player
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f)); // Rotate the enemy to face the player

            // Check if the enemy has reached the player, for example
            if (Vector2.Distance(transform.position, player.position) < 0.1f)
            {
                // Stop chasing (or add other behavior when the enemy reaches the player)
                Anim.SetBool("run", false);
            }
        }
        else
        {
            Debug.LogError("Player reference is missing!");
        }
    }

    //  **ALERT ENEMY IF A SHOT IS FIRED NEARBY**
    public void AlertEnemy(Vector2 shotPosition)
    {
        if (currentState == EnemyState.Dormant)
        {
            float distanceToShot = Vector2.Distance(transform.position, shotPosition);

            if (distanceToShot <= alertRadius)
            {
                StartWakingUp();
            }
        }
    }
}
