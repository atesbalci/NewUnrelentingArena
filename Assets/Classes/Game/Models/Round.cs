using UniRx;
using System;
using UnrelentingArena.Classes.Utility;

namespace UnrelentingArena.Classes.Game.Models {
	public enum RoundState {
		None, Pre, Started, End
	}

	public class Round : GameModel {
		public GameSet Game { get; set; }
		public float Time { get; private set; }

		private RoundState _state;

		public Round() {
			Time = 0;
			_state = RoundState.None;
		}

		public RoundState State {
			get {
				return _state;
			}
			set {
				Time = 0;
				_state = value;
                if(State == RoundState.Started)
                {
                    foreach (var pl in Game.Players)
                    {
                        pl.Value.Player.Dead.Value = false;
                    }
                }
                Game.RoundStateChange(this, State);
			}
		}

		public void EndRound() {
            //TODO: End round
            foreach (var pl in Game.Players) {
                pl.Value.Player.Dead.Value = true;
            }
            State = RoundState.End;
		}

		public void Update(float delta) {
			Time += delta;
		}
	}
}