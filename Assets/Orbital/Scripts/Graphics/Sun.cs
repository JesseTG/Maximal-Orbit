using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LensFlare), typeof(MeshFilter), typeof(MeshRenderer))]
public class Sun : QualityAdjuster
{
    public float[] Heights = {0, 0, -2};

    private LensFlare _flare;
    private MeshFilter _mesh;
    private Renderer _renderer;

    void Awake()
    {
        this._flare = this.GetComponent<LensFlare> ();
        this._mesh = this.GetComponent<MeshFilter> ();
        this._renderer = this.GetComponent<Renderer>();
        this.SetQuality(QualitySettings.GetQualityLevel());
    }

    public override void SetQuality (int index)
    {
        Vector3 pos = this.transform.position;
        pos.z = Heights[index];
        this.transform.position = pos;

        switch (index) {
        case 0:
            this._flare.enabled = false;
            _renderer.enabled = true;
            break;
        case 1:
            this._flare.enabled = true;
            _renderer.enabled = false;
            break;
        case 2:
            this._flare.enabled = true;
            _renderer.enabled = false;
            break;
        }
    }
}
