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
        [SerializeField]
        AudioClip normal;
        [SerializeField]
        AudioClip battle;
        [SerializeField]
        bool battleTrack = false;

        bool wait = false;

        public void OnEnable()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusic(normal, battle);
                AudioManager.Instance.SwitchToBattle(battleTrack);
            }
            else
                wait = true;
        }

        private void Start()
        {
            if (wait == true)
            {
                AudioManager.Instance.PlayMusic(normal, battle);
                AudioManager.Instance.SwitchToBattle(battleTrack);
            }
        }

    } 

} // #PROJECTNAME# namespace