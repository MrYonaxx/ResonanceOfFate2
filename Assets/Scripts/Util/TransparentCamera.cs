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
    public class TransparentCamera: MonoBehaviour
    {
        [SerializeField]
        CameraLock cameraLock;


        List<TransparentObject> transparentObjects = new List<TransparentObject>();
        RaycastHit[] hits;
        Transform cameraFocus;
        Transform cameraTarget;
        Vector3 direction;
        int layerMask;

        private void Start()
        {
            layerMask = 1 << 13;
        }

        private void FixedUpdate()
        {
            DesactivateTransparent();
            cameraFocus = cameraLock.GetFocus();
            if (cameraFocus != null)
            {
                direction = cameraFocus.position - transform.position;
                Raycast();
            }

            cameraTarget = cameraLock.GetTarget();
            if (cameraTarget != null)
            {
                direction = cameraTarget.position - transform.position;
                Raycast();
            }
        }

        private void DesactivateTransparent()
        {
            for (int i = 0; i < transparentObjects.Count; i++)
            {
                transparentObjects[i].TransparentOff();
            }
            transparentObjects.Clear();
        }

        private void Raycast()
        {
            hits = Physics.RaycastAll(transform.position, direction, direction.magnitude, layerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                TransparentObject rend = hit.transform.GetComponent<TransparentObject>();
                if (rend)
                {
                    transparentObjects.Add(rend);
                    rend.TransparentOn();
                }
            }
        }

    } 

} // #PROJECTNAME# namespace