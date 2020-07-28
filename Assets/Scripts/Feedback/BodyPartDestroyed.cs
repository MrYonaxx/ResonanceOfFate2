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
    [RequireComponent(typeof(SpriteRenderer))]
    public class BodyPartDestroyed: MonoBehaviour
    {

        [SerializeField]
        SpriteRenderer spriteRenderer;
        [SerializeField]
        float time = 2f;
        [SerializeField]
        Vector3 gravity = new Vector3(0,-0.1f,0);
        [SerializeField]
        Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);

        Color finalColor;
        Vector3 inertia;
        float t = 0f;


        [ExecuteInEditMode]
        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void CreateBodyPart(SpriteRenderer sprite)
        {
            spriteRenderer.sprite = sprite.sprite;
            spriteRenderer.transform.position = sprite.transform.position;
            spriteRenderer.transform.rotation = sprite.transform.rotation;
            spriteRenderer.transform.localScale = sprite.transform.lossyScale;
            inertia = new Vector3(0, 5f, 0);
            finalColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        }

        private void Update()
        {
            inertia += gravity;
            spriteRenderer.transform.position += inertia * Time.deltaTime;
            t += (Time.deltaTime / time);
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, finalColor, (t / time));
            spriteRenderer.transform.localScale += scale;
            if (t > 1f)
                Destroy(this.gameObject);
        }

    } 

} // #PROJECTNAME# namespace