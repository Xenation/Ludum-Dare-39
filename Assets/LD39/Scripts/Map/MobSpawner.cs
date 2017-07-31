using UnityEngine;

namespace LD39.Map {
	[AddComponentMenu("LD39/Map/Spawner")]
	public class MobSpawner : MonoBehaviour {

		public GameObject mobPrefab;
		public float chance = .3f;

		public void Start() {
			//SpawnMob(0f);
		}

		public void SpawnMob(float probMult) {
			if (mobPrefab == null) return;
			float r = Random.Range(0f, 1f);
			if (r < chance * probMult) {
				GameObject mob = Instantiate(mobPrefab);
				mob.transform.position = transform.position;
			}
		}

	}
}
