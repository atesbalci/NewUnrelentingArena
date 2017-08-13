using UnityEngine;

namespace Game.Utility.Hexagon
{
    public class Hexagon : MonoBehaviour
    {
        public float TargetHeight;

        public Transform Trans { get; set; }
        public Vector3 Position { get; set; }

        private float _currentHeight;
        private HexTiler _tiler;

        public void Init(HexTiler tiler)
        {
            Trans = transform;
            TargetHeight = Trans.localScale.y;
            _currentHeight = Trans.localScale.y;
            Position = Trans.position;
            _tiler = tiler;
        }

        public void Refresh()
        {
            if (Abs(_currentHeight - TargetHeight) > 0.01f)
            {
                var scale = Trans.localScale;
                _currentHeight = Mathf.Lerp(scale.y, TargetHeight, Time.deltaTime * 5);
                Trans.localScale = new Vector3(scale.x, _currentHeight, scale.z);
            }
        }

        private static float Abs(float f)
        {
            return f > 0 ? f : -f;
        }
    }
}