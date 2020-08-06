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
    public class BaseInputController: MonoBehaviour
    {

        protected ControllerConfigurationData control;

        [SerializeField]
        protected ControllerConfigurationData controllerConfig;
        [SerializeField]
        protected ControllerConfigurationData controllerConfigPS4;


        protected void Controller()
        {
            control = controllerConfig;
            string[] controllers;
            controllers = Input.GetJoystickNames();
            for (int i = 0; i < controllers.Length; i++)
            {
                Debug.Log(controllers[i]);
                if (controllers[i] == "Wireless Controller")
                {
                    Debug.Log("Aye");
                    control = controllerConfigPS4;
                }
            }
        }

        protected void Start()
        {
            Controller();
        }

    } 

} // #PROJECTNAME# namespace