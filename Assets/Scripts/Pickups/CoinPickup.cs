using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Settings")]
    public int coinValue = 1;
    public AudioClip pickupSound;
    public float destroyDelay = 0.1f; // Small delay to ensure sound plays

    private AudioSource audioSource;
    private bool wasCollected = false; // Prevent double collection

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource if missing
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if collected by player and not already collected
        if ((other.CompareTag("Cat") || other.CompareTag("Dog")) && !wasCollected)
        {
            wasCollected = true;
            
            // Disable visuals and collision immediately
            GetComponent<Collider>().enabled = false;
            MeshRenderer mesh = GetComponent<MeshRenderer>();
            if (mesh != null) mesh.enabled = false;

            // Add score
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(coinValue);
            }
            else
            {
                Debug.LogWarning("ScoreManager instance not found!");
            }

            // Play sound if available
            if (pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }
            else
            {
                Debug.LogWarning("Missing sound component or AudioClip!");
            }

            // Destroy after small delay (ensures sound plays)
            Destroy(gameObject, destroyDelay);
        }
    }
}