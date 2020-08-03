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
    [CreateAssetMenu(fileName = "GlobalCameraData", menuName = "GlobalCameraData", order = 1)]
    public class GlobalCamera: ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Camera cameraDefault;
        /*[SerializeField]
        Vector2 clampRotation = Vector2.zero;*/

        [SerializeField]
        Camera cameraAction;
        /*[SerializeField]
        Vector2 clampRotationAction = Vector2.zero;*/

        [SerializeField]
        AudioListener defaultListener;
        [SerializeField]
        AudioListener actionListener;


        [SerializeField]
        Animator canvasAction;

        Camera cameraActive;
        Vector3 forward = Vector3.forward;
        Quaternion rotation = Quaternion.identity;

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

        public void AssignCamera(Camera camDefault, Camera camAction, Animator canvas)
        {
            cameraDefault = camDefault;
            cameraAction = camAction;
            cameraActive = cameraDefault;
            canvasAction = canvas;
            defaultListener = camDefault.GetComponent<AudioListener>();
            actionListener = camAction.GetComponent<AudioListener>();
        }

        public void ActivateCameraAction(bool b)
        {
            cameraAction.enabled = b;
            actionListener.enabled = b;
            cameraDefault.enabled = !b;
            defaultListener.enabled = !b;
            canvasAction.SetBool("Appear", b);
            if (b == true)
                cameraActive = cameraAction;
            else
                cameraActive = cameraDefault;
        }

        public Transform GetCameraAction()
        {
            return cameraAction.transform;
        }

        public Vector3 Position()
        {
            return cameraActive.transform.position;
        }
        public Vector3 Forward()
        {
            return cameraActive.transform.forward;
        }
        public Vector3 Right()
        {
            return cameraActive.transform.right;
        }
        public Transform TransformDefault()
        {
            return cameraDefault.transform;
        }

        public Quaternion Rotation()
        {
            return cameraActive.transform.rotation;
        }


        public Vector3 WorldToScreenPoint(Vector3 pos)
        {
            return cameraActive.WorldToScreenPoint(pos);
        }


        #endregion

    } 

} // #PROJECTNAME# namespace