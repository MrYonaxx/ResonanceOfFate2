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
    public class CollisionInstantiate: MonoBehaviour, IAttackCollision
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GameObject objectToInstantiate;
        [SerializeField]
        int number;
        [SerializeField]
        Vector3 instantiateOffset;
        [SerializeField]
        float innerRadius;
        [SerializeField]
        float outerRadius;

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
        public void SetAttackData(AttackData attack)
        {
            
        }

        public void Play(Transform target)
        {
            for (int i = 0; i < number; i++)
            {
                Instantiate(objectToInstantiate, new Vector3(Random.Range(innerRadius, outerRadius) * ((Random.Range(0, 2) - 0.5f) * 2f), this.transform.position.y, Random.Range(innerRadius, outerRadius)) * ((Random.Range(0, 2) - 0.5f) * 2f) + instantiateOffset, Quaternion.identity);
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace