using UnityEngine;
using System.Collections;

public class WolfMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float distance = 5f;
    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 targetPoint;

    [Header("Scare Behavior")]
    public Transform dog;
    public float scareDistance = 10f;
    public float scareSpeedMultiplier = 2f;

    [Header("Animation")]
    public Sprite[] runSprites;
    public float frameRate = 10f;

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;

    private BoxCollider wolfCollider;
    private CharacterController wolfCharacterController;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        pointA = transform.position;
        pointB = pointA + Vector3.right * distance;
        targetPoint = pointB;

        // Get components from the parent object (regular 3D components)
        wolfCollider = GetComponentInParent<BoxCollider>();
        wolfCharacterController = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        MoveWolf();
        AnimateWolf();

        if (dog != null && Vector3.Distance(transform.position, dog.position) < scareDistance && Input.GetKeyDown(KeyCode.B))
        {
            ScaredByBark(dog.position);
        }
    }

    void MoveWolf()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        // Flip sprite based on direction
        if (targetPoint.x > transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        // When destination reached, switch direction
        if (Vector3.Distance(transform.position, targetPoint) < 0.05f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }
    }

    void AnimateWolf()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % runSprites.Length;
            spriteRenderer.sprite = runSprites[currentFrame];
        }
    }

    public void ScaredByBark(Vector3 sourcePosition)
    {
        Debug.Log($"{gameObject.name} got scared by the dog!");

        // Disable the collider and CharacterController on the parent object
        if (wolfCollider != null)
            wolfCollider.enabled = false;

        if (wolfCharacterController != null)
            wolfCharacterController.enabled = false;

        moveSpeed *= scareSpeedMultiplier;

        StartCoroutine(ResetWolfState());
    }

    private IEnumerator ResetWolfState()
    {
        // Allow for time for the dog or cat to jump over
        yield return new WaitForSeconds(2f);

        // Re-enable the collider and CharacterController on the parent object
        if (wolfCollider != null)
            wolfCollider.enabled = true;

        if (wolfCharacterController != null)
            wolfCharacterController.enabled = true;

        moveSpeed /= scareSpeedMultiplier;
    }
}
