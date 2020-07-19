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
    public class PropsDamage: MonoBehaviour, IDamageable, ITargetable
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        CharacterData characterData;

        [SerializeField]
        protected Transform characterCenter;
        public Transform CharacterCenter
        {
            get { return characterCenter; }
        }

        [SerializeField]
        ShakeSprite shakeSprite;

        /*private bool isKnockback;
        public bool IsKnockback { get { return isKnockback; } }*/
        [SerializeField]
        float[] healthPercentage;
        [SerializeField]
        GameObject[] objectAtHealth;

        private bool isDead;
        public bool IsDead { get { return isDead; } }

        private CharacterStatController characterStatController;
        public CharacterStatController CharacterStatController
        {
            get { return characterStatController; }
            set { characterStatController = value; }
        }

        public event HitAction OnHit;
        public event Action OnDead;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        private void Awake()
        {
            if (characterData != null)
                characterStatController = new CharacterStatController(characterData);
        }

        public void Damage(Vector3 pos, AttackData attackData)
        {
            if (isDead == true)
                return;
            DamageMessage msg = attackData.AttackProcessor.ProcessAttack(attackData.AttackDataStat, characterStatController);
            msg.SetDamagePosition(pos);

            if (OnHit != null) OnHit.Invoke(msg);

            shakeSprite.Shake(0.05f, 0.2f);
            if (characterStatController.Hp <= 0)
            {
                //Death(attackData);
                Destroy(this.gameObject);
                return;
            }
            float hpMax = characterStatController.GetHPMax();
            for (int i = 0; i < healthPercentage.Length; i++)
            {
                if ((characterStatController.Hp / hpMax) >= healthPercentage[i])
                {
                    objectAtHealth[i].gameObject.SetActive(true);
                    return;
                }
                else
                {
                    objectAtHealth[i].gameObject.SetActive(false);
                }
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace