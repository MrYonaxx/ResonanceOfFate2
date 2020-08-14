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
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Database/ItemDatabase", order = 1)]
    public class ItemDatabase : ScriptableObject
    {


        [AssetList(AutoPopulate = true)]
        [SerializeField]
        private List<ItemData> itemDatas;
        public List<ItemData> ItemDatas
        {
            get { return itemDatas; }
        }

        public ItemData GetItemData(string contractName)
        {
            return itemDatas.Find(x => x.name == contractName);
        }

        /*public PlayerData GetPlayerData(CharacterData characterData)
        {
            return itemDatas.Find(x => x.CharacterData == characterData);
        }*/

    }

} // #PROJECTNAME# namespace