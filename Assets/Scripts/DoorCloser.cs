using UnityEngine;

public class DoorCloser : MonoBehaviour
{
    public SlidingDoor slidingDoor;  // Reference to the SlidingDoor script

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // Correct the tag comparison
        {
            slidingDoor.LockDoor();  // Lock the door and make it close
            Debug.Log("Player entered the area. Locking door.");
        }
    }
}
