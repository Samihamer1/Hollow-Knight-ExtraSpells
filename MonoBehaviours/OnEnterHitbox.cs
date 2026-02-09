using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraSpells.MonoBehaviours
{
    public class OnEnterHitbox : MonoBehaviour
    {
        private HitInstance hitInstance;

        public void SetHitInstance(HitInstance hitInstance)
        {
            this.hitInstance = hitInstance;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            HealthManager hm = col.gameObject.GetComponent<HealthManager>();

            if (hm == null) { return; }

            hm.Hit(hitInstance);
            OnHit(hm);
        }

        public virtual void OnHit(HealthManager hm) { }
    }
}
