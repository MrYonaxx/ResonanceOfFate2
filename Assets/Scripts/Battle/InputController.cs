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

    public enum InputState
    {
        Default,
        Moving,
        Aiming,
        HeroAction,
        HeroActionTri,
        TriAttack,
        NoInput
    } 

    public class InputController: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GlobalTimeData timeData;

        [SerializeField]
        BattlePartyManager battlePartyManager;
        [SerializeField]
        BattleEnemyManager battleEnemyManager;
        [SerializeField]
        BattleTargetController battleTarget;
        [SerializeField]
        TriAttackManager triAttackManager;
        [SerializeField]
        AimReticle aimReticle;
        [SerializeField]
        GlobalFeedbackManager globalFeedbackManager;


        //[SerializeField]

        [Space]
        [SerializeField]
        CameraLock cameraLock;
        [SerializeField]
        Animator cameraBlur;


        [Title("Son (A dégager quand j'aurai moins la flemme)")]
        [SerializeField]
        AudioClip switchTargetClip;
        [SerializeField]
        AudioClip switchCharacterClip;
        [SerializeField]
        AudioClip heroActionClip;
        [SerializeField]
        AudioClip heroActionCancelClip;
        [SerializeField]
        AudioClip heroActionStartClip;

        PlayerCharacter c;

        float inputX = 0;
        float inputZ = 0;

        bool reverseDirection = false;
        bool padDown = false;


        private InputState inputState = InputState.Default;
        public InputState InputState
        {
            get { return inputState; }
            set { inputState = value; OnInputStateChanged.Invoke(inputState); }
        }

        private IEnumerator timeComboCoroutine = null;
        private IEnumerator switchTargetSlowMoCoroutine = null;

        public delegate void InputStateAction(InputState state);
        public event InputStateAction OnInputStateChanged;





        ControllerConfigurationData control;

        [SerializeField]
        ControllerConfigurationData controllerConfig;
        [SerializeField]
        ControllerConfigurationData controllerConfigPS4;

        string controllerA = "A";
        string controllerB = "B";
        string controllerX = "X";
        string controllerY = "Y";

        string controllerR1 = "R1";
        string controllerL1 = "L1";

        string controllerLeftHorizontal = "ControllerLeftHorizontal";
        string controllerLeftVertical = "ControllerLeftVertical";

        string controllerRightHorizontal = "ControllerRightHorizontal";
        string controllerRightVertical = "ControllerRightVertical";

        string controllerDPadHorizontal = "ControllerDPadHorizontal";
        string controllerTrigger = "ControllerTrigger";

        protected void Controller()
        {
            control = controllerConfig;
            string[] controllers;
            controllers = Input.GetJoystickNames();
            for (int i = 0; i < controllers.Length; i++)
            {
                Debug.Log(controllers[i]);
                if (controllers[i] == "Wireless Controller")
                {
                    Debug.Log("Aye");
                    control = controllerConfigPS4;
                }
            }
            /*if (controllerConfig != null)
            {
                controllerA = controllerConfig.controllerA + "_" + playerID;
                controllerB = controllerConfig.controllerB + "_" + playerID;
                controllerX = controllerConfig.controllerX + "_" + playerID;
                controllerY = controllerConfig.controllerY + "_" + playerID;
            }
            else
            {
                controllerA += "_" + playerID;
                controllerB += "_" + playerID;
                controllerX += "_" + playerID;
                controllerY += "_" + playerID;
            }
            controllerR1 += "_" + playerID;
            controllerL1 += "_" + playerID;
            controllerLeftHorizontal += "_" + playerID;
            controllerLeftVertical += "_" + playerID;*/
        }



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
        private void Start()
        {
            Controller();
            triAttackManager.SetParty(battlePartyManager.GetParty());
            battleEnemyManager.OnEndAttacks += NewTurn;
            for (int i = 0; i < battlePartyManager.GetParty().Count; i++)
            {
                battlePartyManager.GetCharacter(i).CharacterAction.OnEndAction += EndShoot;
                battlePartyManager.GetCharacter(i).CharacterTriAttack.IdAttacker = i;
                battlePartyManager.GetCharacter(i).CharacterTriAttack.OnEndAction += EndTriAttack;
            }
            c = battlePartyManager.GetCharacter();
        }

        private void Update()
        {
            cameraLock.MoveCamera(Input.GetAxis(control.stickRightHorizontal), Input.GetAxis(control.stickRightVertical));
            if (c.CharacterMovement.CanInput == false)
            {
                return;
            }
            switch (inputState)
            {
                case InputState.Default:
                    UpdateDefault();
                    break;
                case InputState.Moving:
                    UpdateMovement();
                    break;
                case InputState.Aiming:
                    UpdateAiming();
                    break;
                case InputState.HeroAction:
                    UpdateHeroAction();
                    break;
                case InputState.HeroActionTri:
                    UpdateHeroActionTri();
                    break;
                case InputState.TriAttack:
                    UpdateTriAttack();
                    break;
            }
        }


        private void UpdateDefault()
        {
            InputMovement();
            InputAim();
            InputHeroAction();
            InputTriAttackMode();
            InputSwitchCharacters();
            InputSwitchTargets();
        }

        private void UpdateMovement()
        {
            InputMovement();
            //InputSwitchCharacters();
            if (battlePartyManager.ReducePlayerTime(20f) == true) // Donc le perso n'a plus de time
            {
                c.CharacterMovement.Move(0, 0);
                NextTurn();
            }
        }

        private void UpdateAiming()
        {
            InputCancelAim();
            InputShoot();
        }

        private void UpdateHeroAction()
        {
            InputHeroAction();
            InputHeroActionStart();
            InputTriAttackMode();
            InputHeroActionCancel();
            InputSwitchCharacters();
            InputSwitchTargets();
        }

        private void UpdateHeroActionTri()
        {
            InputTriAttackStart();
            InputTriAttackModeReverse();
            InputTriAttackModeCancel();
            InputSwitchCharacters();
            InputSwitchTargets();
        }

        private void UpdateTriAttack()
        {
            InputSwitchTargets();
            InputShoot();
            InputTriAttackJump();
        }





        private void InputMovement()
        {
            if (Input.GetAxis(control.rightTrigger) > 0.8f)
            {
                if (Mathf.Abs(Input.GetAxis(control.stickLeftVertical)) > 0.8f)
                    inputZ = Input.GetAxis(control.stickLeftVertical);
                else
                    inputZ = 0;

                if (Mathf.Abs(Input.GetAxis(control.stickLeftHorizontal)) > 0.8f)
                    inputX = Input.GetAxis(control.stickLeftHorizontal);
                else
                    inputX = 0;

                if (inputX == 0 && inputZ == 0)
                {
                    timeData.TimeFlow = false;
                    InputState = InputState.Default;
                }
                else
                {
                    timeData.TimeFlow = true;
                    InputState = InputState.Moving;
                }
                c.CharacterMovement.Move(inputX, inputZ);
            }
            else
            {
                timeData.TimeFlow = false;
                c.CharacterMovement.Move(0, 0);
                InputState = InputState.Default;
            }
        }



        // =================================================================================
        // Shoot

        private void InputAim()
        {
            if(Input.GetButtonDown(control.buttonA) && aimReticle.enabled == true)
            {
                timeData.TimeFlow = true;
                InputState = InputState.Aiming;
                c.CharacterDirection.LookAt(aimReticle.TargetAim);
                cameraLock.SetTarget(aimReticle.TargetAim);
                cameraLock.LockOn(true);
                cameraLock.SetState(1);
                aimReticle.AddCharacterAiming(c);
                aimReticle.StartAim();
            }
        }
        private void InputShoot()
        {
            if (Input.GetButtonDown(control.buttonA) && aimReticle.enabled == true)
            {
                if (aimReticle.GetBulletNumber(c) >= 1 && c.CharacterAction.isAttacking() == false)
                {
                    //timeData.TimeFlow = false;
                    if (switchTargetSlowMoCoroutine != null)
                        StopCoroutine(switchTargetSlowMoCoroutine);
                    StopTimeComboCoroutine();
                    aimReticle.PauseAim();
                    aimReticle.HideAimReticle();
                    c.CharacterAction.StartShoot(c.CharacterEquipement.GetWeaponAttackData(aimReticle.GetBulletNumber(c)), aimReticle.TargetAim);
                }
            }
        }

        private void InputCancelAim()
        {
            if (Input.GetButtonDown(control.buttonB))
            {
                timeData.TimeFlow = false;
                cameraLock.LockOn(false);
                cameraLock.SetTarget(null);
                cameraLock.SetState(0);
                aimReticle.StopAim(c);
                if (battleEnemyManager.CheckEnemyAttack() == true)
                {
                    timeData.TimeFlow = false;
                    StopInput();
                }
                else
                {
                    InputState = InputState.Default;
                }
            }
        }

        private void EndShoot()
        {

            aimReticle.ShowAimReticle();
            if (c.CharacterTriAttack.IsTriAttacking == true)
            {
                // Le joueur est en train de courir
                if (cameraLock.GetTarget() == null) // On lock une nouvelle cible si la précédente est morte
                {
                    Debug.Log("Hey");
                    cameraLock.LockOn(true);
                    battleTarget.TargetEnemy();
                }
                timeData.TimeFlow = true;
                aimReticle.ResetAim(c);
                if (triAttackManager.IsTriAttacking) // Si on tri attack on va check si les copains peuvent taper
                {
                    ComboTriAttack();
                }
            }
            else
            {
                // Y'a plus rien on reset
                battlePartyManager.CurrentCharacterInactive();
                cameraLock.LockOn(false);
                cameraLock.SetTarget(null);
                cameraLock.SetState(0);
                aimReticle.StopAim(c);
                NextTurn();
            }
        }




        // =================================================================================
        // HERO ACTION
        private void InputHeroAction()
        {
            if (Input.GetAxis(control.rightTrigger) > 0.8f)
            {
                cameraLock.SetState(0);
                c.CharacterHeroAction.Desactivate();
                InputMovement();
                return;
            }
            if (Input.GetButton(control.buttonB))
                return;
            if (Mathf.Abs(Input.GetAxis(control.stickLeftVertical)) > 0.8f)
            {
                cameraLock.SetState(2);
                inputZ = Input.GetAxis(control.stickLeftVertical);
            }
            else
            {
                inputZ = 0;
            }

            if (Mathf.Abs(Input.GetAxis(control.stickLeftHorizontal)) > 0.8f)
            {
                cameraLock.SetState(2);
                inputX = Input.GetAxis(control.stickLeftHorizontal);
            }
            else
            {
                inputX = 0;
            }

            if (inputX != 0 || inputZ != 0)
            {
                InputState = InputState.HeroAction;

            }

            c.CharacterHeroAction.PerformHeroAction(inputX, inputZ);
        }

        private void InputHeroActionStart()
        {
            if (Input.GetButtonDown(control.buttonA) || Input.GetButtonDown(control.buttonX))
            {
                PlaySound(heroActionStartClip);
                timeData.TimeFlow = true;
                if(aimReticle.enabled == true) 
                {
                    c.CharacterDirection.LookAt(aimReticle.TargetAim);
                    cameraLock.SetTarget(aimReticle.TargetAim);
                    cameraLock.LockOn(true);
                    aimReticle.AddCharacterAiming(c);
                    aimReticle.StartAim();
                    aimReticle.SetIndexMainCharacter(0);
                }
                cameraLock.SetState(3);
                if (c.CharacterHeroAction.Intersection == true)
                    c.CharacterTriAttack.AddIntersectionTime(c.CharacterHeroAction.IntersectionT);
                c.CharacterTriAttack.AddTriAttackPosition(c.CharacterHeroAction.GetCursorPositionV3());
                c.CharacterTriAttack.StartTriAttack();
                c.CharacterHeroAction.Desactivate();
                InputState = InputState.TriAttack;
                cameraBlur.SetBool("Blur", true);
            }
        }

        private void InputHeroActionCancel()
        {
            if (Input.GetButtonDown(control.buttonB))
            {
                PlaySound(heroActionCancelClip);
                AudioManager.Instance.SwitchToBattle(false);
                c.CharacterHeroAction.Desactivate();
                cameraLock.SetState(0); // Set default
                InputState = InputState.Default;
            }
        }



        // =================================================================================
        // Hero Action Tri
        private void InputTriAttackMode()
        {
            if (Input.GetButtonDown(control.buttonY) && triAttackManager.ResonancePoint > 0)
            {
                PlaySound(heroActionClip);
                reverseDirection = false;
                battlePartyManager.DrawOrder();
                triAttackManager.PrepareTriAttack(battlePartyManager.GetIndexSelection(), reverseDirection);
                cameraLock.SetState(2);
                cameraLock.SetTarget(aimReticle.TargetAim);
                InputState = InputState.HeroActionTri;
            }
        }

        private void InputTriAttackModeReverse()
        {
            if (Input.GetButtonDown(control.buttonY))
            {
                reverseDirection = !reverseDirection;
                battlePartyManager.DrawOrder(reverseDirection);
                triAttackManager.PrepareTriAttack(battlePartyManager.GetIndexSelection(), reverseDirection);
            }
        }

        private void InputTriAttackStart()
        {
            if (Input.GetButtonDown(control.buttonA) || Input.GetButtonDown(control.buttonX))
            {
                PlaySound(heroActionStartClip);
                timeData.TimeFlow = true;
                battlePartyManager.ResetPlayerTurn();
                triAttackManager.StartTriAttack(battlePartyManager.GetIndexSelection(), reverseDirection);
                cameraLock.SetState(3);
                if (aimReticle.enabled == true)
                {
                    cameraLock.SetTarget(aimReticle.TargetAim);
                    cameraLock.LockOn(true);
                    aimReticle.AddCharacterAiming(battlePartyManager.GetParty(), battlePartyManager.GetIndexSelection());
                    aimReticle.SetIndexMainCharacter(triAttackManager.IndexLeader);
                    aimReticle.StartAim();
                }
                InputState = InputState.TriAttack;
                cameraBlur.SetBool("Blur", true);
            }
        }

        private void InputTriAttackModeCancel()
        {
            if (Input.GetButtonDown(control.buttonB))
            {
                PlaySound(heroActionCancelClip);
                AudioManager.Instance.SwitchToBattle(false);
                battlePartyManager.HideOrder();
                triAttackManager.CancelTriAttack();
                cameraLock.SetState(0); // Set default
                InputState = InputState.Default;
            }
        }

        private void ComboTriAttack()
        {
            for (int i = 0; i < battlePartyManager.GetParty().Count - 1; i++)
            {
                if (reverseDirection == true)
                    SwitchCharactersLeft();
                else
                    SwitchCharactersRight();
                if (aimReticle.GetBulletNumber(c) >= 1)
                {
                    aimReticle.PauseAim();
                    aimReticle.SetIndexMainCharacter(battlePartyManager.GetIndexSelection());
                    globalFeedbackManager.SetMotionSpeed(0.1f);
                    InputState = InputState.TriAttack;
                    cameraLock.SetTarget(aimReticle.TargetAim);

                    timeComboCoroutine = TimeComboCoroutine();
                    StartCoroutine(timeComboCoroutine);
                    return;
                }
            }
            aimReticle.ResumeAim();
            globalFeedbackManager.SetMotionSpeed(1f); // Pas nécessaire mais on sait jamais
            GetCharacter(triAttackManager.IndexLeader);
            battlePartyManager.SetIndexSelection(triAttackManager.IndexLeader);
            cameraLock.SetTarget(aimReticle.TargetAim);
            timeComboCoroutine = null;
            InputState = InputState.TriAttack;
        }

        private IEnumerator TimeComboCoroutine()
        {
            yield return new WaitForSeconds(2f);
            aimReticle.ResumeAim();
            globalFeedbackManager.SetMotionSpeed(1f);
            GetCharacter(triAttackManager.IndexLeader);
            battlePartyManager.SetIndexSelection(triAttackManager.IndexLeader);
            cameraLock.SetTarget(aimReticle.TargetAim);
            timeComboCoroutine = null;
        }

        private void StopTimeComboCoroutine()
        {
            if (timeComboCoroutine != null)
                StopCoroutine(timeComboCoroutine);
        }


        // =================================================================================
        // Tri Attack
        private void InputTriAttackJump()
        {
            if (Input.GetButtonDown(control.buttonX) || Input.GetButtonDown(control.buttonB))
            {
                if (timeComboCoroutine != null)
                {
                    c.CharacterAnimation.SetCharacterMotionSpeed(1f);
                }
                c.CharacterTriAttack.Jump();
            }
        }

        // Event Call
        public void EndTriAttack(int id)
        {
            battlePartyManager.CharacterInactive(id);
            aimReticle.StopAim(battlePartyManager.GetCharacter(id));
            triAttackManager.EndTriAttack();
            if (triAttackManager.IsTriAttacking == true) // La tri attack n'est pas fini
            {
                if (id == battlePartyManager.GetIndexSelection()) // C'est le joueur qu'on contrôle donc on switch vers un perso qui lui est en train de triAttaquer
                {
                    for (int i = 0; i < battlePartyManager.GetParty().Count - 1; i++)
                    {
                        SwitchCharactersRight();
                        if (c.CharacterTriAttack.IsTriAttacking == true)
                        {
                            cameraLock.SetTarget(aimReticle.TargetAim);
                            return;
                        }
                    }
                }

            }
            else // La tri attack est fini on reset
            {
                timeData.TimeFlow = false;
                cameraBlur.SetBool("Blur", false);
                cameraLock.LockOn(false);
                battlePartyManager.HideOrder();
                cameraLock.SetState(0);
                NextTurn();
            }
        }




        // =================================================================================
        // Switch Characters
        private void InputSwitchCharacters()
        {
            if (Input.GetButtonDown(control.buttonRB))
            {
                if (inputState != InputState.HeroActionTri)
                {
                    InputState = InputState.Default;
                    c.CharacterHeroAction.Desactivate();
                }
                if (battlePartyManager.GetPlayerTime(battlePartyManager.GetIndexSelection()) < 100)
                {
                    battlePartyManager.CurrentCharacterInactive();
                    if (battleEnemyManager.CheckEnemyAttack() == true)
                    {
                        StopInput();
                        return;
                    }
                }
                PlaySound(switchCharacterClip);
                SwitchCharactersRight();
            }
            else if (Input.GetButtonDown(control.buttonLB))
            {
                if (inputState != InputState.HeroActionTri)
                {
                    InputState = InputState.Default;
                    c.CharacterHeroAction.Desactivate();
                }
                if (battlePartyManager.GetPlayerTime(battlePartyManager.GetIndexSelection()) < 100)
                {
                    battlePartyManager.CurrentCharacterInactive();
                    if (battleEnemyManager.CheckEnemyAttack() == true)
                    {
                        StopInput();
                        return;
                    }
                }
                PlaySound(switchCharacterClip);
                SwitchCharactersLeft();
            }
        }



        private void SwitchCharactersLeft()
        {
            UnsubscribeNewCharacter();
            battlePartyManager.FocusCameraLeft();
            c = battlePartyManager.GetCharacter();
            SubscribeNewCharacter();
        }

        private void SwitchCharactersRight()
        {
            UnsubscribeNewCharacter();
            battlePartyManager.FocusCameraRight();
            c = battlePartyManager.GetCharacter();
            SubscribeNewCharacter();
        }



        private void UnsubscribeNewCharacter()
        {
            c.CharacterDirection.Selected(false);
        }
        private void SubscribeNewCharacter()
        {
            c.CharacterDirection.Selected(true);
            aimReticle.SetMainColor(c.CharacterEquipement.GetWeapon());
            if (inputState == InputState.HeroActionTri)
                battlePartyManager.DrawOrder();
        }
        private void GetCharacter(int i)
        {
            UnsubscribeNewCharacter();
            c = battlePartyManager.GetCharacter(i);
            SubscribeNewCharacter();
        }


        // =================================================================================
        // Switch Targets
        private void InputSwitchTargets()
        {
            if (Input.GetAxis(control.dpadHorizontal) > 0 && padDown == false)
                SwitchTargetLeft();
            else if (Input.GetAxis(control.dpadHorizontal) < 0 && padDown == false)
                SwitchTargetRight();
            else if (Input.GetAxis(control.dpadHorizontal) == 0 && padDown == true)
                padDown = false;
        }

        private void SwitchTarget()
        {
            PlaySound(switchTargetClip);
            aimReticle.ResetAim();
            if(InputState == InputState.TriAttack)
            {
                if (switchTargetSlowMoCoroutine != null)
                    StopCoroutine(switchTargetSlowMoCoroutine);
                switchTargetSlowMoCoroutine = SwitchTargetSlowMo();
                StartCoroutine(switchTargetSlowMoCoroutine);
            }
        }

        private void SwitchTargetLeft()
        {
            SwitchTarget();
            padDown = true;
            battleTarget.TargetLeft();
        }
        private void SwitchTargetRight()
        {
            SwitchTarget();
            padDown = true;
            battleTarget.TargetRight();
        }

        private IEnumerator SwitchTargetSlowMo()
        {
            //InputState = InputState.NoInput;
            float t = 0f;
            while(t < 1f)
            {
                t += (Time.deltaTime);
                globalFeedbackManager.SetMotionSpeed(t);
                yield return null;
            }
            globalFeedbackManager.SetMotionSpeed(1);
            switchTargetSlowMoCoroutine = null;
            //InputState = InputState.TriAttack;
        }

        // =================================================================================

        public void NextTurn()
        {
            if (battleEnemyManager.CheckEnemyAttack() == true)
            {
                timeData.TimeFlow = false;
                StopInput();
            }
            else
            {
                NewTurn();
            }
        }

        public void NewTurn()
        {
            PlaySound(switchCharacterClip);
            InputState = InputState.Default;
            SwitchCharactersRight();
        }

        public void StopInput()
        {
            InputState = InputState.NoInput;
        }



        public void PlaySound(AudioClip s)
        {
            AudioManager.Instance.PlaySound(s);
        }
        // à dégager



        private void OnDestroy()
        {
            c.CharacterAction.OnEndAction -= EndShoot;
            c.CharacterTriAttack.OnEndAction -= EndTriAttack;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace