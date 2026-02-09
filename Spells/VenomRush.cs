using ExtraSpells.GameObjects;
using HutongGames.PlayMaker.Actions;
using SpellChanger;
using SpellChanger.AbilityClasses;

namespace ExtraSpells.Spells
{
    public static class VenomRush
    {
        private static int slashes = 0;
        private static int slashmax = 5;
        public static void CreateSpell()
        {
            Sprite sprite1 = ResourceLoader.LoadSprite("ExtraSpells.Resources.shadesummon1.png");
            Sprite sprite2 = ResourceLoader.LoadSprite("ExtraSpells.Resources.shadesummon2.png");
            Sprite[] sprites = new Sprite[] { sprite1, sprite2 };
            CustomCycloneSlash venomRush = new CustomCycloneSlash("VenomRush", "INV_NAME_SPELL_VENOMRUSH", "INV_DESC_SPELL_VENOMRUSH", sprites);
            PlayMakerFSM NailArts = venomRush.storedFSM;
            FsmOwnerDefault ownerDefault = Helper.GetKnightOwnerDefault();

            FsmState SlashStartState = venomRush.CreateState("Venom Rush Start");
            SlashStartState.AddMethod(() =>
            {
                slashes = 0;
                HeroController.instance.RegainControl();
            });

            FsmState SlashCheckState = venomRush.CreateState("Venom Slash Check");
            SlashCheckState.AddMethod(() =>
            {
                if (slashes < slashmax)
                {
                    slashes++;
                    NailArts.SendEvent("SLASHCONTINUE");
                } else
                {
                    NailArts.SendEvent("ENDSLASH");
                }
            });

            FsmState RegularSlashState = venomRush.CreateState("Venom Slash");
            RegularSlashState.AddMethod(CreateAugmentedSlash);
            //RegularSlashState.AddAction(new SetVelocity2d { everyFrame = true, gameObject = ownerDefault, vector = new Vector2(), x = 0, y = 0 });
            RegularSlashState.AddMethod(() =>
            {
                Helper.PlayAnim("Slash");
                GameManager.instance.StartCoroutine(Helper.SendEventAfterTime("ANIMEND", 0.1F, NailArts));   
            });

            FsmState AltSlashState = venomRush.CreateState("Alt Venom Slash");

            FsmState SlashEnd = venomRush.CreateState("Slash End");

            venomRush.AddTransition("FINISHED", "Venom Slash Check", "Venom Rush Start");
            venomRush.AddTransition("ANIMEND", "Venom Slash Check", "Venom Slash");
            venomRush.AddTransition("SLASHCONTINUE", "Venom Slash", "Venom Slash Check");
            venomRush.AddTransition("ENDSLASH", "Slash End", "Venom Slash Check");

            SpellHelper.AddSpell(venomRush, true);
        }

        private static void CreateAugmentedSlash()
        {
            GameObject knight = HeroController.instance.gameObject;
            if (knight == null)
            {
                return;
            }

            //Really can't be too sure if people will mess with the poor little guy in their mods, so I have to check
            GameObject attacks = knight.Child("Attacks");

            if (attacks == null)
            {
                return;
            }

            GameObject slash = attacks.Child("Slash");

            if (slash == null)
            {
                return;
            }

            GameObject newslash = UnityEngine.Object.Instantiate(slash);
            newslash.AddComponent<VenomSlashObject>();

            newslash.transform.parent = knight.transform;
            newslash.transform.localPosition = new Vector3(-0.01f, - 0.41f, - 0.001f);



            newslash.SetActive(true);
        }
    }
}
