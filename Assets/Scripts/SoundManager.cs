using System;
using UnityEngine;

[Serializable]
struct AudioStruct
{
    public string key;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioStruct[] audioClips;

    public void PlayAudioClip(string givenKey)
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            if(audioClips[i].key == givenKey)
            {
                audioSource.clip = audioClips[i].clip;
                audioSource.Play();
            }
        }
    }
}
