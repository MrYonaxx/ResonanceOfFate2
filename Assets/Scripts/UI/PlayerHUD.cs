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
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class PlayerHUD: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Animator animator;
        [SerializeField]
        TextMeshProUGUI textPlayerName;
        [SerializeField]
        GaugeDrawer hpGauge;
        public GaugeDrawer HpGauge
        {
            get { return hpGauge; }
        }
        [SerializeField]
        GaugeDrawer scratchGauge;
        public GaugeDrawer ScratchGauge
        {
            get { return scratchGauge; }
        }


        [Title("Weapon")]
        [SerializeField]
        TypeDictionary weaponTypeDatabase;
        [SerializeField]
        Image weaponLayout;
        [SerializeField]
        Image weaponIcon;
        [SerializeField]
        TextMeshProUGUI weaponName;

        [Title("Gauge")]
        [SerializeField]
        TextMeshProUGUI textTriAttackOrder;
        [SerializeField]
        GaugeDrawer gaugeTriAttack;
        public GaugeDrawer GaugeTriAttack
        {
            get { return gaugeTriAttack; }
        }
        [SerializeField]
        GaugeDrawer gaugeMoveTime;
        public GaugeDrawer GaugeMoveTime
        {
            get { return gaugeMoveTime; }
        }


        [Title("TargetMarker")]
        [SerializeField]
        Image targetMarker;
        [SerializeField]
        Color colorNotTargeted;
        [SerializeField]
        Color colorTargeted;
        [SerializeField]
        Color colorTargetedInterrupt;

        List<bool> targetInterrupt = new List<bool>();

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
        public void HudSelected(bool b)
        {
            animator.SetBool("Selected", b);
        }
        public void HudInactive(bool b)
        {
            animator.SetBool("Inactive", b);
        }

        public void DrawCharacter(CharacterStatController stat, CharacterEquipementController characterEquipement)
        {
            textPlayerName.text = stat.CharacterData.CharacterName;
            hpGauge.DrawGauge(stat.Hp, stat.GetHPMax());
            DrawWeapon(characterEquipement.GetWeapon());
            //hpGauge.DrawHealthGauge(stat.Hp, stat.HpMax, stat.Scratch);
        }

        public void DrawWeapon(WeaponData weaponData)
        {
            weaponName.text = weaponData.WeaponName;
            weaponIcon.sprite = weaponTypeDatabase.GetSpriteIcon(weaponData.GetWeaponType());
            weaponLayout.color = weaponTypeDatabase.GetColorType(weaponData.GetWeaponType());
        }

        public void DrawOrder(int i)
        {
            textTriAttackOrder.text = i.ToString();
        }

        public void HideOrder()
        {
            textTriAttackOrder.text = "";
        }

        public void Target(bool interrupt)
        {
            targetInterrupt.Add(interrupt);
            DrawMarker();
        }

        public void Untarget(bool interrupt)
        {
            targetInterrupt.Remove(interrupt);
            DrawMarker();
        }

        public void DrawMarker()
        {
            if(targetInterrupt.Count == 0)
            {
                targetMarker.enabled = false;
            }
            else
            {
                targetMarker.enabled = true;
                for (int i = 0; i < targetInterrupt.Count; i++)
                {
                    if(targetInterrupt[i] == true)
                    {
                        targetMarker.color = colorTargetedInterrupt;
                        return;
                    }
                }
                targetMarker.color = colorTargeted;
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace