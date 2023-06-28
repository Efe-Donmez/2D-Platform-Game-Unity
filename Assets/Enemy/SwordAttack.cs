using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;

    public static float AttackDelay = 0.4f;
    float nextAttackTime = 0;

    public float AttackTime;
    public Animator animator;
    public AudioClip hitAudio;


    private void Start()
    {
        AttackTime = Time.time;
    }
    public void onAttack()
    {

        if (!PlayerController.isJump && Time.time > AttackTime + 2)
        {
            AttackTime = Time.time;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            UIController.attackInt++;
            animator.SetTrigger("isAttack");
            foreach (Collider2D enemy in hitEnemies)
            {
                PlayerInfos.health -= 10;
            }

            GlobalObjects.playerAudioSource.PlayOneShot(hitAudio);

            nextAttackTime = Time.time + AttackDelay;
            Invoke("offAttack", AttackDelay);


        }


    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void offAttack()
    {
        PlayerController.isAttack = false;
    }



}
