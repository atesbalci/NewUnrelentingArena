#if UNITY_EDITOR
using Game.Utility.Hexagon;
using UnityEditor;
using UnityEngine;

namespace Classes.Editor
{
    [CustomEditor(typeof(HexTiler))]
    public class HexTilerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var hexTiler = (HexTiler) target;
            if (EditorApplication.isPlaying && GUILayout.Button("Refresh"))
            {
                hexTiler.RefreshHexagons();
            }
        }
    }
}
#endif
