/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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


        [SerializeField]
        UnityEvent battleStartEvent;

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
            hud.SetActive(false);
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
            hud.SetActive(true);
            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.5f);
            input.InputState = InputState.Default;
            battleStartEvent.Invoke();
        }

        #endregion

    } 

} // #PROJECTNAME# namespace