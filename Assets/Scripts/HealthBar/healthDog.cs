using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class HealthDog : MonoBehaviour
{
    public int curHealthDog = 100;
    public int maxHealthDog = 100;
    public HealthBarDog healthBarDog;
    public int damage = 10;

    public GameObject liveDog;  // Assign the main dog GameObject
    public GameObject deadDog;  // Assign the dead sprite GameObject (initially inactive)
    public GameObject deadText; // Assign the UI "Dead" text (initially inactive)

    private float damageCooldown = 1f;
    private float lastDamageTime = 0f;
    private Camera mainCamera; // Reference to the main camera
    private Vector3 originalDeadDogScale;

    void Start()
    {
        curHealthDog = maxHealthDog;

        mainCamera = Camera.main; // Get the main camera

        if (healthBarDog == null)
        {
            healthBarDog = Object.FindAnyObjectByType<HealthBarDog>();
        }

        if (healthBarDog == null)
        {
            Debug.LogError("HealthBarDog is NULL! Make sure it's assigned.");
            return;
        }

        healthBarDog.SetHealth(curHealthDog);

        // Hide the dead dog sprite and dead text at start
        if (deadDog != null)
        {
            deadDog.SetActive(false);
            originalDeadDogScale = deadDog.transform.localScale;
        }
        else
        {
            Debug.LogError("Dead Dog Sprite is NOT assigned in the Inspector!");
        }

        if (deadText != null)
        {
            deadText.SetActive(false);
        }
        else
        {
            Debug.LogError("Dead Text is NOT assigned in the Inspector!");
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collided with: " + hit.collider.gameObject.name);
        if (hit.collider.CompareTag("Enemy"))
        {
            DamagePlayerDog(damage);
        }
    }

    public void DamagePlayerDog(int damage)
    {
        if (Time.time - lastDamageTime >= damageCooldown)
        {
            lastDamageTime = Time.time;
            curHealthDog -= damage;
            curHealthDog = Mathf.Clamp(curHealthDog, 0, maxHealthDog);

            healthBarDog.SetHealth(curHealthDog);

            if (curHealthDog <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        Debug.Log("Dog has died!");

        if (mainCamera == null)
        {
            Debug.LogError("No main camera found! Ensure there is an active camera in the scene.");
            return;
        }

        if (liveDog != null)
        {
            // Move the camera away from the live dog before deactivating
            mainCamera.transform.SetParent(null);
            liveDog.SetActive(false);
        }

        if (deadDog != null)
        {
            deadDog.transform.position = liveDog.transform.position;
            deadDog.transform.localScale = originalDeadDogScale; // Restore original scale
            deadDog.SetActive(true);
            Debug.Log("Live Dog Position: " + liveDog.transform.position);
            Debug.Log("Dead Dog Position before setting: " + deadDog.transform.position);
            deadDog.transform.position = liveDog.transform.position;
            Debug.Log("Dead Dog Position after setting: " + deadDog.transform.position);
        }
        else
        {
            Debug.LogError("deadDog is NULL! Make sure it's assigned in the Inspector.");
        }

        if (deadText != null)
        {
            // UI text positioning
            deadText.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            deadText.SetActive(true);
            Debug.Log("Dead text should now be visible.");
        }
        else
        {
            Debug.LogError("deadText is NULL! Make sure it's assigned in the Inspector.");
        }

        // Move the camera to a better position and angle
        Vector3 targetPosition = deadDog.transform.position + new Vector3(0, 2, -5);
        StartCoroutine(SmoothCameraTransition(mainCamera.transform.position, targetPosition, 1f));

        Invoke("RestartLevel", 1f);
    }

    // Smoothly transition camera position over time
    private IEnumerator SmoothCameraTransition(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = targetPosition; // Ensure it reaches the target position
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }
}
