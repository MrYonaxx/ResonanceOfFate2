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
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class FeverTimeManager: MonoBehaviour, IListListener<EnemyController>
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GlobalCamera globalCamera;
        [SerializeField]
        GlobalFeedbackManager feedbackManager;
        [SerializeField]
        GlobalEnemyList globalEnemyList;


        [SerializeField]
        float smoothCamera = 2;
        [SerializeField]
        float smoothRotation = 2;

        [Space]
        [SerializeField]
        RectTransform reticlePosition;

        //[SerializeField]
        //AimReticle aimReticleNormal;

        [SerializeField]
        AimReticle aimReticleFever;

        [SerializeField]
        Image fillFeverRight;
        [SerializeField]
        Image fillFeverLeft;

        [Title("Feedback")]
        [SerializeField]
        Animator feverOutlineFeedback;
        [SerializeField]
        GameObject feverDecisionButton;
        [SerializeField]
        List<GameObject> feverButton;
        [SerializeField]
        AttackController tripleFeverAnim;

        AttackController attackController;
        //[SerializeField]
        Enemy targetAim;

        bool feverUpdate = false;
        private float feverTimeValue = 0f;
        private float feverTimeMaxValue = 200f;

        public event Action OnFeverDecision;
        public event Action OnFeverEnd;

        //[SerializeField]
        PlayerCharacter currentCharacter;
        List<PlayerCharacter> playerCharacters = new List<PlayerCharacter>();

        private IEnumerator feverCoroutine;

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
            globalEnemyList.AddListener(this);
        }
        private void OnDestroy()
        {
            globalEnemyList.RemoveListener(this);
        }

        public void OnListAdd(EnemyController enemy)
        {
            enemy.Enemy.CharacterDamage.OnHitAerial += AddFill;
        }
        public void OnListRemove(EnemyController enemy)
        {
            enemy.Enemy.CharacterDamage.OnHitAerial -= AddFill;
        }



        public void AssignShooter(PlayerCharacter shooter)
        {
            currentCharacter = shooter;
            //playerCharacters.Add(shooter);
        }
        public void ClearShooters()
        {
            playerCharacters.Clear();
        }
        public void AssignTarget(EnemyController target)
        {
            if (target == null)
            {
                targetAim = null;
                return;
            }
            targetAim = target.Enemy;
        }


        // ================================================================================
        private void StopCoroutine()
        {
            //Debug.Log("Qui");
            if (feverCoroutine != null)
                StopCoroutine(feverCoroutine);
        }

        public void AddFill(int value)
        {
            if (value <= 0)
                return;
            //if (playerCharacters.Count == 0 )
            //    return;
            if (currentCharacter.CharacterAnimation.State == CharacterState.Jump) // si le perso qui vise n'est pas au sol ça marche pas
                return;
            if (globalCamera.GetCameraAction().gameObject.activeInHierarchy == false) // On ne peut pas ajouter du fever time si une action n'est pas en cours
                return;
            feverTimeValue += value;
            feverUpdate = true;
            DrawFill();
            StopCoroutine();
            feverCoroutine = FeverCoroutine();
            StartCoroutine(feverCoroutine);
        }

        public void ResetFill()
        {
            //playerCharacters.Clear();
            feverTimeValue = 0;
            StopCoroutine();
            reticlePosition.gameObject.SetActive(false);
        }

        private void DrawFill()
        {
            reticlePosition.gameObject.SetActive(true);
            fillFeverRight.fillAmount = feverTimeValue / feverTimeMaxValue;
            fillFeverRight.fillAmount *= 0.5f;

            fillFeverLeft.fillAmount = feverTimeValue / feverTimeMaxValue;
            fillFeverLeft.fillAmount *= 0.5f;
        }



        private IEnumerator FeverCoroutine()
        {
            while (true)
            {
                if (targetAim == null)
                {
                    reticlePosition.gameObject.SetActive(false);
                    StopCoroutine();
                    yield break;
                }
                if (globalCamera.CameraAction().enabled == false) // On fait automatiquement disparaitre le reticle si on quitte le mode action
                {
                    reticlePosition.gameObject.SetActive(false);
                    StopCoroutine();
                    yield break;
                }
                reticlePosition.anchoredPosition = Vector2.Lerp(reticlePosition.anchoredPosition, globalCamera.WorldToScreenPoint(targetAim.CharacterCenter.transform.position), 0.5f);
                yield return null;
            }
        }

        // ================================================================================

        public void CheckFever(AttackController attack)
        {
            attackController = attack;
            if (feverUpdate == true && feverTimeValue >= feverTimeMaxValue)
            {
                playerCharacters.Add(currentCharacter);
                targetAim.CharacterDamage.IsInvulnerable = true; // Pour que targetAim ne se prenne pas une balle perdue et foute tout en l'air
                feverOutlineFeedback.SetTrigger("Appear");
                feverDecisionButton.gameObject.SetActive(true);
                feedbackManager.SetMotionSpeed(0f);
                OnFeverDecision.Invoke();

                for (int i = 0; i < feverButton.Count; i++)
                    feverButton[i].SetActive(false);
                feverButton[playerCharacters.Count - 1].SetActive(true);

                StopCoroutine();
                feverCoroutine = CameraCoroutine();
                StartCoroutine(feverCoroutine);
                feverUpdate = false;
                return;
            }
            CancelFeverAim();
        }

        private IEnumerator CameraCoroutine()
        {
            Vector3 destination = globalCamera.GetCameraAction().position + (targetAim.CharacterCenter.transform.position - globalCamera.GetCameraAction().position) * 0.2f;
            float t = 0f;
            while (t < 1.5f)
            {
                t += Time.deltaTime;
                reticlePosition.anchoredPosition = Vector2.Lerp(reticlePosition.anchoredPosition, globalCamera.WorldToScreenPoint(targetAim.CharacterCenter.transform.position), 0.5f);

                globalCamera.GetCameraAction().position = Vector3.Lerp(globalCamera.GetCameraAction().position, destination, smoothCamera);
                UpdateLockRotation();
                yield return null;
            }
            RefuseFeverAim();
        }

        private void UpdateLockRotation()
        {
            Transform pivot = globalCamera.GetCameraAction();
            Vector3 originalRot = pivot.localEulerAngles;
            pivot.LookAt(targetAim.CharacterCenter.position);
            Vector3 newRot = pivot.localEulerAngles;
            pivot.localEulerAngles = originalRot;
            float x = Mathf.LerpAngle(pivot.localEulerAngles.x, newRot.x, smoothRotation);
            float y = Mathf.LerpAngle(pivot.localEulerAngles.y, newRot.y, smoothRotation);
            float z = Mathf.LerpAngle(pivot.localEulerAngles.z, newRot.z, smoothRotation);
            pivot.localEulerAngles = new Vector3(x, y, z);
        }


        // ========================================================================================================

        public void StartFeverAim()
        {
            feverDecisionButton.gameObject.SetActive(false);
            feverOutlineFeedback.SetTrigger("Start");
            feverUpdate = false;
            targetAim.CharacterAnimation.SetCharacterMotionSpeed(0.8f);
            targetAim.CharacterDamage.IsInvulnerable = false;
            reticlePosition.gameObject.SetActive(false);
            aimReticleFever.ShowAimReticle();
            aimReticleFever.SetTarget(targetAim.CharacterCenter);
            aimReticleFever.AddCharacterAiming(playerCharacters[playerCharacters.Count-1]);
            aimReticleFever.StartAim();
            aimReticleFever.SetMainColor(playerCharacters[playerCharacters.Count - 1].CharacterEquipement.GetWeapon());
            globalCamera.ActivateCameraAction(false);
            globalCamera.GetCameraAction().SetParent(null, false);
            attackController.SetAnimSpeed(); // Set la vitesse de l'anim a 1 puisque l'anim se fige au début du fever time
            for(int i = 0; i < playerCharacters.Count; i++)
            {
                playerCharacters[i].CharacterDirection.LookAt(targetAim.transform);
                playerCharacters[i].CharacterAction.ShowFeverAim(true);
                playerCharacters[i].CharacterAnimation.UserDisappear();
                //playerCharacters[i].CharacterAnimation.PlayTrigger("FeverAim");
            }
            StopCoroutine();
            feverCoroutine = FeverTimeCoroutine();
            StartCoroutine(feverCoroutine);
        }


        private IEnumerator FeverTimeCoroutine()
        {
            while (targetAim.CharacterMovement.IsGrounded() == false)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            EndFeverAim();
            yield return new WaitForSeconds(0.5f);
            aimReticleFever.HideAimReticle();
            yield return new WaitForSeconds(0.5f);
            EndShootFeverTime();
        }


        public void EndFeverAim()
        {
            globalCamera.ActivateCameraAction(true);
            Transform cam = globalCamera.GetCameraAction();
            cam.SetParent(globalCamera.GetCameraDefault(), false);
            cam.localPosition = Vector3.zero;
            cam.localEulerAngles = Vector3.zero;
            aimReticleFever.StopAim();
            //playerCharacters.Clear();
        }



        public void CancelFeverAim()
        {
            reticlePosition.gameObject.SetActive(false);
            attackController.EndAttack();
            StopCoroutine();
            feverUpdate = false;
        }
        public void RefuseFeverAim()
        {
            feverDecisionButton.gameObject.SetActive(false);
            targetAim.CharacterDamage.IsInvulnerable = false;
            feverOutlineFeedback.SetTrigger("Disappear");
            CancelFeverAim();
        }


        public bool ShootFeverTime()
        {
            if(aimReticleFever.GetBulletNumber(playerCharacters[playerCharacters.Count-1]) >= 1)
            {
                StopCoroutine();
                targetAim.CharacterAnimation.SetCharacterMotionSpeed(1f);
                PlayerCharacter c = playerCharacters[playerCharacters.Count - 1];
                for (int i = 0; i < playerCharacters.Count; i++)
                {
                    playerCharacters[i].CharacterAction.ShowFeverAim(false);
                    playerCharacters[i].CharacterAction.FeverAction(playerCharacters[i].CharacterEquipement.GetWeaponAttackData(aimReticleFever.GetBulletNumber(c)), 
                                                                    aimReticleFever.TargetAim, 
                                                                    EndShootFeverTime);
                }
                if (playerCharacters.Count >= 3)
                {
                    tripleFeverAnim.CreateAttack(targetAim.CharacterCenter);
                }
                else
                {
                    c.CharacterAction.RotateFever();
                }

                aimReticleFever.StopAim();
                aimReticleFever.HideAimReticle();
                return true;
            }
            return false;
        }

        public void EndShootFeverTime()
        {
            for (int i = 0; i < playerCharacters.Count; i++)
            {
                playerCharacters[i].CharacterAnimation.UserAppear();
                playerCharacters[i].CharacterAction.ShowFeverAim(false);
            }
            playerCharacters.Clear();
            ResetFill();
            CancelFeverAim();
        }

        public void ForceStopFever()
        {
            for (int i = 0; i < playerCharacters.Count; i++)
            {
                playerCharacters[i].CharacterAction.ForceStopFever();
            }
            EndShootFeverTime();
        }


        #endregion

    } 

} // #PROJECTNAME# namespace