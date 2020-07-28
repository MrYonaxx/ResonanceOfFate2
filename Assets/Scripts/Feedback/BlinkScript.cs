using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing {
    public class BlinkScript : MonoBehaviour
    {
        float flash = 0;
        Renderer renderer;
        private IEnumerator blinkCoroutine;


        [ContextMenu("Blink")]
        public void Blink()
        {
            if(renderer == null)
                renderer = GetComponent<Renderer>();
            if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine);
            blinkCoroutine = BlinkCoroutine(1f, Color.white);
            StartCoroutine(blinkCoroutine);
        }

        private IEnumerator BlinkCoroutine(float time, Color blinkColor)
        {
            flash = 1;
            float t = 0f;
            renderer.material.SetColor("_Color", blinkColor);
            while (t < 1f)
            {
                flash = Mathf.Lerp(1, 0, t);
                t += Time.deltaTime * time;
                renderer.material.SetFloat("_FlashAmount", flash);
                yield return null;
            }
        }
    }
}
