using UniRx;

namespace UnrelentingArena.Classes.Utility {
	public abstract class GameEvent { }

	public class MessageManager {
		public static IObservable<T> ReceiveEvent<T>() {
			return MessageBroker.Default.Receive<T>();
		}

		public static void SendEvent<T>(T eve) {
			MessageBroker.Default.Publish(eve);
		}
	}
}