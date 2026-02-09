using ExtraSpells.GameObjects;
using HutongGames.PlayMaker.Actions;
using SpellChanger;
using SpellChanger.AbilityClasses;

namespace ExtraSpells.Spells
{
    public static class SilkBurst
    {
        //An example of a purely method based spell.
        //No inbuild FSM actions.
        public static void CreateSpell()
        {
            Sprite sprite1 = ResourceLoader.LoadSprite("ExtraSpells.Resources.silkburst.png");
            Sprite[] sprites = new Sprite[] { sprite1, sprite1 };
            CustomScream silkBurst = new CustomScream("SilkBurst", "INV_NAME_SPELL_SILKBURST", "INV_DESC_SPELL_SILKBURST", sprites);

            FsmOwnerDefault ownerDefault = Helper.GetKnightOwnerDefault();
            PlayMakerFSM spellControl = silkBurst.storedFSM;

            //Antic state
            FsmState AnticState = silkBurst.CreateState("Silk Burst Antic");
            AnticState.AddMethod(() => {
                Helper.StopKnightControl();
                HeroController.instance.AffectedByGravity(false);
                GameManager.instance.StartCoroutine(Helper.SendEventAfterAnim("ANIM END", "Scream Start", spellControl));
            });
            AnticState.AddAction(new SetVelocity2d { everyFrame = true, gameObject = ownerDefault, vector = new Vector2(), x = 0, y = 0 });


            //Burst State
            FsmState BurstState = silkBurst.CreateState("Silk Burst");
            BurstState.AddAction(new Tk2dPlayAnimationV2 { gameObject = ownerDefault, clipName = "Collect Magical 2" });
            BurstState.AddMethod(() => {
                GameManager.instance.StartCoroutine(Helper.SendEventAfterTime("ANIM END", 0.75f, spellControl));

                GameObject silkburst = UnityEngine.Object.Instantiate(ResourceLoader.silkburstObject);
                silkburst.transform.parent = HeroController.instance.transform;
                silkburst.AddComponent<SilkBurstObject>();
                silkburst.transform.localPosition = Vector3.zero;
                silkburst.SetActive(true);
            });
            BurstState.AddAction(new SetVelocity2d { everyFrame = true, gameObject = ownerDefault, vector = new Vector2(), x = 0, y = 0 });
            BurstState.AddAction(new AudioPlaySimple { gameObject = ownerDefault, volume = 1f, oneShotClip = ResourceLoader.silkburstAudioOneShot });

            //End state
            FsmState EndState = silkBurst.CreateState("Silk Burst End");
            EndState.AddMethod(() => {
                Helper.StartKnightControl();
                HeroController.instance.AffectedByGravity(true);
            });

           
            silkBurst.AddTransition("ANIM END", "Silk Burst End", "Silk Burst");
            silkBurst.AddTransition("ANIM END", "Silk Burst", "Silk Burst Antic");


            SpellHelper.AddSpell(silkBurst, true);
        }
    }
}
