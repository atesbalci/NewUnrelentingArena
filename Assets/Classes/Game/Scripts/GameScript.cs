using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnrelentingArena.Classes.Game.Models;

namespace UnrelentingArena.Classes.Game.Scripts {
	public abstract class GameScript : MonoBehaviour {
		public GameModel Model { get; set; }

		protected virtual void Start() { }
		protected virtual void Update() { }
	}
}