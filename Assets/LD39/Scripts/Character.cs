using UnityEngine;

namespace LD39 {
	[AddComponentMenu("LD39/Entities/Character")]
	public class Character : LivingEntity {

		public const string ANIM_RUNNING = "isRunning";
		public const string ANIM_ATTACK = "attack";
		public const string ANIM_HURT = "isHurt";
		public const string ANIM_DIE = "dies";
		public const float DEATH_TIMEOUT = 0f;

		public Weapon held;
		public float attackCenterDistance = .7f;

		private float prevAttackTime = 0f;

		protected Animator animator;
		protected AttackDetector attackDetector;

		public override void StartState() {
			base.StartState();
			animator = GetComponent<Animator>();
			if (animator == null) {
				animator = gameObject.AddComponent<Animator>();
			}
			if (held == null) {
				held = new Weapon();
			}
			CreateAttackDetector();
		}

		public override void TakeDamage(Weapon weapon) {
			base.TakeDamage(weapon);
			animator.SetTrigger(ANIM_HURT);

		}

		public override void Die() {
			base.Die();
			animator.SetTrigger(ANIM_DIE);
			GetComponent<CapsuleCollider>().enabled = false;
			Destroy(gameObject, DEATH_TIMEOUT);
		}

		public void CreateAttackDetector() {
			GameObject sphere = new GameObject();
			sphere.transform.position = transform.position + transform.forward * attackCenterDistance;
			attackDetector = sphere.AddComponent<AttackDetector>();
			attackDetector.range = held.range;
			attackDetector.parent = this;
			sphere.transform.SetParent(transform, true);
			//Vector3 nScale = sphere.transform.localScale;
			//nScale.x /= sphere.transform.parent.localScale.x;
			//nScale.y /= sphere.transform.parent.localScale.y;
			//nScale.z /= sphere.transform.parent.localScale.z;
			//sphere.transform.localScale = nScale;
		}

		public override void UpdateState() {
			base.UpdateState();
			if (attackDetector.IsAttacking) {
				attackDetector.IsAttacking = false;
			}
		}

		public void Attack() {
			if (!alive) return;
			//Debug.Log(Time.time + " - " + prevAttackTime + " < " + held.cooldown);
			//Debug.Log((Time.time - prevAttackTime) + " < " + held.cooldown);
			if (Time.time - prevAttackTime < held.cooldown) {
				return;
			}
			prevAttackTime = Time.time;
			animator.SetTrigger(ANIM_ATTACK);
			attackDetector.IsAttacking = true;
		}

	}
}
