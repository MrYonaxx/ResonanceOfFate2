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
using UnityEngine.AI;

namespace VoiceActing
{
    [System.Serializable]
    public class EnemyBehaviorPhase
    {
        [SerializeField]
        public EnemyBehavior[] enemyBehaviors;
    }

    public class EnemyController: MonoBehaviour, IAttackPerformer
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Data")]
        [SerializeField]
        GlobalEnemyList enemyList;
        [SerializeField]
        GlobalTimeData timeData;

        [Title("Enemy")]

        [SerializeField]
        Enemy enemy;
        public Enemy Enemy
        {
            get { return enemy; }
        }

        [SerializeField]
        private NavMeshAgent navMeshAgent;
        public NavMeshAgent NavMeshAgent
        {
            get { return navMeshAgent; }
        }


        [SerializeField]
        List<EnemyBehaviorPhase> enemyBehaviors;

        EnemyBehavior currentBehavior;


        Character target;
        public Character Target
        {
            get { return target; }
        }


        bool pause = false;
        bool attackCharged = false;
        bool updateInterruption = false;
        int phase = 0;

        private float aimTime = 0;
        public float AimTime
        {
            get { return aimTime; }
            set { aimTime = Mathf.Clamp(value, 0, 1f);  if(OnAimChanged != null) OnAimChanged.Invoke(aimTime, 1f); }
        }



        public delegate void AimAction(float value, float maxValue);
        public delegate void SelectionAction(Character target, bool interruption);
        public delegate void EnemyAction(EnemyController enemyController, bool interruption);


        public event AimAction OnAimChanged;

        public event SelectionAction OnAttackSelected;
        //public event SelectionAction OnTargetLost;
        public event EnemyAction OnAttackCharged;
        public event EnemyAction OnAttackInterrupted;
        public event EnemyAction OnAttackEnded;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        public bool CanAttackInterruptPlayer()
        {
            return currentBehavior.CanInterruptPlayer();

        }

        public AttackController GetAttack()
        {
            return currentBehavior.GetAttackController();
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        [ExecuteInEditMode]
        private void Reset()
        {
            enemy = GetComponent<Enemy>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }




        bool hasBeenRemoved = false;
        bool applicationQuit = false;
        private void Start()
        {
            enemyList.AddEnemy(this);
            StartCoroutine(SelectAttackStartup());
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


        private IEnumerator SelectAttackStartup()
        {
            yield return null;
            SelectAttack();
        }








        // =============================================================
        // Enemy Logic


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
                if (currentBehavior != null)
                    currentBehavior.ResumeBehavior();
                pause = false;
                return;
            }

            if (currentBehavior == null) // Pas d'attaque donc on va en chercher une 
            {
                SelectAttack();
                return;
            }

            if (timeData.TimeFlow == true)
            {
                AimTime += (currentBehavior.UpdateBehavior(enemy, target, out updateInterruption) * Time.deltaTime) * enemy.CharacterAnimation.GetMotionSpeed();
                if (updateInterruption == true && aimTime < 1f)
                {
                    updateInterruption = false;
                    InterruptBehavior();
                    //ResetBehavior();
                    SelectAttack();
                }

                if (aimTime >= 1f && attackCharged == false)
                {
                    attackCharged = true;
                    if (OnAttackCharged != null) OnAttackCharged.Invoke(this, currentBehavior.CanInterruptPlayer());
                }
                else if (aimTime < 1f && attackCharged == true)
                {
                    attackCharged = false;
                    if (OnAttackInterrupted != null) OnAttackInterrupted.Invoke(this, currentBehavior.CanInterruptPlayer());
                }
            }
        }


        private void SelectAttack()
        {
            if (currentBehavior != null)
                enemy.CharacterStatController.RemoveStat(currentBehavior.GetWeaponData().BaseStat, StatModifierType.Flat);

            EnemyBehavior[] behaviors = enemyBehaviors[phase].enemyBehaviors;
            int indexAttack = Random.Range(0, behaviors.Length);
            for (int i = 0; i < behaviors.Length; i++)
            {
                target = behaviors[indexAttack].SelectTarget(enemy);
                if (target != null)
                {
                    // Pattern sélectionné
                    currentBehavior = behaviors[indexAttack];
                    enemy.CharacterStatController.AddStat(currentBehavior.GetWeaponData().BaseStat, StatModifierType.Flat);
                    if (OnAttackSelected != null) OnAttackSelected.Invoke(target, currentBehavior.CanInterruptPlayer());
                    return;
                }
                indexAttack += 1;
                if (indexAttack >= behaviors.Length)
                    indexAttack = 0;
            }
            // Si on en est là c'est que y'a personne à dérailler
            // Executer le pattern par défaut
        }

        public void PerformAction()
        {
            if(currentBehavior == null) // Pour je ne sais quel raison
            {
                Debug.Log(this.gameObject.name);
                ResetBehavior();
                SelectAttack();
            }
            // J'ai besoin de générer un attack data avec les stats de l'ennemi et les stats de l'arme
            AttackData enemyAttackData = new AttackData(currentBehavior.GetWeaponData().AttackProcessor, enemy.CharacterStatController, currentBehavior.GetWeaponData());

            enemy.CharacterAction.OnEndAction += NextPattern; // ??? Je saîs plus 
            enemy.CharacterAction.Action(enemyAttackData, currentBehavior.GetAttackController(), target.CharacterCenter);

        }

        public void NextPattern()
        {
            enemy.CharacterAction.OnEndAction -= NextPattern; // ??? Je saîs plus 
            if(currentBehavior.EndBehavior(enemy, target) == true)
            {
                ResetBehavior();
                SelectAttack();
            }
            else
            {
                AimTime = 0;
            }

        }





        private void EnemyPause()
        {
            if (pause == false)
            {
                if (currentBehavior != null)
                    currentBehavior.PauseBehavior();
                pause = true;
            }
            if (currentBehavior != null)
                AimTime += currentBehavior.UpdatePausedBehavior(enemy, target) * Time.deltaTime;
        }

        private void ResetBehavior()
        {
            if (currentBehavior != null)
            {
                AimTime = 0;
                enemy.CharacterStatController.RemoveStat(currentBehavior.GetWeaponData().BaseStat, StatModifierType.Flat);
                currentBehavior.InterruptBehavior();
                if (OnAttackEnded != null) OnAttackEnded.Invoke(this, currentBehavior.CanInterruptPlayer());
                currentBehavior = null;
                attackCharged = false;
            }

        }
        public void InterruptBehavior()
        {
            if (currentBehavior != null)
            {
                AimTime = 0;
                enemy.CharacterStatController.RemoveStat(currentBehavior.GetWeaponData().BaseStat, StatModifierType.Flat);
                currentBehavior.InterruptBehavior();
                if (OnAttackInterrupted != null) OnAttackInterrupted.Invoke(this, currentBehavior.CanInterruptPlayer());
                currentBehavior = null;
                attackCharged = false;
            }

        }

        public void ChangePhase(int newPhase)
        {
            phase = newPhase;

        }

        #endregion

    } 

} // #PROJECTNAME# namespace