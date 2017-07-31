using UnityEngine;
using Xenon.Processes;

namespace LD39 {
	public class CanvasFadeInProcess : InterpolateProcess {

		private GameObject root;
		private CanvasRenderer[] renderers;

		public CanvasFadeInProcess(float duration, GameObject root) : base(duration, 0f, 1f) {
			this.root = root;
			renderers = root.GetComponentsInChildren<CanvasRenderer>();
		}

		public override void OnBegin() {
			base.OnBegin();
			root.SetActive(true);
			TimeUpdated();
		}

		public override void TimeUpdated() {
			base.TimeUpdated();
			foreach (CanvasRenderer renderer in renderers) {
				renderer.SetAlpha(CurrentValue);
			}
		}

	}
}
