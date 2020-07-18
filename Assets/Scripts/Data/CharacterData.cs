using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData", order = 1)]
    public class CharacterData : ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        [HorizontalGroup("CharacterBasic", Width = 96, PaddingLeft = 10)]
        [HideLabel]
        [PreviewField(ObjectFieldAlignment.Left, Height = 96)]
        [SerializeField]
        Sprite characterFace;
        public Sprite CharacterFace
        {
            get { return characterFace; }
        }

        [HorizontalGroup("CharacterBasic", PaddingLeft = 10)]
        [VerticalGroup("CharacterBasic/Right")]
        [SerializeField]
        string characterName;
        public string CharacterName
        {
            get { return characterName; }
        }

        [VerticalGroup("CharacterBasic/Right")]
        [SerializeField]
        int characterLevel;
        public int CharacterLevel
        {
            get { return characterLevel; }
        }

        [SerializeField]
        [HideLabel]
        StatController statController;
        public StatController StatController
        {
            get { return statController; }
        }
        /*[Title("Audio")]
        [SerializeField]
        private AudioClip[] hitVoice;
        public AudioClip[] HitVoice
        {
            get { return hitVoice; }
        }
        [SerializeField]
        private AudioClip[] deadVoice;
        public AudioClip[] DeadVoice
        {
            get { return deadVoice; }
        }*/


        #endregion

    }

} // #PROJECTNAME# namespace
