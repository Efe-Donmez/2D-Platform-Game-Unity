using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public float moveSpeed;
    public float jumpSpeed;
    public Animator animator;
    private bool isAttack = false;
    private bool isJump = false;
    private Vector2 keyInput;
    void Start()
    {
    }

    // Update is called once per frame

    private void FixedUpdate()
    {

        rb.velocity = new Vector2(keyInput.x * moveSpeed, rb.velocity.y);

        if (keyInput.x > 0)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (keyInput.x < 0)
        {

            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (keyInput.x == 0) {
            animator.SetBool("isRun",false);
        }
        else {
            animator.SetBool("isRun", true);
        }

        animator.SetBool("isAttack", isAttack);

    }

    public void OnMove(InputAction.CallbackContext context) {
        keyInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJump )
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            FallingAnimate(true);
        }
    }

    public void FallingAnimate(bool jumpValue) {
        animator.SetBool("isJump", jumpValue);
        isJump = jumpValue;
    }


    public void OnAttack(InputAction.CallbackContext context) {
      isAttack =   context.ReadValueAsButton();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.CompareTag("ground") ) {
            FallingAnimate(false);
        }
    }
}
