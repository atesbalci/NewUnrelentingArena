using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnrelentingArena.Classes.Game.Models;
using UnrelentingArena.Classes.Game.Scripts;
using System.Linq;

namespace UnrelentingArena.Classes.Game.Network {
	public class GameNetworkSynchronizer : GameNetworker {
		public GameSet Game { get; set; }
		public Dictionary<PlayerData, NetworkConnection> PlayerConnections { get; set; }

		public override void Start() {
			base.Start();
			InitializeServer();
		}

		public void InitializeServer() {
			base.OnStartServer();
			Game = new GameSet(true);
			PlayerConnections = new Dictionary<PlayerData, NetworkConnection>();
			PlayerConnections[Game.LocalPlayer] = null;
		}

		public void InitializeServer(NetworkConnection conn) {
			Game = new GameSet(false);
		}

		public void Disconnect(NetworkConnection conn) {
			Game = null;
			PlayerConnections = null;
		}

		public override void Update() {
			base.Update();
			Game.Update(Time.deltaTime);
		}
	}
}