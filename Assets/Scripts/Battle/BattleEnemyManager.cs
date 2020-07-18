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
    public class BattleEnemyManager: MonoBehaviour, IEnemyListener
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
        [SerializeField]
        TextPopupManager textPopupManager;

        [BoxGroup("Debug")]
        [SerializeField]
        List<EnemyController> enemyList = new List<EnemyController>();

        List<EnemyController> attacksQueue = new List<EnemyController>();

        [Title("Sound (A dégager)")]
        [SerializeField]
        AudioClip defeatClip;

        [Title("Target")]
        [SerializeField]
        CameraLock cameraLock;
        [SerializeField]
        TargetInfoHUD targetHUD;




        List<EnemyHealthBar> enemyHealthBar = new List<EnemyHealthBar>();
        int indexSelection = 0;

        public delegate void EndAttack();
        public event EndAttack OnEndAttacks;
        public event EndAttack OnEnemyDefeated;
        public event EndAttack OnWin;




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

        /*private void Start()
        {
            InitializeEnemies();            
        }

        private void InitializeEnemies()
        {
            enemyHealthBar = new List<EnemyHealthBar>(enemyList.Count);
            for (int i = 0; i < enemyList.Count;i++ )
            {
                enemyHealthBar.Add(Instantiate(healthBarPrefab, healthBarCanvas));
                //AddEnemy(ne)

            }
        }*/



        public void OnTargetAdd(EnemyController enemy)
        {
            AddEnemy(enemy);
        }
        public void OnTargetRemove(EnemyController enemy)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if(enemy == enemyList[i])
                    RemoveEnemy(i);
            }
        }

        public void AddEnemy(EnemyController newEnemy)
        {
            enemyList.Add(newEnemy);
            int i = enemyList.Count - 1;
            if (enemyHealthBar.Count < enemyList.Count)
                enemyHealthBar.Add(Instantiate(healthBarPrefab, healthBarCanvas));
            enemyHealthBar[i].SetTarget(enemyList[i].Enemy.HealthBar);
            enemyHealthBar[i].gameObject.SetActive(true);
            //enemyList[i].Enemy.CharacterDamage.OnHit += textPopupManager.DrawDamagePopup;
            enemyList[i].Enemy.CharacterDamage.OnDead += EnemyDefeated;
            enemyList[i].Enemy.CharacterStatController.OnHPChanged += enemyHealthBar[i].HealthGauge.DrawGauge;
            enemyList[i].Enemy.CharacterStatController.OnScratchChanged += enemyHealthBar[i].ScratchGauge.DrawGauge;

            enemyList[i].Enemy.CharacterAction.OnEndAction += ResolveAction;

            enemyList[i].OnAimChanged += enemyHealthBar[i].ActionGauge.DrawGauge;
            enemyList[i].OnAttackSelected += enemyHealthBar[i].DrawTarget;
            enemyList[i].OnAttackCharged += QueueAttack;
            enemyList[i].OnAttackInterrupted += CancelQueue;
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





        public void QueueAttack(EnemyController attacker)
        {
            attacksQueue.Add(attacker);
        }
        public void CancelQueue(EnemyController attacker)
        {
            attacksQueue.Remove(attacker);
        }

        /*public bool CheckEnemyAttack()
        {
            return (attacksQueue.Count != 0);
        }*/

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












        #endregion

    } 

} // #PROJECTNAME# namespace