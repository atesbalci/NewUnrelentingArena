using System;
using System.Linq;
using Game.Models;
using Game.Network;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game.Views {
    public class GameUI : GameView {
        [Header("Scores")]
        public RectTransform Scores;
        public RectTransform ScoresContent;
        public Button StartGameButton;
        public GameObject ScoreEntryPrefab;

        public void Init(GameNetworkSynchronizer sync) {
            sync.Game.AsObservable().Subscribe(val => {
                Bind(val);
                RefreshUI();
            });
            MessageManager.ReceiveEvent<GameStateChangedEvent>().Subscribe(ev => {
                RefreshUI();
            });
            MessageManager.ReceiveEvent<PlayerListChangedEvent>().Subscribe(ev => {
                ObserveScores(ev.Players);
            });
            RefreshUI();
        }

        public override void Bind(GameModel model) {
            if (model == null) {
                Unbind();
            } else {
                base.Bind(model);
                var game = Model as GameSet;
                ObserveScores(game.Players);
            }
        }

        public void RefreshUI() {
            Scores.gameObject.SetActive(false);

            if (Model != null) {
                var game = Model as GameSet;
                if (game.State == RoundState.None) {
                    StartGameButton.gameObject.SetActive(game.IsServer);
                    Scores.gameObject.SetActive(true);
                    if (game.IsServer) {
                        StartGameButton.onClick.RemoveAllListeners();
                        StartGameButton.onClick.AddListener(() => {
                            game.StartGame();
                        });
                    }
                }
            }
        }

        public void ObserveScores(System.Collections.Generic.Dictionary<uint, PlayerData> players) {
            Action refresh = () => {
                foreach (Transform t in ScoresContent) {
                    Destroy(t.gameObject);
                }
                foreach (var pl in players.OrderByDescending(x => x.Value.Points.Value)) {
                    var entry = Instantiate(ScoreEntryPrefab, ScoresContent, false).transform;
                    entry.GetChild(0).GetComponent<Text>().text = pl.Value.Name;
                    entry.GetChild(1).GetComponent<Text>().text = pl.Value.Points + "";
                }
                LayoutRebuilder.ForceRebuildLayoutImmediate(Scores);
            };
            foreach(var pl in players) {
                pl.Value.Points.AsObservable().Subscribe(val => {
                    refresh();
                });
            }
            refresh();
        }
    }
}