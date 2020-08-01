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
    public class BattleInitializer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        bool initializeAtStart = true;

        [SerializeField]
        AttackController introCamera;
        [SerializeField]
        GameObject hud = null;
        [SerializeField]
        InputController input = null;

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
            if (initializeAtStart == true)
                StartCoroutine(LoadStart());
        }

        private IEnumerator LoadStart()
        {
            yield return null;
            yield return null;
            InitializeBattle();
        }

        [ContextMenu("Hey")]
        public void InitializeBattle()
        {
            if (introCamera != null)
            {
                introCamera.StartAnimation();
                introCamera.OnEndAction += InitializeBattleForReal;
            }
            else
            {
                InitializeBattleForReal();
            }
        }

        public void InitializeBattleForReal()
        {
            input.InputState = InputState.Default;
            hud.SetActive(true);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace