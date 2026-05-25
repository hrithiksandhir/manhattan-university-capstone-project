using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;



public class HealthCat : MonoBehaviour
{
    public int curHealthCat = 100;
    public int maxHealthCat = 100;
    public HealthBarCat healthBarCat;
    public int damage = 10;

    public GameObject liveCat;  // Assign the main cat GameObject
    public GameObject deadCat;  // Assign the dead sprite GameObject (initially inactive)
    public GameObject deadText; // Assign the UI "Dead" text (initially inactive)

    private float damageCooldown = 1f;
    private float lastDamageTime = 0f;
    private Camera mainCamera; // Reference to the main camera

    private Vector3 originalDeadCatScale;


    void Start()
    {
        curHealthCat = maxHealthCat;

        mainCamera = Camera.main; // Get the main camera

        if (healthBarCat == null)
        {
            healthBarCat = Object.FindAnyObjectByType<HealthBarCat>();
        }

        if (healthBarCat == null)
        {
            Debug.LogError("HealthBarCat is NULL! Make sure it's assigned.");
            return;
        }

        healthBarCat.SetHealth(curHealthCat);

        // Hide the dead cat sprite and dead text at start
        if (deadCat != null)
        {
            deadCat.SetActive(false);
            originalDeadCatScale = deadCat.transform.localScale;
        }
        else
        {
            Debug.LogError("Dead Cat Sprite is NOT assigned in the Inspector!");
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
            DamagePlayerCat(damage);
        }
    }

    public void DamagePlayerCat(int damage)
    {
        if (Time.time - lastDamageTime >= damageCooldown)
        {
            lastDamageTime = Time.time;
            curHealthCat -= damage;
            curHealthCat = Mathf.Clamp(curHealthCat, 0, maxHealthCat);

            healthBarCat.SetHealth(curHealthCat);

            if (curHealthCat <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        Debug.Log("Cat has died!");

        if (mainCamera == null)
        {
            Debug.LogError("No main camera found! Ensure there is an active camera in the scene.");
            return;
        }

        if (liveCat != null)
        {
            // Move the camera away from the live cat before deactivating
            mainCamera.transform.SetParent(null);
            liveCat.SetActive(false);
        }

        if (deadCat != null)
        {
            deadCat.transform.position = liveCat.transform.position;
            deadCat.transform.localScale = originalDeadCatScale; // Restore original scale
            deadCat.SetActive(true);
        }
        else
        {
            Debug.LogError("deadCat is NULL! Make sure it's assigned in the Inspector.");
        }

        if (deadText != null)
        {
            deadText.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            deadText.SetActive(true);
            Debug.Log("Dead text should now be visible.");
        }
        else
        {
            Debug.LogError("deadText is NULL! Make sure it's assigned in the Inspector.");
        }

        // Move the camera to a better position and angle
        Vector3 targetPosition = deadCat.transform.position + new Vector3(0, 2, -5);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}