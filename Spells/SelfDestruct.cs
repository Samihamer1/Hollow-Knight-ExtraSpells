using ExtraSpells.GameObjects;
using SpellChanger;
using SpellChanger.AbilityClasses;

namespace ExtraSpells.Spells
{
    public static class SelfDestruct
    {
        public static void CreateSpell()
        {
            Sprite sprite1 = ResourceLoader.LoadSprite("ExtraSpells.Resources.selfdestruct1.png");
            Sprite sprite2 = ResourceLoader.LoadSprite("ExtraSpells.Resources.selfdestruct2.png");
            Sprite[] sprites = new Sprite[] { sprite1, sprite2 };
            CustomQuake selfDestruct = new CustomQuake("SelfDestruct", "INV_NAME_SPELL_SELFDESTRUCT", "INV_DESC_SPELL_SELFDESTRUCT", sprites);

            FsmState SelfDestructState = selfDestruct.CreateState("Self Destruct");
            SelfDestructState.AddMethod(() =>
            {
                GameObject explosion = UnityEngine.Object.Instantiate(ResourceLoader.explosionObject);
                explosion.transform.position = HeroController.instance.transform.position;

                explosion.AddComponent<SelfDestructExplosion>();
                explosion.SetActive(true);

                Helper.BigCameraShake();
            });

            SpellHelper.AddSpell(selfDestruct, true);
        }
    }
}
