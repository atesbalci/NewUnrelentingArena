using UniRx;
using System;
using UnrelentingArena.Classes.Utility;

namespace UnrelentingArena.Classes.Game.Models {
	public class SpawnPlayerEvent : GameEvent {
		public Player Player { get; set; }
	}

	public class PlayerSpawnedEvent {
		public Player Player { get; set; }
	}

	public class Round : GameModel {
		public GameSet Game { get; set; }
		public float Time { get; private set; }
		public int SpawnedPlayerAmount { get; set; }
		public bool SpawnNextPlayer { get; set; }

		public Round() {
			Time = 0;
			foreach (var player in Game.Players) {
				player.Player.Dead.Subscribe(val => {
					int aliveAmt = 0;
					foreach (var pl in Game.Players) {
						if (!pl.Player.Dead.Value)
							aliveAmt++;
					}
					if (aliveAmt <= 1)
						EndRound();
				});
			}
			SpawnedPlayerAmount = 0;
			SpawnNextPlayer = true;
			MessageManager.ReceiveEvent<PlayerSpawnedEvent>().Subscribe(ev => {
				if (Game.Players[SpawnedPlayerAmount].Player == ev.Player) {
					SpawnedPlayerAmount++;
					SpawnNextPlayer = true;
				}
			});
		}

		private void EndRound() {
			//TODO: End round
		}

		public void Update(float delta) {
			Time += delta;
			if (SpawnedPlayerAmount < Game.Players.Count) {
				if (SpawnNextPlayer) {
					MessageManager.SendEvent(new SpawnPlayerEvent {
						Player = Game.Players[SpawnedPlayerAmount].Player
					});
					SpawnNextPlayer = false;
				}
			} else {

			}
		}
	}
}