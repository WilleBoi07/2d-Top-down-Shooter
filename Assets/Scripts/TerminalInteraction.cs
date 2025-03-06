using UnityEngine;

public class TerminalInteraction : MonoBehaviour
{
    public GameObject scanCircle; // Assign the circle area in the Inspector
    public GameObject progressBarUI; // Assign the progress bar UI
    private bool playerNearTerminal = false;

    private void Start()
    {
        scanCircle.SetActive(false);  
        progressBarUI.SetActive(false);
    }
    void Update()
    {
        if (playerNearTerminal && Input.GetKeyDown(KeyCode.E))
        {
            scanCircle.SetActive(true);  // Show the scan area
            progressBarUI.SetActive(true);  // Show the progress bar UI
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearTerminal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearTerminal = false;
        }
    }
}
