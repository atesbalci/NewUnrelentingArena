using UniRx;
using System;

namespace Assets.Classes.Game.Models {
	public class Round : GameModel {
		public GameSet Game { get; set; }
		public float Time { get; private set; }

		public Round() {
			Time = 0;
			foreach(var player in Game.Players) {
				player.Player.Dead.Subscribe(val => {
					int aliveAmt = 0;
					foreach(var pl in Game.Players) {
						if (!pl.Player.Dead.Value)
							aliveAmt++;
					}
					if (aliveAmt <= 1)
						EndRound();
				});
			}
		}

		private void EndRound() {
			//TODO: End round
		}

		public void Update(float delta) {
			Time += delta;
		}
	}
}