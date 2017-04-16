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
		
		public ReactiveDictionary<uint, PlayerData> Players { get; set; }
		public ReactiveProperty<int> RoundNo { get; set; }
		public Round[] Rounds { get; set; }
		public bool IsServer { get; set; }

		public GameSet(bool isServer, int connectionId) {
			IsServer = isServer;
			RoundNo = new ReactiveProperty<int>(0);
			Players = new ReactiveDictionary<uint, PlayerData>();
			Rounds = new Round[6];
			for(int i = 0; i < Rounds.Length; i++)
				Rounds[i] = new Round();
		}
		
		public Round CurrentRound {
			get {
				return Rounds[RoundNo.Value];
			}
		}

		public void AddPlayer(uint id, string name, Color color) {
			Players[id] = new PlayerData() {
				Player = new Player(color),
				Name = name,
				Points = new ReactiveProperty<int>(0)
			};
		}

		public void RemovePlayer(uint id) {
			Players.Remove(id);
		}

		public void Update(float delta) {
			if(CurrentRound != null)
				CurrentRound.Update(delta);
		}

		public void RefreshPlayers(uint[] ids, string[] names) {
			Players.Clear();
			for(int i = 0; i < names.Length; i++) {
				AddPlayer(ids[i], names[i], Colors[i]);
			}
		}
	}
}