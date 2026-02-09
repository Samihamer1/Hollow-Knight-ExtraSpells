using ExtraSpells.MonoBehaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExtraSpells.GameObjects
{
    public class VenomSlashObject : MonoBehaviour
    {
        
        public void Awake()
        {
            foreach (PlayMakerFSM fsm in GetComponents<PlayMakerFSM>())
            {
                Destroy(fsm);
            }

            transform.localScale = new Vector3(1.6011f * HeroController.instance.transform.localScale.x, 1.6452f, 0);
            GetComponent<PolygonCollider2D>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<AudioSource>().Play();
            GetComponent<tk2dSpriteAnimator>().Play("SlashEffect");
            StartCoroutine(DestroyAfterAnim());

            VenomSlashHitbox hitbox = gameObject.AddComponent<VenomSlashHitbox>();
            hitbox.SetHitInstance(new HitInstance
            {
                AttackType = AttackTypes.Generic,
                CircleDirection = false,
                DamageDealt = (int)(HeroController.instance.playerData.nailDamage * 0.3f),
                MagnitudeMultiplier = 0,
                Direction = 0,
                IgnoreInvulnerable = true,
                IsExtraDamage = false,
                MoveAngle = 0,
                MoveDirection = false,
                Multiplier = 1,
                Source = gameObject,
                SpecialType = SpecialTypes.None
            });

        }

        private IEnumerator DestroyAfterAnim()
        {
            yield return new WaitForSeconds(0.15f);
            Destroy(gameObject);
        }
    }
}
