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
    public interface IEnemyListener
    {
        void OnTargetAdd(EnemyController enemyController);
        void OnTargetRemove(EnemyController enemyController);
    } 

} // #PROJECTNAME# namespace