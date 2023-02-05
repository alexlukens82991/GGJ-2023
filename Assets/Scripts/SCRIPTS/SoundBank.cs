using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LukensUtils;

public class SoundBank : Singleton<SoundBank>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource oneShotAudioSource;
    [SerializeField] private List<AudioClip> m_Clips;

    private void Start()
    {
        PlayStartMenuMusic(true);
    }

    public void PlayOneShot(int audioInt)
    {
        oneShotAudioSource.PlayOneShot(m_Clips[audioInt]);
    }

    public void PlayStartMenuMusic(bool play)
    {
        audioSource.Stop();

        if (play)
        {
            PlayClip(0);
        }
        else
        {
            audioSource.Stop();
        }
       
    }

    public void PlayBattleMusic(bool play)
    {
        audioSource.Stop();
        if (play)
        {
            PlayClip(1);
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void PlayClip(int clipInt)
    {
        audioSource.clip = m_Clips[clipInt];
        audioSource.Play();
    }
}
