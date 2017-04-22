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
    public class InitializePlayerAvatarEvent {
        public uint Id { get; set; }
        public string Name { get; set; }
        public NetworkInstanceId PlayerObject { get; set; }
    }

	public class PlayerRemovedEvent : GameEvent {
		public uint Id { get; set; }
	}

	public class GameNetworkSynchronizer : GameNetworker {
		public ReactiveProperty<GameSet> Game { get; set; }

        public void Awake() {
            Game = new ReactiveProperty<GameSet>();
        }

        public override void Start() {
            base.Start();
            MessageManager.ReceiveEvent<InitializePlayerAvatarEvent>().Subscribe(ev => {
                if (isServer) {
                    AddPlayer(ev.Id, ev.Name);
                }
            });
			MessageManager.ReceiveEvent<PlayerRemovedEvent>().Subscribe(ev => {
                if (isServer) {
                    if (Game.Value.Started) {
                        RemovePlayer(ev.Id);
                    } else {
                        RpcMarkDisconnected(ev.Id);
                    }
				}
			});
            MessageManager.ReceiveEvent<GameStateChangedEvent>().Subscribe(ev => {
                if(ev.RoundNo == 0 && ev.State == RoundState.Pre) {
                    foreach(var kvp in Game.Value.Players) {
                        NetworkServer.FindLocalObject(new NetworkInstanceId(kvp.Key)).
                            GetComponent<PlayerScript>().Model = kvp.Value.Player;
                    }
                }
            });
		}

		public void InitializeServer() {
			base.OnStartServer();
			Game.Value = new GameSet(true, 0);
		}

		public void InitializeClient(NetworkConnection conn) {
			Game.Value = new GameSet(false, conn.connectionId);
        }

		public void Disconnect(NetworkConnection conn) {
			Game = null;
        }

		public void AddPlayer(uint id, string name) {
			Game.Value.AddPlayer(id, name, GameSet.Colors[Game.Value.Players.Count]);
			uint[] ids;
			string[] names;
			PlayersDictionaryToArrays(out ids, out names);
			RpcRefreshPlayerList(ids, names);
		}

		public void RemovePlayer(uint id) {
			Game.Value.RemovePlayer(id);
			uint[] ids;
			string[] names;
			PlayersDictionaryToArrays(out ids, out names);
			RpcRefreshPlayerList(ids, names);
		}

		public override void Update() {
			base.Update();
			Game.Value.Update(Time.deltaTime);
		}

		public void PlayersDictionaryToArrays(out uint[] ids, out string[] names) {
			var dict = Game.Value.Players;
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
			Game.Value.RefreshPlayers(ids, names);
		}

        [ClientRpc]
        public void RpcMarkDisconnected(uint id) {
            Game.Value.Players[id].Disconnected = true;
        }
	}
}