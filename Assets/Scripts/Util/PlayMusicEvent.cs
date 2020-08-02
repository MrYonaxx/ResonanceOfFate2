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

        public void OnEnable()
        {
            AudioManager.Instance.PlayMusic(normal, battle);
        }

    } 

} // #PROJECTNAME# namespace