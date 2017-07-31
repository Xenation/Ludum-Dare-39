using UnityEngine;

namespace LD39 {
	[AddComponentMenu("LD39/Entities/Player")]
	public class Player : Character {

		public override void UpdateState() {
			base.UpdateState();
			if (!alive) return;
			Vector3 vel = new Vector3(0, 0, 0);

			float horiz = Input.GetAxisRaw("Vertical");
			if (horiz > 0) {
				vel += Vector3.forward * speed;
			} else if (horiz < 0) {
				vel -= Vector3.forward * speed;
			}
			float vert = Input.GetAxisRaw("Horizontal");
			if (vert > 0) {
				vel += Vector3.right * speed;
			} else if (vert < 0) {
				vel -= Vector3.right * speed;
			}

			if (vel != Vector3.zero) {
				transform.rotation = Quaternion.LookRotation(vel);
				animator.SetBool(ANIM_RUNNING, true);
			} else {
				animator.SetBool(ANIM_RUNNING, false);
			}
			body.velocity = vel;

			float attack = Input.GetAxisRaw("Attack");
			if (attack > 0) {
				Attack();
			}
		}

	}
}
