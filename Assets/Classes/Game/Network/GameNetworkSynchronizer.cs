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

		public override void Start() {
			base.Start();
			InitializeServer();
		}

		public void InitializeServer() {
			base.OnStartServer();
			Game = new GameSet(true, 0);
		}

		public void InitializeClient(NetworkConnection conn) {
			Game = new GameSet(false, conn.connectionId);
		}

		public void Disconnect(NetworkConnection conn) {
			Game = null;
		}

		public override void Update() {
			base.Update();
			Game.Update(Time.deltaTime);
		}

		[TargetRpc]
		public void TargetSynchronizePlayerList(NetworkConnection target, string[] name, int[] ids) {

		}
	}
}