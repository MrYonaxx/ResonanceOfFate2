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
    public class AttackPropertiesDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        AimReticle aimReticle;
        [SerializeField]
        AttackPropertyDrawer propertyDrawerPrefab;
        [SerializeField]
        Transform propertyParent;

        List<AttackPropertyDrawer> propertyDrawer = new List<AttackPropertyDrawer>();

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
        public void Start()
        {
            aimReticle.OnCharge += DrawAttackProperties;
            aimReticle.OnStop += HideAttackProperties;
        }


        public void DrawAttackProperties(int magazineNumber, PlayerCharacter c)
        {
            int indexDraw = 0;
            List<AttackAimProperty> attackProperties = c.CharacterEquipement.AttackProperties; // c'est pas bien à changer
            for(int i = 0; i < attackProperties.Count; i++)
            {
                if(attackProperties[i].CanAddProperty(magazineNumber) == true)
                {
                    if (indexDraw >= propertyDrawer.Count)
                    {
                        propertyDrawer.Add(Instantiate(propertyDrawerPrefab, propertyParent));
                        propertyDrawer[indexDraw].gameObject.SetActive(true);
                    }
                    propertyDrawer[indexDraw].DrawProperty(attackProperties[i].GetLabel(), attackProperties[i].GetLabelValue(magazineNumber));
                    indexDraw += 1;
                }
            }

        }

        public void HideAttackProperties()
        {
            for (int i = 0; i < propertyDrawer.Count; i++)
            {
                propertyDrawer[i].HideProperty();
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace