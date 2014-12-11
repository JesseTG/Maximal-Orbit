using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PointGravity2D : MonoBehaviour
{
    public Vector2 Center;
    private float _gravity;

    void Start ()
    {
        this._gravity = Physics2D.gravity.magnitude;
    }

    void FixedUpdate ()
    {
        Vector2 force = this.Center - this.rigidbody2D.position;
        force.Normalize (); // Direction

        force *= this._gravity * this.rigidbody2D.mass;
        // Acceleration and mass

        this.rigidbody2D.AddForce (force);
    }
}
