using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Utility;

namespace Game.Models
{
    public enum RoundState
    {
        None,
        Pre,
        Started,
        End
    }

    public class PlayerData : GameModel
    {
        public Player Player { get; set; }
        public string Name { get; set; }
        public ReactiveProperty<int> Points { get; set; }
        public bool Disconnected { get; set; }
    }

    public class GameStateChangedEvent : GameEvent
    {
        public Round Round { get; set; }
        public int RoundNo { get; set; }
        public RoundState State { get; set; }
    }

    public class PlayerListChangedEvent : GameEvent
    {
        public System.Collections.Generic.Dictionary<uint, PlayerData> Players { get; set; }
    }

    public class EndPreGameEvent : GameEvent
    {
    }

    public class GameSet : GameModel
    {
        public readonly static Color[] Colors =
            {Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow};

        public System.Collections.Generic.Dictionary<uint, PlayerData> Players { get; set; }
        public ReactiveProperty<int> RoundNo { get; set; }
        public Round[] Rounds { get; set; }
        public bool IsServer { get; set; }
        public bool Started { get; set; }

        private RoundState _state;
        private List<IDisposable> _disposables;

        public GameSet(bool isServer)
        {
            _disposables = new List<IDisposable>();
            _state = RoundState.None;
            IsServer = isServer;
            RoundNo = new ReactiveProperty<int>(0);
            Players = new System.Collections.Generic.Dictionary<uint, PlayerData>();
            Rounds = new Round[6];
            Started = false;
            for (int i = 0; i < Rounds.Length; i++)
                Rounds[i] = new Round();
            if (IsServer)
            {
                _disposables.Add(MessageManager.ReceiveEvent<EndPreGameEvent>()
                    .Subscribe(ev => { State = RoundState.Started; }));
            }
        }

        ~GameSet()
        {
            foreach (var disp in _disposables)
            {
                disp.Dispose();
            }
        }

        public RoundState State
        {
            get { return _state; }
            set
            {
                _state = value;
                switch (State)
                {
                    case RoundState.Pre:

                        break;
                    case RoundState.Started:
                        foreach (var pl in Players)
                        {
                            pl.Value.Player.Dead.Value = false;
                        }
                        break;
                    case RoundState.End:

                        break;
                }
                MessageManager.SendEvent(new GameStateChangedEvent
                {
                    RoundNo = RoundNo.Value,
                    State = State
                });
            }
        }

        public void StartGame()
        {
            Started = true;
            foreach (var player in Players)
            {
                _disposables.Add(player.Value.Player.Dead.Subscribe(val =>
                {
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
            State = RoundState.Pre;
        }

        public Round CurrentRound
        {
            get { return Rounds[RoundNo.Value]; }
        }

        public void AddPlayer(uint id, string name, Color color)
        {
            Players[id] = new PlayerData()
            {
                Player = new Player(color),
                Name = name,
                Points = new ReactiveProperty<int>(0),
                Disconnected = false
            };
        }

        public void RemovePlayer(uint id)
        {
            Players.Remove(id);
        }

        public void Update(float delta)
        {
            if (Started)
                CurrentRound.Update(delta);
        }

        public void RefreshPlayers(uint[] ids, string[] names)
        {
            Players.Clear();
            for (int i = 0; i < names.Length; i++)
            {
                AddPlayer(ids[i], names[i], Colors[i]);
            }
            MessageManager.SendEvent(new PlayerListChangedEvent
            {
                Players = Players
            });
        }
    }
}