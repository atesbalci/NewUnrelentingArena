using Game.Models;
using Game.Scripts;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Utility;

namespace Game.Network
{
    public class InitializePlayerAvatarEvent
    {
        public uint Id { get; set; }
        public string Name { get; set; }
    }

    public class PlayerRemovedEvent : GameEvent
    {
        public uint Id { get; set; }
        public bool IsMe { get; set; }
    }

    public class GameNetworkSynchronizer : GameNetworker
    {
        public ReactiveProperty<GameSet> Game { get; set; }
        public System.Collections.Generic.Dictionary<uint, PlayerScript> PlayerObjects { get; set; }

        public void Init()
        {
            Game = new ReactiveProperty<GameSet>();
            PlayerObjects = new System.Collections.Generic.Dictionary<uint, PlayerScript>();
            MessageManager.ReceiveEvent<InitializePlayerAvatarEvent>().Subscribe(ev =>
            {
                if (isServer)
                {
                    AddPlayer(ev.Id, ev.Name);
                }
            });
            MessageManager.ReceiveEvent<PlayerRemovedEvent>().Subscribe(ev =>
            {
                if (isServer)
                {
                    if (Game.Value.Started)
                    {
                        RemovePlayer(ev.Id);
                    }
                    else
                    {
                        RpcMarkDisconnected(ev.Id);
                    }
                }
            });
            MessageManager.ReceiveEvent<GameStateChangedEvent>().Subscribe(ev =>
            {
                if (isServer)
                {
                    RpcGameStateChange(ev.RoundNo, (int) ev.State);
                }
                if (ev.RoundNo == 0 && ev.State == RoundState.Pre)
                {
                    foreach (var kvp in Game.Value.Players)
                    {
                        var plObj = ClientScene.FindLocalObject(new NetworkInstanceId(kvp.Key))
                            .GetComponent<PlayerScript>();
                        PlayerObjects[kvp.Key] = plObj;
                        plObj.Model = kvp.Value.Player;
                    }
                }
                if (ev.State == RoundState.Pre)
                {
                    int i = 0;
                    float angleInc = (2 * Mathf.PI) / Game.Value.Players.Count;
                    const float dist = 10;
                    foreach (var pl in Game.Value.Players)
                    {
                        var spawnPos = Vector3.zero + dist *
                                       new Vector3(Mathf.Cos(angleInc * i), 0, Mathf.Sin(angleInc * i));
                        var plObj = PlayerObjects[pl.Key];
                        plObj.transform.position = spawnPos;
                        plObj.SetActivePlayer(true);
                        i++;
                    }
                }
            });
        }

        public void InitializeServer()
        {
            Game.Value = new GameSet(true);
        }

        public void InitializeClient(NetworkConnection conn)
        {
            if (!isServer)
            {
                Game.Value = new GameSet(false);
            }
        }

        public void Disconnect()
        {
            Game.Value = null;
        }

        public void AddPlayer(uint id, string name)
        {
            Game.Value.AddPlayer(id, name, GameSet.Colors[Game.Value.Players.Count]);
            uint[] ids;
            string[] names;
            PlayersDictionaryToArrays(out ids, out names);
            RpcRefreshPlayerList(ids, names);
        }

        public void RemovePlayer(uint id)
        {
            Game.Value.RemovePlayer(id);
            uint[] ids;
            string[] names;
            PlayersDictionaryToArrays(out ids, out names);
            RpcRefreshPlayerList(ids, names);
        }

        public override void Update()
        {
            base.Update();
            if (Game.Value != null)
            {
                Game.Value.Update(Time.deltaTime);
            }
        }

        public void PlayersDictionaryToArrays(out uint[] ids, out string[] names)
        {
            var dict = Game.Value.Players;
            int i = 0;
            ids = new uint[dict.Count];
            names = new string[dict.Count];
            foreach (var kvp in dict)
            {
                ids[i] = kvp.Key;
                names[i] = kvp.Value.Name;
                i++;
            }
        }

        [ClientRpc]
        public void RpcRefreshPlayerList(uint[] ids, string[] names)
        {
            Game.Value.RefreshPlayers(ids, names);
        }

        [ClientRpc]
        public void RpcMarkDisconnected(uint id)
        {
            Game.Value.Players[id].Disconnected = true;
        }

        [ClientRpc]
        public void RpcGameStateChange(int roundNo, int state)
        {
            if (!isServer)
            {
                Game.Value.RoundNo.Value = roundNo;
                Game.Value.State = (RoundState) state;
            }
        }
    }
}