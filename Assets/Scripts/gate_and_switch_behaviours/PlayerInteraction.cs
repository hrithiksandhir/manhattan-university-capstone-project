using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 1.5f;  // Distance at which the Player can interact with the lever
    public Collider leverCollider;  // Assign this in Inspector (drag & drop lever)
    public DoorBehaviour doorBehaviour1; // Reference to the first door (assign in Inspector)
    public DoorBehaviour doorBehaviour2; // Reference to the second door (assign in Inspector)
    private SwitchBehaviour leverBehaviour;

    private void Start()
    {
        if (leverCollider == null)
        {
            // Try to find the lever automatically if not assigned
            GameObject leverObj = GameObject.Find("Lever");
            if (leverObj != null)
            {
                leverCollider = leverObj.GetComponent<Collider>();
            }
        }

        if (leverCollider == null)
        {
            Debug.LogError("Lever object or Collider component NOT FOUND!");
            return;
        }

        Debug.Log($"Lever Collider found on object: {leverCollider.gameObject.name}");

        leverBehaviour = leverCollider.GetComponent<SwitchBehaviour>();

        if (leverBehaviour == null)
        {
            Debug.LogError("SwitchBehaviour component NOT FOUND on the Lever!");
            return;
        }

        // Ensure that DoorBehaviour references are set
        if (doorBehaviour1 == null || doorBehaviour2 == null)
        {
            Debug.LogError("DoorBehaviour references not assigned in Inspector!");
            return;
        }

        Debug.Log("PlayerInteraction initialized successfully with Lever.");
    }

    private void Update()
    {
        if (leverCollider == null || leverBehaviour == null) return;

        float distance = Vector3.Distance(transform.position, leverCollider.bounds.center);
        Debug.DrawLine(transform.position, leverCollider.bounds.center, Color.red);  // Visualize Distance

        Debug.Log($"Player Distance to Lever (using bounds): {distance}");

        if (distance < interactionDistance)
        {
            Debug.Log("Player is close to lever - Trying to trigger interaction!");
            leverBehaviour.TriggerLeverInteraction();

            // When the lever is down, trigger both doors to open
            if (leverBehaviour != null)
            {
                doorBehaviour1.TriggerDoorInteraction();
                doorBehaviour2.TriggerDoorInteraction();
            }
        }
        else
        {
            Debug.Log("Player is too far away.");
        }
    }
}
