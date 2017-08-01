using UnityEngine;

namespace LD39 {
	[AddComponentMenu("LD39/Entities/Item")]
	public class ItemEntity : Entity {

		public const float COL_RADIUS = .4f;

		public Food item;

		private SphereCollider col;

		public override void StartState() {
			base.StartState();
			body.isKinematic = true;
			tag = "Item";
			col = gameObject.AddComponent<SphereCollider>();
			col.isTrigger = true;
			col.radius = COL_RADIUS;
		}

		public void Triggered() {
			if (item == null) return;
			item.Pickup();
			Destroy(gameObject);
		}

	}
}
