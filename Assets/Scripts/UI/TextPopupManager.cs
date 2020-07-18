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
    public class TextPopupManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GlobalCamera globalCamera;
        [SerializeField]
        TypeDictionary damageType;

        [SerializeField]
        Vector3 randomRange;

        [SerializeField]
        List<TextMeshProUGUI> listPrefab = new List<TextMeshProUGUI>();
        [SerializeField]
        List<RectTransform> listRectTransforms = new List<RectTransform>();

        [SerializeField]
        TextMeshProUGUI popupDamageRaw;
        [SerializeField]
        TextMeshProUGUI popupDamageScratch;
        [SerializeField]
        RectTransform transformDamageRawScratch;

        int index = 0;
        List<float> popupTime = new List<float>();
        List<RectTransform> popupRectTransforms = new List<RectTransform>();
        List<Vector3> popupPosition = new List<Vector3>();

        private IEnumerator popupCoroutine;

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

        public void DrawDamagePopup(DamageMessage damage)
        {
            if (damage.damageRaw > 0 && damage.damageScratch > 0) 
            {
                popupDamageRaw.text = damage.damageRaw.ToString() + "+";
                popupDamageScratch.text = damage.damageScratch.ToString();
                transformDamageRawScratch.gameObject.SetActive(true);
                AddPopup(damage.damagePosition, transformDamageRawScratch, 90f);
                return;
            }
            else if (damage.damageRaw > 0)
                DrawPopup(damage.damagePosition, damage.damageRaw.ToString(), damageType.GetColorType(damage.damageType));
            else if (damage.damageScratch > 0)
                DrawPopup(damage.damagePosition, damage.damageScratch.ToString(), damageType.GetColorType(damage.damageType));
            AddPopup(damage.damagePosition);
        }

        public void DrawPopup(Vector3 pos, string text)
        {
            listPrefab[index].text = text;
            listPrefab[index].color = Color.white;
            listPrefab[index].transform.SetSiblingIndex(listPrefab.Count - 1);
            listRectTransforms[index].gameObject.SetActive(true);
            AddPopup(pos);
        }

        public void DrawPopup(Vector3 pos, string text, Color color)
        {
            listPrefab[index].text = text;
            listPrefab[index].color = color;
            listPrefab[index].transform.SetSiblingIndex(listPrefab.Count - 1);
            listRectTransforms[index].gameObject.SetActive(true);
            AddPopup(pos);
        }


        private void AddPopup(Vector3 pos, RectTransform rectTransform, float time)
        {
            popupTime.Add(time / 60f);
            popupPosition.Add(pos + new Vector3(Random.Range(-randomRange.x, randomRange.x), Random.Range(-randomRange.y, randomRange.y), Random.Range(-randomRange.z, randomRange.z)));
            popupRectTransforms.Add(rectTransform);

            if (popupCoroutine == null)
            {
                popupCoroutine = PopupPositionUpdate();
                StartCoroutine(popupCoroutine);
            }
        }

        private void AddPopup(Vector3 pos)
        {
            popupTime.Add(40 / 60f);
            popupPosition.Add(pos + new Vector3(Random.Range(-randomRange.x, randomRange.x), Random.Range(-randomRange.y, randomRange.y), Random.Range(-randomRange.z, randomRange.z)));
            popupRectTransforms.Add(listRectTransforms[index]);

            index += 1;
            if (index >= listPrefab.Count)
                index = 0;

            if(popupCoroutine == null)
            {
                popupCoroutine = PopupPositionUpdate();
                StartCoroutine(popupCoroutine);
            }
        }

        private IEnumerator PopupPositionUpdate()
        {
            while (popupTime.Count != 0)
            {
                for (int i = 0; i < popupTime.Count; i++)
                {
                    popupTime[i] -= Time.deltaTime;
                    popupRectTransforms[i].anchoredPosition = globalCamera.WorldToScreenPoint(popupPosition[i]);

                    if(popupTime[i] <= 0)
                    {
                        popupRectTransforms[i].gameObject.SetActive(false);
                        popupRectTransforms.RemoveAt(i);
                        popupPosition.RemoveAt(i);
                        popupTime.RemoveAt(i);
                        i -= 1;
                    }
                }
                yield return null;
            }
            popupCoroutine = null;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace