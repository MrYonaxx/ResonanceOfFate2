using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "FeedbackManagerData", menuName = "FeedbackManager", order = 1)]
    public class GlobalFeedbackManager : ScriptableObject
    {
        //!\ S'assurer que Clear la liste au changement de scene /!\
        private List<IMotionSpeed> charactersScene = new List<IMotionSpeed>();
        public List<IMotionSpeed> CharactersScene
        {
            get { return charactersScene; }
        }


        /*private CameraBattleController cameraController;
        private Shake cameraShake;
        private RippleEffect rippleEffect;*/



        /*public void AssignComponent(BattleFeedbackManagerData battleFeedbackManager)
        {
            cameraController = battleFeedbackManager.CameraController;
            cameraShake = battleFeedbackManager.CameraShake;
            rippleEffect = battleFeedbackManager.RippleEffect;
        }*/

        public void AddCharacter(IMotionSpeed character)
        {
            charactersScene.Add(character);
        }

        public void RemoveCharacter(IMotionSpeed character)
        {
            charactersScene.Remove(character);
        }








        /*public void RippleEffect(float x, float y)
        {
            Vector3 pos = cameraController.CameraComponent.WorldToViewportPoint(new Vector3(x, y, y));
            rippleEffect.EmitRipple(pos.x, pos.y);
        }

        public void ShakeScreen()
        {
            cameraShake.ShakeEffect();
        }

        public void ShakeScreen(float power, int time)
        {
            cameraShake.ShakeEffect(power, time);
        }

        public void CameraZoom()
        {
            cameraController.Zoom();
        }
        public void CameraBigZoom()
        {
            cameraController.BigZoom();
        }

        public void SetCameraPriorityFocus(Transform newFocus)
        {
            cameraController.SetFocusPriority(newFocus);
        }*/

        public void SetMotionSpeed(float motionSpeed, float time)
        {
            for (int i = 0; i < charactersScene.Count; i++)
            {
                charactersScene[i].SetCharacterMotionSpeed(motionSpeed, time);
            }
        }

        public void SetMotionSpeed(float motionSpeed)
        {
            for (int i = 0; i < charactersScene.Count; i++)
            {
                charactersScene[i].SetCharacterMotionSpeed(motionSpeed);
            }
        }


    }
}
