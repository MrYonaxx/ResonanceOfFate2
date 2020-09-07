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
    [ExecuteInEditMode]
    public class MenuStartInput: MonoBehaviour
    {
        [SerializeField]
        ControllerConfigurationData control;
        [SerializeField]
        MenuBase menuParty;
        [SerializeField]
        GameObject canvasMainMenu;

        [SerializeField]
        InputController inputController;
        [SerializeField]
        BattleEnemyManager battleEnemyManager;

        [SerializeField]
        AudioClip menuClip;

        bool canInput = true;


#if UNITY_EDITOR
        private void Awake()
        {
            if (inputController == null)
                inputController = FindObjectOfType<InputController>();
            if (battleEnemyManager == null)
                battleEnemyManager = FindObjectOfType<BattleEnemyManager>();
        }
#endif


        private void Start()
        {
            if (Application.isPlaying)
            {
                menuParty.OnQuit += Quit;
            }
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                if (canInput == false)
                    return;
                if (Input.GetButtonDown(control.start))
                {
                    if (battleEnemyManager.EnemyList.Count == 0 && inputController.InputState == InputState.Default)
                    {
                        inputController.InputState = InputState.NoInput;
                        canvasMainMenu.gameObject.SetActive(true);
                        menuParty.OpenMenu();
                        canInput = false;
                        AudioManager.Instance.PlaySound(menuClip);
                    }
                }
            }
        }

        private void Quit()
        {
            inputController.InputState = InputState.Default;
            canvasMainMenu.gameObject.SetActive(false);
            canInput = true;
        }

    } 

} // #PROJECTNAME# namespace