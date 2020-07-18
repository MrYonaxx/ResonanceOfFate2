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
    public class ShakeSprite: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        
        Transform spriteTransform;
        private IEnumerator shakeCoroutine;

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
            spriteTransform = GetComponent<Transform>();
        }


        /*public void Shake(AttackBehavior attack)
        {
            if (shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);
            shakeCoroutine = ShakeSpriteCoroutine(attack.TargetShakePower, attack.HitStop);
            StartCoroutine(shakeCoroutine);
        }*/
        public void Shake(float power, float time)
        {
            if (shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);
            shakeCoroutine = ShakeSpriteCoroutine(power, time);
            StartCoroutine(shakeCoroutine);
        }

        public void Shake()
        {
            if (shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);
            shakeCoroutine = ShakeSpriteCoroutine(0.02f, 0.2f);
            StartCoroutine(shakeCoroutine);
        }

        private IEnumerator ShakeSpriteCoroutine(float power, float time)
        {
            float t = 0f;
            while (t < time)
            {
                t += Time.deltaTime;
                if(Time.deltaTime != 0)
                    spriteTransform.localPosition = new Vector3(0 + Random.Range(-power, power), spriteTransform.localPosition.y, spriteTransform.localPosition.z);
                yield return null;
            }
            this.transform.localPosition = new Vector3(0, spriteTransform.localPosition.y, spriteTransform.localPosition.z);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace