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
        TriAttackManager triAttackManager;

        [SerializeField]
        GameObject commandPanel;
        [SerializeField]
        GameObject[] commandUI;

        [Title("Specific command")]
        [SerializeField]
        List<GameObject> moveCommand;
        [SerializeField]
        List<GameObject> aimCommand;
        [SerializeField]
        List<GameObject> triAttackCommand;

        int index = 0;
        int show = 0;
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
            show = PlayerPrefs.GetInt("ShowCommand");
            if (show == 1)
                commandPanel.gameObject.SetActive(false);
        }

        public void UpdateCommand(InputState newIndex)
        {
            if (show == 1)
                return;
            if ((int)newIndex == index || (int) newIndex >= commandUI.Length)
                return;
            commandUI[index].SetActive(false);
            index = (int)newIndex;
            commandUI[index].SetActive(true);
            CheckSpecific();
        }

        private void CheckSpecific()
        {

            if(triAttackManager.ResonancePoint == 0 || inputController.canTriAttack == false)
            {
                for (int i = 0; i < triAttackCommand.Count; i++)
                {
                    triAttackCommand[i].SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < triAttackCommand.Count; i++)
                {
                    triAttackCommand[i].SetActive(true);
                }
            }

            if (inputController.canMove == false)
            {
                for (int i = 0; i < moveCommand.Count; i++)
                {
                    moveCommand[i].SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < moveCommand.Count; i++)
                {
                    moveCommand[i].SetActive(true);
                }
            }

            if (inputController.canAim == false)
            {
                for (int i = 0; i < aimCommand.Count; i++)
                {
                    aimCommand[i].SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < aimCommand.Count; i++)
                {
                    aimCommand[i].SetActive(true);
                }
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace