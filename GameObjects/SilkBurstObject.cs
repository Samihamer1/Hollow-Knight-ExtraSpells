using ExtraSpells.MonoBehaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraSpells.GameObjects
{
    public class SilkBurstObject : MonoBehaviour
    {
        private FlashHitbox hitbox;

        private int damagenumber = 10;
        private int damagenumber2 = 25;
        private float shamanmultiplier = 1.5f;
        public void Awake()
        {
            Destroy(GetComponent<DamageHero>());
        }

        public void Start()
        {
            hitbox = gameObject.AddComponent<FlashHitbox>();
            hitbox.SetCollider(GetComponent<Collider2D>());

            int damage = damagenumber;
            float multiplier = 1;

            if (HeroController.instance.playerData.fireballLevel == 2)
            {
                damage = damagenumber2;
            }

            //shaman stone
            if (HeroController.instance.playerData.equippedCharm_19)
            {
                transform.localScale += new Vector3((float)-0.2, (float)-0.2);
                multiplier = shamanmultiplier;
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

            GameManager.instance.StartCoroutine(Hitboxes(hitinstance));

            ModHooks.AfterTakeDamageHook += ModHooks_AfterTakeDamageHook;
        }

        public void OnDestroy()
        {
            ModHooks.AfterTakeDamageHook -= ModHooks_AfterTakeDamageHook;
        }

        private int ModHooks_AfterTakeDamageHook(int hazardType, int damageAmount)
        {
            if (damageAmount == 0)
            {
                return 0;
            }

            Destroy(gameObject);

            return damageAmount-1;
        }

        private IEnumerator Hitboxes(HitInstance hitinstance)
        {
            for (int i = 0; i < 3; i++)
            {
                if (gameObject == null) { break; }
                hitbox.HitActivate(hitinstance);
                yield return new WaitForSeconds(0.25f);
            }
            Destroy(gameObject);
        }
    }
}
