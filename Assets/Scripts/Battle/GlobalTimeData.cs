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
    [CreateAssetMenu(fileName = "GlobalTimeData", menuName = "GlobalTimeData", order = 1)]
    public class GlobalTimeData: ScriptableObject
    {


        private bool timeFlow = false;
        public bool TimeFlow
        {
            get { return timeFlow; }
            set { timeFlow = value; }
        }



    } 

} // #PROJECTNAME# namespace