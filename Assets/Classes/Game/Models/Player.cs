﻿using UniRx;
using System.Collections.Generic;
using UnityEngine;

namespace UnrelentingArena.Classes.Game.Models {
	public class Player : GameModel {
		public ReactiveProperty<float> Health { get; set; }
		public ReactiveProperty<float> Energy { get; set; }
		public ReactiveProperty<bool> Dead { get; set; }
		public ReactiveProperty<float> CurrentSpeed { get; set; }

		public List<Skill> Skills { get; set; }
		public Color Color { get; set; }

		public Player(Color color) {
            Color = color;
            CurrentSpeed = new ReactiveProperty<float>(10);
			Dead = new ReactiveProperty<bool>(false);
            Health = new ReactiveProperty<float>();
            Energy = new ReactiveProperty<float>();
		}
	}
}
