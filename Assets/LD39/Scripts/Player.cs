using UnityEngine;

namespace LD39 {
	public enum PlayerState {
		SKINNY,
		NORMAL,
		FAT
	}

	[AddComponentMenu("LD39/Entities/Player")]
	public class Player : Character {

		public delegate void OnRoomChangeHandler(MapChunk nRoom);
		public event OnRoomChangeHandler OnRoomChange;

		public MapChunk currentChunk;

		public float fatness = 100f;
		public GameObject meshSkinny;
		public GameObject meshNormal;
		public GameObject meshFat;

		private PlayerState _state = PlayerState.NORMAL;
		public PlayerState State {
			get {
				return _state;
			}
			set {
				switch (_state) {
					case PlayerState.SKINNY:
						meshSkinny.SetActive(true);
						meshNormal.SetActive(false);
						meshFat.SetActive(false);
						break;
					case PlayerState.NORMAL:
						meshSkinny.SetActive(false);
						meshNormal.SetActive(true);
						meshFat.SetActive(false);
						break;
					case PlayerState.FAT:
						meshSkinny.SetActive(false);
						meshNormal.SetActive(false);
						meshFat.SetActive(true);
						break;
				}
				_state = value;
			}
		}

		public void Awake() {
			held = new Weapon();
			held.cooldown = .5f;
			held.range = 1f;
			held.damage = 1;
			held.knockback = 6f;
			health = 10;
		}

		public override void UpdateState() {
			base.UpdateState();
			if (!alive) return;
			MapChunk nChunk = MapManager.I.Grid.GetChunkAtWorldPos(transform.position);
			if (nChunk != currentChunk) {
				currentChunk = nChunk;
				if (OnRoomChange != null) {
					OnRoomChange.Invoke(nChunk);
				}
			}

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

			if (fatness > 150) {

			} else if (fatness < 50) {

			}
		}

		public void OnTriggerEnter(Collider other) {
			if (!alive) return;
			switch (other.tag) {
				case "Finish":
					DifficultyManager.I.NextLevel();
					break;
				case "Item":
					other.GetComponent<ItemEntity>().Triggered();
					break;
			}
		}

		public override void Die() {
			alive = false;
			animator.SetTrigger(ANIM_DIE);
			GetComponent<CapsuleCollider>().enabled = false;
		}

	}
}
