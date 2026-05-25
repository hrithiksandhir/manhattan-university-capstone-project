using UnityEngine;
using System.Collections;

public class EnemyAI_Skull : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float runDuration = 2f;
    [SerializeField] private float idleSpeed = 2f;
    [SerializeField] private float idleDistance = 3f;

    private bool isRunning = false;
    private Vector3 runDirection;
    private CharacterController controller;

    private Vector3 idleDirection;
    private Vector3 idleStartPosition;

    private Animator spriteAnimator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Start()
    {
        idleStartPosition = transform.position;
        idleDirection = Vector3.right;
        StartCoroutine(IdleMovementRoutine());
    }

    public void RunAway(Vector3 barkSource)
    {
        if (!isRunning)
        {
            isRunning = true;
            runDirection = (transform.position - barkSource).normalized;
            StopCoroutine(IdleMovementRoutine());
            StartCoroutine(RunAwayRoutine());
        }
    }

    private void Die()
    {
        spriteAnimator.SetTrigger("Die");
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        float deathAnimationDuration = spriteAnimator.GetCurrentAnimatorStateInfo(3).length;
        yield return new WaitForSeconds(deathAnimationDuration);
        Destroy(gameObject);
    }

    private IEnumerator IdleMovementRoutine()
    {
        while (!isRunning)
        {
            float moveAmount = Mathf.PingPong(Time.time * idleSpeed, idleDistance);
            Vector3 targetPosition = idleStartPosition + idleDirection * moveAmount;

            Vector3 move = targetPosition - transform.position;
            controller.Move(move * Time.deltaTime);

            spriteAnimator.SetFloat("Speed", Mathf.Abs(move.x));
            FlipSprite(move.x);

            yield return null;
        }
    }

    private IEnumerator RunAwayRoutine()
    {
        float timer = 0f;
        while (timer < runDuration)
        {
            Vector3 move = runDirection * runSpeed;
            controller.Move(move * Time.deltaTime);

            timer += Time.deltaTime;

            spriteAnimator.SetFloat("Speed", Mathf.Abs(move.x));  // Update animation speed
            FlipSprite(move.x);

            yield return null;
        }
        isRunning = false;
        StartCoroutine(IdleMovementRoutine());
    }

    private void Update()
    {
        // Remove gravity handling, since the enemy is floating
        if (!isRunning)
        {
            // No gravity for floating enemy, but still check if idle movement is necessary
            controller.Move(Vector3.zero);  // Keep position but no movement unless run is triggered
        }
    }

    private void FlipSprite(float moveDirection)
    {
        if (moveDirection > 0)
        {
            spriteRenderer.flipX = false; // Sprite faces right
        }
        else if (moveDirection < 0)
        {
            spriteRenderer.flipX = true; // Sprite faces left
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeathBlock"))
        {
            Die();
        }
    }
}
