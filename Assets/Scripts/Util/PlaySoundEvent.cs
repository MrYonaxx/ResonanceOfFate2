using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlaySoundEvent : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;


    [Button]
    private void AutoCompletion()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Used for animation event
    public void PlaySound()
    {
        audioSource.Play();
    }
}
