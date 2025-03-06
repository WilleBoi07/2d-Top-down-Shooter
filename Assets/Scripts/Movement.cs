using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float crouchSpeed = 1.5f;

    private Rigidbody2D rb;
    private Vector2 movementInput;

    public bool isWalking { get; private set; }
    public bool isRunning { get; private set; }
    public bool isCrouching { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get movement input
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Toggle crouch when pressing Left Control
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;  // Toggle crouch state
        }

        // Running logic
        if (isCrouching)
        {
            isRunning = false;  // Prevent running while crouching
        }
        else
        {
            isRunning = Input.GetKey(KeyCode.LeftShift);  // Allow running only if not crouching
        }

        // Determine speed
        float speed = isRunning ? runSpeed : (isCrouching ? crouchSpeed : walkSpeed);

        // Walking is true if moving and neither running nor crouching
        isWalking = movementInput.magnitude > 0 && !isRunning && !isCrouching;

        // Rotate towards the mouse
        RotateTowardsMouse();

        // Move the player
        rb.linearVelocity = movementInput * speed;
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
