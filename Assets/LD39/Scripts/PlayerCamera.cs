using UnityEngine;

namespace LD39 {
	[AddComponentMenu("LD39/Camera")]
	public class PlayerCamera : MonoBehaviour {

		public Player player;
		public Vector3 offset;
		public float speed;

		public void Update() {
			if (player.currentChunk == null) return;
			Vector3 targetPos = player.currentChunk.transform.position + offset;
			transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
		}

	}
}
