/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        BattlePartyManager battlePartyManager;
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

        [Title("Health")]
        [SerializeField]
        Image healthBar;
        [SerializeField]
        Image scratchBar;
        [SerializeField]
        Transform scratchTransform;

        [Title("Player Position")]
        [SerializeField]
        RectTransform playerPosition;

        [Title("BodyPart")]
        [SerializeField]
        List<RectTransform> bodyLayer = new List<RectTransform>();
        [SerializeField]
        CircleGaugeDrawer bodyPartDrawer;

        List<CircleGaugeDrawer> gaugeDrawers = new List<CircleGaugeDrawer>();

        ITargetable previousTarget;


        Transform targetDirection;
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
            battlePartyManager.OnCharacterChange += SetPlayer;
            SetPlayer(battlePartyManager.GetCharacter());
        }

        private void Update()
        {
            if (player == null)
                return;
            if (targetDirection == null)
            {
                directionTarget = Vector2.zero;
                return;
            }
            else
            {
                directionTarget = new Vector2(-targetDirection.up.x, -targetDirection.up.z);
                directionCharacter = new Vector2(targetDirection.position.x - player.position.x, targetDirection.position.z - player.position.z);
                playerPosition.localEulerAngles = new Vector3(0, 0, Vector2.SignedAngle(directionTarget, directionCharacter));
            }
        }


        public void DrawTargetInfo(ITargetable characterStat)
        {
            if (previousTarget != null)
            {
                previousTarget.CharacterStatController.OnHPChanged -= DrawHealth;
                previousTarget.CharacterStatController.OnScratchChanged -= DrawScratch;
                List<BodyPartController> bodyPart = previousTarget.GetBodyParts();
                if (bodyPart != null)
                {
                    for (int i = 0; i < bodyPart.Count; i++) // Potentiel memoryLeak là (en gros ça ne prend pas en compte les body part remove entre l'abonnement et le désabonnement)
                    {
                        bodyPart[i].StatController.OnHPChanged -= gaugeDrawers[i].DrawHealth;
                        bodyPart[i].StatController.OnScratchChanged -= gaugeDrawers[i].DrawScratch;
                    }
                }
            }
            animator.SetTrigger("Disappear");
            if (characterStat == null)
            {
                previousTarget = null;
                targetDirection = null;
                //Hide(); 
                return;
            }
            targetDirection = characterStat.TargetDirection;
            animator.SetTrigger("Appear");
            textTargetName.text = characterStat.CharacterStatController.CharacterData.CharacterName;
            if (characterStat.CharacterStatController.Level <= 0)
            {
                textTargetLevel.gameObject.SetActive(false);
            }
            else
            {
                textTargetLevel.gameObject.SetActive(true);
                textTargetLevelDigit.text = characterStat.CharacterStatController.Level.ToString();
            }
            characterStat.CharacterStatController.OnHPChanged += DrawHealth;
            characterStat.CharacterStatController.OnScratchChanged += DrawScratch;
            previousTarget = characterStat;
            DrawHealth(characterStat.CharacterStatController.Hp, characterStat.CharacterStatController.GetHPMax());
            DrawScratch(characterStat.CharacterStatController.Scratch, characterStat.CharacterStatController.Hp);
            DrawBodyPart(characterStat.GetBodyParts());
        }

        public void DrawHealth(float hp, float hpMax)
        {
            healthBar.fillAmount = (hp / hpMax);
            scratchTransform.localEulerAngles = new Vector3(0, 0, (1- (hp / hpMax)) * 360f);
        }

        public void DrawScratch(float scratch, float hp)
        {
            scratchBar.fillAmount = (scratch / hp);
            scratchBar.fillAmount = Mathf.Clamp(scratchBar.fillAmount, 0, 1f - (scratchTransform.localEulerAngles.z / 360f));
        }


        public void DrawBodyPart(List<BodyPartController> bodyPart)
        {
            int lenght = 0;
            if (bodyPart != null)
                lenght = bodyPart.Count;
            int maxLayer = 1;
            for(int i = 0; i < lenght; i++)
            {
                if (i >= gaugeDrawers.Count)
                    gaugeDrawers.Add(Instantiate(bodyPartDrawer));
                gaugeDrawers[i].gameObject.SetActive(true);
                gaugeDrawers[i].transform.SetParent(bodyLayer[bodyPart[i].Layer-1]);
                gaugeDrawers[i].CreateGauge(bodyPart[i].AngleMin, bodyPart[i].AngleMax);
                gaugeDrawers[i].DrawHealth(bodyPart[i].StatController.Hp, bodyPart[i].StatController.GetHPMax());

                bodyPart[i].StatController.OnHPChanged += gaugeDrawers[i].DrawHealth;
                bodyPart[i].StatController.OnScratchChanged += gaugeDrawers[i].DrawScratch;

                if (maxLayer < bodyPart[i].Layer)
                    maxLayer = bodyPart[i].Layer;
            }
            playerPosition.sizeDelta = bodyLayer[maxLayer-1].sizeDelta;
            for (int i = lenght; i < gaugeDrawers.Count; i++)
                gaugeDrawers[i].gameObject.SetActive(false);
        }


        public void SetPlayer(PlayerCharacter playerCharacter)
        {
            SetPlayer(playerCharacter.transform);
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