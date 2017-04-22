using UniRx;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnrelentingArena.Classes.Utility;

namespace UnrelentingArena.Classes.Game.Models {
    public class GameStateChangedEvent : GameEvent {
        public Round Round;
        public int RoundNo;
        public RoundState State;
    }
	public class PlayerData : GameModel {
		public Player Player { get; set; }
		public string Name { get; set; }
		public ReactiveProperty<int> Points { get; set; }
        public bool Disconnected { get; set; }
	}
	public class GameSet : GameModel {
		public readonly static Color[] Colors = { Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow };
		
		public ReactiveDictionary<uint, PlayerData> Players { get; set; }
		public ReactiveProperty<int> RoundNo { get; set; }
		public Round[] Rounds { get; set; }
		public bool IsServer { get; set; }
        public bool Started { get; set; }

        private List<IDisposable> _disposables;

		public GameSet(bool isServer, int connectionId) {
            _disposables = new List<IDisposable>();
			IsServer = isServer;
			RoundNo = new ReactiveProperty<int>(0);
			Players = new ReactiveDictionary<uint, PlayerData>();
			Rounds = new Round[6];
            Started = false;
			for(int i = 0; i < Rounds.Length; i++)
				Rounds[i] = new Round();
		}

        ~GameSet()
        {
            foreach(var disp in _disposables)
            {
                disp.Dispose();
            }
        }

        public void StartGame()
        {
            Started = true;
            foreach (var player in Players)
            {
                _disposables.Add(player.Value.Player.Dead.Subscribe(val => {
                    if (val)
                    {
                        int aliveAmt = 0;
                        foreach (var pl in Players)
                        {
                            if (!pl.Value.Player.Dead.Value)
                                aliveAmt++;
                        }
                        if (aliveAmt <= 1)
                            Rounds[RoundNo.Value].EndRound();
                    }
                }));
            }
            CurrentRound.State = RoundState.Pre;
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
                Points = new ReactiveProperty<int>(0),
                Disconnected = false
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

        public void RoundStateChange(Round source, RoundState state) {
            if (source == CurrentRound) {
                MessageManager.SendEvent(new GameStateChangedEvent {
                    Round = source,
                    RoundNo = RoundNo.Value,
                    State = state
                });
            }
        }
	}
}