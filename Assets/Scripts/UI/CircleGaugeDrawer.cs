/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CircleGaugeDrawer: MonoBehaviour
    {
        [SerializeField]
        float rotationOffset = -3.5f;
        [SerializeField]
        float fillOffset = 0.02f;

        [Title("Health")]
        [SerializeField]
        Image gauge;
        [SerializeField]
        Image healthBar;
        [SerializeField]
        Image scratchBar;


        public void CreateGauge(float angleMin, float angleMax)
        {
            gauge.rectTransform.anchoredPosition = Vector2.zero;
            gauge.rectTransform.sizeDelta = Vector2.zero;
            gauge.rectTransform.localEulerAngles = new Vector3(0, 0, angleMax);// - ((angleMax - angleMin) * 0.5f));
            gauge.fillAmount = (angleMax - angleMin) / 360f;
        }

        public void DrawHealth(float hp, float hpMax)
        {
            if (hp == 0)
                gauge.gameObject.SetActive(false);
            healthBar.fillAmount = (hp / hpMax) * (gauge.fillAmount - fillOffset);
            scratchBar.rectTransform.localEulerAngles = new Vector3(0, 0, -gauge.fillAmount * 360f);
        }

        public void DrawScratch(float scratch, float hp)
        {
            scratchBar.fillAmount = (scratch / hp) * (gauge.fillAmount - fillOffset);
            scratchBar.fillAmount = Mathf.Clamp(scratchBar.fillAmount, 0, 1f - (scratchBar.rectTransform.localEulerAngles.z / 360f));
        }

    } 

} // #PROJECTNAME# namespace