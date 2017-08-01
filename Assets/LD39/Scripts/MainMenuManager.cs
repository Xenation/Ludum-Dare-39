namespace LD39 {
	public class MainMenuManager : Singleton<MainMenuManager> {

		public void Start() {
			
		}

		public void StartBtn() {
			DifficultyManager.I.NextLevel();
		}

		public void QuitBtn() {
			DifficultyManager.I.ExitGame();
		}

	}
}
