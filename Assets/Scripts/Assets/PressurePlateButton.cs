using UnityEngine;

public class PressurePlateButton : MonoBehaviour
{
    [Header("Visuals")]
    public Sprite unpressedSprite;
    public Sprite pressedSprite;
    private SpriteRenderer spriteRenderer;

    [Header("Door Connection")]
    public GameObject doorObject;
    public float doorMoveDistance = 3f; // Total distance to move
    public float doorMoveSpeed = 2f;
    public Vector3 doorMoveDirection = Vector3.up; // Direction in which to move the door (default is up)

    private Vector3 doorClosedPosition;
    private Vector3 doorOpenPosition;
    private bool isPressed = false;
    private int objectsOnPlate = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogError("No SpriteRenderer found on pressure plate!");

        // Set door closed and open positions based on direction
        doorClosedPosition = doorObject.transform.position;
        doorOpenPosition = doorClosedPosition + doorMoveDirection.normalized * doorMoveDistance;
    }

    void Update()
    {
        // Moves the door in the assigned direction
        doorObject.transform.position = Vector3.MoveTowards(
            doorObject.transform.position,
            isPressed ? doorOpenPosition : doorClosedPosition,
            doorMoveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cat") || other.CompareTag("Dog"))
        {
            objectsOnPlate++;
            Debug.Log($"Object entered: {other.name}. Total on plate: {objectsOnPlate}");
            UpdateButtonState();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cat") || other.CompareTag("Dog"))
        {
            objectsOnPlate--;
            Debug.Log($"Object exited: {other.name}. Total on plate: {objectsOnPlate}");
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
            Debug.Log($"Button state changed to: {(isPressed ? "PRESSED" : "RELEASED")}");
        }
    }
}
