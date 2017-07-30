using Game.Scripts;
using UnityEngine.Networking;

namespace Game.Network {
	public class GameNetworker : NetworkBehaviour {
		public GameScript Script;

		public virtual void Start() { }
		public virtual void Update() { }
		public override void OnStartServer() {
			base.OnStartServer();
		}
		public override void OnStartLocalPlayer() {
			base.OnStartLocalPlayer();
		}
		public override void OnStartClient() {
			base.OnStartClient();
		}
	}
}