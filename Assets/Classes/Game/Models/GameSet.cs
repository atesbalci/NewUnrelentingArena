using UniRx;

namespace UnrelentingArena.Classes.Game.Models {
	public class PlayerData : GameModel {
		public Player Player { get; set; }
		public string Name { get; set; }
		public ReactiveProperty<int> Points { get; set; }
	}
	public class GameSet : GameModel {
		public PlayerData LocalPlayer { get; set; }
		public ReactiveCollection<PlayerData> Players { get; set; }
		public ReactiveProperty<int> RoundNo;
		public Round CurrentRound { get; set; }

		public GameSet() {
			RoundNo = new ReactiveProperty<int>(0);
			LocalPlayer = new PlayerData() {
				Player = new Player(),
				Name = "Player", //TODO: Get player name
				Points = new ReactiveProperty<int>(0)
			};
			Players = new ReactiveCollection<PlayerData>();
			Players.Add(LocalPlayer);
		}

		public void Update(float delta) {
			
		}
	}
}