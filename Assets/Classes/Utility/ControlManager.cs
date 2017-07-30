using UnityEngine;

namespace Utility {
	public enum GameKey {
		BasicAttack,
		SkillAttack,
		Up,
		Down,
		Right,
		Left,
		Dodge
	}

	public class ControlManager {
		public static ControlManager Instance { get; private set; }

		public KeyCode[] Keys { get; set; }

		public ControlManager() {
			Keys = new KeyCode[7];
			Load();
		}

		public void Load() {
			Keys[(int)GameKey.BasicAttack] = (KeyCode)PlayerPrefs.GetInt("BasicAttackButton", (int)KeyCode.Mouse0);
			Keys[(int)GameKey.SkillAttack] = (KeyCode)PlayerPrefs.GetInt("SkillAttackButton", (int)KeyCode.Mouse1);
			Keys[(int)GameKey.Up] = (KeyCode)PlayerPrefs.GetInt("UpButton", (int)KeyCode.W);
			Keys[(int)GameKey.Down] = (KeyCode)PlayerPrefs.GetInt("DownButton", (int)KeyCode.S);
			Keys[(int)GameKey.Right] = (KeyCode)PlayerPrefs.GetInt("RightButton", (int)KeyCode.D);
			Keys[(int)GameKey.Left] = (KeyCode)PlayerPrefs.GetInt("LeftButton", (int)KeyCode.A);
			Keys[(int)GameKey.Dodge] = (KeyCode)PlayerPrefs.GetInt("DodgeButton", (int)KeyCode.LeftShift);
		}

		public void Save() {
			PlayerPrefs.SetInt("BasicAttackButton", (int)Keys[(int)GameKey.BasicAttack]);
			PlayerPrefs.SetInt("SkillAttackButton", (int)Keys[(int)GameKey.SkillAttack]);
			PlayerPrefs.SetInt("UpButton", (int)Keys[(int)GameKey.Up]);
			PlayerPrefs.SetInt("DownButton", (int)Keys[(int)GameKey.Down]);
			PlayerPrefs.SetInt("RightButton", (int)Keys[(int)GameKey.Right]);
			PlayerPrefs.SetInt("LeftButton", (int)Keys[(int)GameKey.Left]);
			PlayerPrefs.SetInt("DodgeButton", (int)Keys[(int)GameKey.Dodge]);
			PlayerPrefs.Save();
		}
	}
}