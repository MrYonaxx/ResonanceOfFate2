﻿/*****************************************************************
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
        GlobalCamera cameraScreen;
        [SerializeField]
        SpriteRenderer spriteRenderer;

        [Title("Indicateur de direction")]
        [SerializeField]
        Transform directionTransform;
        public Transform DirectionTransform { get { return directionTransform; } }

        /*[SerializeField]
        SpriteRenderer directionSprite;*/
        //[SerializeField]
        Animator directionAnimator = null;

        [Title("Parameter")]
        [SerializeField]
        bool flipScale = false;
        [SerializeField]
        bool animationDirection = false;
        [ShowIf("animationDirection", true)]
        [SerializeField]
        Animator animator;

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
        public void SetDirection(Transform direction)
        {
            //directionTransform.LookAt(target);
            directionTransform.eulerAngles = direction.eulerAngles;
        }

        public void LookAt(Transform target)
        {
            directionTransform.LookAt(target);
            directionTransform.eulerAngles = new Vector3(90, directionTransform.localEulerAngles.y, directionTransform.localEulerAngles.z);
        }

        private void Start()
        {
            right = new Vector3(-spriteRenderer.transform.localScale.x, spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);
            left= new Vector3(spriteRenderer.transform.localScale.x, spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);
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
            float orientation = Vector3.Dot(Vector3.up, Vector3.Cross(a, b));
            // Calcul d'imprécision (Vector3.cross a du mal quand a et b sont colinéaires, donc ça donne des valeurs très petites et après dot donne des -1/1 en boucle en mode possédé
            if (orientation < 0.01f && -0.01f < orientation)
                orientation = 0.01f;
            float sign = Mathf.Sign(orientation);
            finalAngle *= sign;

            if (finalAngle <= 0) 
            {
                //Debug.Log("Ham " + finalAngle);
                //spriteRenderer.transform.localScale = right
                if (flipScale == true)
                {
                    spriteRenderer.transform.localScale = right;
                }
                else if (animationDirection == true)
                {
                    animator.SetBool("Flip", true);
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
                //spriteRendererAction.flipX = true;
            }
            else if (finalAngle >= 0)
            {
                if (flipScale == true)
                {
                    spriteRenderer.transform.localScale = left;
                }
                else if (animationDirection == true)
                {
                    animator.SetBool("Flip", false);
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }

        }

        public void HideDirectionSprite()
        {
            directionTransform.gameObject.SetActive(false);
            //directionSprite.enabled = false;
        }
        public void ShowDirectionSprite()
        {
            directionTransform.gameObject.SetActive(true);
            //directionSprite.enabled = true;
        }
        public void Selected(bool b)
        {
            if (directionAnimator == null)
                directionAnimator = directionTransform.GetComponent<Animator>();
            directionAnimator.SetBool("Selected", b);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace