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

        [Header("Loop")]
        [SerializeField]
        AudioLoopManager loopManager;


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
                SetMusicVolume(PlayerPrefs.GetInt("MusicVolume", 10));
                audioMusic.ignoreListenerVolume = true;
                audioMusic2.ignoreListenerVolume = true;
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

        public void PlayMusic(AudioClip music, AudioClip musicBattle, AudioClip musicLoop, AudioClip musicBattleLoop, float timeFade = 1)
        {
            if (musicLoop == audioMusic.clip)
            {
                audioMusic.volume = musicVolumeMax;
                return;
            }
            audioMusic2.clip = musicBattle;
            audioMusic.clip = music;
            audioMusic.Play();
            audioMusic2.Play();
            StopAllCoroutines();
            PlayMusicCoroutine(timeFade);

            loopManager.StopAllLoop();
            loopManager.Loop(audioMusic, musicLoop);
            loopManager.Loop(audioMusic2, musicBattleLoop);
        }

        private IEnumerator PlayMusicCoroutine(float timeFade)
        {
            if (timeFade <= 0)
                timeFade = 1;
            //float speedFade = (musicVolumeMax - audioMusic.volume) / timeFade;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / timeFade;
                audioMusic.volume = Mathf.Lerp(0, musicVolumeMax, t);
                yield return null;
            }
            audioMusic.volume = musicVolumeMax;
        }




        public void StopMusic(float timeFade = 1)
        {
            StopAllCoroutines();
            StartCoroutine(StopMusicCoroutine(timeFade));
        }

        private IEnumerator StopMusicCoroutine(float timeFade)
        {
            if (timeFade <= 0)
                timeFade = 1;
            float music1Volume = audioMusic.volume;
            float music2Volume = audioMusic2.volume;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / timeFade;
                audioMusic.volume = Mathf.Lerp(music1Volume, 0, t);
                audioMusic2.volume = Mathf.Lerp(music2Volume, 0, t);
                yield return null;
            }
            loopManager.StopAllLoop();
            audioMusic.volume = 0;
            audioMusic.clip = null;
            audioMusic2.volume = 0;
            audioMusic2.clip = null;
        }



        public void StopMusicWithScratch(float time)
        {
            StopAllCoroutines();
            StartCoroutine(StopMusicCoroutine(time));
            StartCoroutine(FadeVolumeWithPitch(time*0.25f));
        }

        private IEnumerator FadeVolumeWithPitch(float time)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / time;
                audioMusic.pitch = Mathf.Lerp(2, 1, t);
                audioMusic2.pitch = Mathf.Lerp(2, 1, t);
                yield return null;
            }
            audioMusic.pitch = 1f;
            audioMusic2.pitch = 1f;
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
            musicVolumeMax = (value / 10f);
            if(audioMusic.volume != 0)
                audioMusic.volume = musicVolumeMax;
            if(audioMusic2.volume != 0)
                audioMusic2.volume = musicVolumeMax;
        }

        public void SetSoundVolume(int value)
        {
            
            soundVolumeMax = (value / 10f);
            audioSound.volume = soundVolumeMax;
        }

        public void SwitchToBattle(bool b)
        {
            //trackManager.SetBool("Battle", b);
            if(b == true)
            {
                StopAllCoroutines();
                StartCoroutine(SwitchMusicTrack(0, 0.25f, 0, 1));
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(SwitchMusicTrack(1, 2, 0.8f, 0));
            }
        }


        private IEnumerator SwitchMusicTrack(float initialWait, float timeFade, float volumeNormal, float volumeBattle)
        {
            yield return new WaitForSeconds(initialWait);
            float music1Volume = audioMusic.volume;
            float music2Volume = audioMusic2.volume;
            volumeNormal *= musicVolumeMax;
            volumeBattle *= musicVolumeMax;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / timeFade;
                audioMusic.volume = Mathf.Lerp(music1Volume, volumeNormal, t);
                audioMusic2.volume = Mathf.Lerp(music2Volume, volumeBattle, t);
                yield return null;
            }
            audioMusic.volume = volumeNormal;
            audioMusic2.volume = volumeBattle;
        }
    } // AudioManager class

} // #PROJECTNAME# namespace