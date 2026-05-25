using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [SerializeField] float speed;  
    [SerializeField] public float jumpForce;
    Vector2 moveInput;
    public bool isGrounded;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>()*Time.deltaTime* speed;
    }
    public void Jump(InputAction.CallbackContext context)
    {
         if (context.performed && isGrounded)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        isGrounded = false; // Prevents double jumping
    }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveInput);
    }
    
}
