using UnityEngine;

public class AttackBox : MonoBehaviour
{
    [SerializeField] private int damage = 50; // Damage dealt by the attack box

    private void OnTriggerEnter(Collider other)
    {
        // Log the name of the object that entered the trigger
        Debug.Log("Attack box triggered with: " + other.gameObject.name);

        // Check if the triggered object has the "Enemy" tag
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy detected: " + other.gameObject.name);

            // Get the EnemyAI component
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                Debug.Log("Applying " + damage + " damage to enemy: " + other.gameObject.name);
                enemy.TakeDamage(damage); // Apply damage to the enemy
            }
            else
            {
                Debug.LogWarning("EnemyAI component not found on: " + other.gameObject.name);
            }
        }
        else
        {
            Debug.Log("Triggered object is not an enemy: " + other.gameObject.name);
        }
    }
}