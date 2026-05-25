using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public GameObject doorClosedSprite;  // Reference to the closed door sprite
    public GameObject doorOpenSprite;    // Reference to the open door sprite
    private bool isDoorOpen = false;     // To track the door's state

    private void Start()
    {
        // Initially, set the door to the "closed" position
        doorClosedSprite.SetActive(true);
        doorOpenSprite.SetActive(false);
    }

    // Method to switch the door state
    public void TriggerDoorInteraction()
    {
        if (!isDoorOpen)
        {
            Debug.Log("Door opening...");

            // Switch door sprite to open
            doorClosedSprite.SetActive(false);
            doorOpenSprite.SetActive(true);

            // Update the door state
            isDoorOpen = true;
        }
        else
        {
            Debug.Log("Door is already open!");
        }
    }
}
