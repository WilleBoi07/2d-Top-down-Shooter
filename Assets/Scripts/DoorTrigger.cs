using UnityEngine;
using UnityEngine.UI;

public class DoorTrigger : MonoBehaviour
{
    public GameObject warningUI; // Assign a warning UI pop-up in the Inspector
    public GameObject terminal;  // The terminal object to enable interaction

    private bool playerNearDoor = false;

    private void Start()
    {
        warningUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearDoor = true;
            warningUI.SetActive(true); // Show warning pop-up
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearDoor = false;
            warningUI.SetActive(false); // Hide warning
        }
    }
}
