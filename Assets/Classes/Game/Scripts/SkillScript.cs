using Game.Models;
using UnityEngine;

namespace Game.Scripts {
	public abstract class SkillScript : GameScript {
		public float RemainingTime { get; set; }

		private bool _isDead;

		public Skill Skill {
			get {
				return Model as Skill;
			}
		}

		protected override void Start() {
			base.Start();
			_isDead = false;
			Color color = Skill.Player.Color;
			foreach (var rend in GetComponentsInChildren<Renderer>()) {
				foreach (var mat in rend.materials) {
					if (mat.name.Contains("PlayerColored")) {
						mat.color = color;
						mat.SetColor("_TintColor", color);
					}
				}
			}
		}

		protected override void Update() {
			base.Update();
			if (!_isDead) {
				UpdateAlive();
				RemainingTime -= Time.deltaTime;
				if (RemainingTime <= 0)
					Die();
			} else {
				UpdateDead();
			}
		}

		public virtual void Die() {
			_isDead = true;
		}

		protected abstract void UpdateAlive();

		protected abstract void UpdateDead();
	}
}