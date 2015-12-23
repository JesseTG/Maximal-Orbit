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
}
