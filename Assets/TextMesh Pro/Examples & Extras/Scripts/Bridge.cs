using UnityEngine;
using UnityEngine.Tilemaps;

public class BridgeActivator : MonoBehaviour
{
    [Header("Visuals")]
    public Sprite unpressedSprite;
    public Sprite pressedSprite;
    private SpriteRenderer spriteRenderer;

    [Header("Bridge Tilemap")]
    public GameObject bridgeTilemap; // Assign the bridge Tilemap GameObject here

    private bool isPressed = false;
    private int objectsOnPlate = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogError("No SpriteRenderer found on pressure plate!");

        if (bridgeTilemap != null)
            bridgeTilemap.SetActive(false); // Start hidden
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dog") || other.CompareTag("Cat"))
        {
            objectsOnPlate++;
            UpdateButtonState();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dog") || other.CompareTag("Cat"))
        {
            objectsOnPlate--;
            UpdateButtonState();
        }
    }

    void UpdateButtonState()
    {
        bool shouldBePressed = objectsOnPlate > 0;

        if (shouldBePressed != isPressed)
        {
            isPressed = shouldBePressed;
            spriteRenderer.sprite = isPressed ? pressedSprite : unpressedSprite;

            // Show/hide bridge
            if (bridgeTilemap != null)
                bridgeTilemap.SetActive(isPressed);

            Debug.Log($"Button {(isPressed ? "PRESSED" : "RELEASED")} - Bridge {(isPressed ? "shown" : "hidden")}");
        }
    }
}
