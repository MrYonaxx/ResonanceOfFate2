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

    public struct DamageMessage
    {
        public int damageType;
        public int damageRaw;
        public int damageScratch;
        public int exp;

        public bool knockback;
        public bool launch;

        public Vector3 damagePosition;

        public DamageMessage(int type, int raw, int scratch)
        {
            damageType = type;
            damageRaw = raw;
            damageScratch = scratch;
            damagePosition = Vector3.zero;

            exp = 0;
            knockback = false;
            launch = false;
        }

        public void SetDamagePosition(Vector3 pos)
        {
            damagePosition = pos;
        }
    }

    public abstract class AAttackProcessor: ScriptableObject
    {

        public virtual AttackDataStat CreateAttack(CharacterStatController user, WeaponData weaponData)
        {
            AttackDataStat res = new AttackDataStat(weaponData);
            return res;
        }


        public virtual DamageMessage ProcessAttack(AttackDataStat attack, CharacterStatController target)
        {
            DamageMessage res = new DamageMessage();
            return res;
        }

#if UNITY_EDITOR
        private static List<string> GetStatList()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterStatData>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("StatDictionary")[0])).StatNames;
        }
#endif

    }

} // #PROJECTNAME# namespace