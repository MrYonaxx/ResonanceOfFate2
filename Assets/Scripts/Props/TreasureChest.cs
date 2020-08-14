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
using UnityEditor;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [ExecuteInEditMode]
    public class TreasureChest: InteractionObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Space]
        [Title("Chest")]
        [SerializeField]
        [ReadOnly]
        string chestID = "";
        [SerializeField]
        [ReadOnly]
        GameVariableDatabase chestDatabase;

        [SerializeField]
        PartyData party;
        //[OnValueChanged("CreateChestID")]
        [SerializeField]
        ItemData item;
        [SerializeField]
        Animator chestAnimator;

        [Title("UI")]
        [SerializeField]
        GameObject chestPanel;
        [SerializeField]
        TextMeshProUGUI objectName;
        [SerializeField]
        TextMeshProUGUI objectDescription;

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




        private void CheckOpen()
        {
            if (chestDatabase.GetValue(chestID) == 1)
                canInteract = false;
            else
                canInteract = true;
            DrawChest();
        }

        private void DrawChest()
        {
            chestAnimator.SetBool("Open", !canInteract);
        }


        public override void Interact(PlayerCharacter c)
        {
            if (canInteract == true)
            {
                base.Interact(c);
                chestDatabase.SetValue(chestID, 1);
                canInteract = false;
                DrawChest();
                StartCoroutine(ChestCoroutine(c));
            }
        }

        private IEnumerator ChestCoroutine(PlayerCharacter c)
        {
            yield return new WaitForSeconds(0.5f);

            chestPanel.gameObject.SetActive(true);
            objectName.text = item.ItemName;
            objectDescription.text = item.ItemDescription;

            while (!Input.GetButtonDown(control.buttonA))
            {
                yield return null;
            }

            party.Inventory.Add(item.name);
            chestPanel.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.5f);
            c.CharacterMovement.SetInput(true);
        }




        private void CreateChestID()
        {
            if(chestID != null)
                chestDatabase.RemoveVariable(chestID);
            chestID = "Chest___" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "___" + item.ItemName + "___" + chestDatabase.GameVariables.Count;
            chestDatabase.AddGameVariable(chestID, 0);
        }


        private void Awake()
        {
#if UNITY_EDITOR
            base.Awake();
            if (chestPanel == null) // Ouch
                chestPanel = GameObject.Find("CanvasInteract/Parent/PanelTreasure");
            if (objectName == null) // Ouch
                objectName = GameObject.Find("CanvasInteract/Parent/PanelTreasure/PopupTreasure/ObjectIcon/ObjectName").GetComponent<TextMeshProUGUI>();
            if (objectDescription == null) // Ouch
                objectDescription = GameObject.Find("CanvasInteract/Parent/PanelTreasure/PopupObjectDescription/TextObjectDescription").GetComponent<TextMeshProUGUI>();
            if (chestAnimator == null)
                chestAnimator = GetComponent<Animator>();

            if (chestDatabase == null)
                chestDatabase = UnityEditor.AssetDatabase.LoadAssetAtPath<GameVariableDatabase>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("VariableDatabaseChest")[0]));
            //chestID = "Chest___" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "___" + item.ItemName + "___" + chestDatabase.GameVariables.Count;
            //chestDatabase.AddGameVariable(chestID, 0);
            if (chestDatabase.CheckDuplicate(chestID) == true) 
            {
                //chestDatabase.RemoveLastVariable();
                chestID = "Chest_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_" + chestDatabase.GameVariables.Count;
                chestDatabase.AddGameVariable(chestID, 0);
            }
            /*if (chestID == "")
            {
                chestID = "Chest_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_" + chestDatabase.GameVariables.Count;
                chestDatabase.AddGameVariable(chestID, 0);
            }*/
            /*else
            {
                if (chestDatabase.GetValue(chestID) == -1)
                {
                    chestID = "Chest_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_" + item.ItemName + "_" + chestDatabase.GameVariables.Count;
                    chestDatabase.AddGameVariable(chestID, 0);
                }
            }*/
            /*if (chestAnimator == null)
                chestAnimator = GetComponent<Animator>();*/
#endif
            /*if (UnityEditor.EditorApplication.isPlaying)
            {
                CheckOpen();
            }*/
        }

        private void Start()
        {
            CheckOpen();
        }


        private void OnDestroy()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                if (Time.frameCount != 0 && Time.renderedFrameCount != 0)//not loading scene
                {
                    chestDatabase.RemoveVariable(chestID);
                }
            }
#endif
        }

        #endregion

    }

} // #PROJECTNAME# namespace