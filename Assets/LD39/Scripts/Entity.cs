using UnityEngine;

namespace LD39 {
	[AddComponentMenu("LD39/Entities/Entity")]
	public class Entity : MonoBehaviour {

		protected Rigidbody body;

		public void Start() {
			body = GetComponent<Rigidbody>();
			if (body == null) {
				body = gameObject.AddComponent<Rigidbody>();
			}
			body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY;
			body.useGravity = false;
			StartState();
		}

		public void Update() {
			UpdateState();
		}

		public virtual void StartState() {

		}

		public virtual void UpdateState() {

		}

	}
}
