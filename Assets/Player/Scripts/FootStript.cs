using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStript : MonoBehaviour
{
     void OnTriggerStay2D(Collider2D other)
    {
                if (other.CompareTag("ground"))
        {

            PlayerController.isJump = false;
            PlayerController.isFall = false;
        }
    }
}
