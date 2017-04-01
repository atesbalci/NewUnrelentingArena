using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnrelentingArena.Classes.Game.Model;
using UnrelentingArena.Classes.Utility;
using UniRx;

namespace UnrelentingArena.Classes.Game.Script {
	public class PlayerScript : GameScript {
		public Vector3ReactiveProperty CurrentMovement { get; set; }
		public bool IsLocalPlayer { get; set; }
		public Player Player {
			get {
				return Model as Player;
			}
		}

		private ControlManager _controlManager;

		protected override void Start() {
			base.Start();
			Model = new Player();
			CurrentMovement = new Vector3ReactiveProperty(Vector3.zero);
			_controlManager = ControlManager.Instance;
			if (_controlManager == null)
				_controlManager = new ControlManager();
			foreach (var rend in GetComponentsInChildren<Renderer>()) {
				foreach (var mat in rend.materials) {
					if (mat.name.Contains("PlayerColored")) {
						mat.color = Player.Color;
						mat.SetColor("_TintColor", Player.Color);
					}
				}
			}
			IsLocalPlayer = true;
		}

		protected override void Update() {
			base.Update();
			if (IsLocalPlayer) {
				MovementInputUpdate();
			}
			transform.position += CurrentMovement.Value * Player.CurrentSpeed.Value * Time.deltaTime;
		}

		private void MovementInputUpdate() {
			Vector3 movement = Vector3.zero;

			#region Get Input
			if (Input.GetKey(_controlManager.Keys[(int)GameKey.Up]))
				movement += Vector3.forward;
			if (Input.GetKey(_controlManager.Keys[(int)GameKey.Down]))
				movement += Vector3.back;
			if (Input.GetKey(_controlManager.Keys[(int)GameKey.Right]))
				movement += Vector3.right;
			if (Input.GetKey(_controlManager.Keys[(int)GameKey.Left]))
				movement += Vector3.left;
			#endregion

			CurrentMovement.Value = movement;
		}
	}
}