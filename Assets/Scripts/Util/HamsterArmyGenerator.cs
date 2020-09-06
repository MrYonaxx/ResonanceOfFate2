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
    public class HamsterArmyGenerator: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        List<Transform> prefabs;
        [SerializeField]
        List<float> prefabsRatio;

        [SerializeField]
        Vector2 number;

        [SerializeField]
        Vector2 spawnSpace;

        [SerializeField]
        List<Transform> scenary;

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

        [Button]
        public void Generate()
        {
            Destroy();
            for (int i = 0; i < Random.Range(number.x, number.y); i++)
            {
                Transform prefab = SelectPrefab();
                Vector2 randPosition = Random.insideUnitCircle.normalized;
                randPosition *= Random.Range(spawnSpace.x, spawnSpace.y);
                scenary.Add(Instantiate(prefab, new Vector3(randPosition.x, this.transform.position.y, randPosition.y) , Quaternion.identity));
                scenary[i].SetParent(this.transform);
                //scenary[i].localScale *= Random.Range(prefabSize.x, prefabSize.y);
                //scenary[i].localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            }
        }

        private Transform SelectPrefab()
        {
            int r = Random.Range(0, 100);
            for (int i = 0; i < prefabsRatio.Count; i++)
            {
                if (r < prefabsRatio[i])
                    return prefabs[i];
            }
            return prefabs[prefabs.Count - 1];
        }


        [Button]
        public void Destroy()
        {
            for (int i = scenary.Count - 1; i > 0; i--)
            {
                DestroyImmediate(scenary[i].gameObject);
            }
            scenary.Clear();
        }
        #endregion

    } 

} // #PROJECTNAME# namespace