namespace LD39 {
	public class Food : Item {

		public float fatnessGain;

		public override void Pickup() {
			base.Pickup();
			EntityManager.I.player.fatness += fatnessGain;
		}

	}
}
