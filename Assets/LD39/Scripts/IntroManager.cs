using UnityEngine;
using UnityEngine.UI;
using Xenon.Processes;

namespace LD39 {
	[AddComponentMenu("LD39/Managers/Intro")]
	public class IntroManager : Singleton<IntroManager> {

		public Graphic blackFader;

		public void Start() {
			FadeInProcess fadeIn = new FadeInProcess(1f, blackFader);
			TimedProcess wait = new TimedProcess(2f);
			fadeIn.Attach(wait);
			FadeOutProcess fadeOut = new FadeOutProcess(1f, blackFader);
			wait.Attach(fadeOut);
			fadeOut.TerminateCallback += DifficultyManager.I.BackToMenu;
			DifficultyManager.I.procManager.LaunchProcess(fadeIn);
		}

	}
}
