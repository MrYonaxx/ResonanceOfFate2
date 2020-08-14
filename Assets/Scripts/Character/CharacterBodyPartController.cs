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
    /*[System.Serializable]
    public class BodyPart
    {
        protected CharacterData bodyStat;
        [SerializeField]
        protected float angleMin;
        [SerializeField]
        protected float angleMax;
        [SerializeField]
        protected bool aerialPart;
        [SerializeField]
        [ProgressBar(0, 3)]
        protected int layer = 1;

        [SerializeField]
        List<StatModifier> bodyPartBonus;

        [SerializeField]
        protected CharacterStatController statController;

        public virtual bool CheckHit(float angle)
        {
            return (angleMin < angle && angle < angleMax);
        }

        public virtual DamageMessage Damage(AttackData attackData)
        {
            //statController.AddStat()
            return attackData.AttackProcessor.ProcessAttack(attackData.AttackDataStat, statController);
        }

        public virtual bool PartDestroy()
        {
            if (statController.Hp <= 0)
            {
                // On fait des trucs
                return true;
            }
            return false;
        }

    }*/



    public class CharacterBodyPartController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        /*[SerializeField]
        List<Transform> attachementPos;*/

        [SerializeField]
        List<BodyPartController> bodyPart;
        public List<BodyPartController> BodyPart
        {
            get { return bodyPart; }
        }

        Vector2 directionEnemy;
        Vector2 directionPlayer;
        float attackAngle;

        private CharacterStatController mainBodyStatController;
        public CharacterStatController MainBodyStatController
        {
            get { return mainBodyStatController; }
        }

        public event Action OnPartDestroy;

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
        public CharacterBodyPartController(CharacterStatController characterStatController, List<BodyPartController> bodyPartControllers)
        {
            bodyPart = bodyPartControllers;
            mainBodyStatController = characterStatController;
            for(int i = 0; i < bodyPart.Count; i++)
            {
                bodyPart[i].InitializeBodyPart();
            }
        }


        public DamageMessage Damage(AttackData attackData, Transform enemyDirection)
        {
            // Calcul de la position de l'impact
            // -> Si la position est supérieur à l'ennemi, ça touche les partis aerienne en prio, sinon random
            directionEnemy = new Vector2(enemyDirection.up.x, enemyDirection.up.z);
            directionPlayer = new Vector2(enemyDirection.position.x - attackData.AttackDataStat.UserPosition.x, enemyDirection.position.z - attackData.AttackDataStat.UserPosition.z);
            attackAngle = Vector2.SignedAngle(directionEnemy, directionPlayer) + 180f;

            // Check parmis les bodyPart lequel correspond
            DamageMessage damageMessage = new DamageMessage();
            bool hitBody = true;
            for (int i = bodyPart.Count-1; i >= 0; i--)
            {
                if(bodyPart[i].CheckHit(attackAngle) == true)
                {
                    // Enlève des points de vies au bodyPart correspondant
                    hitBody = false;
                    damageMessage = bodyPart[i].Damage(attackData);
                    // Check si la partie est détruite et on la vire de la liste
                    if(bodyPart[i].StatController.Hp <= 0)
                    {
                        bodyPart[i].PartDestroy();
                        bodyPart.RemoveAt(i);
                        if(OnPartDestroy != null) OnPartDestroy.Invoke();
                    }
                    break;
                }
            }
            if(hitBody == true) // On a touché aucune partie donc go taper le body
                damageMessage = attackData.AttackProcessor.ProcessAttack(attackData.AttackDataStat, mainBodyStatController);


            return damageMessage;
        }

        public void DestroyBodyPart(BodyPartController part)
        {
            part.PartDestroy();
            bodyPart.Remove(part);
            if (OnPartDestroy != null) OnPartDestroy.Invoke();

        }

        #endregion

    } 

} // #PROJECTNAME# namespace