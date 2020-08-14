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

        ITargetable currentTarget;
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
            Untarget();
        }



        public void OnListAdd(ITargetable target)
        {
            if (targetable.Count == 0)
            {
                TargetRight();
                cameraLock.LockOn(false);
            }
                //TargetRight();
        }

        public void OnListRemove(ITargetable target)
        {
            if (target == currentTarget) 
            {
                if (cameraLock.CameraDefault.enabled == false) // On est en mode attack donc go rien faire
                {
                    Untarget();
                }
                else
                {
                    TargetNearestEnemy();
                }
            }
        }


        public List<ITargetable> GetTarget()
        {
            return targetable;
        }

        public void AddTargetable()
        {

        }


        /*private bool CheckTag(Collider collision)
        {
            //for (int i = 0; i < targetable.Count; i++)
            //{
            //    if(collision.gameObject == targetable[i])
            //}
            string tag = collision.gameObject.tag;
            for (int i = 0; i < targetTags.Count; i++)
            {
                if (tag == targetTags[i].ToString())
                {
                    return true;
                }
            }
            return false;
        }*/







        public void TargetLeft()
        {
            targetable = globalTargetable.TargetsList;
            if (targetable.Count == 0)
                return;
            CalculateTargetScreenPos(1);
            /*if (cameraLock.GetTarget() == null)
            {
                Target();
                return;
            }*/

            /*indexSelection -= 1;
            if (indexSelection < 0)
                indexSelection = targetable.Count - 1;
            Target();*/
        }

        public void TargetRight()
        {
            targetable = globalTargetable.TargetsList;
            if (targetable.Count == 0)
                return;
            CalculateTargetScreenPos(-1);
            /*if (cameraLock.GetTarget() == null)
            {
                Target();
                return;
            }*/

            /*indexSelection += 1;
            if (indexSelection >= targetable.Count)
                indexSelection = 0;
            Target();*/
        }

        /*public void Target()
        {
            if (targetable.Count == 0)
            {
                Untarget();
                return;
            }

            aimReticle.SetTarget(targetable[indexSelection].CharacterCenter);
            cameraLock.SetTarget(targetable[indexSelection].CharacterCenter);
            if (OnTargeted != null) OnTargeted.Invoke(targetable[indexSelection]);
        }*/

        public void Target(ITargetable newTarget)
        {
            currentTarget = newTarget;
            aimReticle.SetTarget(currentTarget.CharacterCenter);
            cameraLock.SetTarget(currentTarget.CharacterCenter);
            if (OnTargeted != null)
            {
                OnTargeted.Invoke(currentTarget);
            }
        }

        public void Untarget()
        {
            aimReticle.SetTarget(null);
            cameraLock.SetTarget(null);
            cameraLock.LockOn(false);
            if (OnTargeted != null) OnTargeted.Invoke(null);
        }


        public void TargetNearestEnemy()
        {
            targetable = globalTargetable.TargetsListEnemy;
            if(targetable.Count == 0)
            {
                Untarget();
                return;
            }
            float distance = 0;
            float minDistance = 999;
            int bestIndex = 0;
            for (int i = 0; i < targetable.Count; i++)
            {
                distance = Vector3.Distance(targetable[i].CharacterCenter.position, cameraLock.GetFocus().position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    bestIndex = i;
                }
            }
            Target(targetable[bestIndex]);
        }

        private void CalculateTargetDistance()
        {
            //List<float>
        }

        private void CalculateTargetScreenPos(int direction)
        {
            Camera cam = cameraLock.CameraDefault;
            List<float> posX = new List<float>();
            List<ITargetable> finalTarget = new List<ITargetable>();
            bool addInBetween = false;
            float addX = 0;
            targetable = globalTargetable.TargetsList;

            for (int i = 0; i < targetable.Count; i++)
            {
                addX = cam.WorldToScreenPoint(targetable[i].CharacterCenter.position).x;
                addInBetween = false;
                for (int j = 0; j < posX.Count; j++) // On ajoute la position pour avoir une liste croissante
                {
                    if (posX[j] > addX)
                    {
                        posX.Insert(j, addX);
                        finalTarget.Insert(j, targetable[i]);
                        addInBetween = true;
                        break;
                    }
                }
                if (addInBetween == false)
                {
                    posX.Add(addX);
                    finalTarget.Add(targetable[i]);
                }
            }
            int finalIndex = -1;
            for (int i = 0; i < finalTarget.Count; i++)
            {
                if (currentTarget == finalTarget[i])
                {
                    finalIndex = i;
                    break;
                }
            }
            /*if(finalIndex == -1) // Le current target n'est pas à l'écran
            {
                finalIndex += -direction;
            }
            else // Le current target est à l'écran
            {
                finalIndex += direction;
            }*/
            finalIndex += direction;
            if (finalIndex < 0)
                finalIndex = finalTarget.Count - 1;
            else if (finalIndex >= finalTarget.Count)
                finalIndex = 0;
            Target(finalTarget[finalIndex]);

        }

        #endregion

    }

} // #PROJECTNAME# namespace