using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float distance = 5f;

    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 targetPoint;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        pointA = transform.position;
        pointB = pointA + Vector3.right * distance;
        targetPoint = pointB;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        MoveCar();
    }

    void MoveCar()
    {
        // Move toward the target point
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        // Flip sprite based on direction
        if (targetPoint.x > transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        // Switch direction at destination
        if (Vector3.Distance(transform.position, targetPoint) < 0.05f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }
    }
}
