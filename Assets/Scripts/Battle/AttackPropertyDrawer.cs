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
    public class AttackPropertyDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Animator animator;
        [SerializeField]
        TextMeshProUGUI textLabel;
        [SerializeField]
        TextMeshProUGUI textValue;

        bool show = true;

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
        public void DrawProperty(string label, float value)
        {
            show = true;
            animator.SetBool("Show", show);
            textLabel.text = label;
            textValue.text = value.ToString();
        }

        public void HideProperty()
        {
            show = false;
            animator.SetBool("Show", show);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace