using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    public GameObject leverUpSprite;  // Reference to the up sprite game object
    public GameObject leverDownSprite;  // Reference to the down sprite game object
    private bool isLeverDown = false;  // To track the lever state

    private void Start()
    {
        if (leverUpSprite == null || leverDownSprite == null)
        {
            Debug.LogError("Lever sprites are NOT assigned!");
            return;
        }

        // Initially, set the lever to the "up" position
        leverUpSprite.SetActive(true);
        leverDownSprite.SetActive(false);
    }

    public void TriggerLeverInteraction()
    {
        Debug.Log("Attempting to trigger lever interaction...");

        if (!isLeverDown)
        {
            Debug.Log("Lever switching to DOWN!");

            leverUpSprite.SetActive(false);
            leverDownSprite.SetActive(true);

            isLeverDown = true;
        }
        else
        {
            Debug.Log("Lever is already down!");
        }
    }
}
