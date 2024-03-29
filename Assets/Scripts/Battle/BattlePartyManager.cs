﻿/*****************************************************************
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
    public class BattlePartyManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Debug")]
        [SerializeField]
        List<PlayerData> debugPartyData = new List<PlayerData>();
        [SerializeField]
        List<GameVariableDatabase> debugVariableDatabase = new List<GameVariableDatabase>();


        [Title("Parameter")]
        [SerializeField]
        List<Transform> entranceTransforms = new List<Transform>();
        [SerializeField]
        List<PlayerCharacter> party = new List<PlayerCharacter>();

        [Title("Data")]
        [SerializeField]
        PartyData partyData;
        [SerializeField]
        CameraLock mainCamera;
        [SerializeField] // à bouger dans un SO probablement
        List<Vector3> playerPositionOffset = new List<Vector3>();

        [Title("UI")]
        [SerializeField]
        Transform hudTransform;
        [SerializeField]
        PlayerHUD playerHudPrefab;

        int partySize = 0;
        int indexSelection = 0;

        int antiloop = 0;


        List<float> timePlayer = new List<float>();
        List<PlayerHUD> playerHUDs = new List<PlayerHUD>();
        public List<PlayerHUD> PlayerHUDs
        {
            get { return playerHUDs; }
        }


        public delegate void ActionCharacterChange(PlayerCharacter playerCharacter);
        public event ActionCharacterChange OnCharacterChange;


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
        /*public List<Character> GetPartyAsCharacter()
        {
            List<Character> list = new List<Character>(party.Count);
            for (int i = 0; i < party.Count; i++)
            {
                list.Add(party[i]);
            }
            return list;
        }*/
        public List<PlayerCharacter> GetParty()
        {
            return party;
        }
        public PlayerCharacter GetCharacter()
        {
            return party[indexSelection];
        }
        public PlayerCharacter GetCharacter(int index)
        {
            return party[index];
        }
        public int GetIndexSelection()
        {
            return indexSelection;
        }
        public void SetIndexSelection(int newIndex)
        {
            playerHUDs[indexSelection].HudSelected(false);
            indexSelection = newIndex;
            SelectCharacter();
            FocusCamera();
        }



        private void Awake()
        {
            CreateDebugParty();
            InitializeCharacters();
        }
        private void CreateDebugParty() // Sans doute à bouger un jour peut etre quand on aura un vrai jeu fini
        {
            if (partyData.GetInitialize() == false)
            {
                partyData.SetInitialize();
                partyData.CharacterStatControllers.Clear();
                partyData.CharacterEquipement.Clear();
                partyData.CharacterGrowths.Clear();
                for (int i = 0; i < debugPartyData.Count; i++)
                {
                    partyData.AddCharacter(debugPartyData[i]);
                }
                partyData.NextZoneEntrance = 999;
                for (int i = 0; i < debugVariableDatabase.Count; i++)
                {
                    debugVariableDatabase[i].ResetDatabase();
                }
            }
        }

        private void InitializeCharacters()
        {
            for(int i = 0; i < partyData.CharacterStatControllers.Count; i++)
            {
                party[i].gameObject.SetActive(true);
                party[i].CharacterStatController = partyData.CharacterStatControllers[i];
                party[i].CharacterEquipement = partyData.CharacterEquipement[i];

                playerHUDs.Add(Instantiate(playerHudPrefab, hudTransform));
                playerHUDs[i].DrawCharacter(party[i].CharacterStatController, party[i].CharacterEquipement);
                playerHUDs[i].gameObject.SetActive(true);

                timePlayer.Add(100f);

                party[i].CharacterTriAttack.OnTimeChanged += playerHUDs[i].GaugeTriAttack.DrawGauge;
                party[i].CharacterStatController.OnHPChanged += playerHUDs[i].HpGauge.DrawGauge;
                party[i].CharacterStatController.OnScratchChanged += playerHUDs[i].ScratchGauge.DrawGauge;

                if (entranceTransforms.Count > partyData.NextZoneEntrance) 
                {
                    party[i].CharacterMovement.SetPosition(entranceTransforms[partyData.NextZoneEntrance].position + (Quaternion.Euler(0, entranceTransforms[partyData.NextZoneEntrance].eulerAngles.y, entranceTransforms[partyData.NextZoneEntrance].eulerAngles.z) * playerPositionOffset[i]));
                    party[i].CharacterDirection.SetDirection(entranceTransforms[partyData.NextZoneEntrance]);
                }
            }
            partySize = partyData.CharacterStatControllers.Count;
        }

        private void Start()
        {
            FocusCamera();
            mainCamera.transform.position = party[indexSelection].CharacterCenter.position;
            mainCamera.SetCameraAxis(0, party[indexSelection].CharacterDirection.DirectionTransform.localEulerAngles.y);
        }



        // Character Turn ===============================================
        public void CurrentCharacterInactive()
        {
            timePlayer[indexSelection] = 0;
            playerHUDs[indexSelection].GaugeMoveTime.DrawGauge(timePlayer[indexSelection], 100f);
            playerHUDs[indexSelection].HudInactive(true);
        }

        public void CharacterInactive(int index)
        {
            timePlayer[index] = 0;
            playerHUDs[index].GaugeMoveTime.DrawGauge(timePlayer[index], 100f);
            playerHUDs[index].HudInactive(true);
        }

        public float GetPlayerTime(int index)
        {
            return timePlayer[indexSelection];
        }
        public bool ReducePlayerTime(float amount)
        {
            timePlayer[indexSelection] -= amount * Time.deltaTime;
            playerHUDs[indexSelection].GaugeMoveTime.DrawGauge(timePlayer[indexSelection], 100f);
            if (timePlayer[indexSelection] <= 0)
            {
                timePlayer[indexSelection] = 0f;
                playerHUDs[indexSelection].GaugeMoveTime.DrawGauge(timePlayer[indexSelection], 100f);
                playerHUDs[indexSelection].HudInactive(true);
                return true;
            }
            return false;
        }

        public void ResetPlayerTurn()
        {
            for (int i = 0; i < party.Count; i++)
            {
                timePlayer[i] = 100;
                playerHUDs[i].HudInactive(false);
            }
            //indexSelection = 0;
            FocusCamera();
        }




        // Reset/Stop Players ===============================================
        public void StopPlayers()
        {
            for (int i = 0; i < party.Count; i++)
            {
                party[i].CharacterAction.ForceStopAction();
            }
        }




        public void FocusCameraLeft()
        {
            if (antiloop == party.Count) // On a loop donc aucun perso n'est dispo donc go Reset
            {
                SelectRight();
                ResetPlayerTurn();
            }
            SelectLeft();
            if (timePlayer[indexSelection] <= 0)
            {
                antiloop += 1;
                FocusCameraLeft();
            }
            else
            {
                antiloop = 0;
                FocusCamera();
            }
        }
        private void SelectLeft()
        {
            playerHUDs[indexSelection].HudSelected(false);
            indexSelection -= 1;
            if (indexSelection < 0)
                indexSelection = partySize - 1;
            SelectCharacter();
        }


        public void FocusCameraRight()
        {
            if(antiloop == party.Count) // On a loop donc aucun perso n'est dispo donc go Reset
            {
                SelectLeft();
                ResetPlayerTurn();
            }
            SelectRight();
            if (timePlayer[indexSelection] <= 0)
            {
                antiloop += 1;
                FocusCameraRight();
            }
            else
            {
                antiloop = 0;
                FocusCamera();
            }
        }

        private void SelectRight()
        {
            playerHUDs[indexSelection].HudSelected(false);
            indexSelection += 1;
            if (indexSelection >= partySize)
                indexSelection = 0;
            SelectCharacter();
        }

        public void FocusCamera()
        {
            playerHUDs[indexSelection].HudSelected(true);
            mainCamera.SetFocus(party[indexSelection].CharacterCenter);
            mainCamera.SetTarget(null);
        }



        private void SelectCharacter()
        {
            if (OnCharacterChange != null) OnCharacterChange.Invoke(party[indexSelection]);
        }





        public void DrawOrder(bool reverse = false)
        {
            DrawOrder(indexSelection, reverse);
        }
        public void DrawOrder(int index, bool reverse)
        {
            for(int i = 0; i < party.Count; i++)
            {
                playerHUDs[index].DrawOrder(i + 1);
                if (reverse == false)
                {              
                    index += 1;
                    if (index >= party.Count)
                        index = 0;
                }
                else
                {
                    index -= 1;
                    if (index < 0)
                        index = party.Count-1;
                }
            }
        }

        public void HideOrder()
        {
            for (int i = 0; i < party.Count; i++)
            {
                playerHUDs[i].HideOrder();
            }
        }




        private void OnDestroy()
        {
            for (int i = 0; i < party.Count; i++)
            {
                party[i].CharacterStatController.OnHPChanged -= playerHUDs[i].HpGauge.DrawGauge;
                party[i].CharacterStatController.OnScratchChanged -= playerHUDs[i].ScratchGauge.DrawGauge;
            }
        }









        /*private IEnumerator DeathCoroutine()
        {
            // flash rouge
            Time.timeScale = 0.1f;
            yield return null;
        }*/

















        #endregion

    } 

} // #PROJECTNAME# namespace