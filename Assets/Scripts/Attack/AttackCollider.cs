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
    public class AttackCollider: MonoBehaviour, IAttackCollision
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GameObject onHitParticle = null;
        AttackData attackData = null;

        List<ContactPoint> collisionEvents;

        #endregion
        void Awake()
        {
            collisionEvents = new List<ContactPoint>();
        }


        public void SetAttackData(AttackData attack)
        {
            attackData = attack;
        }


        public void Play(Transform target)
        {
            //this.gameObject.SetActive(true)
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag != this.tag)
            {
                //Debug.Log(collider.gameObject.name);
                CheckDamage(collider);
                /*for (int i = 0; i < numCollisionEvents; i++)
                {
                    CheckDamage(collisionEvents[i]);
                }*/
            }
        }


        void CheckDamage(Collider collider)
        {
            var d = collider.gameObject.GetComponent<IDamageable>();
            if (d == null)
                return;
            d.Damage(collider.transform.position, attackData);
        }

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */


        #endregion

    } 

} // #PROJECTNAME# namespace