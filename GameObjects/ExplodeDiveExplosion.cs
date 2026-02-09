using ExtraSpells.MonoBehaviours;
using GlobalEnums;
using Modding.Utils;
using System.Collections;

namespace ExtraSpells.GameObjects
{
    public class ExplodeDiveExplosion : MonoBehaviour
    {
        float damagenumber = 40;
        private FlashHitbox hitbox;

        public void Awake()
        {
            Destroy(GetComponent<DamageHero>());
            hitbox = gameObject.AddComponent<FlashHitbox>();
            hitbox.SetCollider(GetComponent<CircleCollider2D>());
        }

        public void Start()
        {
            PlayMakerFSM damagesEnemy = gameObject.LocateMyFSM("damages_enemy");
            if (damagesEnemy == null) { return; }
            Destroy(damagesEnemy);

            gameObject.layer = (int)PhysLayers.HERO_ATTACK;

            float multiplier = 1f;

            if (HeroController.instance.playerData.equippedCharm_19)
            {
                transform.localScale += new Vector3((float)0.3, (float)0.3);
                multiplier *= 1.25f;
            }

            if (HeroController.instance.playerData.fireballLevel == 2)
            {
                transform.localScale += new Vector3((float)0.3, (float)0.3);
                damagenumber = 55;
            }

            HitInstance hitinstance = new HitInstance
            {
                AttackType = AttackTypes.Spell,
                CircleDirection = false,
                DamageDealt = (int)(damagenumber * multiplier),
                MagnitudeMultiplier = 0,
                Direction = 0,
                IgnoreInvulnerable = true,
                IsExtraDamage = false,
                MoveAngle = 0,
                MoveDirection = false,
                Multiplier = 1,
                Source = gameObject,
                SpecialType = SpecialTypes.None
            };

            hitbox.HitActivate(hitinstance);
        }
    }
}
