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
using UnityEngine.UI;
using TMPro;

namespace VoiceActing
{
    public class InteractionObject: MonoBehaviour, IInteraction
    {
        [InfoBox("Necessite le Prefab Canvas Interaction pour fonctionner")]
        [SerializeField]
        protected ControllerConfigurationData control;


        [Title("UI")]
        [SerializeField]
        protected GameObject commandPrompt;
        [SerializeField]
        protected TextMeshProUGUI textButton;
        [SerializeField]
        protected string promptMessage = "Interact";

        protected bool drawButton = false;
        protected bool canInteract = true;

#if UNITY_EDITOR
        [ExecuteInEditMode]
        protected void Awake()
        {
            if(commandPrompt == null)
                commandPrompt = GameObject.Find("CanvasInteract/Parent/ButtonPanel");
            if (textButton == null)
                textButton = GameObject.Find("CanvasInteract/Parent/ButtonPanel/ButtonPanel/TextPrompt").GetComponent<TextMeshProUGUI>();
        }
#endif

        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && canInteract == true)
            {
                other.GetComponent<PlayerCharacter>().Interactions.Add(this);
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerCharacter>().Interactions.Remove(this);
            }
        }


        public virtual void DrawInteract()
        {
            if (drawButton == false && canInteract == true)
            {
                if(commandPrompt.activeInHierarchy == false)
                    commandPrompt.gameObject.SetActive(true);
                textButton.text = promptMessage;
                drawButton = true;
                StopAllCoroutines();
                StartCoroutine(DrawInteractionPrompt());
            }
        }

        protected IEnumerator DrawInteractionPrompt()
        {
            while(drawButton == true)
            {
                drawButton = false;
                yield return new WaitForSeconds(0.2f);
            }
            commandPrompt.gameObject.SetActive(false);
        }

        public virtual void Interact(PlayerCharacter c)
        {
            c.CharacterMovement.SetInput(false);
        }

        public void EndInteract()
        {

        }

    } 

} // #PROJECTNAME# namespace