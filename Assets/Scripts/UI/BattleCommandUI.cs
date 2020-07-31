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
    public class BattleCommandUI: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        InputController inputController;

        [SerializeField]
        GameObject[] commandUI;

        int index = 0;

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
        public void Awake()
        {
            inputController.OnInputStateChanged += UpdateCommand;
        }

        public void UpdateCommand(InputState newIndex)
        {
            if ((int)newIndex == index || (int) newIndex >= commandUI.Length)
                return;
            commandUI[index].SetActive(false);
            index = (int)newIndex;
            commandUI[index].SetActive(true);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace