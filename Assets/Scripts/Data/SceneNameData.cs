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
    [CreateAssetMenu(fileName = "SceneNameData", menuName = "Data/SceneNameData", order = 1)]
    public class SceneNameData: ScriptableObject
    {
        [HorizontalGroup]
        [SerializeField]
        string[] scenesID;
        [HorizontalGroup]
        [SerializeField]
        string[] scenesNames;

        [Button]
        private void AutoFill()
        {

            scenesID = new string[UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings];
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
            {
                scenesID[i] = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            }
        }

        public string GetSceneName()
        {
            int id = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            return scenesNames[id];
        }

    } 

} // #PROJECTNAME# namespace