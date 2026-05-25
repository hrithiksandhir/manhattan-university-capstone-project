using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathFloor : MonoBehaviour
{
    public HealthCat player1Health;
    public HealthDog player2Health;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cat"))
        {
            player1Health.Die();
            player2Health.RestartLevel(); // Restart the level for the other player
        }
        else if (other.CompareTag("Dog"))
        {
            player2Health.Die();
            player1Health.RestartLevel(); // Restart the level for the other player
        }

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}

