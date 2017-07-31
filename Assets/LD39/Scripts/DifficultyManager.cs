using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Xenon;
using Xenon.Processes;

namespace LD39 {
	[AddComponentMenu("LD39/Managers/Difficulty Manager")]
	public class DifficultyManager : Singleton<DifficultyManager> {
		
		/// <summary>
		/// -1: main menu
		/// 0: intro scene
		/// |
		/// 3: 1st Level
		/// </summary>
		public int currentDifficulty = -1;
		public Graphic blackFader;
		public GameObject creditsRoot;
		public GameObject mainMenuRoot;

		public ProcessManager procManager;

		public void Awake() {
			DontDestroyOnLoad(gameObject);
			procManager = new ProcessManager();
		}

		public void Start() {
			// FADE IN
			FadeInProcess fadeIn = new FadeInProcess(2f, blackFader);
			// TITLE
			CanvasFadeInProcess fadeInCredits = new CanvasFadeInProcess(1f, creditsRoot);
			fadeIn.Attach(fadeInCredits);
			TimedProcess creditsWait = new TimedProcess(1f);
			fadeInCredits.Attach(creditsWait);
			CanvasFadeOutProcess fadeOutCredits = new CanvasFadeOutProcess(1f, creditsRoot);
			creditsWait.Attach(fadeOutCredits);
			// FADE UI
			CanvasFadeInProcess fadeInUI = new CanvasFadeInProcess(2f, mainMenuRoot);
			fadeOutCredits.Attach(fadeInUI);

			procManager.LaunchProcess(fadeIn);
		}

		public void Update() {
			procManager.UpdateProcesses(Time.deltaTime);
		}

		public void NextLevel() {
			switch (currentDifficulty) {
				case -1: // Main Menu
					currentDifficulty = 0;
					SceneManager.LoadScene("intro");
					break;
				case 0: // Intro
					currentDifficulty = 3;
					SceneManager.LoadScene("level");
					break;
				default:
					currentDifficulty++;
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
					break;
			}
		}

		public void ExitGame() {
			Application.Quit();
		}

	}
}
