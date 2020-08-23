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
    public class MenuParty: MenuBase
    {
        [Title("Main Menu")]
        [SerializeField]
        PartyData partyData;
        [SerializeField]
        TypeDictionary weaponDictionary;

        [Title("Parameter")]
        [SerializeField]
        MenuItemListDrawer subMenuSelectionList;
        [SerializeField]
        List<MenuBase> subMenu;


        [Title("UI")]
        [SerializeField]
        TextMeshProUGUI selectionName;
        [SerializeField]
        List<GameObject> characterPanel;
        [SerializeField]
        List<Image> imageCharacterFace;
        [SerializeField]
        List<TextMeshProUGUI> textCharacterName;
        [SerializeField]
        List<TextMeshProUGUI> textCharacterLevel;
        [SerializeField]
        List<Image> imageCharacterWeapon;
        [SerializeField]
        List<GaugeDrawer> gaugeCharacterHP;

        [Space]
        [SerializeField]
        TextMeshProUGUI currentScene;


        private void Start()
        {
            subMenuSelectionList.OnValidate += Validate;
            subMenuSelectionList.OnSelect += DrawTextSelected;
            subMenuSelectionList.OnQuit += CloseMainMenu;
            //OpenMenu();
        }

        public override void OpenMenu()
        {
            base.OpenMenu();
            DrawCharacters();
            currentScene.text = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // c'est pas bon
        }

        public override void OpenMenuLate()
        {
            subMenuSelectionList.SetInput(true);
        }

        // Ferme le menu mais ne lance pas l'event
        public override void CloseMenu()
        {
            subMenuSelectionList.SetInput(false);
            menuPanel.gameObject.SetActive(false);
        }

        // Ferme le menu et lance l'event (généralement l'event de ce menu est celui qui renvois au jeu)
        public void CloseMainMenu()
        {
            subMenuSelectionList.SetInput(false);
            base.CloseMenu();
        }

        public void DrawCharacters()
        {
            for (int i = 0; i < partyData.CharacterStatControllers.Count; i++)
            {
                characterPanel[i].SetActive(true);
                imageCharacterFace[i].sprite = partyData.CharacterStatControllers[i].CharacterData.CharacterFace;
                textCharacterName[i].text = partyData.CharacterStatControllers[i].CharacterData.CharacterName;
                textCharacterLevel[i].text = partyData.CharacterStatControllers[i].Level.ToString();
                imageCharacterWeapon[i].sprite = weaponDictionary.GetSpriteIcon(partyData.CharacterEquipement[i].GetWeaponType());
                gaugeCharacterHP[i].DrawGauge(partyData.CharacterStatControllers[i].Hp, partyData.CharacterStatControllers[i].GetHPMax(), partyData.CharacterStatControllers[i].Hp + " / " + partyData.CharacterStatControllers[i].GetHPMax());
            }

            // C'est nul faire une classe ou un truc
            for (int i = partyData.CharacterStatControllers.Count; i < imageCharacterFace.Count; i++)
            {
                characterPanel[i].SetActive(false);
            }
        }

        private void DrawTextSelected(int index)
        {
            selectionName.text = subMenuSelectionList.ListItem[index].GetText();
        }


        public void Validate(int index)
        {
            if (subMenu[index] == null ||index >= subMenu.Count)
                return;
            subMenu[index].OpenMenu();
            subMenu[index].OnQuit += OpenMenu;
            CloseMenu();
        }

    } 

} // #PROJECTNAME# namespace