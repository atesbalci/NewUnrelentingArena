using Game.Network;
using Game.Views;
using UnityEngine;

namespace Game {
	public class GameController : MonoBehaviour {
        public GameNetworkSynchronizer Synchronizer;
        public GameUI GameUI;

        void Awake() {
            Synchronizer.Init();
            GameUI.Init(Synchronizer);
        }
	}
}