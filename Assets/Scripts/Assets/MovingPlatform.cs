using UnityEngine;

public class MovingPlatformRaycast : MonoBehaviour
{
    
    public float speed = 2.0f;
    public float rayDistance = 1.8f;
    public float maxTravelDistance = 20.0f;
    public bool moveVertically = false;

    private int direction = 1;
    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        Vector3 movementDirection = moveVertically ? Vector3.up : Vector3.right;

        Ray ray = new Ray(transform.position, movementDirection * direction);
        RaycastHit hit;

        Debug.DrawRay(transform.position, movementDirection * direction * rayDistance, Color.red);

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Climbable"))
            {
                direction *= -1;
            }
        }

        float distanceFromStart = Vector3.Distance(transform.position, startingPosition);
        if (distanceFromStart >= maxTravelDistance)
        {
            direction *= -1;
        }

        transform.Translate(movementDirection * direction * speed * Time.deltaTime);
    }
}