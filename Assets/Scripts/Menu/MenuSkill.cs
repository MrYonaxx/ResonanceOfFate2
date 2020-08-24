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
    public class MenuSkill: MenuBase
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Menu Equipement")]
        [SerializeField]
        ControllerConfigurationData control;
        [SerializeField]
        PartyData partyData;
        [SerializeField]
        TypeDictionary weaponDictionary;

        [Title("Movelist")]
        [SerializeField]
        List<CharacterData> characterDatas;
        [SerializeField]
        List<GameObject> movelist;

        [Title("Weapon Level")]
        [SerializeField]
        Image imageWeaponIcon;
        [SerializeField]
        TextMeshProUGUI textWeaponName;
        [SerializeField]
        TextMeshProUGUI textWeaponLevel;
        [SerializeField]
        TextMeshProUGUI textWeaponNext;

        [Title("Skills")]
        [SerializeField]
        Transform skillListTransform;
        [SerializeField]
        SkillButton skillButton;

        // Faire un stat Drawer si jamais
        [Title("Stat")]
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private List<string> stat;
        [SerializeField]
        List<TextMeshProUGUI> textOldStat;
        //[SerializeField]
        //List<TextMeshProUGUI> textNewStat;
        [SerializeField]
        List<float> statMultiplier; // Pour donner des valeurs "propres" et pas de 1,2 chelou

        [Title("UI")]
        [SerializeField]
        TextMeshProUGUI textCharacterName;
        [SerializeField]
        Image imageCharacterFaceOutline;
        [SerializeField]
        Image imageCharacterFace;

        [Title("Feedback (Note faire plusieurs canvas par menu)")]
        [SerializeField]
        Animator animatorFeedback;

        List<SkillButton> skillButtons = new List<SkillButton>();
        StatController currentCharacterStat;
        int indexCharacterSelection = 0;
        bool canInput = false;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */
#if UNITY_EDITOR
        private static List<string> GetStatList()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterStatData>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("StatDictionary")[0])).StatNames;
        }
#endif
        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */


        public override void OpenMenu()
        {
            base.OpenMenu();
            DrawCharacter();
        }
        public override void OpenMenuLate()
        {
            canInput = true;
        }

        public override void CloseMenu()
        {
            canInput = false;
            base.CloseMenu();

        }


        protected void Update()
        {
            if (canInput == false)
                return;
            if (Input.GetButtonDown(control.buttonB))
            {
                CloseMenu();
                return;
            }
            else if (Input.GetButtonDown(control.buttonRB))
            {
                indexCharacterSelection += 1;
                if (indexCharacterSelection >= partyData.CharacterStatControllers.Count)
                    indexCharacterSelection = 0;
                animatorFeedback.SetTrigger("Feedback");
                DrawCharacter();
                return;
            }
            else if (Input.GetButtonDown(control.buttonLB))
            {
                indexCharacterSelection -= 1;
                if (indexCharacterSelection < 0)
                    indexCharacterSelection = partyData.CharacterStatControllers.Count - 1;
                animatorFeedback.SetTrigger("Feedback");
                DrawCharacter();
                return;
            }

        }

        public void DrawCharacter()
        {
            textCharacterName.text = partyData.CharacterStatControllers[indexCharacterSelection].CharacterData.CharacterName;
            imageCharacterFaceOutline.sprite = partyData.CharacterStatControllers[indexCharacterSelection].CharacterData.CharacterFace;
            imageCharacterFace.sprite = partyData.CharacterStatControllers[indexCharacterSelection].CharacterData.CharacterFace;
            currentCharacterStat = partyData.CharacterStatControllers[indexCharacterSelection].StatController;
            DrawCharacterStat();
            DrawWeaponLevel();
            DrawSkillList();
            for (int i = 0; i < characterDatas.Count; i++)
            {
                if(characterDatas[i] == partyData.CharacterStatControllers[indexCharacterSelection].CharacterData)
                {
                    movelist[i].SetActive(true);
                    continue;
                }
                movelist[i].SetActive(false);
            }

        }
        public void DrawCharacterStat()
        {
            for (int i = 0; i < stat.Count; i++)
            {
                textOldStat[i].text = ((int)(currentCharacterStat.GetValue(stat[i]) * statMultiplier[i])).ToString();
            }
        }


        private void DrawWeaponLevel()
        {
            CharacterEquipementController c = partyData.CharacterEquipement[indexCharacterSelection];
            imageWeaponIcon.sprite = weaponDictionary.GetSpriteIcon(c.GetWeaponType());
            textWeaponName.text = c.GetWeapon().WeaponName;
            textWeaponLevel.text = partyData.CharacterStatControllers[indexCharacterSelection].Level.ToString();
            textWeaponNext.text = c.GetWeaponNextExp().ToString();
        }

        private void DrawSkillList()
        {
            List<AttackAimProperty> skills = partyData.CharacterEquipement[indexCharacterSelection].AttackProperties;
            for (int i = 0; i < skills.Count; i++)
            {
                if (i >= skillButtons.Count)
                    skillButtons.Add(Instantiate(skillButton, skillListTransform));
                skillButtons[i].gameObject.SetActive(true);
                skillButtons[i].DrawButton(skills[i].GetLabel() + skills[i].GetLabelValue(partyData.CharacterStatControllers[indexCharacterSelection].Level).ToString(), 
                                           skills[i].GetChargeByMagazine(partyData.CharacterStatControllers[indexCharacterSelection].Level).ToString());
            }
            for (int i = skills.Count; i < skillButtons.Count; i++)
            {
                skillButtons[i].gameObject.SetActive(false);
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace