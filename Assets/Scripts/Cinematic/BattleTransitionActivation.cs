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
    public class BattleTransitionActivation: MonoBehaviour
    {
        public ScreenShatter screenShatter;

        private void OnEnable()
        {
            screenShatter.ShatterScreen();
        }

    } 

} // #PROJECTNAME# namespace