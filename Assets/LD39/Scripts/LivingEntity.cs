using UnityEngine;

namespace LD39 {
	[AddComponentMenu("LD39/Entities/Living Entity")]
	public class LivingEntity : Entity {

		public int health = 5;
		public float speed;

		protected bool alive = true;

		public override void UpdateState() {
			base.UpdateState();

		}

		public virtual void TakeDamage(Weapon weapon) {
			if (!alive) return;
			health -= weapon.damage;
			if (health <= 0) {
				health = 0;
				Die();
			}
			//body.AddExplosionForce(weapon.knockback, transform.position + transform.forward, 10f);
			body.AddForce((transform.forward * -1f) * weapon.knockback, ForceMode.Impulse);
		}

		public virtual void Die() {
			alive = false;
		}

		public void OnTriggerStay(Collider other) {
			AttackDetector attDetect = other.GetComponent<AttackDetector>();
			if (attDetect == null) return;
			attDetect.TriggeredCol(this);
		}

	}
}
