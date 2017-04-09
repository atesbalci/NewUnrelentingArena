using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnrelentingArena.Classes.Game.Models;
using UniRx;
using UnrelentingArena.Classes.Game.Scripts;
using UnityEngine.Networking.Match;
using System.Linq;

namespace UnrelentingArena.Classes.Game.Network {
	public class GameNetworkManager : NetworkManager {
		public GameNetworkSynchronizer Synchronizer;

		public override void OnStartServer() {
			base.OnStartServer();
			Synchronizer.InitializeServer();
		}

		public override void OnClientConnect(NetworkConnection conn) {
			base.OnClientConnect(conn);
			Synchronizer.InitializeClient(conn);
		}
	}
}