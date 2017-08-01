using UnityEngine;
using UnityEngine.UI;

namespace LD39 {
	public enum PlayerState {
		SKINNY,
		NORMAL,
		FAT
	}

	[AddComponentMenu("LD39/Entities/Player")]
	public class Player : Character {

		public const float FAT_LOWER_LIMIT = 50f;
		public const float FAT_UPPER_LIMIT = 150f;
		public const float FAT_LOSS = 1.5f;

		public delegate void OnRoomChangeHandler(MapChunk nRoom);
		public event OnRoomChangeHandler OnRoomChange;

		public MapChunk currentChunk;

		public Slider healthSlider;
		public Slider hungerSlider;

		public float fatness = 100f;
		public GameObject meshSkinny;
		public GameObject meshNormal;
		public GameObject meshFat;
		public float skinySpeed = 6f;
		public float normalSpeed = 4.5f;
		public float fatSpeed = 3.5f;
		public float skinyBuff = .8f;
		public float normalBuff = 1f;
		public float fatBuff = 1.5f;
		public float skinyWeakness = 3.5f;
		public float normalWeakness = 1f;
		public float fatWeakness = 0.5f;

		private float weakness = 1f;

		private Vector3 prevPos;

		private PlayerState _state = PlayerState.NORMAL;
		public PlayerState State {
			get {
				return _state;
			}
			set {
				switch (value) {
					case PlayerState.SKINNY:
						meshSkinny.SetActive(true);
						meshNormal.SetActive(false);
						meshFat.SetActive(false);
						speed = skinySpeed;
						held.buff = skinyBuff;
						weakness = skinyWeakness;
						break;
					case PlayerState.NORMAL:
						meshSkinny.SetActive(false);
						meshNormal.SetActive(true);
						meshFat.SetActive(false);
						speed = normalSpeed;
						held.buff = normalBuff;
						weakness = normalWeakness;
						break;
					case PlayerState.FAT:
						meshSkinny.SetActive(false);
						meshNormal.SetActive(false);
						meshFat.SetActive(true);
						speed = fatSpeed;
						held.buff = fatBuff;
						weakness = fatWeakness;
						break;
				}
				_state = value;
			}
		}

		public void Awake() {
			prevPos = transform.position;
			held = new Weapon();
			held.cooldown = .5f;
			held.range = 1f;
			held.damage = 1;
			held.knockback = 6f;
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
			if (!SequenceManager.I.canControl) return;

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

			switch (State) {
				case PlayerState.SKINNY:
					if (fatness > FAT_LOWER_LIMIT) {
						State = PlayerState.NORMAL;
					}
					break;
				case PlayerState.NORMAL:
					if (fatness < FAT_LOWER_LIMIT) {
						State = PlayerState.SKINNY;
					} else if (fatness > FAT_UPPER_LIMIT) {
						State = PlayerState.FAT;
					}
					break;
				case PlayerState.FAT:
					if (fatness < FAT_UPPER_LIMIT) {
						State = PlayerState.NORMAL;
					}
					break;
			}

			if (fatness > 0) {
				float dist = Vector3.Distance(prevPos, transform.position);
				fatness -= FAT_LOSS * dist;
				prevPos = transform.position;
				if (fatness < 0) {
					fatness = 0;
				}
			}

			hungerSlider.value = (fatness <= 200) ? fatness/200 : 1;

			healthSlider.value = (health != 0) ? 1 / (10 - health) : 1;
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
			body.velocity = Vector3.zero;
			SequenceManager.I.Gameover();
		}

		public override void TakeDamage(Weapon weapon) {
			if (!alive) return;
			health -= weapon.damage * weapon.buff * weakness;
			if (health <= 0) {
				health = 0;
				Die();
			}
		}

	}
}
