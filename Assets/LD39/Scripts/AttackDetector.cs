using UnityEngine;

namespace LD39 {
	public class AttackDetector : MonoBehaviour {

		public float range;
		public Character parent;
		private bool _isAttacking;
		public bool IsAttacking {
			get {
				return _isAttacking;
			}
			set {
				_isAttacking = value;
			}
		}

		private SphereCollider col;

		public void Start() {
			col = gameObject.AddComponent<SphereCollider>();
			col.isTrigger = true;
			col.radius = range;
		}

		public void TriggeredCol(LivingEntity ent) {
			if (!IsAttacking || ent.gameObject == parent.gameObject) return;
			ent.TakeDamage(parent.held);
		}

	}
}
