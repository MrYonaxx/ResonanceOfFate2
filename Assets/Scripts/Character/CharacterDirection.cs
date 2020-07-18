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
    public class CharacterDirection: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        SpriteRenderer spriteRenderer;
        [SerializeField]
        Transform directionTransform;
        [SerializeField]
        SpriteRenderer directionSprite;
        [SerializeField]
        Animator directionAnimator;

        [SerializeField]
        GlobalCamera cameraScreen;

        //int direction = -1;
        Vector3 right;
        Vector3 left;
        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        public Transform GetDirection()
        {
            return directionTransform;
        }
        public int GetDirectionInt()
        {
            if (spriteRenderer.flipX == true)
                return -1;
            return 1;
        }
        /*public Transform GetScreen()
        {
            return cameraScreen;
        }*/

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public void LookAt(Transform target)
        {
            directionTransform.LookAt(target);
            directionTransform.eulerAngles = new Vector3(90, directionTransform.localEulerAngles.y, directionTransform.localEulerAngles.z);
        }

        private void Start()
        {
            right = new Vector3(-1, this.transform.localScale.y, this.transform.localScale.z);
            left= new Vector3(-1, this.transform.localScale.y, this.transform.localScale.z);
        }

        private void Update()
        {
            UpdateDirection();
        }

        protected virtual void UpdateDirection()
        {
            var a = directionTransform.up;// * direction;
            var b = cameraScreen.Forward();
            float finalAngle = Vector3.Angle(a, b);
            float sign = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(a, b)));
            finalAngle *= sign;

            if (finalAngle <= 1) 
            {
                //spriteRenderer.transform.localScale = right
                spriteRenderer.flipX = true;
                //spriteRendererAction.flipX = true;
            }
            else if (finalAngle > 1)
            {
                spriteRenderer.flipX = false;
                //spriteRendererAction.flipX = false;
            }

        }

        public void HideDirectionSprite()
        {
            directionSprite.enabled = false;
        }
        public void ShowDirectionSprite()
        {
            directionSprite.enabled = true;
        }
        public void Selected(bool b)
        {
            directionAnimator.SetBool("Selected", b);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace