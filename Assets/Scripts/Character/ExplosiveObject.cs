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
    public class ExplosiveObject : MonoBehaviour, IDamageable, ITargetable
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

        private CharacterStatController characterStatController;
        public CharacterStatController CharacterStatController
        {
            get { return characterStatController; }
            set { characterStatController = value; }
        }

        [SerializeField]
        private WeaponData explosionAttackData;
        [SerializeField]
        private AttackCollider explosion;

        private bool isDead;
        public bool IsDead { get { return isDead; } }


        public delegate void Action();
        public delegate void HitAction(DamageMessage damageMessage);
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

            //shakeSprite.Shake(0.05f, 0.2f);
            if (characterStatController.Hp <= 0)
            {
                Death();
                //Destroy(this.gameObject);
                return;
            }
        }

        private void Death()
        {
            if(OnDead != null) OnDead.Invoke();
            explosion.gameObject.SetActive(true);
            explosion.SetAttackData(new AttackData(explosionAttackData.AttackProcessor, characterStatController, explosionAttackData));
        }

        #endregion

    } 

} // #PROJECTNAME# namespace