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
    public class MenuBase: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        protected GameObject menuPanel;

        /*public event ItemAction OnSelect;
        public event ItemAction OnValidate;*/
        public event Action OnQuit;

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

        public virtual void OpenMenu()
        {
            menuPanel.gameObject.SetActive(true);
            StartCoroutine(WaitBuffer());
        }

        public virtual void CloseMenu()
        {
            menuPanel.gameObject.SetActive(false);
            if (OnQuit != null) OnQuit.Invoke();

        }

        private IEnumerator WaitBuffer()
        {
            // On attend une frame sinon unity fait l'update de l'autre menu sur la même frame (En gros ça appuie 2x sur le bouton A alors qu'on appuie qu'une fois)
            yield return null;
            OpenMenuLate();
        }

        public virtual void OpenMenuLate()
        {

        }

        #endregion

    } 

} // #PROJECTNAME# namespace