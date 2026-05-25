using UnityEngine;
using UnityEngine.EventSystems; // Required for UI event handling

public class HoverLiftEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalPosition; // Store the button's starting position
    public float liftAmount = 10f; // Amount to lift the button when hovered over
    public float liftSpeed = 0.1f; // Speed of the movement

    private void Start()
    {
        originalPosition = transform.localPosition; // Save the original position of the button
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Move the button up when the mouse enters
        StopAllCoroutines(); // Stop previous animations
        StartCoroutine(MoveButton(originalPosition + new Vector3(0, liftAmount, 0)));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Move the button back when the mouse exits
        StopAllCoroutines();
        StartCoroutine(MoveButton(originalPosition));
    }

    private System.Collections.IEnumerator MoveButton(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, liftSpeed);
            yield return null;
        }
        transform.localPosition = targetPosition; // Ensure it lands exactly at the target
    }
}
