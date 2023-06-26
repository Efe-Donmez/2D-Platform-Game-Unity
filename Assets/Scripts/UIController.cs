using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static int attackInt = 0;
    public TextMeshProUGUI text;

    private void FixedUpdate()
    {
        text.text = attackInt.ToString();
    }
}
