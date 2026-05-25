using UnityEngine;
using System.Collections;

public class TeleportationTrigger : MonoBehaviour
{
    public Transform secondDoorTransform;  // Reference to the second door's transform (position)

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.gameObject.name}"); // Debugging which object triggers

        // Allow both "Cat" and "Dog" to teleport
        if (other.CompareTag("Cat") || other.CompareTag("Dog"))
        {
            Debug.Log($"{other.gameObject.name} entered the first door. Teleporting to second door...");

            if (secondDoorTransform == null)
            {
                Debug.LogError("secondDoorTransform is not assigned!");
                return;
            }

            Debug.Log($"Second door position: {secondDoorTransform.position}");
            Debug.Log($"Before Teleport: {other.transform.position}");

            // Try NavMeshAgent teleport if available
            UnityEngine.AI.NavMeshAgent agent = other.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(secondDoorTransform.position);
            }
            else
            {
                // Forcefully teleport by disabling & enabling
                other.gameObject.SetActive(false);
                other.transform.position = secondDoorTransform.position;
                other.gameObject.SetActive(true);
            }

            Debug.Log($"After Teleport: {other.transform.position}");

            Debug.Log("Teleportation complete!");

            // Delay disabling collider to prevent immediate re-triggering
            StartCoroutine(DisableColliderDelayed());
        }
    }

    private IEnumerator DisableColliderDelayed()
    {
        yield return new WaitForEndOfFrame(); // Wait for a frame
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1f); // Allow time for other characters to enter before re-enabling
        GetComponent<Collider>().enabled = true;
    }
}

