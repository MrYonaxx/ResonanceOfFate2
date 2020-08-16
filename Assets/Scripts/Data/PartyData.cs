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
    [System.Serializable]
    public class CharacterSave
    {
        [SerializeField]
        public string playerDataID;
        [SerializeField]
        public List<int> playerExperience = new List<int>();
        [SerializeField]
        public List<string> armorEquipped = new List<string>();

        public CharacterSave(string ID, List<int> totalExperience, List<string> armors)
        {
            playerDataID = ID;
            playerExperience = totalExperience;
            armorEquipped = armors;
        }
    }

    [CreateAssetMenu(fileName = "PartyData", menuName = "PartyData", order = 1)]
    public class PartyData: ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        // Pour les saves
        // [SerializeField]
        // List<string> playerDataID = new List<string>();
        // Les données qu'on doit sauvegarder (On sauvegarde pas tout pour pouvoir modifier les stat et que les saves soient compatible)
        [SerializeField]
        List<CharacterSave> characterSavesData = new List<CharacterSave>();
        [SerializeField]
        List<GameVariable> gameVariables = new List<GameVariable>();
        [SerializeField]
        List<GameVariable> chestVariable = new List<GameVariable>();
        [SerializeField]
        List<string> inventory = new List<string>();
        public List<string> Inventory
        {
            get { return inventory; }
        }

        // Pour les saves



        //[SerializeField]
        List<CharacterStatController> characterStatControllers = new List<CharacterStatController>();
        public List<CharacterStatController> CharacterStatControllers
        {
            get { return characterStatControllers; }
        }

        //[SerializeField]
        List<CharacterEquipementController> characterEquipement = new List<CharacterEquipementController>();
        public List<CharacterEquipementController> CharacterEquipement
        {
            get { return characterEquipement; }
        }

        //[SerializeField]
        List<CharacterGrowthController> characterGrowths = new List<CharacterGrowthController>();
        public List<CharacterGrowthController> CharacterGrowths
        {
            get { return characterGrowths; }
        }


        // Playable Characters
        // Stat
        // Weapons
        // Levels

        // Battle Party
        // Stat
        // Weapons
        // Levels

        // Inventory

        // System (Config / options)

        [SerializeField]
        private string sceneName;
        public string SceneName
        {
            get { return sceneName; }
            set { sceneName = value; }
        }

        [SerializeField]
        private int nextZoneEntrance;
        public int NextZoneEntrance
        {
            get { return nextZoneEntrance; }
            set { nextZoneEntrance = value; }
        }


        public class PartyInitialized
        {
            public bool Initialized = false;
            public PartyInitialized()
            {
                //Initialized = true;
            }
        }
        PartyInitialized partyInitialized = null;

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

        public void SetInitialize()
        {
            partyInitialized = new PartyInitialized();
            inventory.Clear();
        }
        public bool GetInitialize()
        {
            if (partyInitialized == null)
                return false;
            return true;
        }

        // On sauvegarde les valeurs à sauver 
        public void SavePartyData(PlayerDatabase database, GameVariableDatabase variableDatabase, GameVariableDatabase chestDatabase)
        {
            characterSavesData.Clear();
            for (int i = 0; i < characterStatControllers.Count; i++)
            {
                characterSavesData.Add(new CharacterSave(database.GetPlayerData(characterStatControllers[i].CharacterData).name, characterEquipement[i].GetWeaponTotalExp(), characterEquipement[i].GetArmors()));
            }
            gameVariables = variableDatabase.SaveVariables();
            chestVariable = chestDatabase.SaveVariables();
        }

        // On utilise les valeurs qu'on a sauver pour re générer les joueurs 
        // Bon le Load c'est un peu n'importe quoi ça fait beaucoup de chose, à refactor peut etre
        public void LoadPartyData(PlayerDatabase database, GameVariableDatabase variableDatabase, GameVariableDatabase chestDatabase, ItemDatabase itemDatabase)
        {
            variableDatabase.LoadVariable(gameVariables);
            chestDatabase.LoadVariable(chestVariable);
            characterStatControllers.Clear();
            characterGrowths.Clear();
            characterEquipement.Clear();
            for (int i = 0; i < characterSavesData.Count; i++)
            {
                PlayerData playerData = database.GetPlayerData(characterSavesData[i].playerDataID);
                characterStatControllers.Add(new CharacterStatController(playerData.CharacterData));
                characterGrowths.Add(new CharacterGrowthController(playerData.CharacterGrowth, characterStatControllers[i]));

                // Convertit les string du json en ArmorData
                List<ArmorData> res = new List<ArmorData>(characterSavesData[i].armorEquipped.Count);
                for (int j = 0; j < characterSavesData[i].armorEquipped.Count; j++)
                {
                    res.Add((ArmorData) itemDatabase.GetItemData(characterSavesData[i].armorEquipped[j]));
                }
                characterEquipement.Add(new CharacterEquipementController(playerData.WeaponEquipped, res, playerData.WeaponLevels, characterSavesData[i].playerExperience, characterStatControllers[i], characterGrowths[i]));

            }
            partyInitialized = new PartyInitialized();
        }

        #endregion

    } 

} // #PROJECTNAME# namespace