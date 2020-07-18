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
    public class EnemyController: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GlobalEnemyList enemyList;
        [SerializeField]
        GlobalTimeData timeData;

        [SerializeField]
        Enemy enemy;
        public Enemy Enemy
        {
            get { return enemy; }
        }

        Character target;

        [SerializeField]
        EnemyBehavior[] enemyBehaviors;


        bool pause = false;
        bool attackCharged = false;
        int indexAttack = -1;

        private float aimTime = 0;
        public float AimTime
        {
            get { return aimTime; }
            set { aimTime = Mathf.Clamp(value, 0, 1f);  if(OnAimChanged != null) OnAimChanged.Invoke(aimTime, 1f); }
        }



        public delegate void AimAction(float value, float maxValue);
        public delegate void SelectionAction(Character target);
        public delegate void EnemyAction(EnemyController enemyController);


        public event AimAction OnAimChanged;
        public event SelectionAction OnAttackSelected;
        public event EnemyAction OnAttackCharged;
        public event EnemyAction OnAttackInterrupted;

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
        bool hasBeenRemoved = false;
        bool applicationQuit = false;
        private void Start()
        {
            enemyList.AddEnemy(this);
        }
        private void OnEnable()
        {
            // Ajoute l'ennemi et le distribue aux listeners qui seront quoi en faire
            if (hasBeenRemoved == true)
            {
                enemyList.AddEnemy(this);
                //hasBeenRemoved = false;
            }
        }
        private void OnDisable()
        {
            if (applicationQuit == true)
                return;
            hasBeenRemoved = true;
            enemyList.RemoveEnemy(this);
        }
        private void OnApplicationQuit()
        {
            applicationQuit = true;
        }







        private void Update()
        {
            if (enemy.CharacterDamage.IsDead == true)
            {
                InterruptBehavior();
                return;
            }
            if (enemy.CharacterDamage.IsKnockback == true)
            {
                InterruptBehavior();
                EnemyPause();
                return;
            }
            else if (enemy.CharacterAction.isAttacking() == true || timeData.TimeFlow == false)
            {
                EnemyPause();
                return;
            }
            else if (timeData.TimeFlow == true && pause == true)
            {
                if (indexAttack >= 0)
                    enemyBehaviors[indexAttack].ResumeBehavior();
                pause = false;
                return;
            }

            if (indexAttack == -1) // Pas d'attaque donc on va en chercher une 
            {
                SelectAttack();
                return;
            }

            if (timeData.TimeFlow == true)
            {
                AimTime += (enemyBehaviors[indexAttack].UpdateBehavior(enemy, target) * Time.deltaTime) * enemy.CharacterAnimation.GetMotionSpeed();
                if (aimTime >= 1f && attackCharged == false)
                {
                    attackCharged = true;
                    if (OnAttackCharged != null) OnAttackCharged.Invoke(this);
                }
                else if (aimTime < 1f && attackCharged == true)
                {
                    attackCharged = false;
                    if (OnAttackInterrupted != null) OnAttackInterrupted.Invoke(this);
                }
            }

        }


        private void SelectAttack()
        {
            // Plein de if hp <  et de check de status
            //indexAttack = Random.Range(0, attackControllers.Length);
            if (indexAttack >= 0)
                enemy.CharacterStatController.RemoveStat(enemyBehaviors[indexAttack].GetWeaponData().BaseStat, StatModifierType.Flat);
            indexAttack = Random.Range(0, enemyBehaviors.Length);
            target = enemyBehaviors[indexAttack].SelectTarget(enemy);
            if (target == null)
            {
                indexAttack = -1;
            }
            else
            {
                enemy.CharacterStatController.AddStat(enemyBehaviors[indexAttack].GetWeaponData().BaseStat, StatModifierType.Flat);
                if (OnAttackSelected != null) OnAttackSelected.Invoke(target);
            }
        }

        public void PerformAction()
        {
            // J'ai besoin de générer un attack data avec les stats de l'ennemi et les stats de l'arme
            AttackData enemyAttackData = new AttackData(enemyBehaviors[indexAttack].GetWeaponData().AttackProcessor, enemy.CharacterStatController, enemyBehaviors[indexAttack].GetWeaponData());

            enemy.CharacterAction.Action(enemyAttackData, enemyBehaviors[indexAttack].GetAttackController(), target.CharacterCenter);
            ResetBehavior();
            SelectAttack();
        }






        private void EnemyPause()
        {
            if (pause == false)
            {
                if (indexAttack >= 0)
                    enemyBehaviors[indexAttack].PauseBehavior();
                pause = true;
            }
        }

        private void ResetBehavior()
        {
            if (indexAttack >= 0)
            {
                AimTime = 0;
                enemy.CharacterStatController.RemoveStat(enemyBehaviors[indexAttack].GetWeaponData().BaseStat, StatModifierType.Flat);
                indexAttack = -1;
                attackCharged = false;
            }

        }
        private void InterruptBehavior()
        {
            if (indexAttack >= 0)
            {
                AimTime = 0;
                enemy.CharacterStatController.RemoveStat(enemyBehaviors[indexAttack].GetWeaponData().BaseStat, StatModifierType.Flat);
                indexAttack = -1;
                attackCharged = false;
                if (OnAttackInterrupted != null) OnAttackInterrupted.Invoke(this);
            }

        }

        #endregion

    } 

} // #PROJECTNAME# namespace