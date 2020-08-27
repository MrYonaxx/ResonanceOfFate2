/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class GaugeDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Transform gauge;
        /*[SerializeField]
        Transform scratchGauge;*/
        [SerializeField]
        TextMeshProUGUI textGauge;

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
        public void DrawGauge(float amount, float maxAmount)
        {
            DrawGauge(amount, maxAmount, null);
        }
        public void DrawGauge(float amount, float maxAmount, string text)
        {
            maxAmount = Mathf.Max(1, maxAmount);
            gauge.localScale = new Vector3(Mathf.Max(0, amount / maxAmount), 1, 1);
            if (textGauge != null && text != null)
                textGauge.text = text;
            else if (textGauge != null)
                textGauge.text = amount.ToString();
        }



        /*public void DrawHealthGauge(float hp, float hpMax, float scratch = 0)
        {
            gauge.localScale = new Vector3(hp / hpMax, 1, 1);
            scratchGauge.localScale = new Vector3(scratch / hp, 1, 1);
            if (textGauge != null)
                textGauge.text = hp.ToString();
        }*/
        
        #endregion

    } 

} // #PROJECTNAME# namespace