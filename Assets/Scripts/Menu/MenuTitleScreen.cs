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
    public class MenuTitleScreen: BaseInputController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Animator titleAnimator;
        [SerializeField]
        SaveLoadManager saveLoadManager;
        [SerializeField]
        string startScene;

        bool padDown = false;
        int index = -3;

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
            base.Start();
            StartCoroutine(StartLagCoroutine());
        }

        private IEnumerator StartLagCoroutine()
        {
            yield return new WaitForSeconds(3f);
            index = -2;
        }

        private void Update()
        {
            if (index < 0 && index >= -2)
            {
                if (Input.GetButtonDown(control.buttonA) || Input.GetButtonDown(control.buttonB) || Input.GetButtonDown(control.buttonX) || Input.GetButtonDown(control.buttonY))
                {
                    StartTitle();
                }
            }
            else
            {
                InputPad();
                InputConfirm();
            }
        }

        private void StartTitle()
        {
            if (index == -2)
            {
                titleAnimator.SetTrigger("Skip");
            }
            else if (index == -1)
            {
                titleAnimator.SetTrigger("Menu");
            }
            index += 1;
        }

        private void NewGame()
        {
            saveLoadManager.PartyData.SceneName = startScene;
            StartCoroutine(StartGameCoroutine());
        }


        private void Continue()
        {
            if(saveLoadManager.LoadPlayerProfile(0) == true)
            {
                StartCoroutine(StartGameCoroutine());
            }
        }

        private IEnumerator StartGameCoroutine()
        {
            index = -3;
            titleAnimator.SetTrigger("Disappear");
            AudioManager.Instance.StopMusic(2);
            yield return new WaitForSeconds(3);
            saveLoadManager.LoadScene();
        }

        private void InputPad()
        {
            if (Input.GetAxis(control.dpadVertical) > 0 && padDown == false)
            {
                index += 1;
                if (index >= 2)
                    index = 0;
                padDown = true;
                titleAnimator.SetInteger("Selection", index);
            }
            else if (Input.GetAxis(control.dpadVertical) < 0 && padDown == false)
            {
                index -= 1;
                if (index < 0)
                    index = 1;
                padDown = true;
                titleAnimator.SetInteger("Selection", index);
            }
            else if (Input.GetAxis(control.dpadVertical) == 0 && padDown == true)
                padDown = false;
        }
        private void InputConfirm()
        {
            if (Input.GetButtonDown(control.buttonA))
            {
                if (index == 0)
                    NewGame();
                else if (index == 1)
                    Continue();
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace