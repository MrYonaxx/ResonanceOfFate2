/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class InteractionLever: InteractionObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        AttackController attackController;
        [SerializeField]
        UnityEvent leverEvent;

        [Title("UI")]
        [SerializeField]
        GameObject textPanel;

        bool anim = true;


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


        public override void Interact(PlayerCharacter c)
        {
            if (canInteract == true)
            {
                base.Interact(c);
                canInteract = false;
                StartCoroutine(LeverCoroutine(c));
            }
        }

        private IEnumerator LeverCoroutine(PlayerCharacter c)
        {
            attackController.StartAnimation();
            while(anim == true)
            {
                yield return null;
            }
            while (!Input.GetButtonDown(control.buttonA))
            {
                yield return null;
            }
            attackController.EndAttack();
            textPanel.gameObject.SetActive(false);
            c.CharacterMovement.SetInput(true);
            leverEvent.Invoke();
        }

        // Call by anim or event
        public void DrawTextbox()
        {
            anim = false;
            textPanel.gameObject.SetActive(true);
        }

        // Call by anim or event
        public void SetLeverActive()
        {
            canInteract = false;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace