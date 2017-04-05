using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnrelentingArena.Classes.Game.Models;
using UnrelentingArena.Classes.Game.Scripts;

namespace UnrelentingArena.Classes.Game.Network {
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