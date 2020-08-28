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
        NoInput,
        Skip,
        FeverDecision,
        FeverAim
    } 

    public class InputController: BaseInputController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Battle Input Manager")]
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
        FeverTimeManager feverTimeManager;
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


        [Title("À dégager peut-être aussi parce que input Controller il fait beaucoup de truc")]
        [SerializeField]
        public bool canMove = true;

        [SerializeField]
        public bool canAim = true;
        //[SerializeField]
        //bool canSwitchCharacters = true;
        [SerializeField]
        public bool canHeroAction = true;
        [SerializeField]
        public bool canTriAttack = true;

        PlayerCharacter c;

        float inputX = 0;
        float inputZ = 0;

        bool reverseDirection = false;
        bool padDown = false;
        bool preventB = false;


        private InputState inputState = InputState.NoInput;
        public InputState InputState
        {
            get { return inputState; }
            set { inputState = value; OnInputStateChanged.Invoke(inputState); }
        }

        private IEnumerator timeComboCoroutine = null;
        private IEnumerator switchTargetSlowMoCoroutine = null;

        public delegate void InputStateAction(InputState state);
        public event InputStateAction OnInputStateChanged;

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
            base.Start();
            feverTimeManager.OnFeverDecision += CallFeverDecision;
            triAttackManager.SetParty(battlePartyManager.GetParty());

            battleEnemyManager.OnEndAttacks += NewTurn;

            for (int i = 0; i < battlePartyManager.GetParty().Count; i++)
            {
                battlePartyManager.GetCharacter(i).CharacterAction.OnEndAction += EndShoot;
                battlePartyManager.GetCharacter(i).CharacterTriAttack.IdAttacker = i;
                battlePartyManager.GetCharacter(i).CharacterTriAttack.OnEndAction += EndTriAttack;

                battlePartyManager.GetCharacter(i).CharacterAction.SubscribeAttackControllers(feverTimeManager.CheckFever);
            }
            c = battlePartyManager.GetCharacter();
            timeData.TimeFlow = false;
        }

        private void Update()
        {
            if (c.CharacterMovement.CanInput == false)
            {
                return;
            }
            if(inputState != InputState.NoInput)
                cameraLock.MoveCamera(Input.GetAxis(control.stickRightHorizontal), Input.GetAxis(control.stickRightVertical));
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
                case InputState.Skip:
                    InputSkip();
                    break;

                case InputState.FeverDecision:
                    InputFeverDecision();
                    break;
                case InputState.FeverAim:
                    InputFeverAim();
                    break;
            }
        }


        private void UpdateDefault()
        {
            if(c.Interactions.Count != 0)
            {
                if(InputInteraction()) return;
            }
            InputMovement();
            if(InputAim()) return;
            InputHeroAction();
            InputSkip();
            InputTriAttackMode();
            InputSwitchCharacters();
            InputSwitchTargets();
        }

        private void UpdateMovement()
        {
            InputMovement();
            if (battleEnemyManager.EnemyList.Count == 0)
            {
                battlePartyManager.ReducePlayerTime(-20f);
                c.CharacterMovement.SetSpeed(c.CharacterStatController.GetStat("Speed") + 0.75f); // C'est nul, si un jour j'ajoute les status faudra utiliser ça à la place de cette ligne
            }

            if (battlePartyManager.ReducePlayerTime(20f) == true) // Donc le perso n'a plus de time
            {
                c.CharacterMovement.Move(0, 0);
                NextTurn();
            }

        }

        private void UpdateAiming()
        {
            if (battlePartyManager.ReducePlayerTime(20f) == true) // Donc le perso n'a plus de time
            {
                timeData.TimeFlow = false;
                cameraLock.LockOn(false);
                cameraLock.SetTarget(null);
                cameraLock.SetState(0);
                aimReticle.StopAim(c);
                NextTurn();
                return;
            }
            if (InputCancelAim()) return;
            if (InputShoot()) return;
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
            if (InputShoot()) return;
            InputTriAttackJump();
        }





        private bool InputInteraction()
        {
            c.Interactions[0].DrawInteract();
            if (Input.GetButtonDown(control.buttonA))
            {
                c.Interactions[0].Interact(c);
                return true;
            }
            return false;
        }


        private void InputMovement()
        {
            if (canMove == false)
                return;
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


        private void InputSkip()
        {
            if(Input.GetButton(control.buttonB) && preventB == false)
            {
                InputState = InputState.Skip;
                timeData.TimeFlow = true;
                if (battlePartyManager.ReducePlayerTime(150f) == true) // Donc le perso n'a plus de time
                {
                    c.CharacterMovement.Move(0, 0);
                    NextTurn();
                    preventB = true;
                }
            }
            else if(Input.GetButtonUp(control.buttonB))
            {
                preventB = false;
                InputState = InputState.Default;
                timeData.TimeFlow = false;
            }
        }


        // =================================================================================
        // Shoot

        private bool InputAim()
        {
            if (canAim == false)
                return false;
            if (Input.GetButtonDown(control.buttonA) && aimReticle.TargetAim != null)
            {
                timeData.TimeFlow = true;
                InputState = InputState.Aiming;
                c.CharacterDirection.LookAt(aimReticle.TargetAim);
                cameraLock.SetTarget(aimReticle.TargetAim);
                cameraLock.LockOn(true);
                cameraLock.SetState(1);
                aimReticle.AddCharacterAiming(c);
                aimReticle.StartAim();
                return true;
            }
            return false;
        }
        private bool InputShoot()
        {
            if (Input.GetButtonDown(control.buttonA) && aimReticle.TargetAim != null)
            {
                if (aimReticle.GetBulletNumber(c) >= 1 && c.CharacterAction.isAttacking() == false)
                {
                    //timeData.TimeFlow = false;
                    if (switchTargetSlowMoCoroutine != null)
                        StopCoroutine(switchTargetSlowMoCoroutine);
                    StopTimeComboCoroutine();
                    aimReticle.PauseAim();
                    aimReticle.HideAimReticle();
                    feverTimeManager.AssignShooter(c);
                    feverTimeManager.AssignTarget(battleEnemyManager.GetEnemyController(aimReticle.TargetAim));
                    c.CharacterAction.StartShoot(c.CharacterEquipement.GetWeaponAttackData(aimReticle.GetBulletNumber(c)), aimReticle.TargetAim);
                    InputState = InputState.NoInput;
                }
                return true;
            }
            return false;
        }

        private bool InputCancelAim()
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
                    EnemyAttack();
                }
                else
                {
                    InputState = InputState.Default;
                    preventB = true;
                }
                return true;
            }
            return false;
        }

        private void EndShoot()
        {
            //aimReticle.ShowAimReticle();
            if (cameraLock.GetTarget() == null) // On lock une nouvelle cible si la précédente est morte
            {
                aimReticle.ResetAim();
                battleTarget.TargetNearestEnemy();
                cameraLock.LockOn(true);
            }

            if (c.CharacterTriAttack.IsTriAttacking == true)
            {
                // Le joueur est en train de courir
                timeData.TimeFlow = true;
                if(c.CharacterAnimation.State == CharacterState.Jump)
                    cameraLock.SetState(4);
                else
                    cameraLock.SetState(3);
                aimReticle.ResetAim(c);
                InputState = InputState.TriAttack;
                if (triAttackManager.IsTriAttacking) // Si on tri attack on va check si les copains peuvent taper
                {
                    ComboTriAttack();
                }
                else
                {
                    feverTimeManager.ClearShooters();
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
                feverTimeManager.ClearShooters();
                feverTimeManager.ResetFill();
                NextTurn();
            }
        }




        // =================================================================================
        // HERO ACTION
        private void InputHeroAction()
        {
            if (canHeroAction == false)
                return;
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
                if(aimReticle.TargetAim != null)
                {
                    c.CharacterDirection.LookAt(aimReticle.TargetAim);
                    cameraLock.SetTarget(aimReticle.TargetAim);
                    cameraLock.LockOn(true);
                    aimReticle.AddCharacterAiming(c);
                    aimReticle.StartAim();
                    aimReticle.SetIndexMainCharacter(0);
                }
                cameraLock.SetState(3);

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
                preventB = true;
            }
        }



        // =================================================================================
        // Hero Action Tri
        private void InputTriAttackMode()
        {
            if (canTriAttack == false)
                return;
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
                if (aimReticle.TargetAim != null)
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
                preventB = true;
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
                    aimReticle.SetIndexMainCharacter(battlePartyManager.GetCharacter());
                    globalFeedbackManager.SetMotionSpeed(0.1f);
                    InputState = InputState.TriAttack;
                    cameraLock.SetTarget(aimReticle.TargetAim);

                    timeComboCoroutine = TimeComboCoroutine();
                    StartCoroutine(timeComboCoroutine);
                    return;
                }
            }
               
            // Si il n'y a pas de combo Tri attack on vide la liste pour que l'attaque combinée ne soit pas possible         
            feverTimeManager.ClearShooters();

            GetCharacter(triAttackManager.IndexLeader);
            battlePartyManager.SetIndexSelection(triAttackManager.IndexLeader);

            aimReticle.ResumeAim();
            aimReticle.SetIndexMainCharacter(battlePartyManager.GetCharacter());

            globalFeedbackManager.SetMotionSpeed(1f);
            InputState = InputState.TriAttack;
            cameraLock.SetTarget(aimReticle.TargetAim);
            timeComboCoroutine = null;

        }

        private IEnumerator TimeComboCoroutine()
        {
            yield return new WaitForSeconds(2f);
            
            // Si il n'y a pas de combo Tri attack on vide la liste pour que l'attaque combinée ne soit pas possible         
            feverTimeManager.ClearShooters();

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
                cameraLock.SetSmoothRotationWhenJump();
                cameraLock.SetState(4);
                c.CharacterTriAttack.Jump();
            }
        }

        // Event Call
        public void EndTriAttack(int id)
        {
            battlePartyManager.CharacterInactive(id);
            aimReticle.StopAim(battlePartyManager.GetCharacter(id));
            triAttackManager.EndTriAttack(id);
            StartCoroutine(EndTriAttackLag(id));
        }

        private IEnumerator EndTriAttackLag(int id)
        {
            if (triAttackManager.IsTriAttacking == true) // La tri attack n'est pas fini
            {
                yield return new WaitForSeconds(0.4f);
                if (id == battlePartyManager.GetIndexSelection()) // C'est le joueur qu'on contrôle donc on switch vers un perso qui lui est en train de triAttaquer
                {
                    for (int i = 0; i < battlePartyManager.GetParty().Count - 1; i++)
                    {
                        SwitchCharactersRight();
                        if (c.CharacterTriAttack.IsTriAttacking == true)
                        {
                            cameraLock.SetTarget(aimReticle.TargetAim);
                            yield break;
                        }
                    }
                }

            }
            else // La tri attack est fini on reset
            {
                cameraBlur.SetBool("Blur", false);
                cameraLock.LockOn(false);
                cameraLock.SetState(0);
                yield return new WaitForSeconds(0.4f);
                timeData.TimeFlow = false;
                battlePartyManager.HideOrder();
                if (battleEnemyManager.EnemyList.Count == 0)
                {
                    battlePartyManager.ResetPlayerTurn();
                    InputState = InputState.Default;
                    //SwitchCharactersLeft();
                    //NewTurn();
                }
                else
                {
                    NextTurn();
                }
                feverTimeManager.ResetFill();
            }
        }



        // =================================================================================
        // Switch Characters
        #region SwitchCharacter
        private void InputSwitchCharacters()
        {
            if (Input.GetButtonDown(control.buttonRB))
            {
                if (inputState != InputState.HeroActionTri)
                {
                    InputState = InputState.Default;
                    c.CharacterHeroAction.Desactivate();
                }
                if (battleEnemyManager.CheckEnemyAttack() == true)
                {
                    battlePartyManager.CurrentCharacterInactive();
                    EnemyAttack();
                    return;
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
                if (battleEnemyManager.CheckEnemyAttack() == true)
                {
                    battlePartyManager.CurrentCharacterInactive();
                    EnemyAttack();
                    return;
                }
                PlaySound(switchCharacterClip);
                SwitchCharactersLeft();
            }
        }


        private void UnsubscribeNewCharacter()
        {
            c.CharacterDirection.Selected(false);
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
        #endregion
        // =================================================================================

        // =================================================================================
        // Switch Targets
        #region SwitchTarget 
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
            float t = 0f;
            while(t < 1f)
            {
                t += (Time.deltaTime);
                globalFeedbackManager.SetMotionSpeed(t);
                yield return null;
            }
            globalFeedbackManager.SetMotionSpeed(1);
            switchTargetSlowMoCoroutine = null;
        }
        #endregion
        // =================================================================================



        // =================================================================================
        // Fever Time
        #region FeverTime 
        private void InputFeverDecision()
        {
            AcceptFeverTime();
            CancelFeverTime();
        }

        public void CallFeverDecision()
        {
            inputState = InputState.FeverDecision;
        }

        public void CancelFeverTime()
        {
            if (Input.GetButtonDown(control.buttonB))
            {
                feverTimeManager.RefuseFeverAim();
            }
        }

        public void AcceptFeverTime()
        {
            if(Input.GetButtonDown(control.buttonA))
            {
                feverTimeManager.StartFeverAim();
                cameraLock.SetState(5);
                inputState = InputState.FeverAim;
                //aimReticle.StopAim(c);
            }
        }


        // ====================================
        // Fever Time aim

        private void InputFeverAim()
        {
            FeverTimeShoot();
        }

        public void FeverTimeShoot()
        {
            if (Input.GetButtonDown(control.buttonA))
            {
                if(feverTimeManager.ShootFeverTime() == true)
                {
                    inputState = InputState.NoInput;
                }
            }
        }
        public void EndFeverTime()
        {

            cameraLock.SetState(5);
        }



        #endregion
        // =================================================================================


        // =================================================================================
        // Menu
        #region Menu
        /*private bool InputMenu()
        {
            if (Input.GetButtonDown(control.start))
            {
                MenuParty menuParty = null;
                if (battleEnemyManager.EnemyList.Count == 0)
                {
                    inputState = InputState.NoInput;
                    menuParty.OpenMainMenu();
                    return true;
                }
            }
            return false;
        }

        private void QuitMenu()
        {
            inputState = InputState.Default;
        }*/

        #endregion
        // =================================================================================



        public void NextTurn()
        {
            if (battleEnemyManager.CheckEnemyAttack() == true)
            {
                EnemyAttack();
            }
            else
            {
                NewTurn();
            }
        }

        public void NewTurn()
        {
            aimReticle.ShowAimReticle();
            PlaySound(switchCharacterClip);
            InputState = InputState.Default;
            SwitchCharactersRight();
        }

        public void StopInput()
        {
            InputState = InputState.NoInput;
        }

        public void StopInputAndTime()
        {
            InputState = InputState.NoInput;
            timeData.TimeFlow = false;
        }

        public void ResumeInput()
        {
            if (c.CharacterTriAttack.IsTriAttacking == true)
            {
                timeData.TimeFlow = true;
                InputState = InputState.TriAttack;
            }
            else
            {
                CancelAim();
                InputState = InputState.Default;
            }
        }


        public void EnemyAttack()
        {
            preventB = false;
            aimReticle.HideAimReticle();
            StopInputAndTime();
        }

        public void CancelAim()
        {
            timeData.TimeFlow = false;
            cameraLock.LockOn(false);
            cameraLock.SetTarget(null);
            cameraLock.SetState(0);
            aimReticle.StopAim(c);
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