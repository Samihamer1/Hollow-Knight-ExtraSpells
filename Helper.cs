using HutongGames.PlayMaker.Actions;
using System.Collections;
using Vasi;

namespace ExtraSpells
{
    internal static class Helper
    {
        public static tk2dSpriteAnimator GetKnightAnimator()
        {
            return HeroController.instance.GetComponent<tk2dSpriteAnimator>();
        }

        public static void StopKnightControl()
        {
            HeroController.instance.RelinquishControl();
            HeroController.instance.StopAnimationControl();
        }

        public static void StartKnightControl()
        {
            HeroController.instance.RegainControl();
            HeroController.instance.StartAnimationControl();
        }

        public static FsmOwnerDefault GetKnightOwnerDefault()
        {
            PlayMakerFSM spellcontrol = HeroController.instance.spellControl;
            if (spellcontrol == null) { return null; }
            Tk2dPlayAnimationWithEvents action = spellcontrol.GetAction<Tk2dPlayAnimationWithEvents>("Fireball Antic",0);
            if (action == null) { return null; }
            return action.gameObject;
        }

        private static FsmEvent GetFsmEvent(this PlayMakerFSM fsm, string eventName)
        {
            foreach (FsmEvent Event in fsm.Fsm.Events)
            {
                if (Event.Name == eventName) { return Event; }

            }

            return null;
        }

        public static IEnumerator SendEventAfterAnim(string eventName, string anim, PlayMakerFSM fsm)
        {
            tk2dSpriteAnimator animator = GetKnightAnimator();
            
            yield return animator.PlayAnimWait(anim);
            
            FsmEvent @event = fsm.GetFsmEvent(eventName);
            if (@event == null)
            {
                fsm.SendEvent(eventName);
            } else
            {
                fsm.ChangeState(@event);
            }
        }

        public static IEnumerator SendEventAfterTime(string eventName, float time, PlayMakerFSM fsm)
        {
            yield return new WaitForSeconds(time);

            fsm.SendEvent(eventName);
        }

        public static void PlayAnim(string anim)
        {
            tk2dSpriteAnimator animator = GetKnightAnimator();

            animator.Play(anim);
        }

        public static IEnumerator PlayAnimWait(string anim)
        {
            tk2dSpriteAnimator animator = GetKnightAnimator();

            yield return animator.PlayAnimWait(anim);
        }

        public static IEnumerator WaitAnimDuration(string anim)
        {
            tk2dSpriteAnimator animator = GetKnightAnimator();

            tk2dSpriteAnimationClip clipByName = animator.GetClipByName(anim);
            yield return new WaitForSeconds(clipByName.Duration);
            yield return new WaitForEndOfFrame();
        }

        private static void CameraShake(string shake)
        {
            GameCameras.instance.gameObject.Child("CameraParent").GetComponent<PlayMakerFSM>().SendEvent(shake);
        }

        public static void SmallCameraShake()
        {
            CameraShake("SmallShake");
        }
        public static void AverageCameraShake()
        {
            CameraShake("AverageShake");
        }

        public static void BigCameraShake()
        {
            CameraShake("BigShake");
        }
    }
}
