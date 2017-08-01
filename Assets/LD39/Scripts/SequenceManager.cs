using UnityEngine;
using UnityEngine.UI;
using Xenon;
using Xenon.Processes;

namespace LD39 {
	public class SequenceManager : Singleton<SequenceManager> {

		public Graphic blackFader;
		public Graphic gameoverText;

		public bool canControl = false;

		public ProcessManager procManager = new ProcessManager();

		public void Start() {

			FadeInProcess fadein = new FadeInProcess(2f, blackFader);
			fadein.TerminateCallback += OnFadeEnd;
			procManager.LaunchProcess(fadein);

		}

		public void Update() {
			procManager.UpdateProcesses(Time.deltaTime);
		}

		public void OnFadeEnd() {
			canControl = true;
		}

		public void Gameover() {
			FadeOutProcess fadeout = new FadeOutProcess(3f, blackFader);
			FadeOutProcess fadetext = new FadeOutProcess(1f, gameoverText);
			fadeout.Attach(fadetext);
			TimedProcess wait = new TimedProcess(5f);
			fadetext.Attach(wait);
			FadeInProcess fadetextout = new FadeInProcess(2f, gameoverText);
			fadetextout.TerminateCallback += OnGameoverEnd;
			wait.Attach(fadetextout);
			procManager.LaunchProcess(fadeout);
		}

		public void OnGameoverEnd() {
			DifficultyManager.I.BackToMenu();
		}

	}
}
