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
        [SerializeField]
        protected ControllerConfigurationData control;

        [SerializeField]
        protected ControllerConfigurationData controllerConfig;
        [SerializeField]
        protected ControllerConfigurationData controllerConfigPS4;


        protected void Controller()
        {
            control.SetConfigurationData(controllerConfig);
            string[] controllers;
            controllers = Input.GetJoystickNames();
            for (int i = 0; i < controllers.Length; i++)
            {
#if UNITY_EDITOR
                Debug.Log(controllers[i]);
#endif
                if (controllers[i] == "Wireless Controller")
                {
                    control.SetConfigurationData(controllerConfigPS4);
                }
            }
        }

        protected virtual void Start()
        {
            Controller();
        }

    } 

} // #PROJECTNAME# namespace