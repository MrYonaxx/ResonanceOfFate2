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
    public class BulletDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        TypeDictionary bulletType;

        [SerializeField]
        float offsetX = 10;

        [SerializeField]
        RectTransform panelBullet;
        [SerializeField]
        Image bulletPrefab;

        float width = 0;
        Sprite bulletSprite;
        Color bulletColor;
        List<Image> listBullets = new List<Image>();

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
        public void SetBulletType(int weaponType)
        {
            width = bulletPrefab.rectTransform.sizeDelta.x;
            bulletSprite = bulletType.GetSpriteIcon(weaponType);
            bulletColor = bulletType.GetColorType(weaponType);
        }

        public void DrawBullet(int number)
        {
            if (number == 0)
            {
                return;
            }
            if(listBullets.Count <= number)
                listBullets.Add(Instantiate(bulletPrefab, panelBullet));
            listBullets[number - 1].sprite = bulletSprite;
            listBullets[number - 1].color = bulletColor;
            listBullets[number - 1].rectTransform.sizeDelta = new Vector2(width + offsetX * number, listBullets[number - 1].rectTransform.sizeDelta.y);
            listBullets[number - 1].gameObject.SetActive(true);
        }

        public void DrawAllBullet(int number)
        {
            if (number == 0)
            {
                HideBullet();
                return;
            }
            for (int i = 0; i < number; i++)
            {
                if (listBullets.Count <= number)
                    listBullets.Add(Instantiate(bulletPrefab, panelBullet));
                listBullets[i].sprite = bulletSprite;
                listBullets[i].color = bulletColor;
                listBullets[i].rectTransform.sizeDelta = new Vector2(width + offsetX * number, listBullets[i].rectTransform.sizeDelta.y);
                listBullets[i].gameObject.SetActive(true);
            }
        }

        public void HideBullet()
        {
            for (int i = 0; i < listBullets.Count; i++)
            {
                listBullets[i].gameObject.SetActive(false);
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace