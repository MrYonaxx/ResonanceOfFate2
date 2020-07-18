﻿/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class EnemyNavmeshController: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        CharacterMovement characterMovement;
        [SerializeField]
        NavMeshAgent navMeshAgent;

        Vector3 destinationVelocity;

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

        private void Start()
        {
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;
        }

        public void MoveToTarget(Transform target)
        {
            navMeshAgent.destination = target.position;
            destinationVelocity = navMeshAgent.desiredVelocity;

            characterMovement.MoveCharacterWorld(destinationVelocity.x, destinationVelocity.z, 0.5f);
            navMeshAgent.velocity = characterMovement.Velocity();
        }

        public void StopMove()
        {
            characterMovement.EndMove();
            navMeshAgent.velocity = Vector3.zero;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace