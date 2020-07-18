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
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class EnemyHealthBar: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GlobalCamera globalCamera;

        [SerializeField]
        GaugeDrawer healthGauge;
        public GaugeDrawer HealthGauge
        {
            get { return healthGauge; }
        }

        [SerializeField]
        GaugeDrawer scratchGauge;
        public GaugeDrawer ScratchGauge
        {
            get { return scratchGauge; }
        }

        [SerializeField]
        GaugeDrawer actionGauge;
        public GaugeDrawer ActionGauge
        {
            get { return actionGauge; }
        }

        [SerializeField]
        TextMeshProUGUI textTarget;

        Transform target;
        RectTransform transformGauge;
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
            transformGauge = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (Vector3.Dot(globalCamera.Forward(), target.position - globalCamera.Position()) >= 0)
            {
                //gameObject.SetActive(true);
                transformGauge.anchoredPosition = globalCamera.WorldToScreenPoint(target.position);
            }
            else
            {
                transformGauge.anchoredPosition = new Vector2(-9999, -9999);
            }
        }



        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        /*public void UpdateGauge(CharacterStatController characterStat)
        {
            healthGauge.DrawHealthGauge(characterStat.GetHP(), characterStat.GetHPMax(), characterStat.GetScratch());
        }*/
        public void DrawTarget(Character c)
        {
            textTarget.text = c.CharacterStatController.CharacterData.CharacterName[0].ToString();
        }


        public void Destroy()
        {
            Destroy(this.gameObject);
        }
        #endregion

    } 

} // #PROJECTNAME# namespace