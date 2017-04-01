﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnrelentingArena.Classes.Game.Model;

namespace UnrelentingArena.Classes.Game.Script {
	public abstract class GameScript : MonoBehaviour {
		public GameModel Model { get; set; }

		protected virtual void Start() { }
		protected virtual void Update() { }
	}
}