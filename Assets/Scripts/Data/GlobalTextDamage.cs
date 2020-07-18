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
    [CreateAssetMenu(fileName = "GlobalTextDamage", menuName = "System/GlobalTextDamage", order = 1)]
    public class GlobalTextDamage: ScriptableObject
    {
        [SerializeField]
        TextPopupManager textPopupManager;
        [SerializeField]
        TextBattleManager textBattleManager;

        public void DrawPopup(DamageMessage message)
        {
            textPopupManager.DrawDamagePopup(message);
        }

    } 

} // #PROJECTNAME# namespace