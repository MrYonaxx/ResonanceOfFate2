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

    public enum TargetTag
    {
        Player,
        Enemy,
        Interactable
    }

    public class TargetController: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        List<TargetTag> targetTags = new List<TargetTag>();
        [SerializeField]
        List<GameObject> bannedObject = new List<GameObject>();

        [Title("Debug")]
        [SerializeField]
        List<Character> targetable = new List<Character>();

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
        public List<Character> GetTarget()
        {
            return targetable;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (CheckTag(collision) == true)
            {
                targetable.Add(collision.gameObject.GetComponent<Character>());
            }
        }
        private void OnTriggerExit(Collider collision)
        {
            if (CheckTag(collision) == true)
            {
                targetable.Remove(collision.gameObject.GetComponent<Character>());
            }
        }


        private bool CheckTag(Collider collision)
        {
            for (int i = 0; i < bannedObject.Count; i++)
            {
                if (collision.gameObject == bannedObject[i])
                    return false;
            }
            string tag = collision.gameObject.tag;
            for (int i = 0; i < targetTags.Count; i++ )
            {
                if(tag == targetTags[i].ToString())
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace