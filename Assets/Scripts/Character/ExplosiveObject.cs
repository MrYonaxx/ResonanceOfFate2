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
        WeaponData explosionAttackData;

        [Title("Targetable")]
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
        private Transform targetDirection;
        public Transform TargetDirection
        {
            get { return targetDirection; }
        }


        [Title("Targetable")]
        [SerializeField]
        private Animator explosionAnimation;
        [SerializeField]
        private AttackCollider explosion;

        private bool isDead;
        public bool IsDead { get { return isDead; } }


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
            if (characterStatController.Hp <= 0 || characterStatController.Scratch == characterStatController.Hp)
            {
                characterStatController.Hp = 0;
                Death();
                //Destroy(this.gameObject);
                return;
            }
        }

        private void Death()
        {
            if(OnDead != null) OnDead.Invoke();
            explosionAnimation.enabled = true;
            explosion.SetAttackData(new AttackData(explosionAttackData.AttackProcessor, characterStatController, explosionAttackData));
        }

        #endregion

    } 

} // #PROJECTNAME# namespace