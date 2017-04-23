using UniRx;
using UnrelentingArena.Classes.Game.Models;
using UnrelentingArena.Classes.Game.Network;
using UnrelentingArena.Classes.Game.Views;
using UnityEngine;

namespace UnrelentingArena.Classes.Game {
	public class GameController : MonoBehaviour {
        public GameNetworkSynchronizer Synchronizer;
        public GameUI GameUI;

        void Awake() {
            Synchronizer.Init();
            GameUI.Init(Synchronizer);
        }
	}
}