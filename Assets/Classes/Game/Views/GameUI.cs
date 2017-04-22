using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnrelentingArena.Classes.Utility;
using UnrelentingArena.Classes.Game.Models;
using UnrelentingArena.Classes.Game.Network;
using UniRx;

namespace UnrelentingArena.Classes.Game.Views {
    public class GameUI : GameView {
        public GameNetworkSynchronizer Sync;

        [Header("Scores")]
        public RectTransform Scores;
        public RectTransform ScoresContent;
        public Button StartGameButton;

        protected override void Start() {
            base.Start();
            Sync.Game.AsObservable().Subscribe(val => {
                Bind(val);
            });
            RefreshUI();
            MessageManager.ReceiveEvent<GameStateChangedEvent>().Subscribe(ev => {
                RefreshUI();
            });
        }

        public override void Bind(GameModel model) {
            base.Bind(model);
            var game = Model as GameSet;
        }

        public void RefreshUI() {
            var game = Model as GameSet;
            if(game.CurrentRound.State == RoundState.None) {
                Scores.gameObject.SetActive(true);
                if(game.IsServer) {
                    StartGameButton.gameObject.SetActive(true);
                    StartGameButton.onClick.RemoveAllListeners();
                    StartGameButton.onClick.AddListener(() => {
                        game.StartGame();
                    });
                }
            }
        }
    }
}