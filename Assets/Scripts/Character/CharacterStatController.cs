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
    /*public class Status
    {
        public StatusEffectData StatusEffect;
        public List<StatusEffect> StatusController = new List<StatusEffect>();
        public List<StatusEffectUpdate> StatusUpdate = new List<StatusEffectUpdate>();

        public Status(StatusEffectData statusEffectData)
        {
            StatusEffect = statusEffectData;

            for (int i = 0; i < statusEffectData.StatusController.Count; i++)
                StatusController.Add(statusEffectData.StatusController[i].Copy());

            for (int i = 0; i < statusEffectData.StatusUpdates.Count; i++)
                StatusUpdate.Add(statusEffectData.StatusUpdates[i].Copy());
        }
    }*/




    
    [System.Serializable]
    // Contient toute la logique lié aux stats
    public class CharacterStatController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        [SerializeField]
        [HideLabel]
        protected CharacterData characterData;
        public CharacterData CharacterData
        {
            get { return characterData; }
        }



        [SerializeField]
        protected int level = 1;
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public delegate void ActionStat(float stat, float statMax);
        public event ActionStat OnHPChanged;
        public event ActionStat OnScratchChanged;

        [HorizontalGroup("HP", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10, Width = 0.75f)]
        [HideLabel]
        [ProgressBar(0, "GetHPMax", Height = 20, R = 1, B = 0.15f)]
        [Title("HP")]
        [SerializeField]
        protected float hp = 1;
        public float Hp
        {
            get { return hp; }
            set
            {
                hp = Mathf.Clamp(value, 0, GetHPMax());
                if(OnHPChanged != null) OnHPChanged.Invoke(hp, GetHPMax());
            }
        }


        [HorizontalGroup("Scratch Damage", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10, Width = 0.75f)]
        [HideLabel]
        [ProgressBar(0, "Hp", Height = 10)]
        [SerializeField]
        protected float scratch = 0;
        public float Scratch
        {
            get { return scratch; }
            set
            {
                scratch = Mathf.Clamp(value, 0, hp);
                if (OnScratchChanged != null) OnScratchChanged.Invoke(scratch, hp);
            }
        }




        [SerializeField]
        [BoxGroup]
        protected StatController statController;
        public StatController StatController
        {
            get { return statController; }
            set
            {
                statController = value;
            }
        }


        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */
        // En dur parce que flemme (Pardon j'ai échoué)
        public float GetHPMax()
        {
            return statController.GetValue("HPMax");
        }
        public float GetAimSpeed()
        {
            return GetStat("Aim Speed");
        }
        public float GetMinAimDistance()
        {
            return GetStat("Min Aim Distance");
        }

        public float GetMaxAimDistance()
        {
            return GetStat("Max Aim Distance");
        }
        // En dur parce que flemme (Pardon j'ai échoué)

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */



        public CharacterStatController(CharacterData data)
        {
            characterData = data;
            statController = new StatController(data.StatController);
            hp = GetHPMax();
            scratch = 0;
            level = data.CharacterLevel;
        }









        public float GetAimSpeed(Vector3 user, Vector3 target)
        {
            float distance = (new Vector2(user.x, user.z) - new Vector2(target.x, target.z)).magnitude;
            float multiplier;

            multiplier = 1 - ((distance - GetMinAimDistance()) / (GetMaxAimDistance() - GetMinAimDistance()));
            if (multiplier <= 0.2f)
                multiplier = 0.2f;
            return GetAimSpeed() * multiplier;
        }


        public float GetStat(string statName)
        {
            return statController.GetValue(statName);
        }


        public void AddStat(StatController stat, StatModifierType addType)
        {
            /*if (stat.statModel != statController.statModel)
            {
                Debug.Log("Les deux stats n'ont pas le même modèle, l'opération n'est pas optimisé");
                // faire l'opération pas optimisé
            }*/
            statController.Add(stat, addType);
        }
        public void RemoveStat(StatController stat, StatModifierType addType)
        {
            /*if (stat.statModel != statController.statModel)
            {
                Debug.Log("Les deux stats n'ont pas le même modèle, l'opération n'est pas optimisé");
                // faire l'opération pas optimisé
            }*/
            statController.Remove(stat, addType);
        }


        #endregion

    } 

} // #PROJECTNAME# namespace