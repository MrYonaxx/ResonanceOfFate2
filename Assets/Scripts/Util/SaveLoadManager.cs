/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

namespace VoiceActing
{
    public class SaveLoadManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        PlayerDatabase playerDatabase;
        [SerializeField]
        PartyData partyData;
        public PartyData PartyData
        {
            get { return partyData; }
        }

        [SerializeField]
        string saveFileName = "save";
        //[SerializeField]
        //string saveAllDataName = "savesData";

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

        public void SavePlayerProfile(int index)
        {
            // On save les identifiants des character data vu qu'on peut serializer que des string et des int
            partyData.SceneName = SceneManager.GetActiveScene().name;
            partyData.SavePartyData(playerDatabase);

            string json = JsonUtility.ToJson(partyData);
            string filePath = string.Format("{0}/saves/{1}{2}.json", Application.persistentDataPath, saveFileName, index);
            Debug.Log(filePath);
            FileInfo fileInfo = new FileInfo(filePath);

            if (!fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }
            File.WriteAllText(filePath, json);
        }

        public bool LoadPlayerProfile(int index)
        {
            string filePath = string.Format("{0}/saves/{1}{2}.json", Application.persistentDataPath, saveFileName, index);
            Debug.Log(filePath);
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(dataAsJson, partyData);
                // On load et on recalcule tout
                partyData.LoadPartyData(playerDatabase);
                return true;
            }
            return false;
        }

        public void LoadScene()
        {
            SceneManager.LoadScene(partyData.SceneName);
        }


        #endregion

    } 

} // #PROJECTNAME# namespace