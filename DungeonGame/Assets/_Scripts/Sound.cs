using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class Sound 
{
    [HideInInspector]
    public AudioSource audioSource;

    public enum AudioTypes { music, audioFX}
    public AudioTypes audioType;

    public AudioClip clip;
    public string clipName;
    public bool isLoop;
    public bool playOnAwake;

    [Range(0, 1)]
    public float volume = 0.5f;
}
