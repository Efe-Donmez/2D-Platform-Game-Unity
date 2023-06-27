using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    Rigidbody2D rb;
    public float hitJump;
    public float hitPush;
    public ParticleSystem bloodEffect;
    public Animator animator;


    private void Start() {
        currentHealth = maxHealth;
         rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if(rb.velocity.x != 0){
            animator.SetBool("isRun",true);
        }else{
            animator.SetBool("isRun",false);
        }
    }


    public void onHit(int value){
        bloodEffect.Play();
        currentHealth -= value;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
        Invoke("resetHitColor", AttackController.AttackDelay/2);
        rb.AddForce(new Vector2(PlayerController.rotateInt * hitPush, hitJump), ForceMode2D.Impulse);
        
        if(currentHealth <1){
            Die();
        }
    }
    void resetHitColor(){
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1f, 1f, 1);
    }
    public void Die(){
        Destroy(gameObject);
    }
}
