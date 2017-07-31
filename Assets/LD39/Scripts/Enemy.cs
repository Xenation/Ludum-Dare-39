using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LD39 {
	[AddComponentMenu("LD39/Entities/Enemy")]
	public class Enemy : Character {

		public const float AGENT_DEST_REFRESH_INTERVAL = .5f;
		public const float STOP_DIST = 1.25f;
		public const float ACCEL = 100f;

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

			body.velocity = Vector3.zero;
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
