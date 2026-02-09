using ExtraSpells.MonoBehaviours;
using GlobalEnums;
using Modding.Utils;
using System.Collections;

namespace ExtraSpells.GameObjects
{
    public class SelfDestructExplosion : MonoBehaviour
    {
        private FlashHitbox hitbox;
        private float damagenumberpermask = 30;
        private float damagenumberpermask2 = 40;
        private float damagenumberperlifebloodmask = 20;
        private float damagenumberperlifebloodmask2 = 30;
        private float shamanmultiplier = 1.5f;

        public void Awake()
        {
            Destroy(GetComponent<DamageHero>());
            transform.localScale += new Vector3((float)0.5, (float)0.5);
            hitbox = gameObject.AddComponent<FlashHitbox>();
            hitbox.SetCollider(GetComponent<CircleCollider2D>());
        }

        public void Start()
        {
            float damage = DamagePlayer();

            PlayMakerFSM damagesEnemy = gameObject.LocateMyFSM("damages_enemy");
            if (damagesEnemy == null) { return; }
            Destroy(damagesEnemy);

            gameObject.layer = (int)PhysLayers.HERO_ATTACK;

            float multiplier = 1f;

            if (HeroController.instance.playerData.equippedCharm_19)
            {
                transform.localScale += new Vector3((float)0.5, (float)0.5);
                multiplier *= shamanmultiplier;
            }

            if (HeroController.instance.playerData.fireballLevel == 2)
            {
                transform.localScale += new Vector3((float)0.5, (float)0.5);
            }

            HitInstance hitinstance = new HitInstance
            {
                AttackType = AttackTypes.Spell,
                CircleDirection = false,
                DamageDealt = (int)(damage * multiplier),
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

        private float DamagePlayer()
        {
            int masks = HeroController.instance.playerData.health;
            int lifeblood = HeroController.instance.playerData.healthBlue;

            float damagepermask = damagenumberpermask;
            float damageperlifeblood = damagenumberperlifebloodmask;

            if (HeroController.instance.playerData.fireballLevel == 2)
            {
                damagepermask = damagenumberpermask2;
                damageperlifeblood = damagenumberperlifebloodmask2;
            }

            float damagedealt = (damagepermask * masks) + (damageperlifeblood * lifeblood);

            int damagetaken = Math.Max(1, masks + lifeblood - 1);

            HeroController.instance.TakeDamage(null, CollisionSide.bottom, damagetaken, 0);

            return damagedealt;
        }
    }
}
