using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;
    public AudioClip missSwordAudio;

    public static float AttackDelay = 0.4f;
    float nextAttackTime = 0;




    public void onAttack(InputAction.CallbackContext context)
    {
        if (!PlayerController.isFall)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            if (!PlayerController.isJump && Time.time >= nextAttackTime -0.2f && context.started)
            {


                PlayerController.isAttack = context.ReadValueAsButton();
                UIController.attackInt++;
                if (hitEnemies.Length != 0)
                {
                    foreach (Collider2D enemy in hitEnemies)
                    {
                        enemy.GetComponent<Knight>().onHit(20);
                        enemy.GetComponent<SwordAttack>().AttackTime -= 0.5f;
                    }
                    nextAttackTime = Time.time + AttackDelay;
                }



                Invoke("offAttack", AttackDelay);


            }
            else if (context.canceled)
            {
                PlayerController.isAttack = context.ReadValueAsButton();
            }
            if (hitEnemies.Length == 0 && Time.time >= nextAttackTime)
            {
                GlobalObjects.playerAudioSource.PlayOneShot(missSwordAudio);
                nextAttackTime = Time.time + AttackDelay;
            }


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
