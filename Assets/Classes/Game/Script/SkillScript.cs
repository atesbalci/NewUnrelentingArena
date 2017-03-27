using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Classes.Game.Model;
using System;

namespace Assets.Classes.Game.Script {
	public abstract class SkillScript : GameScript {
		public float RemainingTime { get; set; }

		private bool _isDead;

		public Skill Skill {
			get {
				return Model as Skill;
			}
		}

		public override void Start() {
			base.Start();
			_isDead = false;
		}

		public override void Update() {
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

		public abstract void UpdateAlive();

		public abstract void UpdateDead();
	}
}