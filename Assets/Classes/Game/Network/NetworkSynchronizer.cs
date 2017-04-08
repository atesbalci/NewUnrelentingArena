using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnrelentingArena.Classes.Game.Models;
using UnrelentingArena.Classes.Game.Scripts;
using System.Linq;

namespace UnrelentingArena.Classes.Game.Network {
	public class NetworkSynchronizer : NetworkBehaviour {
		public GameSet Game { get; set; }
		public Dictionary<PlayerData, NetworkConnection> Players { get; set; }

		public override void OnStartServer() {
			base.OnStartServer();
			Game = new GameSet(true);
			Players = new Dictionary<PlayerData, NetworkConnection>();
		}

		public void OnClientConnect(NetworkConnection conn) {
			Game = new GameSet(false);
		}

		public void OnClientDisconnect(NetworkConnection conn) {
			Game = null;
			Players = null;
		}

		public void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
		}

		public void Update() {
			Game.Update(Time.deltaTime);
			if (Game.IsServer) {
				if (Game.CurrentRound.State == RoundState.Pre) {
					if (Game.Players.Where(x => x.Player.Dead.Value).FirstOrDefault() != null) {

					}
				}
			}
		}

		public void Spawn() {

		}
	}
}
}