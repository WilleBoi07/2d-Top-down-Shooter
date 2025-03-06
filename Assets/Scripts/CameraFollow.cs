using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Assign the player GameObject in the Inspector
    public float smoothSpeed = 5f;  // Adjust for smoother movement

    void LateUpdate()
    {
        if (player == null) return;

        // Keep the camera at the player's position but don't rotate
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
