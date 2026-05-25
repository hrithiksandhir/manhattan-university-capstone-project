using UnityEngine;
using UnityEngine.UI;

public class HealthBarDog : MonoBehaviour
{
    public Slider healthBarDog;
    public HealthDog playerHealth; // Reference to the HealthDog script


    private void Start()
    {
        // Find and assign the HealthDog component
        GameObject dogObject = GameObject.FindGameObjectWithTag("Dog");
        if (dogObject != null)
        {
            playerHealth = dogObject.GetComponent<HealthDog>();
        }

        // Get the Slider component
        if (healthBarDog == null)
        {
            healthBarDog = GetComponent<Slider>();
        }

        // Debugging to check if values are properly assigned
        if (playerHealth == null)
        {
            Debug.LogError("HealthDog script not found! Make sure the 'Dog' GameObject has the correct tag and script.");
        }

        if (healthBarDog == null)
        {
            Debug.LogError("Slider component is missing from HealthBarDog! Ensure the UI has a Slider component.");
        }

        if (playerHealth != null && healthBarDog != null)
        {
            healthBarDog.maxValue = playerHealth.maxHealthDog; // Set max value
            healthBarDog.value = playerHealth.maxHealthDog;    // Set initial health value
            Debug.Log("HealthBarDog successfully initialized.");
        }
    }

    public void SetHealth(int hp)
    {
        if (healthBarDog == null)
        {
            Debug.LogError("Cannot set health: HealthBarDog is NULL!");
            return;
        }

        // Use string interpolation to log the current health value correctly
        Debug.Log($"Updating HealthBarDog to: {hp}");
        healthBarDog.value = hp; // Update the slider value
    }

}