using GlobalEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraSpells.MonoBehaviours
{
    public class FlashHitbox : MonoBehaviour
    {
        Collider2D collider;

        public void Awake()
        {
            //
        }

        public void SetCollider(Collider2D collider)
        {
            this.collider = collider;
        }

        public void HitActivate(HitInstance hitinstance)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;

            List<Collider2D> results = new List<Collider2D>();

            collider.OverlapCollider(filter, results);

            List<GameObject> hitobjects = new List<GameObject>();

            foreach (Collider2D col in results)
            {
                if (col.gameObject.layer == (int)PhysLayers.HERO_DETECTOR) { continue; }

                GameObject obj;
                HealthManager manager = col.gameObject.GetComponent<HealthManager>();
                if (manager == null)
                {
                    //edge case for pv, i guess
                    manager = col.GetComponentInParent<HealthManager>();
                }

                if (manager == null) { continue; }

                obj = manager.gameObject;

                if (hitobjects.Find(x => x == obj) != null) { continue; }

                hitobjects.Add(obj);

                manager.Hit(hitinstance);
            }
        }
    }
}
