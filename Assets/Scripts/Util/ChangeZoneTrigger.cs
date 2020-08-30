/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ChangeZoneTrigger: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        PartyData partyData;
        [SerializeField]
        InputController inputController;
        [SerializeField]
        Animator fade;

        [SerializeField]
        int entranceID = 0;
        [SerializeField]
        string zoneName;

        [FoldoutGroup("Event")]
        [SerializeField]
        UnityEngine.Events.UnityEvent OnSceneLoadEvent;

        bool load = false;

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

        [ExecuteInEditMode]
        private void Reset()
        {
            if(inputController == null)
                inputController = FindObjectOfType<InputController>();
            if (fade == null)
                fade = GameObject.Find("ScreenFade").GetComponent<Animator>();
        }

        [Button]
        private void AutoPopulate()
        {
            if (inputController == null)
                inputController = FindObjectOfType<InputController>();
            if (fade == null)
                fade = GameObject.Find("ScreenFade").GetComponent<Animator>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player" && load == false)
            {
                load = true;
                StartCoroutine(LoadZoneCoroutine(other.GetComponent<CharacterMovement>()));
            }
        }

        private IEnumerator LoadZoneCoroutine(CharacterMovement characterMovement)
        {
            partyData.NextZoneEntrance = entranceID;
            inputController.StopInputAndTime();
            OnSceneLoadEvent.Invoke();
            fade.SetBool("Appear", false);
            if (characterMovement == null) // c'est un peu opti mais bon c pas ouf quand meme
            {
                float t = 0f;
                while (t < 1f)
                {
                    inputController.StopInputAndTime();
                    t += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                float t = 0f;
                while (t < 1f)
                {
                    inputController.StopInputAndTime();
                    t += Time.deltaTime;
                    characterMovement.MoveCharacterWorld(this.transform.up.x, this.transform.up.z);
                    yield return null;
                }
            }
            SceneManager.LoadScene(zoneName);
        }

        public void LoadZone()
        {
            partyData.NextZoneEntrance = entranceID;
            SceneManager.LoadScene(zoneName);
        }

        public void LoadZoneWithFeedback()
        {
            load = true;
            StartCoroutine(LoadZoneCoroutine(null));
        }

        #endregion

    } 

} // #PROJECTNAME# namespace