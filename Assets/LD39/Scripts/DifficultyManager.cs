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
		/// 0: intro scene //infact win screen
		/// |
		/// 3: 1st Level
		/// </summary>
		public int currentDifficulty = -1;
		public Graphic blackFader;
		public GameObject creditsRoot;
		public GameObject mainMenuRoot;

		public ProcessManager procManager;

		private bool isInit = false;

		public void Awake() {
			if (DifficultyManager.I.isInit) {
				mainMenuRoot.SetActive(true);
				DestroyImmediate(this);
				return;
			}
			DontDestroyOnLoad(gameObject);
			procManager = new ProcessManager();
			isInit = true;
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
					currentDifficulty = 3;
					SceneManager.LoadScene("level");
					break;
				default:
					if (currentDifficulty >= 13) {
						currentDifficulty = 0;
						SceneManager.LoadScene("intro");
						break;
					}
					currentDifficulty++;
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
					break;
			}
		}

		public void BackToMenu() {
			currentDifficulty = -1;
			SceneManager.LoadScene("mainmenu");
		}

		public void ExitGame() {
			Application.Quit();
		}

	}
}
