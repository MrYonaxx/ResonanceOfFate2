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
    [System.Serializable]
    public class OptionData 
    {
        [HorizontalGroup]
        [HideLabel]
        [SerializeField]
        public TextMeshProUGUI textOptionData;
        [HorizontalGroup(LabelWidth = 150)]
        [SerializeField]
        public bool isSlider;
        [HorizontalGroup]
        [SerializeField]
        public string[] options = { "On", "Off" };

        [ShowIf("isSlider")]
        [HideLabel]
        [SerializeField]
        public Slider sliderOption;
    }

	public class MenuOptions : MenuBase
	{
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        ControllerConfigurationData control;
        [SerializeField]
        MenuItemListDrawer optionsList;




        [Title("Menu Options")]
        [SerializeField]
        int indexFullscreen = 0;
        [SerializeField]
        int indexResolution = 1;
        [SerializeField]
        int indexQuality = 2;

        [SerializeField]
        int indexMusic = 3;
        [SerializeField]
        int indexSound = 4;

        [SerializeField]
        int indexShowCommand = 5;
        [SerializeField]
        int indexLanguage = 6;


        [Title("")]
        [SerializeField]
        OptionData[] optionDatas;

        Resolution[] resolutions;

        bool canInput = false;
        bool dpadDown = false;

        private int[] indexOptions = { 0,0,0,10,10,0,0};
        private int indexCurrentOption;

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

        protected void Awake()
        {
            //base.Start();
            resolutions = Screen.resolutions;
            System.Array.Reverse(resolutions);
            optionDatas[indexResolution].options = new string[resolutions.Length];
            for (int i = 0; i < resolutions.Length; i++)
            {
                optionDatas[indexResolution].options[i] = resolutions[i].width + "x" + resolutions[i].height;
            }

            optionDatas[indexQuality].options = new string[QualitySettings.names.Length];
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                optionDatas[indexQuality].options[i] = QualitySettings.names[i];
            }
            //indexOptions[indexQuality] = QualitySettings.GetQualityLevel();
        }

        private void Start()
        {
            optionsList.OnQuit += CloseMenu;
        }

        public override void OpenMenu()
        {
            base.OpenMenu();
            LoadPlayerOptionData();
            DrawOptionsValue();
        }

        public override void OpenMenuLate()
        {
            canInput = true;
        }

        // Ferme le menu mais ne lance pas l'event
        public override void CloseMenu()
        {
            RewritePlayerPrefs();
            ApplyChange();
            canInput = false;
            base.CloseMenu();

        }

        protected void Update()
        {
            if (canInput == false)
                return;
            if (optionsList.InputList())
                return;

            else if (Input.GetAxis(control.dpadHorizontal) > 0.8 && dpadDown == false)
            {
                dpadDown = true;
                if (optionDatas[optionsList.IndexSelection].isSlider == true)
                    MoveValueSlider(false);
                else
                    MoveValueDropdown(false);
                return;
            }
            else if (Input.GetAxis(control.dpadHorizontal) < -0.8 && dpadDown == false)
            {
                dpadDown = true;
                if (optionDatas[optionsList.IndexSelection].isSlider == true)
                    MoveValueSlider(true);
                else
                    MoveValueDropdown(true);
                return;
            }
            else if (Input.GetAxis(control.dpadHorizontal) == 0 && dpadDown == true)
            {
                dpadDown = false;
                return;
            }

        }




        private void LoadPlayerOptionData()
        {
            indexOptions[indexFullscreen] = PlayerPrefs.GetInt("Fullscreen");
            indexOptions[indexResolution] = PlayerPrefs.GetInt("Resolution");
            indexOptions[indexQuality] = PlayerPrefs.GetInt("Quality");

            indexOptions[indexMusic] = PlayerPrefs.GetInt("MusicVolume");
            indexOptions[indexSound] = PlayerPrefs.GetInt("SoundVolume");

            indexOptions[indexShowCommand] = PlayerPrefs.GetInt("ShowCommand");
            indexOptions[indexLanguage] = PlayerPrefs.GetInt("Language");
        }

        private void DrawOptionsValue()
        {
            for(int i = 0; i < indexOptions.Length; i++)
            {
                if (optionDatas[i].isSlider == true)
                {
                    optionDatas[i].textOptionData.text = indexOptions[i].ToString();
                    optionDatas[i].sliderOption.value = indexOptions[i];
                }
                else
                {
                    optionDatas[i].textOptionData.text = optionDatas[i].options[indexOptions[i]];
                }
            }
        }

        public void RewritePlayerPrefs()
        {
            PlayerPrefs.SetInt("Fullscreen", indexOptions[indexFullscreen]);
            PlayerPrefs.SetInt("Resolution", indexOptions[indexResolution]);
            PlayerPrefs.SetInt("Quality", indexOptions[indexQuality]);


            PlayerPrefs.SetInt("MusicVolume", indexOptions[indexMusic]);
            PlayerPrefs.SetInt("SoundVolume", indexOptions[indexSound]);


            PlayerPrefs.SetInt("ShowCommand", indexOptions[indexShowCommand]);
            PlayerPrefs.SetInt("Language", indexOptions[indexLanguage]);
        }

        public void ApplyChange()
        {
            LoadPlayerOptionData();
            ActivateFullscreen();
            ChangeResolution();
            ChangeMusicVolume();
            ChangeSoundVolume();
            ChangeLanguage();
            ChangeQuality();
        }









        public void MoveValueDropdown(bool left)
        {
            indexCurrentOption = indexOptions[optionsList.IndexSelection];
            if (left == true)
            {
                indexCurrentOption -= 1;
                if (indexCurrentOption == -1)
                    indexCurrentOption = optionDatas[optionsList.IndexSelection].options.Length - 1;
            }
            else
            {
                indexCurrentOption += 1;
                if (indexCurrentOption == optionDatas[optionsList.IndexSelection].options.Length)
                    indexCurrentOption = 0;
            }
            indexOptions[optionsList.IndexSelection] = indexCurrentOption;
            optionDatas[optionsList.IndexSelection].textOptionData.text = optionDatas[optionsList.IndexSelection].options[indexCurrentOption];
        }

        public void MoveValueSlider(bool left)
        {
            indexCurrentOption = (int)optionDatas[optionsList.IndexSelection].sliderOption.value;
            if (left == true)
                optionDatas[optionsList.IndexSelection].sliderOption.value -= 1;
            else
                optionDatas[optionsList.IndexSelection].sliderOption.value += 1;
            indexOptions[optionsList.IndexSelection] = (int)optionDatas[optionsList.IndexSelection].sliderOption.value;
            optionDatas[optionsList.IndexSelection].textOptionData.text = optionDatas[optionsList.IndexSelection].sliderOption.value.ToString();

            ChangeMusicVolume();
            ChangeSoundVolume();
        }








        public void ActivateFullscreen()
        {
            if(indexOptions[indexFullscreen] == 0)
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            else
                Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        public void ChangeResolution()
        {
            Screen.SetResolution(resolutions[indexOptions[indexResolution]].width, resolutions[indexOptions[indexResolution]].height, Screen.fullScreen);
        }

        public void ChangeQuality()
        {
            QualitySettings.SetQualityLevel(indexOptions[indexQuality]);
        }

        public void ChangeMusicVolume()
        {
            AudioManager.Instance.SetMusicVolume(indexOptions[indexMusic]);
        }
        public void ChangeSoundVolume()
        {
            AudioListener.volume = indexOptions[indexSound] * 0.1f;
            //AudioManager.Instance.SetSoundVolume(indexOptions[indexSound]);
        }

        public void ChangeLanguage()
        {
            //I2.Loc.LocalizationManager.CurrentLanguage = optionDatas[indexLanguage].options[indexOptions[indexLanguage]];
            //I2.Loc.LocalizationManager.LocalizeAll();
        }

        #endregion

    } // MenuOptions class
	
}// #PROJECTNAME# namespace
