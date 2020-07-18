using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing {
    public class BlinkScript : MonoBehaviour
    {
        float flash = 0;
        Renderer renderer;
        private IEnumerator blinkCoroutine;

        // Start is called before the first frame update
        void Start()
        {
            renderer = GetComponent<Renderer>();
            renderer.material.color = new Color(0, 1, 1, 0.5f);
        }

        /*public void Blink(AttackBehavior attack)
        {
            if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine);
            blinkCoroutine = BlinkCoroutine(attack.BlinkTarget, attack.BlinkTargetColor);
            StartCoroutine(blinkCoroutine);
        }*/

        private IEnumerator BlinkCoroutine(float time, Color blinkColor)
        {
            flash = 1;
            float t = 0f;
            renderer.material.SetColor("_Color", blinkColor);
            while (t < time)
            {
                flash = Mathf.Lerp(1, 0, (t / time));
                t += Time.deltaTime;
                renderer.material.SetFloat("_FlashAmount", flash);
                yield return null;
            }
        }
    }
}
