using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayClip(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.Play();
    }

    public void PlayRandomClip(AudioClip[] Clips, AudioSource source)
    {
        int randomIndex = Random.Range(0, Clips.Length);

        source.clip = Clips[randomIndex];
        source.Play();
    }
}
