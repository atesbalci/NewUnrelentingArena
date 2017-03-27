using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Classes.Game.Model;

namespace Assets.Classes.Game.Script {
	public abstract class GameScript : MonoBehaviour {
		public GameModel Model { get; set; }

		public virtual void Start() { }
		public virtual void Update() { }
	}
}