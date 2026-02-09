using SpellChanger.AbilityClasses;
using SpellChanger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker.Actions;
using System.Collections;

namespace ExtraSpells.Spells
{
    public class SoulDraw
    {
        private static GameObject newslash;
        private static GameObject newhitbox;

        private static FsmObject DNSlashAudio;
        private static FsmObject NASlashAudio;
        public static void CreateSpell()
        {
            GetDNSlashAudio();
            GetNASlashAudio();

            Sprite sprite1 = ResourceLoader.LoadSprite("ExtraSpells.Resources.souldraw.png");
            Sprite[] sprites = new Sprite[] { sprite1 };
            CustomGreatSlash soulDraw = new CustomGreatSlash("SoulDraw", "INV_NAME_ART_SOULDRAW", "INV_DESC_ART_SOULDRAW", sprites);

            FsmOwnerDefault ownerDefault = Helper.GetKnightOwnerDefault();
            if (ownerDefault == null) { return; }

            FsmState soulDrawSlashState = soulDraw.CreateState("Soul Draw Start");
            soulDrawSlashState.AddMethod(() =>
            {
                GameManager.instance.StartCoroutine(CreateSlash());
            });
            soulDrawSlashState.AddAction(new SetVelocity2d { everyFrame = true, gameObject = ownerDefault, vector = new Vector2(), x = 0, y = 0 });
            soulDrawSlashState.AddAction(new Tk2dPlayAnimationWithEvents { animationCompleteEvent = FsmEvent.Finished, gameObject = ownerDefault, clipName = "DN Slash" });
            soulDrawSlashState.AddAction(new AudioPlaySimple { gameObject = ownerDefault, volume = 1f, oneShotClip = DNSlashAudio });
            soulDrawSlashState.AddAction(new AudioPlaySimple { gameObject = ownerDefault, volume = 1f, oneShotClip = NASlashAudio });

            SpellHelper.AddSpell(soulDraw, true);
        }

        private static IEnumerator CreateSlash()
        {
            //Make sure there's no remnants
            DestroySlash();

            //Hook destroy when hit
            ModHooks.AfterTakeDamageHook += CleanupSlash;

            //An attempt to find the dream nail slash effect.
            GameObject knight = HeroController.instance.gameObject;
            GameObject dfx = knight.Child("Dream Effects");
            if (dfx == null) { yield return null; }

            GameObject dreamslash = dfx.Child("Slash");
            GameObject dreamhitbox = dfx.Child("Hitbox");

            if (dreamslash == null || dreamhitbox == null) { yield return null; }

            //Now making a new version of it
            newslash = UnityEngine.Object.Instantiate(dreamslash, knight.transform);
            newhitbox = UnityEngine.Object.Instantiate(dreamhitbox, knight.transform);
            newslash.transform.localScale = new Vector3(1.5f, 2, 1.5f);
            newhitbox.transform.localScale = new Vector3(1.5f, 2, 1.5f);

            //Activate them
            newslash.SetActive(true);
            newhitbox.SetActive(true);

            PlayNAEffect();

            //Play anim
            newslash.GetComponent<tk2dSpriteAnimator>().Play("DN Slash");
            yield return Helper.WaitAnimDuration("DN Slash");

            //Cleanup
            DestroySlash();
            ModHooks.AfterTakeDamageHook -= CleanupSlash;
        }

        private static int CleanupSlash(int hazardType, int damageAmount)
        {
            DestroySlash();
            return damageAmount;
        }

        private static void DestroySlash()
        {
            if (newslash == null || newhitbox == null) { return; }

            UnityEngine.Object.Destroy(newslash);
            UnityEngine.Object.Destroy(newhitbox);
            newslash = null;
            newhitbox = null;
        }

        private static void GetDNSlashAudio()
        {
            PlayMakerFSM dnFSM = HeroController.instance.gameObject.LocateMyFSM("Dream Nail");
            if (dnFSM == null) { return; }

            DNSlashAudio = dnFSM.GetAction<AudioPlayerOneShotSingle>("Slash", 6).audioClip;
        }

        private static void GetNASlashAudio()
        {
            PlayMakerFSM naFSM = HeroController.instance.gameObject.LocateMyFSM("Nail Arts");
            if (naFSM == null) { return; }

            NASlashAudio = naFSM.GetAction<AudioPlay>("Dash Slash", 1).oneShotClip;
        }

        private static void PlayNAEffect()
        {
            GameObject fx = HeroController.instance.gameObject.Child("Effects");
            if (fx == null) { return; }

            GameObject sdSharpFlash = fx.Child("SD Sharp Flash");
            if (sdSharpFlash == null) { return; }

            sdSharpFlash.SetActive(true);
        }

    }
}
