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
    // Ajoute l'objet auquel il est attaché à une liste globale
    // Cette liste sera utilisé par des listeners pour faire des choses
    public class DamageableRetriever: MonoBehaviour
    {
        [SerializeField]
        IDamageable damageable;
        [SerializeField]
        GlobalDamageable globalDamageable;

        bool hasBeenRemoved = false;
        bool applicationQuit = false;
        private void Start()
        {
            if (damageable == null)
                damageable = GetComponent<IDamageable>();
            AddGlobalDamageable();
        }

        private void OnEnable()
        {
            if (hasBeenRemoved == true)
                AddGlobalDamageable();
        }
        private void OnDisable()
        {
            if (applicationQuit == true)
                return;
            if (hasBeenRemoved == false)
                RemoveGlobalDamageable();
        }
        private void OnApplicationQuit()
        {
            applicationQuit = true;
        }


        public void AddGlobalDamageable()
        {
            // A refactor sans doute
            globalDamageable.AddDamageable(damageable);
            hasBeenRemoved = false;
        }


        public void RemoveGlobalDamageable()
        {
            globalDamageable.RemoveDamageable(damageable);
            hasBeenRemoved = true;
        }

    } 

} // #PROJECTNAME# namespace