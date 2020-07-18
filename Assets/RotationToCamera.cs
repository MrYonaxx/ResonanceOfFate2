/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    public class RotationToCamera: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GlobalCamera globalCamera;

        bool constraint = true;

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

        private void Update()
        {
            this.transform.rotation = globalCamera.Rotation();
            //spriteRenderer.sortingOrder = -(int)((cameraScreen.transform.position - this.transform.position).magnitude * 1000);
            if (constraint == true)
            {
                if (this.transform.eulerAngles.x > 180 && this.transform.eulerAngles.x < 350)
                {
                    this.transform.eulerAngles = new Vector3(350, this.transform.eulerAngles.y, 0);
                }
                else if (this.transform.eulerAngles.x > 40 && this.transform.eulerAngles.x < 180)
                {
                    this.transform.eulerAngles = new Vector3(40, this.transform.eulerAngles.y, 0);
                }
                else
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, 0);
            }
        }

        public void RotationConstraint(bool b)
        {
            constraint = b;
        }



        #endregion

    } 

} // #PROJECTNAME# namespace