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
    public class TargetableController: SerializedMonoBehaviour
    {
        [SerializeField]
        ITargetable targetable;
        [SerializeField]
        GlobalTargetable globalTargetable;





        bool hasBeenRemoved = false;
        bool applicationQuit = false;
        private void Start()
        {
            if (targetable == null)
                targetable = GetComponent<ITargetable>();
            Targetable();
        }

        private void OnEnable()
        {
            if(hasBeenRemoved == true)
                Targetable();
        }
        private void OnDisable()
        {
            if (applicationQuit == true)
                return;
            if (hasBeenRemoved == false)
                Untargetable();
        }
        private void OnApplicationQuit()
        {
            applicationQuit = true;
        }





        public void Targetable()
        {
            // A refactor sans doute
            targetable.CharacterStatController.OnHPChanged += CheckDeath;
            globalTargetable.AddTargetable(targetable);
            hasBeenRemoved = false;
        }


        public void Untargetable() 
        {
            globalTargetable.RemoveTargetable(targetable);
            hasBeenRemoved = true;
        }


        private void CheckDeath(float stat, float statMax)
        {
            if(stat <= 0)
                Untargetable();
        }

    } 

} // #PROJECTNAME# namespace