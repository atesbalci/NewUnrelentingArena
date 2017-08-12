using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HexTiler : MonoBehaviour {
	public GameObject hexPrefab;
	public float hexSideLength;
	public int width;
	public int height;
	public bool refresh;

	public float radius = 10;
	public float closeHeight = 10;
	public float farHeight = 1;

	private Vector4[] radialRises;
	private int riseAmt;
	private Hexagon[] hexagons;

	void Start() {
		radialRises = new Vector4[10];
		riseAmt = 0;
		RefreshHexagons();
	}

	public void RefreshHexagons() {
		Shader.SetGlobalFloat("Radius", radius);
		Shader.SetGlobalFloat("CloseHeight", closeHeight);
		Shader.SetGlobalFloat("FarHeight", farHeight);
		List<Hexagon> hexagons = new List<Hexagon>();
		int childCount = transform.childCount;
		for (int i = 0; i < childCount; i++)
			Destroy(transform.GetChild(i).gameObject);
		List<Hexagon> midLine = new List<Hexagon>();
		float curx = 0;
		for (int i = 0; i < width; i++) {
			if (i % 2 == 1)
				curx += (3 * hexSideLength);
			Hexagon newHex = Instantiate(hexPrefab).GetComponent<Hexagon>();
			newHex.transform.SetParent(gameObject.transform);
			newHex.transform.localPosition = new Vector3(i % 2 == 0 ? curx : -curx, 0, 0);
			newHex.gameObject.name = "Hex";
			newHex.gameObject.layer = gameObject.layer;
			midLine.Add(newHex);
			hexagons.Add(newHex);
		}
		float xshift = Mathf.Cos(Mathf.PI / 3) * hexSideLength + hexSideLength;
		float zshift = Mathf.Sin(Mathf.PI / 3) * hexSideLength;
		for (int i = 0; i < height; i++) {
			int heightMult = (i / 2) + 1;
			foreach (Hexagon obj in midLine) {
				Hexagon newHex = Instantiate(obj.gameObject).GetComponent<Hexagon>();
				newHex.transform.SetParent(gameObject.transform);
				newHex.transform.localPosition = new Vector3(newHex.transform.localPosition.x, 0, newHex.transform.localPosition.z) +
					new Vector3(i % 4 < 2 ? xshift : 0, 0, (i % 2 == 0 ? 1 : -1) * heightMult * zshift);
				newHex.gameObject.name = "Hex";
				newHex.gameObject.layer = gameObject.layer;
				hexagons.Add(newHex);
			}
		}
		this.hexagons = hexagons.ToArray();
	}

	void Update() {
		if (refresh) {
			refresh = false;
			RefreshHexagons();
		}
		float dist;
		for (int i = 0; i < hexagons.Length; i++) {
			hexagons[i].Refresh();
			hexagons[i].targetHeight = farHeight;
			for (int n = 0; n < riseAmt; n++) {
				dist = DistanceSquare(hexagons[i].position, radialRises[n]);
				hexagons[i].targetHeight = Min(hexagons[i].targetHeight,
					Lerp(closeHeight, farHeight, Min(dist / radius, 1)));
			}
		}
		//for (int i = 0; i < radialRises.Length; i++) {
		//	if (riseAmt > i)
		//		Shader.SetGlobalVector("RiserPosition" + i.ToString(), radialRises[i]);
		//	else
		//		Shader.SetGlobalVector("RiserPosition" + i.ToString(), new Vector4(0, 0, 0, -1000000));
		//}
		riseAmt = 0;
	}

	public void RadialRise(Vector3 loc) {
		radialRises[riseAmt] = new Vector4(loc.x, loc.y, loc.z, 1);
		riseAmt++;
	}

	private float Lerp(float a, float b, float ratio) {
		return a + (b - a) * ratio;
	}

	private float Max(float a, float b) {
		return a > b ? a : b;
	}

	private float Min(float a, float b) {
		return a < b ? a : b;
	}

	private float DistanceSquare(Vector3 a, Vector3 b) {
		return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
	}
}
