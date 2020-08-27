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
    public class BattleEnemyManager: MonoBehaviour, IListListener<EnemyController>
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("UI")]
        [SerializeField]
        GlobalEnemyList globalEnemyList;
        [SerializeField]
        Transform healthBarCanvas;
        [SerializeField]
        EnemyHealthBar healthBarPrefab;

        [BoxGroup("Debug")]
        [SerializeField]
        List<EnemyController> enemyList = new List<EnemyController>();
        public List<EnemyController> EnemyList
        {
            get { return enemyList; }
        }


        List<IAttackPerformer> attacksQueue = new List<IAttackPerformer>();
        List<IAttackPerformer> attacksInterruptionQueue = new List<IAttackPerformer>();

        [Title("Sound (A dégager)")]
        [SerializeField]
        AudioClip defeatClip;


        List<EnemyHealthBar> enemyHealthBar = new List<EnemyHealthBar>();
        int indexSelection = 0;

        public delegate void EndAttack();
        public delegate void EndAttackInterruption(IAttackPerformer attackPerformer);
        public event EndAttack OnEndAttacks;
        public event EndAttack OnEnemyDefeated;
        public event EndAttack OnWin;

        public event EndAttackInterruption OnEnemyInterruption;
        public event EndAttack OnEndAttackInterruption;

        int navMeshPriority = 1;

        bool interruptAttackInProgress = false;
        private IEnumerator interruptionCoroutine;


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
            enemyHealthBar = new List<EnemyHealthBar>(enemyList.Count);
            globalEnemyList.AddListener(this);
        }
        private void OnDestroy()
        {
            globalEnemyList.RemoveListener(this);
        }

        public void OnListAdd(EnemyController enemy)
        {
            AddEnemy(enemy);
        }
        public void OnListRemove(EnemyController enemy)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if(enemy == enemyList[i])
                    RemoveEnemy(i);
            }
        }


        // Retourne l'ennemy controller correspondant au characterCenter (accessible via ITargetable)
        public EnemyController GetEnemyController(Transform targetCenter)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if(enemyList[i].Enemy.CharacterCenter == targetCenter)
                {
                    return enemyList[i];
                }
            }
            // La target qu'on vise n'est pas un enemy donc null
            return null;
        }

        public void AddEnemy(EnemyController newEnemy)
        {
            enemyList.Add(newEnemy);
            int i = enemyList.Count - 1;
            if (enemyHealthBar.Count < enemyList.Count)
                enemyHealthBar.Add(Instantiate(healthBarPrefab, healthBarCanvas));
            enemyHealthBar[i].SetTarget(enemyList[i].Enemy.HealthBar);
            enemyHealthBar[i].gameObject.SetActive(true);

            enemyList[i].Enemy.CharacterDamage.OnDead += EnemyDefeated;
            enemyList[i].Enemy.CharacterStatController.OnHPChanged += enemyHealthBar[i].HealthGauge.DrawGauge;
            enemyList[i].Enemy.CharacterStatController.OnScratchChanged += enemyHealthBar[i].ScratchGauge.DrawGauge;

            enemyList[i].Enemy.CharacterAction.OnEndAction += ResolveAction;

            enemyList[i].OnAimChanged += enemyHealthBar[i].ActionGauge.DrawGauge;
            enemyList[i].OnAttackSelected += enemyHealthBar[i].DrawTarget;
            enemyList[i].OnAttackCharged += QueueAttack;
            enemyList[i].OnAttackInterrupted += CancelQueue;


            enemyList[i].NavMeshAgent.avoidancePriority = navMeshPriority;
            navMeshPriority += 1;
        }

        public void RemoveEnemy(int i)
        {
            CancelQueue(enemyList[i]);
            enemyList[i].OnAimChanged -= enemyHealthBar[i].ActionGauge.DrawGauge;
            enemyList[i].OnAttackSelected -= enemyHealthBar[i].DrawTarget;
            enemyList[i].Enemy.CharacterStatController.OnHPChanged -= enemyHealthBar[i].HealthGauge.DrawGauge;
            enemyList[i].Enemy.CharacterStatController.OnScratchChanged -= enemyHealthBar[i].ScratchGauge.DrawGauge;
            enemyHealthBar[i].Destroy();
            if (enemyList.Count == 1) // On le fait à 1 pour pouvoir zoomer sur le dernier kill
            {
                OnWin.Invoke();
            }
            enemyList.RemoveAt(i);
            enemyHealthBar.RemoveAt(i);
        }

        public List<EnemyController> GetEnemies()
        {
            return enemyList;
        }


        public void EnemyDefeated()
        {
            //C'est nul mais tant pis
            for(int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].Enemy.CharacterDamage.IsDead == true) 
                {
                    // Unsubscire enemy event
                    AudioManager.Instance.PlaySound(defeatClip);
                    RemoveEnemy(i);
                }
            }
        }














        // ======================================================================
        // Attack section




        public void QueueAttack(IAttackPerformer attacker, bool interruption)
        {
            if (interruption)
            {
                attacksInterruptionQueue.Add(attacker);
                if (interruptionCoroutine == null)
                {
                    interruptionCoroutine = InterruptionCoroutine();
                    StartCoroutine(interruptionCoroutine);
                }
                return;
            }
            attacksQueue.Add(attacker);
        }

        public void CancelQueue(EnemyController attacker, bool interruption)
        {
            if (interruption)
            {
                attacksInterruptionQueue.Remove(attacker);
            }
            else
            {
                attacksQueue.Remove(attacker);
            }
        }

        public void CancelQueue(EnemyController attacker)
        {
            if (attacksInterruptionQueue.Contains(attacker))
            {
                attacksInterruptionQueue.Remove(attacker);
            }
            else
            {
                attacksQueue.Remove(attacker);
            }
        }

        public void ResetQueue()
        {
            attacksQueue.Clear();
            attacksInterruptionQueue.Clear();
            StopAllCoroutines();
        }
        public bool CheckEnemyAttack()
        {
            if (attacksQueue.Count != 0)
            {
                ResolveAction();
                return true;
            }
            return false;
        }

        public void ResolveAction()
        {
            // Si l'attaque précédente était une interruption faut faire autre chose
            // On résout l'action d'une action Interruption
            if(interruptAttackInProgress)
            {
                if(OnEndAttackInterruption != null) OnEndAttackInterruption.Invoke();
                interruptAttackInProgress = false;
                return;
            }

            // On résout l'action d'une action Normal
            if (attacksQueue.Count != 0)
            {
                StartCoroutine(ActionCoroutine());
            }
            else
            {
                OnEndAttacks.Invoke();
            }
        }

        private IEnumerator ActionCoroutine()
        {
            yield return new WaitForSeconds(0.2f);
            attacksQueue[0].PerformAction();
            attacksQueue.RemoveAt(0);
        }



        private IEnumerator InterruptionCoroutine()
        {
            while(attacksInterruptionQueue.Count != 0 && interruptAttackInProgress == false)
            {
                // Check si on peut interrupt
                if (OnEnemyInterruption != null) OnEnemyInterruption.Invoke(attacksInterruptionQueue[0]);
                yield return null;
            }
            interruptionCoroutine = null;
        }

        public void InterruptionAttack()
        {
            attacksInterruptionQueue[0].PerformAction();
            attacksInterruptionQueue.RemoveAt(0);
            interruptAttackInProgress = true;
        }




        public void ShowHealthBars(bool b)
        {
            healthBarCanvas.gameObject.SetActive(b);
        }



        #endregion

    } 

} // #PROJECTNAME# namespace