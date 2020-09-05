/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using System.Collections;

namespace VoiceActing
{
    /// <summary>
    /// Definition of the AudioManager class
    /// </summary>
    public class AudioLoopManager : MonoBehaviour
    {

        public void StopAllLoop()
        {
            StopAllCoroutines();
        }

        public void Loop(AudioSource audioSource, AudioClip clip)
        {
            StartCoroutine(CoroutineLoop(audioSource, clip));
        }

        private IEnumerator CoroutineLoop(AudioSource audioSource, AudioClip clip)
        {
            audioSource.loop = false;
            while (audioSource.isPlaying)
                yield return null;
            audioSource.loop = true;
            audioSource.clip = clip;
            audioSource.Play();
        }


    } // AudioManager class

} // #PROJECTNAME# namespace
