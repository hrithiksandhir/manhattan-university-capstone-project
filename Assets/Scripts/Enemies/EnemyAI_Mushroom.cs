using UnityEngine;
using System.Collections;

public class EnemyAI_Mushroom : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float runDuration = 2f;
    [SerializeField] private float idleSpeed = 2f;
    [SerializeField] private float idleDistance = 3f;


    [Header("Gravity Settings")]
    [SerializeField] private float gravity = -9.81f;

    private int currentHealth;
    private bool isRunning = false;
    private Vector3 runDirection;
    private CharacterController controller;
    private Vector3 velocity;
    private float lastDamageTime = -Mathf.Infinity;

    private Vector3 idleDirection;
    private Vector3 idleStartPosition;

    private Animator spriteAnimator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        spriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        spriteAnimator = transform.GetChild(1).GetComponent<Animator>();
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
    private IEnumerator IdleMovementRoutine()
    {
        while (!isRunning)
        {
            float moveAmount = Mathf.PingPong(Time.time * idleSpeed, idleDistance);
            Vector3 targetPosition = idleStartPosition + idleDirection * moveAmount;

            Vector3 move = targetPosition - transform.position;
            move.y = velocity.y;
            controller.Move(move * Time.deltaTime);

            if (!controller.isGrounded)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = -2f;
            }

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
            move.y = velocity.y;
            controller.Move(move * Time.deltaTime);

            if (!controller.isGrounded)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = -2f;
            }

            timer += Time.deltaTime;

            spriteAnimator.SetFloat("Speed", Mathf.Abs(move.x));
            FlipSprite(move.x);

            yield return null;
        }
        isRunning = false;
        StartCoroutine(IdleMovementRoutine());
    }

    private void Update()
    {
        if (!isRunning)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void FlipSprite(float moveDirection)
    {
        if (moveDirection > 0)
        {
            spriteRenderer.flipX = true; // Sprite faces right
        }
        else if (moveDirection < 0)
        {
            spriteRenderer.flipX = false; // Sprite faces left
        }
    }
}
