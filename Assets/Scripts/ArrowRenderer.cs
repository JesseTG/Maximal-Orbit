using UnityEngine;
using System.Collections;

public class ArrowRenderer : MonoBehaviour
{
    [Tooltip("The material used to render the arrow")]
    public Material
        ArrowMaterial;

    private PlanetManager _planetManager;

    void Start() {
        this._planetManager = GameObject.FindObjectOfType<PlanetManager>();
    }

    void OnPostRender ()
    {
        if (this._planetManager.State == PlacingState.Placing) {
            Vector3 planet = Camera.main.WorldToScreenPoint (this._planetManager.PlacingDropOff);
            planet.x /= Screen.width;
            planet.y /= Screen.height;
            planet.z = 0;

            Vector3 mouse = Input.mousePosition;
            mouse.x /=  Screen.width;
            mouse.y /= Screen.height;
            mouse.z = 0;


            Vector3 diff = planet - mouse;
            if (diff.magnitude > this._planetManager.MaxDistance) {
                mouse = Vector3.MoveTowards(planet, mouse, this._planetManager.MaxDistance);
            }

            GL.PushMatrix ();
            this.ArrowMaterial.SetPass (0);
            GL.LoadOrtho ();
            GL.Begin (GL.LINES);
            GL.Vertex (mouse);
            GL.Vertex (planet);
            GL.End ();
            GL.PopMatrix ();
        }
    }
}
