using UnityEngine;
using System.Collections;

public class HexRiser : MonoBehaviour {
    public HexTiler hexTiler;
    	
	void Update () {
        hexTiler.RadialRise(transform.position);
	}
}
