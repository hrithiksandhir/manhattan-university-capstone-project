using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    // Movement
    [Header("Movement")]
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float SprintSpeed;
    [SerializeField] private float JumpHeight;
    [SerializeField] private float Gravity;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private float wallJumpUpForce;

    //Barking
    [Header("Bark")]
    [SerializeField] private float barkRadius; // How far the bark affects enemies
    [SerializeField] private LayerMask enemyLayer; // LayerMask for enemies
    [SerializeField] private AudioClip barkSound; // Barking sound effect
    private AudioSource audioSource;

    // Digging
    [Header("Dig")]
    [SerializeField] private float digRadius; // How close the player must be to dig
    [SerializeField] private float digCooldown; // Cooldown time
    private bool canDig = true;
    private List<Transform> digSpots = new List<Transform>();

    // Rope Swinging
    [Header("Rope Swing")]
    [SerializeField] private LayerMask ropeLayer; // Layer to detect ropes
    [SerializeField] private float ropeGrabRadius = 1f; // How close the player must be to grab
    private FixedJoint ropeJoint; // Joint to attach to the rope
    private Rigidbody rb; // Reference to the Rigidbody
    private bool isOnRope = false;
    [SerializeField] private float ropeSwingForce; // How much force is applied to the rope for swinging
    [SerializeField] private float ropeSwingMaxSpeed; // Max speed to limit the swing
    private Vector3 ropeAnchorPoint; // The point on the rope that the player is attached to
    private Vector3 swingDirection; // Direction in which the player is swinging

    // Attack
    [Header("Slash")]
    [SerializeField] private Vector3 attackBoxOffset = new Vector3(1, 0, 0);
    //[SerializeField] private Vector3 attackBoxLocalPositionLeft = new Vector3(1, 0, 0);
    [SerializeField] private GameObject attackBox;
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private float attackCooldown = 0.5f;
    private bool canAttack = true;

    [Header("Stomp")]
    [SerializeField] private float stompBounceForce = 5f;

    [Header("Moving Platform")]
    [SerializeField] private float platformStickForce = 10f;
    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;
    private bool wasOnPlatformLastFrame;

    // Player Index: used to differentiate players
    [Header("Player Index")]
    [SerializeField] private int playerIndex;

    // Climbable wall detection
    private float climbableCheckRadius = 0.5f;
    private LayerMask climbableLayer;

    // Components
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector2 inputVector = Vector2.zero;
    private SpriteRenderer spriteRenderer;
    private Animator spriteAnimator;

    // Checks
    private bool isGrounded;
    private bool isSprinting = false;
    private bool isTouchingClimbable = false;
    private Vector3 wallNormal;
    private bool canMove = true;
    private bool isMultiplayer = false;


    private void Start()
    {
        isMultiplayer = PlayerPrefs.GetString("GameMode") == "Multiplayer";
        // Find all dig spots in the scene
        GameObject[] spots = GameObject.FindGameObjectsWithTag("DigSpot");
        foreach (GameObject spot in spots)
        {
            digSpots.Add(spot.transform);
        }

        if (attackBox != null)
        {
            attackBox.SetActive(false);
        }


    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteAnimator = transform.GetChild(0).GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        if (attackBox != null)
        {
            attackBox.transform.localPosition = attackBoxOffset;
        }

    }
    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public void SetPlayerIndex(int newIndex)
    {
        playerIndex = newIndex;
    }

    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void OnJump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(JumpHeight * -3.0f * Gravity);
        }
        else if (isOnRope)
        {
            // Detach from the rope and apply jump force
            DetachFromRope();
            playerVelocity.y = Mathf.Sqrt(JumpHeight * -2.0f * Gravity);
        }
        else if (!isGrounded && isTouchingClimbable) // Wall Jump
        {
            isTouchingClimbable = false;
            DisableMovement();
            Vector2 jumpDirection = new Vector2(wallNormal.x * wallJumpForce, wallJumpUpForce);
            playerVelocity = jumpDirection;

            // Flip sprite and attack box based on wall normal
            if (wallNormal.x > 0)
            {
                spriteRenderer.flipX = false;
                if (attackBox != null)
                {
                    attackBox.transform.localPosition = attackBoxOffset; // Face right
                }
            }
            else if (wallNormal.x < 0)
            {
                spriteRenderer.flipX = true;
                if (attackBox != null)
                {
                    attackBox.transform.localPosition = new Vector3(-attackBoxOffset.x, attackBoxOffset.y, attackBoxOffset.z); // Face left
                }
            }

            StartCoroutine(ReEnableMovementAfterJump());
        }
        else
        {
            playerVelocity.y += Gravity * Time.deltaTime;
            if (playerVelocity.y < 0)
            {
                playerVelocity.x = 0;
            }
        }
    }

    private IEnumerator ReEnableMovementAfterJump()
    {
        yield return new WaitForSeconds(.5f);
        EnableMovement();
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false; // Disable attacking during the routine

        // Activate the attack box
        attackBox.SetActive(true);

        // Wait for the attack duration
        yield return new WaitForSeconds(attackDuration);

        // Deactivate the attack box
        attackBox.SetActive(false);

        // Wait for the remaining cooldown time
        yield return new WaitForSeconds(attackCooldown - attackDuration);

        canAttack = true; // Re-enable attacking
    }

    // Modify your existing empty Attack method
    public void Attack()
    {
        // Only allow attacking if the character is a Cat and canAttack is true
        if (canAttack && gameObject.CompareTag("Cat") && isGrounded)
        {
            StartCoroutine(AttackRoutine());
            Debug.Log("*attacks*");
        }
    }

    private void Stomp(Collider enemyHeadCollider)
    {
        playerVelocity.y = Mathf.Sqrt(stompBounceForce * -3.0f * Gravity);

        EnemyAI enemy = enemyHeadCollider.GetComponentInParent<EnemyAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(100);
        }

    }
    public void OnSwitch()
    {
        if (isMultiplayer)
        {
            Debug.Log("Character switching is disabled in multiplayer mode.");
            return;
        }
        foreach (Mover mover in FindObjectsOfType<Mover>())
        {
            mover.SetPlayerIndex(mover.GetPlayerIndex() == 0 ? 1 : 0);
        }
        //Debug.Log("Switched player indexes");
    }

    public void StartSprint()
    {
        isSprinting = true;
    }

    public void StopSprint()
    {
        isSprinting = false;
    }

    public void Bark()
    {
        // Play bark animation
        spriteAnimator.SetTrigger("Bark");

        // Play bark sound if available
        if (barkSound != null && audioSource != null)
            audioSource.PlayOneShot(barkSound);

        // Reset the animation trigger after a short delay
        StartCoroutine(ResetBarkAnimation());

        // Detect nearby enemies and make them run away
        Collider[] enemies = Physics.OverlapSphere(transform.position, barkRadius, enemyLayer);

        foreach (Collider enemy in enemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            EnemyAI_Skull enemyAI_skull = enemy.GetComponent<EnemyAI_Skull>();
            EnemyAI_Mushroom enemyAI_mushroom  = enemy.GetComponent<EnemyAI_Mushroom>();
            if (enemyAI != null)
            {
                enemyAI.RunAway(transform.position);
            }
            else if (enemyAI_skull != null)
            {
                enemyAI_skull.RunAway(transform.position);
            }
            else if (enemyAI_mushroom != null)
            {
                enemyAI_mushroom.RunAway(transform.position);
            }
        }
    }

    // Coroutine to reset the Bark animation trigger
    private IEnumerator ResetBarkAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Adjust based on animation length
        spriteAnimator.ResetTrigger("Bark");
    }

    public void OnDig()
    {
        if (!canDig) return;

        // Find the nearest dig spot
        Transform nearestSpot = null;
        float nearestDistance = float.MaxValue;
        foreach (Transform spot in digSpots)
        {
            float distance = Vector3.Distance(transform.position, spot.position);
            if (distance < digRadius && distance < nearestDistance)
            {
                nearestSpot = spot;
                nearestDistance = distance;
            }
        }

        if (nearestSpot == null) return;

        StartCoroutine(DigAndTeleport(nearestSpot));
    }

    private IEnumerator DigAndTeleport(Transform currentSpot)
    {
        canDig = false;

        // Play digging animation
        if (spriteAnimator != null)
        {
            spriteAnimator.SetTrigger("Dig");
        }

        // Wait to simulate digging animation
        yield return new WaitForSeconds(1f);

        // Find a different dig spot
        Transform newSpot = digSpots
            .Where(spot => spot != currentSpot) // Exclude the current spot
            .OrderBy(_ => Random.value) // Randomize selection
            .FirstOrDefault();

        if (newSpot == null)
        {
            canDig = true;
            yield break; // Exit the coroutine safely
        }

        // Teleport character
        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false; // Temporarily disable to allow movement
            controller.transform.position = newSpot.position; // Move instantly
            controller.enabled = true; // Re-enable after moving
        }
        else
        {
            transform.position = newSpot.position;
        }
        yield return new WaitForSeconds(digCooldown);
        canDig = true;
    }

    public void CheckForRope()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, ropeGrabRadius, ropeLayer);
        
        if (hits.Length > 0)
        {
            Transform nearestRopeSegment = hits[0].transform;

            AttachToRope(nearestRopeSegment);
        }
    }

    private void AttachToRope(Transform ropeSegment)
    {
        if (isOnRope) return;

        // Disable CharacterController
        controller.enabled = false;

        // Enable Rigidbody physics
        rb.isKinematic = false;
        rb.useGravity = false;

        // Attach player to rope segment with FixedJoint
        ropeJoint = gameObject.AddComponent<FixedJoint>();
        ropeJoint.connectedBody = ropeSegment.GetComponent<Rigidbody>();

        // Set anchor point
        ropeAnchorPoint = ropeSegment.position;

        // Set swinging direction
        swingDirection = (transform.position - ropeAnchorPoint).normalized;

        isOnRope = true;

        // Activate swing animation
        spriteAnimator.SetBool("isSwinging", true);
    }

    private void SwingOnRope()
    {
        // Get horizontal input
        float horizontalInput = inputVector.x;

        // Apply swing force based on the horizontal input
        Vector3 swingForce = -swingDirection * horizontalInput * ropeSwingForce;

        // Apply force for swinging
        rb.AddForce(swingForce, ForceMode.Force);

        // Limit max swing speed to avoid excessive swinging
        if (rb.linearVelocity.magnitude > ropeSwingMaxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * ropeSwingMaxSpeed;
        }

    }
    public void DetachFromRope()
    {
        if (!isOnRope) return;

        // Remove the FixedJoint
        Destroy(ropeJoint);
        ropeJoint = null;

        // Re-enable CharacterController
        controller.enabled = true;
        rb.isKinematic = true;
        rb.useGravity = true;

        isOnRope = false;
        spriteAnimator.SetBool("isSwinging", false);
    }

    void Update()
    {

        // Wall detection
        CheckForClimbableWalls();

        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            if (playerVelocity.y < 0) playerVelocity.y = -3f;
            playerVelocity.x = 0;
        }
        if (isOnRope)
        {
            SwingOnRope();
        }
        // Handle movement
        if (canMove)
        {
            ProcessMove(inputVector);
        }

        // Apply gravity or climb
        if (isTouchingClimbable && !isGrounded)
        {
            playerVelocity.y = climbSpeed;
            playerVelocity.x = -wallNormal.x * 2f; // Slight push toward the wall
        }
        else
        {
            playerVelocity.y += Gravity * Time.deltaTime;
        }

        if (currentPlatform != null && isGrounded)
        {
            Vector3 platformMovement = currentPlatform.position - lastPlatformPosition;
            controller.Move(platformMovement);
            lastPlatformPosition = currentPlatform.position;
            wasOnPlatformLastFrame = true;
        }
        // If not grounded but was on platform last frame, detach
        else if (wasOnPlatformLastFrame && !isGrounded)
        {
            currentPlatform = null;
            wasOnPlatformLastFrame = false;
        }

        // Update animator parameters
        float currentMaxSpeed = isSprinting ? SprintSpeed : MoveSpeed;
        spriteAnimator.SetFloat("speed", inputVector.magnitude * currentMaxSpeed);
        spriteAnimator.SetFloat("Yvelocity", playerVelocity.y);
        spriteAnimator.SetBool("isGrounded", isGrounded);

        // Move character
        controller.Move(playerVelocity * Time.deltaTime);
    }
    private void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = new Vector3(input.x, 0, input.y) * (isSprinting ? SprintSpeed : MoveSpeed);

        // flips attackBox based on movement input
        if (input.x < 0)
        {
            spriteRenderer.flipX = true; 
            if (attackBox != null)
            {
                
                attackBox.transform.localScale = new Vector3(-Mathf.Abs(attackBox.transform.localScale.x), attackBox.transform.localScale.y, attackBox.transform.localScale.z);
                attackBox.transform.localPosition = new Vector3(-attackBoxOffset.x, attackBoxOffset.y, attackBoxOffset.z);
            }
        }
        else if (input.x > 0)
        {
            spriteRenderer.flipX = false; 
            if (attackBox != null)
            {
                
                attackBox.transform.localScale = new Vector3(Mathf.Abs(attackBox.transform.localScale.x), attackBox.transform.localScale.y, attackBox.transform.localScale.z);
                attackBox.transform.localPosition = attackBoxOffset;
            }
        }

        if (input.magnitude > 0)
        {
            playerVelocity.x = 0;
            isTouchingClimbable = false;
        }

        controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime);
    }


    private void CheckForClimbableWalls()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, climbableCheckRadius, climbableLayer);
        isTouchingClimbable = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if (hit.moveDirection.y < -0.9f)
        {
            if (hit.gameObject.CompareTag("MovingPlatform"))
            {
                currentPlatform = hit.transform;
                lastPlatformPosition = hit.transform.position;
            }
        }
        
        else if (hit.collider.CompareTag("Stomp"))
        {
            Debug.Log("STOMP DETECTED!");
            Stomp(hit.collider);
        }
        else if (hit.gameObject.CompareTag("Climbable"))
        {
            isTouchingClimbable = true;
            wallNormal = hit.normal;
        }
    }




}