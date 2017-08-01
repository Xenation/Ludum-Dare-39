namespace LD39 {
	[System.Serializable]
	public class Food : Item {

		public float fatnessGain;

		public override void Pickup() {
			base.Pickup();
			EntityManager.I.player.fatness += fatnessGain;
			if (EntityManager.I.player.fatness > 200)
				EntityManager.I.player.fatness = 200;
		}

	}
}
