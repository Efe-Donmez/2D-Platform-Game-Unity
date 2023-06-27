using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;

    public static float  AttackDelay = 0.4f;
    float nextAttackTime = 0;


    public void onAttack(InputAction.CallbackContext context)
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        if (!PlayerController.isJump && Time.time >= nextAttackTime && context.started)
        {


            PlayerController.isAttack = context.ReadValueAsButton();
            UIController.attackInt++;
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Knight>().onHit(20);
            }

            nextAttackTime = Time.time +AttackDelay;
            Invoke("offAttack",AttackDelay);

        }
        else if (context.canceled)
        {
            PlayerController.isAttack = context.ReadValueAsButton();
        }


    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void offAttack(){
        PlayerController.isAttack = false;
    }



}
