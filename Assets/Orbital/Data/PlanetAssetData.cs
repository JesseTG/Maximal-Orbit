using UnityEngine;
using System.Collections;

public class PlanetAssetData : ScriptableObject
{
    [Tooltip("Spheres of different fidelity")]
    public Mesh
        PlanetMesh;
    [Tooltip("The planet that will be created")]
    public GameObject
        PlanetPrefab;
    [Tooltip("The prefab used for explosions")]
    public GameObject
        ExplosionPrefab;
    [Tooltip("The materials a planet can be rendered with.  Randomly chosen on creation")]
    public Material[]
        Materials;
}
