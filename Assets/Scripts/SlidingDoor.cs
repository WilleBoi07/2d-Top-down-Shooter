using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Transform door;             // The door itself
    public Transform targetPosition;   // Target position for the door (where it slides)
    public float slideSpeed = 0.01f;      // Speed at which the door slides

    private Vector3 originalPosition;  // Original position of the door
    private bool doorUnlocked = false; // Whether the door is unlocked

    void Start()
    {
        originalPosition = door.position; // Store the original position at the start
    }
    void Update()
    {
        if (doorUnlocked)
        {
            door.position = Vector3.MoveTowards(door.position, targetPosition.position, slideSpeed * Time.deltaTime);
        }
        else
        {
            door.position = Vector3.MoveTowards(door.position, originalPosition, slideSpeed * Time.deltaTime); // Move back to original position
        }
    }

    public void UnlockDoor()
    {
        doorUnlocked = true; // Set door as unlocked
    }

    public void LockDoor()
    {
        doorUnlocked = false; // Lock the door, it will move back to its original position
    }
}
