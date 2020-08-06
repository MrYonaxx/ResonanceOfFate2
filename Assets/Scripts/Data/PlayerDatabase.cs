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
    [CreateAssetMenu(fileName = "PlayerDatabase", menuName = "Database/PlayerData", order = 1)]
    public class PlayerDatabase: ScriptableObject
    {


        [AssetList(AutoPopulate = true)]
        [SerializeField]
        private List<PlayerData> playerDatas;
        public List<PlayerData> PlayerDatas
        {
            get { return playerDatas; }
        }

        public PlayerData GetPlayerData(string contractName)
        {
            return playerDatas.Find(x => x.name == contractName);
        }

        public PlayerData GetPlayerData(CharacterData characterData)
        {
            return playerDatas.Find(x => x.CharacterData == characterData);
        }

    } 

} // #PROJECTNAME# namespace