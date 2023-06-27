using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObjects : MonoBehaviour
{
    public static AudioSource playerAudioSource = GameObject.Find("Player Audio Source").GetComponent<AudioSource>();
}
