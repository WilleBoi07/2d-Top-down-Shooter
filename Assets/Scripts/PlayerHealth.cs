using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;  // Reference to the health slider
    public TextMeshProUGUI healthText;  // Reference to the health text (TextMeshProUGUI instead of Text)
    public float maxHealth = 100f;  // Set your max health here
    private float currentHealth;

    void Start()
    {
        // Initialize the current health to max health
        currentHealth = maxHealth;

        // Set the slider's max value and current value
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        // Update the text display
        UpdateHealthText();
    }

    void Update()
    {
        // Update the slider's value based on current health
        healthSlider.value = currentHealth;

        // Optionally, you can update health based on certain conditions here (e.g., damage, healing)
        // For example, you can test it by pressing the spacebar to reduce health:
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f); // Takes 10 damage when spacebar is pressed
        }

        // Update the health text
        UpdateHealthText();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        // Ensure health doesn't go below 0
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Optionally, you can add a "Game Over" check if health reaches 0
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth); // Prevent overhealing
        Debug.Log("Player healed! Current Health: " + currentHealth);
    }

    public void EnableHealthPickup(HealthPickup pickup)
    {
        // Check for input (e.g., press E)
        if (Input.GetKeyDown(pickup.interactKey))
        {
            Debug.Log("E key pressed, healing " + pickup.healthAmount + " health.");
            Heal(pickup.healthAmount);  // Add health
            Destroy(pickup.gameObject);  // Destroy the health pickup object
        }
    }

    void UpdateHealthText()
    {
        // Update the text to show current health and max health
        healthText.text = "HP " + currentHealth + "/" + maxHealth;
    }
}
