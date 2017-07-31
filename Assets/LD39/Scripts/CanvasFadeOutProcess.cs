using UnityEngine;
using Xenon.Processes;

namespace LD39 {
	public class CanvasFadeOutProcess : InterpolateProcess {

		private GameObject root;
		private CanvasRenderer[] renderers;

		public CanvasFadeOutProcess(float duration, GameObject root) : base(duration, 1f, 0f) {
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

		public override void OnTerminate() {
			base.OnTerminate();
			root.SetActive(false);
		}

	}
}
