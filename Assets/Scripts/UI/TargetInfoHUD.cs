/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class TargetInfoHUD: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        BattleTargetController battleTargetController;
        [SerializeField]
        Animator animator;

        [Title("BaseInfo")]
        [SerializeField]
        TextMeshProUGUI textTargetName;
        [SerializeField]
        TextMeshProUGUI textTargetLevel;
        [SerializeField]
        TextMeshProUGUI textTargetLevelDigit;
        [SerializeField]
        TextMeshProUGUI textTargetLabel;

        [Title("Player Position")]
        [SerializeField]
        RectTransform playerPosition;



        CharacterDirection targetDirection;
        Transform player;


        Vector2 directionTarget;
        Vector2 directionCharacter;

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

        private void Awake()
        {
            battleTargetController.OnTargeted += DrawTargetInfo;
        }

        private void Update()
        {
            if (player == null)
                return;
            if (targetDirection == null)
                directionTarget = Vector2.zero;
            else
                directionTarget = new Vector2(-targetDirection.GetDirection().up.x, -targetDirection.GetDirection().up.z);
            directionCharacter = new Vector2(targetDirection.transform.position.x - player.position.x, targetDirection.transform.position.z - player.position.z);
            playerPosition.localEulerAngles = new Vector3(0,0,Vector2.SignedAngle(directionTarget, directionCharacter));
        }


        public void DrawTargetInfo(ITargetable characterStat)
        {
            animator.SetTrigger("Disappear");
            if (characterStat == null)
            {
                //Hide(); 
                return;
            }
            animator.SetTrigger("Appear");
            textTargetName.text = characterStat.CharacterStatController.CharacterData.CharacterName;
            if (characterStat.CharacterStatController.Level <= 0)
            {
                textTargetLevel.gameObject.SetActive(false);
                return;
            }
            else
            {
                textTargetLevel.gameObject.SetActive(true);
                textTargetLevelDigit.text = characterStat.CharacterStatController.Level.ToString();
            }

        }

        public void DrawHealth()
        {

        }

        public void SetPlayer(Transform playerPos)
        {
            player = playerPos;
        }


        private void Hide()
        {
            animator.SetTrigger("Disappear");
        }

        #endregion

    } 

} // #PROJECTNAME# namespace