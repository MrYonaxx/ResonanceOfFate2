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
    // Target Controller pour le joueur
    public class BattleTargetController: MonoBehaviour, IListListener<ITargetable>
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        AimReticle aimReticle;
        [SerializeField]
        CameraLock cameraLock;
        [SerializeField]
        GlobalTargetable globalTargetable;

        [SerializeField]
        List<TargetTag> targetTags = new List<TargetTag>();

        int indexSelection;

        List<ITargetable> targetable = new List<ITargetable>();

        public delegate void TargetAction(ITargetable targetable);
        public event TargetAction OnTargeted;
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

        // Add to list targetable
        // Check if Enemy
        // Check if already in battleList
        // Add
        // 
        private void Awake()
        {
            globalTargetable.AddListener(this);
        }
        private void OnDestroy()
        {
            globalTargetable.RemoveListener(this);
        }

        public void Update()
        {
            /*for(int i = 0; i < globalTargetable.TargetsList.Count; i++)
            {

            }*/
        }

        public void OnListAdd(ITargetable target)
        {
            if (targetable.Count == 0)
                TargetRight();
        }

        public void OnListRemove(ITargetable target)
        {
            Untarget();
        }


        public List<ITargetable> GetTarget()
        {
            return targetable;
        }

        public void AddTargetable()
        {

        }

        /*private void OnTriggerEnter(Collider collision)
        {
            if (CheckTag(collision) == true)
            {
                Debug.Log("Bonjour : " + collision.gameObject);
                targetable.Add(collision.gameObject.GetComponent<ITargetable>());
                if (targetable.Count == 1)
                {
                    indexSelection = 0;
                    Target();
                }
            }
        }
        private void OnTriggerExit(Collider collision)
        {
            if (CheckTag(collision) == true)
            {
                Debug.Log("Adieu : " + collision.gameObject.name);
                targetable.Remove(collision.gameObject.GetComponent<ITargetable>());
                if (targetable.Count == 0)
                {
                    indexSelection = 0;
                    //indexSelection = 0;
                    Untarget();
                    return;
                }
                else if (indexSelection >= targetable.Count)
                {
                    indexSelection = targetable.Count-1;
                }
                Target();
            }
        }*/


        private bool CheckTag(Collider collision)
        {
            /*for (int i = 0; i < targetable.Count; i++)
            {
                if(collision.gameObject == targetable[i])
            }*/
            string tag = collision.gameObject.tag;
            for (int i = 0; i < targetTags.Count; i++)
            {
                if (tag == targetTags[i].ToString())
                {
                    return true;
                }
            }
            return false;
        }







        public void TargetLeft()
        {
            targetable = globalTargetable.TargetsList;
            if (targetable.Count == 0)
                return;
            /*if (cameraLock.GetTarget() == null)
            {
                Target();
                return;
            }*/
            indexSelection -= 1;
            if (indexSelection < 0)
                indexSelection = targetable.Count - 1;
            Target();
        }

        public void TargetRight()
        {
            targetable = globalTargetable.TargetsList;
            if (targetable.Count == 0)
                return;
            /*if (cameraLock.GetTarget() == null)
            {
                Target();
                return;
            }*/
            indexSelection += 1;
            if (indexSelection >= targetable.Count)
                indexSelection = 0;
            Target();
        }

        public void Target()
        {
            if (targetable.Count == 0)
            {
                Untarget();
                return;
            }

            aimReticle.SetTarget(targetable[indexSelection].CharacterCenter);
            cameraLock.SetTarget(targetable[indexSelection].CharacterCenter);
            OnTargeted.Invoke(targetable[indexSelection]);
            //targetHUD.DrawTargetInfo(enemyList[indexSelection].Enemy.CharacterStatController);
        }

        public void Untarget()
        {
            aimReticle.SetTarget(null);
            cameraLock.SetTarget(null);
            cameraLock.LockOn(false);
            OnTargeted.Invoke(null);
        }


        public void TargetEnemy()
        {
            TargetRight();
            //cameraLock.SetTarget(targetable[indexSelection].CharacterCenter);
        }



        #endregion

    }

} // #PROJECTNAME# namespace