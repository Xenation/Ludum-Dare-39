using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LD39 {
	[AddComponentMenu("LD39/Entities/Enemy")]
	public class Enemy : Character {

		public const float AGENT_DEST_REFRESH_INTERVAL = .5f;
		public const float STOP_DIST = 1.25f;
		public const float ACCEL = 8f;
		public const float ATTACK_DELAY = .5f;

		public float attackRange = 1.1f;
		public bool inRange = false;
		public float inRangeTime = 0f;
		protected NavMeshAgent agent;

		public override void StartState() {
			base.StartState();
			agent = GetComponent<NavMeshAgent>();
			if (agent == null) {
				agent = gameObject.AddComponent<NavMeshAgent>();
			}
			agent.speed = speed;
			agent.acceleration = ACCEL;
			agent.stoppingDistance = STOP_DIST;
			StartCoroutine(UpdatePath());
		}

		public IEnumerator UpdatePath() {
			while (alive) {
				agent.SetDestination(EntityManager.I.player.transform.position);
				yield return new WaitForSeconds(AGENT_DEST_REFRESH_INTERVAL);
			}
		}

		public override void UpdateState() {
			base.UpdateState();
			if (!alive) {
				if (agent.enabled) {
					agent.isStopped = true;
					agent.enabled = false;
				}
				body.velocity = Vector3.zero;
				return;
			}

			if (knockVel != Vector3.zero) {
				agent.velocity += knockVel;
				knockVel = Vector3.zero;
			}

			if (!inRange) {
				animator.SetBool(ANIM_RUNNING, true);
			}

			if (!inRange && Vector3.Distance(transform.position, EntityManager.I.player.transform.position) < attackRange) {
				inRangeTime = Time.time;
				inRange = true;
				if (!inRange) {
					animator.SetBool(ANIM_RUNNING, false);
				}
			}
			if (inRange && Time.time - inRangeTime > ATTACK_DELAY) {
				//Quaternion q = transform.rotation;
				//q.eulerAngles = new Vector3(0, Vector3.Angle(Vector3.right, EntityManager.I.player.transform.position - transform.position), 0);
				//transform.rotation = q;
				transform.LookAt(new Vector3(EntityManager.I.player.transform.position.x, transform.position.y, EntityManager.I.player.transform.position.z));
				Attack();
				if (Vector3.Distance(transform.position, EntityManager.I.player.transform.position) > attackRange) {
					inRange = false;
				}
			}
			
			DebugPath();
		}

		public void DebugPath() {
			Vector3 prev = agent.nextPosition;
			foreach (Vector3 corner in agent.path.corners) {
				Debug.DrawLine(prev, corner, Color.green);
				prev = corner;
			}
		}

	}
}
