using UnityEngine;
using UnityEngine.UI;

public class BioScan : MonoBehaviour
{
    public EnemySpawner enemySpawner; // Reference to the enemy spawner
    public SlidingDoor slidingDoor; // Reference to the SlidingDoor script

    public float scanTime = 60f; // Time required to reach 100%
    private float currentScanProgress = 0f;
    private bool isScanning = false;

    public GameObject scanCircle;
    public GameObject terminalLight;
    public GameObject progressBarUI;
    
    public Slider progressBar; // Assign in Inspector (UI)
    private bool isPlayerInside = false; // Tracks if player is in the scan area

    void Update()
    {
        if (isScanning)
        {
            Debug.Log("Update running. isScanning = " + isScanning + ", isPlayerInside = " + isPlayerInside);

            if (isPlayerInside)
            {
                currentScanProgress += Time.deltaTime;
                Debug.Log("Player inside scan area, increasing progress: " + currentScanProgress);
            }
            else
            {
                currentScanProgress -= Time.deltaTime; // Decrease if player leaves
                Debug.Log("Player left scan area, decreasing progress: " + currentScanProgress);
            }

            currentScanProgress = Mathf.Clamp(currentScanProgress, 0, scanTime);
            UpdateUI(); // Make sure UI updates every frame

            if (currentScanProgress >= scanTime)
            {
                Debug.Log("Scan completed! Stopping scan.");
                StopScan();
                UnlockDoor(); // **NEW: Open the door when scan reaches 100%**
                terminalLight.SetActive(false);
                scanCircle.SetActive(false);
                progressBarUI.SetActive(false);
            }
        }
    }

    public void StartScan()
    {
        if (!isScanning)
        {
            isScanning = true;
            if (enemySpawner != null)
            {
                enemySpawner.StartSpawning();
                Debug.Log("StartScan() called. Scan started. Enemy wave spawning.");
            }
            else
            {
                Debug.LogWarning("No EnemySpawner assigned! Enemies will NOT spawn.");
            }
            UpdateUI();
        }
    }

    public void StopScan()
    {
        isScanning = false;
        if (enemySpawner != null)
        {
            enemySpawner.StopSpawning();
            Debug.Log("StopScan() called. Scan stopped. Enemy wave stopped.");
        }
        else
        {
            Debug.LogWarning("No EnemySpawner assigned! Nothing to stop.");
        }
        UpdateUI();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Player entered scan area.");
            StartScan();  // Auto-start scan when entering the area
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Player exited scan area.");
        }
    }

    void UpdateUI()
    {
        if (progressBar != null)
        {
            progressBar.value = currentScanProgress / scanTime; // Normalize to 0-1
            Debug.Log("UI Updated: Progress bar value = " + progressBar.value);
        }
        else
        {
            Debug.LogWarning("Progress bar reference is missing in the inspector!");
        }
    }

    void UnlockDoor()
    {
        if (slidingDoor != null)
        {
            slidingDoor.UnlockDoor(); // **NEW: Calls the SlidingDoor script**
            Debug.Log("Door unlocked and sliding open!");
        }
        else
        {
            Debug.LogWarning("No SlidingDoor assigned! Door will NOT open.");
        }
    }
}
