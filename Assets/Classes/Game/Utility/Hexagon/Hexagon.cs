using UnityEngine;
using System.Collections;

public class Hexagon : MonoBehaviour {
    public float targetHeight;

    public Transform trans { get; set; }
    public Vector3 position { get; set; }

    private float currentHeight;

    void Start() {
        trans = transform;
        targetHeight = trans.localScale.y;
        currentHeight = trans.localScale.y;
        position = trans.position;
        GetComponent<MeshRenderer>().material.SetVector("_HexPosition", new Vector4(position.x, position.y, position.z, 1));
    }

    public void Refresh() {
        if (Abs(currentHeight - targetHeight) > 0.01f) {
            Vector3 scale = trans.localScale;
            currentHeight = Mathf.Lerp(scale.y, targetHeight, Time.deltaTime * 10);
            trans.localScale = new Vector3(scale.x, currentHeight, scale.z);
        }
    }

    private float Abs(float f) {
        return f > 0 ? f : -f;
    }
}
