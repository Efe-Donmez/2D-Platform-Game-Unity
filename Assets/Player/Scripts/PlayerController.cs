using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    public float moveSpeed;
    public static int rotateInt;
    public float jumpSpeed;
    public Animator animator;
    public static bool isAttack = false;
    public static bool isJump = false;
    public static bool isFall = false;
    private Vector2 keyInput;
    public static bool isSlide = false;
    public static float health = 100;
    public AudioClip stepSound;
    public ParticleSystem bloadEffect;
    void Start()
    {
        GlobalObjects.playerAudioSource.Play();
        health = PlayerInfos.health;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {

        if (transform.rotation.y == 0)
        {
            rotateInt = 1;
        }
        else
        {
            rotateInt = -1;
        }

        if (isSlide && rb.velocity.x == 0)
        {
            offSlide();

        }




        animator.SetBool("isAttack", isAttack);

        if (!isSlide)
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
        }
        if (PlayerInfos.health != health)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
            Invoke("resetHitColor", 0.3f);
            rb.AddForce(new Vector2(PlayerController.rotateInt*-1 * jumpSpeed/2, jumpSpeed/3), ForceMode2D.Impulse);
            bloadEffect.Play();
            health = PlayerInfos.health;
        }

    }

    void resetHitColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1f, 1f, 1);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        keyInput = context.ReadValue<Vector2>();
        if (keyInput.x == 0)
        {
            animator.SetBool("isRun", false);
            pauseSound();
        }
        else
        {
            animator.SetBool("isRun", true);
            Invoke("stepSoundPlay", 0.3f);
        }
        if (isFall && keyInput.y < 0)
        {
            rb.AddForce(new Vector2(0, -20), ForceMode2D.Impulse);
        }
        if (isFall)
        {
            pauseSound();
        }

    }

    void pauseSound()
    {
        GlobalObjects.playerAudioSource.Pause();
    }


    void stepSoundPlay()
    {
        if (rb.velocity.x != 0 && !isFall)
        {
            //GlobalObjects.playerAudioSource.clip = stepSound;
            GlobalObjects.playerAudioSource.Play();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isSlide)
        {
            offSlide();
        }
        if (context.performed && !isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            FallingAnimate(true);
        }
    }
    public void FallingAnimate(bool jumpValue)
    {
        animator.SetBool("isJump", jumpValue);
        isJump = jumpValue;
        if (!jumpValue)
        {
            animator.SetBool("isFall", jumpValue);
            isFall = jumpValue;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            FallingAnimate(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        isJump = true;
        animator.SetBool("isFall", true);
        isFall = true;

    }

    public void OnSlide(InputAction.CallbackContext context)
    {

        if (context.started && !isSlide && PlayerInfos.stamina - 20 >= 0 && !isFall)
        {
            PlayerInfos.stamina -= 20;
            isSlide = true;
            animator.SetBool("isSlide", true);
            rb.AddForce(new Vector2(moveSpeed * 2 * rotateInt, 0), ForceMode2D.Impulse);
            Invoke("offSlide", 0.5f);
        }
    }
    void offSlide()
    {

        isSlide = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
        animator.SetBool("isSlide", false);
    }
}