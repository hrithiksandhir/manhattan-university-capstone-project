using UnityEngine;
using UnityEngine.UI;

public class HealthBarCat : MonoBehaviour
{
    public Slider healthBarCat;
    public HealthCat playerHealth; // Reference to the HealthCat script

    private void Start()
    {
        // Find and assign the HealthCat component
        GameObject catObject = GameObject.FindGameObjectWithTag("Cat");
        if (catObject != null)
        {
            playerHealth = catObject.GetComponent<HealthCat>();
        }

        // Get the Slider component
        if (healthBarCat == null)
        {
            healthBarCat = GetComponent<Slider>();
        }

        // Debugging to check if values are properly assigned
        if (playerHealth == null)
        {
            Debug.LogError("HealthCat script not found! Make sure the 'Cat' GameObject has the correct tag and script.");
        }

        if (healthBarCat == null)
        {
            Debug.LogError("Slider component is missing from HealthBarCat! Ensure the UI has a Slider component.");
        }

        if (playerHealth != null && healthBarCat != null)
        {
            healthBarCat.maxValue = playerHealth.maxHealthCat; // Set max value
            healthBarCat.value = playerHealth.maxHealthCat;    // Set initial health value
            Debug.Log("HealthBarCat successfully initialized.");
        }
    }

    public void SetHealth(int hp)
    {
        if (healthBarCat == null)
        {
            Debug.LogError("Cannot set health: HealthBarCat is NULL!");
            return;
        }

        // Use string interpolation to log the current health value correctly
        Debug.Log($"Updating HealthBarCat to: {hp}");
        healthBarCat.value = hp; // Update the slider value
    }
}
