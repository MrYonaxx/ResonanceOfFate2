/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class SkipCutscene: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        ControllerConfigurationData control;

        [SerializeField]
        PlayableDirector playableDirector;
        [SerializeField]
        float timeSkip;

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
        private void Update()
        {
            if (Input.GetButtonDown(control.start))
            {
                playableDirector.time = (timeSkip / 60f);
                this.enabled = false;
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace