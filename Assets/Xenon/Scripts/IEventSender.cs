namespace Xenon {
	public interface IEventSender {
		
	}

	public static class IEventSenderExt {

		public static void Send(this IEventSender sender, Event ev) {
			EventManager.I.SendEvent(sender, ev);
		}

	}
}
