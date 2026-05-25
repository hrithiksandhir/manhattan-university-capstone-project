using UnityEngine;
using UnityEngine.SceneManagement;

public class FinaleFlag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Cat") || other.CompareTag("Dog"))
        {
            SceneManager.LoadScene("CreditsScreen");
        }
    }
}