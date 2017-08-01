using UnityEngine;

namespace LD39 {
	[AddComponentMenu("LD39/Entities/Living Entity")]
	public class LivingEntity : Entity {

		public float health = 5;
		public float speed;

		protected bool alive = true;

		protected Vector3 knockVel = Vector3.zero;

		public override void UpdateState() {
			base.UpdateState();

		}

		public virtual void TakeDamage(Weapon weapon) {
			if (!alive) return;
			health -= weapon.damage * weapon.buff;
			if (health <= 0) {
				health = 0;
				Die();
			}
			//body.AddExplosionForce(weapon.knockback, transform.position + transform.forward, 10f);
			//body.AddForce((transform.forward * -1f) * weapon.knockback, ForceMode.VelocityChange);
			Vector3 delta = transform.position - EntityManager.I.player.transform.position;
			delta.Normalize();
			knockVel = delta * weapon.knockback;
		}

		public virtual void Die() {
			alive = false;
		}

		public void OnTriggerStay(Collider other) {
			AttackDetector attDetect = other.GetComponent<AttackDetector>();
			if (attDetect == null) return;
			if (tag == "Enemy" && attDetect.transform.parent.tag == "Enemy") return;
			attDetect.TriggeredCol(this);
		}

	}
}
