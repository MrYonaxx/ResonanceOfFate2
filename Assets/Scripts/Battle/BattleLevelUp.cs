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
    public class BattleLevelUp: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        BattlePartyManager partyManager;
        [SerializeField]
        BattleEnemyManager enemyManager;

        [Title("Draw")]
        [SerializeField]
        Animator animatorLevelUp = null;
        [SerializeField]
        TextMeshProUGUI textCharacterName = null;

        [SerializeField]
        TextMeshProUGUI textCharacterOldLevel = null;
        [SerializeField]
        TextMeshProUGUI textCharacterNewLevel = null;

        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statHP;

        [SerializeField]
        TextMeshProUGUI textCharacterOldHP = null;
        [SerializeField]
        TextMeshProUGUI textCharacterNewHP = null;

        [SerializeField]
        TextMeshProUGUI textNextExp = null;



#if UNITY_EDITOR
        private static List<string> GetStatList()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterStatData>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("StatDictionary")[0])).StatNames;

        }
#endif

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
            for (int i = 0; i < enemyManager.GetEnemies().Count; i++)
            {
                enemyManager.GetEnemies()[i].Enemy.CharacterDamage.OnHit += GainExp;
            }
        }


        public void GainExp(DamageMessage damageMessage)
        {
            CharacterEquipementController c = partyManager.GetCharacter().CharacterEquipement;
            int oldLevel = c.CharacterStatController.Level; // pas opti
            if (c.GainWeaponExp(damageMessage.exp) == true)
            {
                LevelUp(c, oldLevel);
            }
        }

        public void LevelUp(CharacterEquipementController c, int oldLevel)
        {
            animatorLevelUp.gameObject.SetActive(true);
            textCharacterName.text = c.CharacterStatController.CharacterData.CharacterName;

            StartCoroutine(LevelUpCoroutine(c, oldLevel));
        }

        private IEnumerator LevelUpCoroutine(CharacterEquipementController c, int oldLevel)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(1f);
            StartCoroutine(DrawTextCoroutine(oldLevel, textCharacterOldLevel));
            yield return new WaitForSecondsRealtime(1f);
            StartCoroutine(DrawTextCoroutine(c.CharacterStatController.Level, textCharacterNewLevel));
            yield return new WaitForSecondsRealtime(0.5f);
            StartCoroutine(DrawTextCoroutine((int)c.CharacterGrowthController.GetStatAtLevel(statHP, oldLevel), textCharacterOldHP));
            yield return new WaitForSecondsRealtime(1f);
            StartCoroutine(DrawTextCoroutine((int)c.CharacterStatController.GetStat(statHP), textCharacterNewHP));
            textNextExp.text = c.GetWeaponNextExp().ToString();
            while (true)
            {
                if (Input.GetButtonDown("A"))
                    break;
                yield return null;
            }
            Time.timeScale = 1f;
            animatorLevelUp.gameObject.SetActive(false);
        }

        private IEnumerator DrawTextCoroutine(int targetValue, TextMeshProUGUI text)
        {
            float t = 0f;
            float val = 1f;
            while (t<1f)
            {
                t += Time.unscaledDeltaTime;
                val = Mathf.Lerp(1, targetValue, t);
                text.text = ((int)val).ToString();
                yield return null;
            }
            text.text = targetValue.ToString();
        }


        #endregion

    } 

} // #PROJECTNAME# namespace