using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject enemyObject;
    void  OnTriggerStay2D(Collider2D other)
    {
     if(other.CompareTag("Player")){
         Invoke("attack",0.4f);
     }
    }

    void attack(){
        enemyObject.GetComponent<SwordAttack>().onAttack();
    }

}

