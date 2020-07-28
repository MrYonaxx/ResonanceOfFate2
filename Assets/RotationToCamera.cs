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

        List<int> defaultChildOrder;
        List<SpriteRenderer> childSprite;
        SpriteRenderer spriteRenderer;
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

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            this.transform.rotation = globalCamera.Rotation();
            //SortOrder();
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

        // c'est soit ça soit des quad
        /*private void SortOrder()
        {
            spriteRenderer.sortingOrder = 9999 - (int)((this.transform.position - globalCamera.Position()).sqrMagnitude * 10);
            for(int i = 0; i < childSprite.Count; i++)
            {
                //childSprite
            }
        }*/

        #endregion

    } 

} // #PROJECTNAME# namespace