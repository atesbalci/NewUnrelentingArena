using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game.Utility.Hexagon
{
    public class HexRiser
    {
        public bool Active { get; set; }
        public Vector3 Location { get; set; }
    }

    public class HexTiler : MonoBehaviour
    {
        public GameObject HexPrefab;
        public float HexSpacing;
        public int Width;
        public int Height;

        public AnimationCurve HeightCurve;
        public float Radius;
        public float CloseHeight;
        public float FarHeight;

        private Hexagon[] _hexagons;
        private List<HexRiser> _risers;
        private List<Hexagon> _blacklistedHexagons;

        private void Start()
        {
            RefreshHexagons();
        }

        public void RefreshHexagons()
        {
            _blacklistedHexagons = new List<Hexagon>();
            var hexagons = new List<Hexagon>();
            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
                Destroy(transform.GetChild(i).gameObject);
            var midLine = new List<Hexagon>();
            float curx = 0;
            for (var i = 0; i < Width; i++)
            {
                if (i % 2 == 1)
                    curx += (3 * HexSpacing);
                var newHex = Instantiate(HexPrefab).GetComponent<Hexagon>();
                newHex.transform.SetParent(gameObject.transform);
                newHex.transform.localPosition = new Vector3(i % 2 == 0 ? curx : -curx, 0, 0);
                newHex.gameObject.name = "Hex";
                midLine.Add(newHex);
                hexagons.Add(newHex);
                newHex.Init(this);
            }
            var xshift = Mathf.Cos(Mathf.PI / 3) * HexSpacing + HexSpacing;
            var zshift = Mathf.Sin(Mathf.PI / 3) * HexSpacing;
            for (var i = 0; i < Height; i++)
            {
                var heightMult = (i / 2) + 1;
                foreach (var obj in midLine)
                {
                    var newHex = Instantiate(obj.gameObject).GetComponent<Hexagon>();
                    newHex.transform.SetParent(gameObject.transform);
                    newHex.transform.localPosition =
                        new Vector3(newHex.transform.localPosition.x, 0, newHex.transform.localPosition.z) +
                        new Vector3(i % 4 < 2 ? xshift : 0, 0, (i % 2 == 0 ? 1 : -1) * heightMult * zshift);
                    newHex.gameObject.name = "Hex";
                    hexagons.Add(newHex);
                    newHex.Init(this);
                }
            }
            _hexagons = hexagons.ToArray();
            _risers = new List<HexRiser>();
        }

        public void AddHexRiser(HexRiser riser)
        {
            _risers.Add(riser);
        }


        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Hexagon")))
                {
                    var riser = new HexRiser
                    {
                        Location = hit.collider.transform.position,
                        Active = true
                    };
                    var hex = hit.collider.GetComponent<Hexagon>();
                    if (!_blacklistedHexagons.Contains(hex))
                    {
                        AddHexRiser(riser);
                        _blacklistedHexagons.Add(hex);
                        Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(lng =>
                        {
                            riser.Active = false;
                            _blacklistedHexagons.Remove(hex);
                        });
                    }
                }
            }
            for(var i = 0; i < _risers.Count; i++)
            {
                if (!_risers[i].Active)
                {
                    _risers.RemoveAt(i);
                    i--;
                }
            }
            foreach (var hex in _hexagons)
            {
                hex.Refresh();
                hex.TargetHeight = FarHeight;
                foreach (var riser in _risers)
                {
                    var dist = DistanceSquare(hex.Position, riser.Location);
                    hex.TargetHeight = Mathf.Max(hex.TargetHeight,
                        Mathf.Lerp(CloseHeight, FarHeight, HeightCurve.Evaluate(Mathf.Min(1, dist / Radius))));
                }
                hex.Refresh();
            }
        }

        private float DistanceSquare(Vector3 a, Vector3 b)
        {
            return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
        }
    }
}