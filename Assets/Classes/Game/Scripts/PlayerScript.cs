using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnrelentingArena.Classes.Game.Models;
using UnrelentingArena.Classes.Utility;
using UniRx;

namespace UnrelentingArena.Classes.Game.Scripts {
	public class SkillCastBeginEvent : GameEvent { }
	public class SkillCastEvent {
		public Vector3 Direction { get; set; }
		public Skill Skill { get; set; }
	}

	public class PlayerScript : GameScript {
		public Vector3ReactiveProperty CurrentMovement { get; set; }
		public QuaternionReactiveProperty CurrentRotation { get; set; }
		public bool IsLocalPlayer { get; set; }
		public Player Player {
			get {
				return Model as Player;
			}
		}

		private ControlManager _controlManager;
		private Plane _plane;

		private void Awake() {
			IsLocalPlayer = false;
		}

		protected override void Start() {
			base.Start();
			CurrentMovement = new Vector3ReactiveProperty(Vector3.zero);
			CurrentRotation = new QuaternionReactiveProperty(Quaternion.identity);
			_controlManager = ControlManager.Instance;
			if (_controlManager == null)
				_controlManager = new ControlManager();
			_plane = new Plane(Vector3.up, transform.position);

			foreach (var rend in GetComponentsInChildren<Renderer>()) {
				foreach (var mat in rend.materials) {
					if (mat.name.Contains("PlayerColored")) {
						mat.color = Player.Color;
						mat.SetColor("_TintColor", Player.Color);
					}
				}
			}
		}

		protected override void Update() {
			base.Update();
			if (IsLocalPlayer) {
				MovementInputUpdate();
			}
			transform.position += CurrentMovement.Value * Player.CurrentSpeed.Value * Time.deltaTime;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, CurrentRotation.Value, Time.deltaTime * 360);
		}

		private void MovementInputUpdate() {
			Vector3 movement = Vector3.MoveTowards(CurrentMovement.Value, Vector3.zero, Time.deltaTime * 2);

			#region Get Input
			if (Input.GetKey(_controlManager.Keys[(int)GameKey.Up]))
				movement += Vector3.forward;
			if (Input.GetKey(_controlManager.Keys[(int)GameKey.Down]))
				movement += Vector3.back;
			if (Input.GetKey(_controlManager.Keys[(int)GameKey.Right]))
				movement += Vector3.right;
			if (Input.GetKey(_controlManager.Keys[(int)GameKey.Left]))
				movement += Vector3.left;

			float hitdist = 0.0f;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Quaternion rotation = transform.rotation;
			if (_plane.Raycast(ray, out hitdist)) {
				rotation = Quaternion.LookRotation(ray.GetPoint(hitdist) - transform.position, Vector3.up);
			}
			#endregion

			movement = Vector3.MoveTowards(Vector3.zero, movement, 1);
			CurrentMovement.Value = movement;
			CurrentRotation.Value = rotation;
		}
	}
}