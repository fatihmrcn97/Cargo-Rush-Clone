using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundType
{
    StackGet,
    
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField] private AudioClip[] soundList;
    private AudioSource audioSource;


    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType soundType, float volume)
    {
        Instance.audioSource.PlayOneShot(Instance.soundList[(int)soundType],.5f);
    }
}

