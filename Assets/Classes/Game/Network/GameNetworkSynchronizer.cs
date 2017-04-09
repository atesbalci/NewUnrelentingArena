using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnrelentingArena.Classes.Game.Models;
using UnrelentingArena.Classes.Game.Scripts;
using System.Linq;
using UnrelentingArena.Classes.Utility;
using UniRx;

namespace UnrelentingArena.Classes.Game.Network {
	public class SendNameEvent : GameEvent {
		public uint Id { get; set; }
		public string Name { get; set; }
	}

	public class PlayerRemovedEvent : GameEvent {
		public uint Id { get; set; }
	}

	public class GameNetworkSynchronizer : GameNetworker {
		public GameSet Game { get; set; }

		public override void Start() {
			base.Start();
			MessageManager.ReceiveEvent<SendNameEvent>().Subscribe(ev => {
				if(isServer) {
					AddPlayer(ev.Id, ev.Name);
				}
			});
			MessageManager.ReceiveEvent<PlayerRemovedEvent>().Subscribe(ev => {
				if (isServer) {
					RemovePlayer(ev.Id);
				}
			});
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

		public void AddPlayer(uint id, string name) {
			Game.AddPlayer(id, name, GameSet.Colors[Game.Players.Count]);
			uint[] ids;
			string[] names;
			PlayersDictionaryToArrays(out ids, out names);
			RpcRefreshPlayerList(ids, names);
		}

		public void RemovePlayer(uint id) {
			Game.RemovePlayer(id);
			uint[] ids;
			string[] names;
			PlayersDictionaryToArrays(out ids, out names);
			RpcRefreshPlayerList(ids, names);
		}

		public override void Update() {
			base.Update();
			Game.Update(Time.deltaTime);
		}

		public void PlayersDictionaryToArrays(out uint[] ids, out string[] names) {
			var dict = Game.Players;
			int i = 0;
			ids = new uint[dict.Count];
			names = new string[dict.Count];
			foreach(var kvp in dict) {
				ids[i] = kvp.Key;
				names[i] = kvp.Value.Name;
				i++;
			}
		}

		[ClientRpc]
		public void RpcRefreshPlayerList(uint[] ids, string[] names) {
			Game.RefreshPlayers(ids, names);
			string test = "";
			foreach (var pl in Game.Players) {
				test += pl.Key + ":" + pl.Value.Name + ", ";
			}
			Debug.Log(test);
		}
	}
}