using UniRx;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Classes.Game.Model {
	public class Player : GameModel {
		public ReactiveProperty<float> Health { get; set; }
		public ReactiveProperty<float> Energy { get; set; }
		public ReactiveProperty<bool> Dead { get; set; }

		public List<Skill> Skills { get; set; }
		public Color Color { get; set; }
	}
}
