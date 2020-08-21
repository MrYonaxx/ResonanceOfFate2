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
    public class EventTutoTriAttack: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Enemy enemy;
        [SerializeField]
        InputController inputController;
        [SerializeField]
        TriAttackManager triAttackManager;
        [SerializeField]
        UnityEngine.Events.UnityEvent unityEvent;

        bool resonanceOk = false;

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
            if(resonanceOk == true)
            {
                if (triAttackManager.ResonancePoint == 0)
                {
                    inputController.canAim = true;
                    inputController.canMove = true;
                    inputController.canHeroAction = true;
                    inputController.canTriAttack = true;
                    Destroy(this.gameObject);
                }
                return;
            }

            if (enemy.CharacterStatController.Scratch > 10f)
            {
                enemy.CharacterStatController.Scratch = 0;
            }
            if (enemy.CharacterStatController.Hp < 140f)
            {
                enemy.CharacterStatController.Hp = enemy.CharacterStatController.GetHPMax();
            }

            if(triAttackManager.ResonancePoint >= 2)
            {
                inputController.canHeroAction = false;
                inputController.canTriAttack = true;
                unityEvent.Invoke();
                resonanceOk = true;
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace