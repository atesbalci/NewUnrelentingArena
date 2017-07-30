namespace Game.Models {
	public class Round : GameModel {
		public GameSet Game { get; set; }
		public float Time { get; private set; }

		public Round() {
			Time = 0;
		}

		public void EndRound() {
            //TODO: End round
            foreach (var pl in Game.Players) {
                pl.Value.Player.Dead.Value = true;
            }
            Game.State = RoundState.End;
		}

		public void Update(float delta) {
			Time += delta;
		}
	}
}