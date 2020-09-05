/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    // Généralement utilisé par les ciné
    public class PlayMusicEvent: MonoBehaviour
    {
        [HorizontalGroup("Normal", LabelWidth = 100)]
        [HideIf("stopMusic")]
        [SerializeField]
        AudioClip normal;

        [HorizontalGroup("Battle", LabelWidth = 100)]
        [HideIf("stopMusic")]
        [SerializeField]
        AudioClip battle;

        [HorizontalGroup("Normal")]
        [HideIf("stopMusic")]
        [SerializeField]
        AudioClip normalLoop;

        [HorizontalGroup("Battle")]
        [HideIf("stopMusic")]
        [SerializeField]
        AudioClip battleLoop;

        [HideIf("stopMusic")]
        [SerializeField]
        bool battleTrack = false;
        [SerializeField]
        bool stopMusic = false;

        bool wait = false;

        public void OnEnable()
        {
            if (AudioManager.Instance != null)
            {
                if (stopMusic == true)
                {
                    AudioManager.Instance.StopMusic(2f);
                    return;
                }
                AudioManager.Instance.PlayMusic(normal, battle, normalLoop, battleLoop);
                AudioManager.Instance.SwitchToBattle(battleTrack);
            }
            else
                wait = true;
        }

        private void Start()
        {
            if (wait == true)
            {
                if (stopMusic == true)
                {
                    AudioManager.Instance.StopMusic(2f);
                    return;
                }
                AudioManager.Instance.PlayMusic(normal, battle, normalLoop, battleLoop);
                AudioManager.Instance.SwitchToBattle(battleTrack);
            }
        }

    } 

} // #PROJECTNAME# namespace