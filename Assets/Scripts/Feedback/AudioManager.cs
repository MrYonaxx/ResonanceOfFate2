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
    public class AudioManager : MonoBehaviour
    {

        [SerializeField]
        private float musicVolumeMax = 1;
        [SerializeField]
        private float soundVolumeMax = 1;
        [SerializeField]
        private float voiceVolumeMax = 1;

        [SerializeField]
        private AudioSource audioMusic;
        [SerializeField]
        private AudioSource audioMusic2;
        [SerializeField]
        private AudioSource audioSound;
        [SerializeField]
        private AudioSource audioVoice;
        [SerializeField]
        private Animator trackManager;


        public static AudioManager Instance;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }



        public AudioSource GetAudioSourceMusic()
        {
            return audioMusic;
        }


        // ===========================================================================================
        // Musique

        public void PlayMusic(AudioClip music, int timeFade = 1)
        {
            if (music == audioMusic.clip)
            {
                audioMusic.volume = musicVolumeMax;
                return;
            }
            audioMusic.clip = music;
            audioMusic.Play();
            StopAllCoroutines();
            StartCoroutine(PlayMusicCoroutine(timeFade));
        }

        public void PlayMusic(AudioClip music, AudioClip musicBattle, int timeFade = 1)
        {
            if (music == audioMusic.clip)
            {
                //audioMusic.volume = musicVolumeMax;
                return;
            }
            audioMusic2.clip = musicBattle;
            audioMusic.clip = music;
            audioMusic.Play();
            audioMusic2.Play();
            StopAllCoroutines();
        }

        private IEnumerator PlayMusicCoroutine(int timeFade)
        {
            if (timeFade <= 0)
                timeFade = 1;
            float speedFade = (musicVolumeMax - audioMusic.volume) / timeFade;
            while (timeFade != 0)
            {
                timeFade -= 1;
                audioMusic.volume += speedFade;
                yield return null;
            }
            audioMusic.volume = musicVolumeMax;
        }




        public void StopMusic(int timeFade = 1)
        {
            StopAllCoroutines();
            StartCoroutine(StopMusicCoroutine(timeFade));
        }

        private IEnumerator StopMusicCoroutine(int timeFade)
        {
            if (timeFade <= 0)
                timeFade = 1;
            float speedFade = audioMusic.volume / timeFade;
            while (timeFade != 0)
            {
                timeFade -= 1;
                audioMusic.volume -= speedFade;
                yield return null;
            }
            audioMusic.volume = 0;
        }



        public void StopMusicWithScratch(int time)
        {
            StopAllCoroutines();
            StartCoroutine(FadeVolumeWithPitch(time));
        }

        private IEnumerator FadeVolumeWithPitch(int time)
        {
            float speed = audioMusic.volume / time;
            audioMusic.pitch += 1;
            while (time != 0)
            {
                time -= 1;
                audioMusic.volume -= speed;
                yield return null;
                if (audioMusic.pitch >= 1)
                    audioMusic.pitch -= 0.01f;
            }
            audioMusic.pitch = 1;
        }

        // ===========================================================================================
        // Son


        public void PlaySound(AudioClip sound, float volumeMultiplier = 1)
        {
            audioSound.PlayOneShot(sound, soundVolumeMax * volumeMultiplier);
        }

        public void PlayVoice(AudioClip sound, float volumeMultiplier = 1)
        {
            audioVoice.PlayOneShot(sound, voiceVolumeMax * volumeMultiplier);
        }



        public void SetMusicVolume(int value)
        {
            musicVolumeMax = (value / 100f);
            audioMusic.volume = musicVolumeMax;
        }

        public void SetSoundVolume(int value)
        {
            soundVolumeMax = (value / 100f);
            audioSound.volume = soundVolumeMax;
        }

        public void SetVoiceVolume(int value)
        {
            voiceVolumeMax = (value / 100f);
        }

        public void SwitchToBattle(bool b)
        {
            trackManager.SetBool("Battle", b);
        }


    } // AudioManager class

} // #PROJECTNAME# namespace