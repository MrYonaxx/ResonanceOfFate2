/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class MenuQuit: MenuBase
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Parameter")]
        [SerializeField]
        MenuItemListDrawer itemListDrawer;

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
            itemListDrawer.OnValidate += Validate;
            itemListDrawer.OnQuit += CloseMenu;
        }

        public override void OpenMenu()
        {
            base.OpenMenu();
        }

        public override void OpenMenuLate()
        {
            itemListDrawer.SetInput(true);
        }

        // Ferme le menu mais ne lance pas l'event
        public override void CloseMenu()
        {
            base.CloseMenu();
            itemListDrawer.SetInput(false);
        }

        public void Validate(int index)
        {
            if(index == 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
            }
            if (index == 1)
            {
                CloseMenu();
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace