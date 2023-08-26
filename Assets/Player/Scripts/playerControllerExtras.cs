using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAdvancedMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    // Movement
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    private bool isRunning = false;

    // Jump
    public float minJumpForce = 5f;
    public float maxJumpForce = 10f;
    bool isJumping = false;

    // Dash
    public float dashSpeed = 15f;
    private bool isDashing = false;
    public float dashDuration = 0.5f;
    private float dashTime;

    // Wall Jump and Slide
    public LayerMask wallLayer;
    public float wallJumpForce = 10f;
    public float wallSlideSpeed = 2f;
    public bool isWallSliding = false;
    public Vector2 wallJumpDirection = new Vector2(1, 2);

    // Jump Buffering
    float jumpBufferCount = 0.2f;
    float currentJumpBuffer = 0;

    // Coyote Time
    float coyoteTime = 0.2f;
    float coyoteCounter = 0;

    // Clamped Fall Speed
    public float maxFallSpeed = -15f;

    private void Update()
    {
        HandleMovement();
        HandleJumpBuffer();
        HandleCoyoteTime();
        HandleDash();
        HandleWallSlide();
    }


    private void FixedUpdate()
    {
        ClampFallSpeed();
    }

    void HandleMovement()
    {
        float speed = isRunning ? runSpeed : walkSpeed;
        rb.velocity = new Vector2(speed * Input.GetAxis("Horizontal"), rb.velocity.y);
    }

    void HandleJumpBuffer()
    {
        if (currentJumpBuffer > 0)
        {
            currentJumpBuffer -= Time.deltaTime;
        }
    }

    void HandleCoyoteTime()
    {
        if (coyoteCounter > 0)
        {
            coyoteCounter -= Time.deltaTime;
        }
    }

    void ClampFallSpeed()
    {
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }

    void HandleDash()
    {
        if (isDashing)
        {
            rb.velocity = new Vector2(dashSpeed * Mathf.Sign(rb.velocity.x), rb.velocity.y);
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;
            }
        }
    }

    void HandleWallSlide()
    {
        isWallSliding = false;

        if (rb.velocity.y < 0 && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, 0), 0.5f, wallLayer);
            if (hit)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
                isWallSliding = true;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Depending on how you set up your Input Actions, 
        // this method might need to handle setting the movement speed and direction.
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRunning = true;
        }
        if (context.canceled)
        {
            isRunning = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && !isDashing)
        {
            isDashing = true;
            dashTime = dashDuration;
        }
    }

    public void OnWallJump(InputAction.CallbackContext context)
    {
        if (isWallSliding && context.started)
        {
            isWallSliding = false;
            Vector2 jumpDirection = new Vector2(-transform.localScale.x, 1).normalized;
            rb.velocity = new Vector2(jumpDirection.x * wallJumpDirection.x * wallJumpForce, wallJumpDirection.y * wallJumpForce);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentJumpBuffer = jumpBufferCount;
        }

        if (context.started && coyoteCounter > 0 && !isJumping)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, minJumpForce);
        }

        if (context.canceled && rb.velocity.y > minJumpForce)
        {
            rb.velocity = new Vector2(rb.velocity.x, minJumpForce);
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            coyoteCounter = coyoteTime;
        }
    }
}
