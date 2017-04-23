using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnrelentingArena.Classes.Game.Models;
using UnrelentingArena.Classes.Game.Scripts;
using UniRx;
using UnrelentingArena.Classes.Utility;

namespace UnrelentingArena.Classes.Game.Network {
	public class PlayerNetwork : GameNetworker {
		public float SyncFrequency = 0.05f;

		//Sync stats
		[SyncVar(hook = "SyncHealth")]
		private float _health;
		[SyncVar(hook = "SyncEnergy")]
		private float _energy;

		//Sync transform data
		[SyncVar]
		private Quaternion _rotation;
		[SyncVar]
		private Vector2 _movementDirection;
		[SyncVar]
		private Vector2 _position;

		private float _timer;

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

		public override void OnStartLocalPlayer() {
			PlayerScript.IsLocalPlayer = true;
            CmdSendName(PlayerPrefs.GetString("PlayerName", "Player"));
        }

		public override void OnStartClient() {
			base.OnStartClient();
            PlayerScript.SetActivePlayer(false);
        }

		public override void Start() {
			base.Start();
			_timer = 0;
			_health = Player.Health.Value;
			_energy = Player.Energy.Value;

			if(isLocalPlayer) {
				Player.Health.Subscribe(val => {
					_health = val;
				});
				Player.Energy.Subscribe(val => {
					_energy = val;
				});
			}
		}

		public override void Update() {
			base.Update();
			if (!isLocalPlayer) {
				PlayerScript.CurrentRotation.Value = _rotation;
				PlayerScript.CurrentMovement.Value = new Vector3(_movementDirection.x,
					PlayerScript.CurrentMovement.Value.y, _movementDirection.y);
				transform.position = Vector3.Lerp(transform.position,
					new Vector3(_position.x, transform.position.y, _position.y), Time.deltaTime * 20);
			} else {
				_timer += Time.deltaTime;
				if (_timer > SyncFrequency) {
					CmdSyncPos(new Vector2(transform.position.x, transform.position.z));
					Vector3 mov = PlayerScript.CurrentMovement.Value;
					CmdSyncMovement(new Vector2(mov.x, mov.z));
					CmdSyncRot(PlayerScript.CurrentRotation.Value);
					_timer -= SyncFrequency;
				}
			}
		}

		private void SyncHealth(float health) {
			Player.Health.Value = health;
		}
		private void SyncEnergy(float energy) {
			Player.Energy.Value = energy;
		}

        public override void OnNetworkDestroy() {
			base.OnNetworkDestroy();
			if (isServer) {
                MessageManager.SendEvent(new PlayerRemovedEvent {
                    Id = netId.Value,
                    IsMe = isLocalPlayer
				});
			}
		}

		[Command]
		private void CmdSyncMovement(Vector2 mov) {
			_movementDirection = mov;
		}

		[Command]
		private void CmdSyncRot(Quaternion rot) {
			_rotation = rot;
		}

		[Command]
		private void CmdSyncPos(Vector2 pos) {
			_position = pos;
		}

		[Command]
		public void CmdSendName(string name) {
            MessageManager.SendEvent(new InitializePlayerAvatarEvent {
                Id = netId.Value,
                Name = name
			});
		}
	}
}