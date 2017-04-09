using UniRx;
using System;
using UnrelentingArena.Classes.Utility;

namespace UnrelentingArena.Classes.Game.Models {
	public enum RoundState {
		Pre, Started, End
	}

	public class Round : GameModel {
		public GameSet Game { get; set; }
		public float Time { get; private set; }

		private RoundState _state;

		public Round() {
			Time = 0;
			foreach (var player in Game.Players) {
				player.Value.Player.Dead.Subscribe(val => {
					if (val) {
						int aliveAmt = 0;
						foreach (var pl in Game.Players) {
							if (!pl.Value.Player.Dead.Value)
								aliveAmt++;
						}
						if (aliveAmt <= 1)
							EndRound();
					}
				});
			}
			State = RoundState.Pre;
		}

		public RoundState State {
			get {
				return _state;
			}
			set {
				Time = 0;
				_state = value;
			}
		}

		private void EndRound() {
			//TODO: End round
			State = RoundState.End;
			foreach (var pl in Game.Players) {
				pl.Value.Player.Dead.Value = true;
			}
		}

		public void Update(float delta) {
			Time += delta;
		}
	}
}