using UniRx;
using UnityEngine;

namespace UnrelentingArena.Classes.Game.Models {
	public class PlayerData : GameModel {
		public Player Player { get; set; }
		public string Name { get; set; }
		public ReactiveProperty<int> Points { get; set; }
	}
	public class GameSet : GameModel {
		public static Color[] Colors = { Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow };

		public PlayerData LocalPlayer { get; set; }
		public ReactiveCollection<PlayerData> Players { get; set; }
		public ReactiveProperty<int> RoundNo { get; set; }
		public Round CurrentRound { get; set; }
		public bool IsServer { get; set; }

		public GameSet(bool isServer) {
			IsServer = isServer;
			RoundNo = new ReactiveProperty<int>(0);
			Players = new ReactiveCollection<PlayerData>();
			AddPlayer("Name"); //TODO: Get player name
			LocalPlayer = Players[0];
		}

		public void AddPlayer(string name) {
			Players.Add(new PlayerData() {
				Player = new Player(),
				Name = name,
				Points = new ReactiveProperty<int>(0)
			});
		}

		public void Update(float delta) {
			if(CurrentRound != null)
				CurrentRound.Update(delta);
		}
	}
}