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
    public class SceneEnemyParameter
    {
        public string sceneName;
        public List<bool> enemiesAlive;

        public SceneEnemyParameter (string name, int count)
        {
            sceneName = name;
        }
    }

    public class RespawnManager: ScriptableObject
    {
        [SerializeField]
        private List<SceneEnemyParameter> sceneEnemyParameters = new List<SceneEnemyParameter>();
        public List<SceneEnemyParameter> SceneEnemyParameters
        {
            get { return sceneEnemyParameters; }
        }


        public void AddSceneRespawnParameter(List<Enemy> enemies, string sceneName)
        {
            // Check si la scene est déjà dans la liste
            for (int i = 0; i < sceneEnemyParameters.Count; i++)
            {
                if (sceneEnemyParameters[i].sceneName == sceneName)
                {

                }
            }

            // Sinon on ajoute
            sceneEnemyParameters.Add(new SceneEnemyParameter(sceneName, enemies.Count));
        }


    } 

} // #PROJECTNAME# namespace