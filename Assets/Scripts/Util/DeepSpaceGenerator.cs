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
    public class DeepSpaceGenerator: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Transform prefab;

        [SerializeField]
        Vector2 number;
        [SerializeField]
        Vector2 prefabSize;

        [SerializeField]
        Vector3 spawnSpace;

        [SerializeField]
        List<Transform> scenary;

        [Title("Feedback")]
        [SerializeField]
        InputController inputController;
        [SerializeField]
        Animator animator;

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
        private void Update()
        {
            if (inputController.InputState == InputState.TriAttack)
                animator.speed = 20f;
            else
                animator.speed = 1f;
        }


        [Button]
        public void Generate()
        {
            Destroy();
            for (int i = 0; i < Random.Range(number.x, number.y); i++)
            {
                scenary.Add(Instantiate(prefab, new Vector3(Random.Range(-spawnSpace.x, spawnSpace.x), Random.Range(-spawnSpace.y, spawnSpace.y), Random.Range(-spawnSpace.z, spawnSpace.z)), Quaternion.identity));
                scenary[i].SetParent(this.transform);
                scenary[i].localScale *= Random.Range(prefabSize.x, prefabSize.y);
                scenary[i].localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            }
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