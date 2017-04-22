using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnrelentingArena.Classes.Game.Models;

namespace UnrelentingArena.Classes.Game.Views {
    public class GameView : MonoBehaviour {
        public GameModel Model { get; private set; }

        public virtual void Bind(GameModel model) {
            Model = model;
        }

        public virtual void Unbind() {
            Model = null;
        }

        protected virtual void Start() { }
        protected virtual void Update() { }
    }
}