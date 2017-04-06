using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnrelentingArena.Classes.Game.Models;

namespace UnrelentingArena.Classes.Game.Network {
	public class GameNetworkManager : NetworkManager {
		public GameSet Game { get; set; }

		public override void OnStartServer() {
			base.OnStartServer();
			Game = new GameSet(true);
		}

		public override void OnClientConnect(NetworkConnection conn) {
			base.OnClientConnect(conn);
			Game = new GameSet(false);
		}

		public override void OnClientDisconnect(NetworkConnection conn) {
			base.OnClientDisconnect(conn);
			Game = null;
		}

		public void Update() {
			Game.Update(Time.deltaTime);
		}
	}
}