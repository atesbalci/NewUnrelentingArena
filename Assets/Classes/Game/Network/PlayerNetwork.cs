using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnrelentingArena.Classes.Game.Models;
using UnrelentingArena.Classes.Game.Scripts;
using UniRx;

namespace UnrelentingArena.Classes.Game.Network {
	public class PlayerNetwork : GameNetworker {
		//Sync stats
		[SyncVar(hook = "SyncHealth")]
		private float _health;
		[SyncVar(hook = "SyncEnergy")]
		private float _energy;

		//Sync transform data
		[SyncVar]
		private Vector2 _position;
		[SyncVar]
		private Quaternion _rotation;
		[SyncVar]
		private Vector2 _movementDirection;

		public Player Player {
			get {
				return Script.Model as Player;
			}
		}

		public PlayerScript PlayerScript {
			get {
				return Script as PlayerScript;
			}
		}

		public override void Start() {
			base.Start();
			_health = Player.Health.Value;
			_energy = Player.Energy.Value;
		}

		public override void OnStartLocalPlayer() {
			base.OnStartLocalPlayer();
			Player.Health.Subscribe(val => {
				_health = val;
			});
			Player.Energy.Subscribe(val => {
				_energy = val;
			});
		}

		public override void Update() {
			base.Update();
			if (!isLocalPlayer) {
				transform.position = Vector3.Lerp(transform.position,
					new Vector3(_position.x, transform.position.y, _position.y),
					Time.deltaTime * 10);
				PlayerScript.CurrentRotation.Value = _rotation;
				PlayerScript.CurrentMovement.Value = new Vector3(_movementDirection.x,
					PlayerScript.CurrentMovement.Value.y, _movementDirection.y);
			} else {
				_position = new Vector2(transform.position.x, transform.position.z);
				_rotation = PlayerScript.CurrentRotation.Value;
				_movementDirection = new Vector2(PlayerScript.CurrentMovement.Value.x,
					PlayerScript.CurrentMovement.Value.z);
			}
		}

		private void SyncHealth(float health) {
			Player.Health.Value = health;
		}
		private void SyncEnergy(float energy) {
			Player.Energy.Value = energy;
		}
	}
}