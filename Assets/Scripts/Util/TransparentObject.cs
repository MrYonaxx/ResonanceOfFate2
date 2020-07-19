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
    public class TransparentObject: MonoBehaviour
    {
        [SerializeField]
        Material transparentMaterial;

        [SerializeField]
        float time = 2f;
        [SerializeField]
        float alphaValue = 0.4f;

        [SerializeField]
        Renderer objectRenderer;
        [Button]
        private void AutoPopulate()
        {
            objectRenderer = GetComponent<Renderer>();
        }

        Material defaultMaterial;
        bool transparent = false;
        bool active = false;

        Color currentColor;
        Color initialColor;
        Color finalColor;

        float t = 0f;

        public void TransparentOn()
        {
            active = true;
            if (transparent == true)
                return;
            transparent = true;
            t = 0f;
            initialColor = objectRenderer.material.color;
            finalColor = objectRenderer.material.color;
            finalColor.a = alphaValue;
            defaultMaterial = objectRenderer.material;

            StartCoroutine(TransparentCoroutine());
        }

        public void TransparentOff()
        {
            active = false;
        }

        private IEnumerator TransparentCoroutine()
        {
            objectRenderer.material = transparentMaterial;
            while (active == true)
            {
                if (t < 1)
                    t += Time.deltaTime * time;
                else
                    t = 1f;
                currentColor = Color.Lerp(initialColor, finalColor, t);
                objectRenderer.material.color = currentColor;
                yield return null;
            }
            while(t > 0)
            {
                t -= Time.deltaTime * time;
                currentColor = Color.Lerp(initialColor, finalColor, t);
                objectRenderer.material.color = currentColor;
                yield return null;
            }
            objectRenderer.material = defaultMaterial;
            transparent = false;
        }

    } 

} // #PROJECTNAME# namespace