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
    public class Savepoint: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        SaveLoadManager saveLoadManager;

        [Title("Feedback")]
        [SerializeField]
        ParticleSystem feedbackParticle;

        Animator animFeedback;

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
            animFeedback = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                feedbackParticle.Play();
                animFeedback.SetTrigger("Feedback");
                saveLoadManager.SavePlayerProfile(0);
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace