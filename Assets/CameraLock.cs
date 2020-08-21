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

    [System.Serializable]
    public struct CameraState
    {
        [SerializeField]
        [HideLabel]
        public string StateName;
        [SerializeField]
        public Vector3 CameraPosition;
        [SerializeField]
        public Vector3 PivotRotation;
        [SerializeField]
        public float SmoothCamera;
        [SerializeField]
        public Vector3 RotationOffset;
    }


    public class CameraLock: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GlobalCamera globalCamera;
        [SerializeField]
        Camera cameraDefault;
        public Camera CameraDefault
        {
            get { return cameraDefault; }
        }

        [SerializeField]
        Camera cameraAction;
        [SerializeField]
        Animator canvasAction;

        [SerializeField]
        Transform cameraScreen;

        [Space]
        [SerializeField]
        CameraState[] cameraStates;

        [Space]
        [SerializeField]
        Transform focus;


        [SerializeField]
        float smoothCamera = 2;
        [SerializeField]
        float smoothRotation = 2;
        [SerializeField]
        float distanceRatio = 4;

        [SerializeField]
        float horizontalSensibility = 50;
        [SerializeField]
        float verticalSensibility = 50;

        [Header("Debug")]
        [SerializeField]
        bool lockOn = false;
        [SerializeField]
        int stateIndex = 0;

        Vector3 velocity = Vector3.zero;
        Vector3 offsetFocus;

        Transform pivot;
        Transform lockTarget;

        float horizontalAxis = 0;
        float verticalAxis = 0;

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
        private void Awake()
        {
            Application.targetFrameRate = -1;
            globalCamera.AssignCamera(cameraDefault, cameraAction, canvasAction);
        }


        private void Start()
        {
            pivot = this.transform;
        }

        public void SetFocus(Transform newFocus)
        {
            focus = newFocus;
        }

        public Transform GetFocus()
        {
            return focus;
        }




        public void SetTarget(Transform newTarget)
        {
            lockTarget = newTarget;
        }
        // Appelé par des event principalement dans le tuto
        public void SetTargetNull()
        {
            lockTarget = null;
        }

        public Transform GetTarget()
        {
            return lockTarget;
        }

        public void LockOn(bool b, Transform target)
        {
            lockTarget = target;
            LockOn(b);
        }
        public void LockOn(bool b)
        {
            lockOn = b;
            horizontalAxis = 0;
            verticalAxis = 0;
        }

        public void SetState(int stateID)
        {
            stateIndex = stateID;
        }

        public void SetState(string stateName)
        {
            for(int i = 0; i < cameraStates.Length; i++)
            {
                if(cameraStates[i].StateName == stateName)
                {
                    stateIndex = i;
                    return;
                }
            }
        }



        private void Update()
        {
            if (lockOn == true)
            {
                offsetFocus = new Vector3(0, 0, 0);
                UpdatePivotPosition(cameraStates[stateIndex]);
                UpdateCameraPosition(cameraStates[stateIndex]);
                UpdateLockRotation();
            }
            else
            {
                if (lockTarget == null)
                {
                    offsetFocus = new Vector3(0, 0, 0);
                    UpdatePivotPosition(cameraStates[stateIndex]);
                }
                else
                {
                    UpdateLockPosition();
                }
                UpdateCameraPosition(cameraStates[stateIndex]);
                UpdatePivotRotation(cameraStates[stateIndex]);

            }

        }

        private void UpdatePivotPosition(CameraState state)
        {
            pivot.position = Vector3.SmoothDamp(pivot.position, focus.transform.position, ref velocity, state.SmoothCamera);
        }

        private void UpdateCameraPosition(CameraState state)
        {
            cameraScreen.localPosition = Vector3.Lerp(cameraScreen.localPosition, state.CameraPosition + offsetFocus, smoothCamera);
        }
        private void UpdatePivotRotation(CameraState state)
        {
            float x = Mathf.LerpAngle(pivot.localEulerAngles.x, state.PivotRotation.x + horizontalAxis, smoothRotation);
            float y = Mathf.LerpAngle(pivot.localEulerAngles.y, state.PivotRotation.y + verticalAxis, smoothRotation);
            float z = Mathf.LerpAngle(pivot.localEulerAngles.z, state.PivotRotation.z, smoothRotation);
            pivot.localEulerAngles = new Vector3(x, y, z);//.Lerp(pivot.localEulerAngles, state.PivotRotation, smoothCamera);
        }




        private void UpdateLockPosition()
        {
            offsetFocus = new Vector3(0, 0, 0);
            if (Vector3.Distance(focus.position, lockTarget.position) > distanceRatio)
            {
                offsetFocus -= new Vector3(0, 1.5f * ((Vector3.Distance(focus.position, lockTarget.position) - distanceRatio) / distanceRatio), Vector3.Distance(focus.position, lockTarget.position) - distanceRatio);
            }
            pivot.position = Vector3.SmoothDamp(pivot.position, ((focus.position + lockTarget.position) / 2), ref velocity, smoothCamera);
        }

        private void UpdateLockRotation()
        {
           /* Vector3 originalRot = pivot.localEulerAngles;// + cameraStates[stateIndex].RotationOffset;
            pivot.LookAt(lockTarget.position);
            Vector3 newRot = pivot.localEulerAngles;// + cameraStates[stateIndex].RotationOffset;
            pivot.localEulerAngles = originalRot;
            float x = Mathf.LerpAngle(pivot.localEulerAngles.x, newRot.x, smoothRotation);
            float y = Mathf.LerpAngle(pivot.localEulerAngles.y, newRot.y, smoothRotation);
            float z = Mathf.LerpAngle(pivot.localEulerAngles.z, newRot.z, smoothRotation);
            pivot.localEulerAngles = new Vector3(x, y, z);*/

            Quaternion originalRot = pivot.localRotation;
            pivot.LookAt(lockTarget.position);
            Quaternion newRot = Quaternion.Euler(pivot.localEulerAngles.x + cameraStates[stateIndex].RotationOffset.x,
                                                pivot.localEulerAngles.y + cameraStates[stateIndex].RotationOffset.y,
                                                pivot.localEulerAngles.z + cameraStates[stateIndex].RotationOffset.z);
            pivot.localRotation = originalRot;
            pivot.localRotation = Quaternion.RotateTowards(pivot.localRotation, newRot, smoothRotation * 100);
        }





        public void MoveCamera(float valueX, float valueY)
        {
            if (lockOn == true)
                return;
            if (Mathf.Abs(valueX) > 0.5f)
            {
                horizontalAxis += valueX * horizontalSensibility * Time.deltaTime;
                horizontalAxis = Mathf.Clamp(horizontalAxis, -35, 0);
            }
            if (Mathf.Abs(valueY) > 0.5f)
            {
                verticalAxis += valueY * verticalSensibility * Time.deltaTime;
            }
        }
        public void SetCameraAxis(float valueX, float valueY)
        {
            /*if (lockOn == true)
                return;*/
            horizontalAxis = valueX;
            verticalAxis = valueY;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace